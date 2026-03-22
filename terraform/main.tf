terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
  }
}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
}

# Resource Group
resource "azurerm_resource_group" "main" {
  name     = "rg-${var.app_name}"
  location = var.location
}

# Log Analytics Workspace (required by Container Apps Environment)
resource "azurerm_log_analytics_workspace" "main" {
  name                = "log-${var.app_name}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

# Container Apps Environment
resource "azurerm_container_app_environment" "main" {
  name                       = "cae-${var.app_name}"
  location                   = azurerm_resource_group.main.location
  resource_group_name        = azurerm_resource_group.main.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.main.id
}

# Identity API
resource "azurerm_container_app" "identity" {
  name                         = "ca-identity"
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode                = "Single"

  template {
    container {
      name   = "identity"
      image  = "mcr.microsoft.com/azuredocs/containerapps-helloworld:latest"
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "Production"
      }
      env {
        name  = "ASPNETCORE_URLS"
        value = "http://+:8080"
      }
      env {
        name        = "ConnectionStrings__DefaultConnection"
        secret_name = "db-connection-string"
      }
      env {
        name        = "SECRET"
        secret_name = "jwt-secret"
      }
      env {
        name  = "JwtSettings__Issuer"
        value = "TicketManagementAPI"
      }
      env {
        name  = "JwtSettings__Audience"
        value = "TicketManagementClient"
      }
      env {
        name  = "JwtSettings__ExpiryMinutes"
        value = "60"
      }
      env {
        name        = "Redis__ConnectionString"
        secret_name = "redis-connection-string"
      }
      env {
        name        = "RabbitMQ__ConnectionString"
        secret_name = "rabbitmq-connection-string"
      }
      env {
        name        = "Sentry__Dsn"
        secret_name = "sentry-dsn-identity"
      }
    }
  }

  secret {
    name  = "db-connection-string"
    value = var.db_connection_string
  }
  secret {
    name  = "jwt-secret"
    value = var.jwt_secret
  }
  secret {
    name  = "redis-connection-string"
    value = var.redis_connection_string
  }
  secret {
    name  = "rabbitmq-connection-string"
    value = var.rabbitmq_connection_string
  }
  secret {
    name  = "sentry-dsn-identity"
    value = var.sentry_dsn_identity
  }

  ingress {
    external_enabled = false
    target_port      = 8080
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  lifecycle {
    ignore_changes = [template[0].container[0].image]
  }
}

# Gateway API
resource "azurerm_container_app" "gateway" {
  name                         = "ca-gateway"
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode                = "Single"

  template {
    container {
      name   = "gateway"
      image  = "mcr.microsoft.com/azuredocs/containerapps-helloworld:latest"
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "Production"
      }
      env {
        name  = "ASPNETCORE_URLS"
        value = "http://+:8080"
      }
      env {
        name  = "IdentityApiUrl"
        value = "http://${azurerm_container_app.identity.name}"
      }
      env {
        name        = "SECRET"
        secret_name = "jwt-secret"
      }
      env {
        name        = "Redis__ConnectionString"
        secret_name = "redis-connection-string"
      }
      env {
        name        = "Sentry__Dsn"
        secret_name = "sentry-dsn-gateway"
      }
      env {
        name  = "CorsOrigins"
        value = "https://${azurerm_container_app.frontend.ingress[0].fqdn}"
      }
      env {
        name  = "ReverseProxy__Clusters__identity-cluster__Destinations__destination1__Address"
        value = "http://${azurerm_container_app.identity.name}/"
      }
      env {
        name  = "ReverseProxy__Clusters__ticket-cluster__Destinations__destination1__Address"
        value = "http://${azurerm_container_app.ticket.name}/"
      }
      env {
        name  = "ReverseProxy__Clusters__teams-cluster__Destinations__destination1__Address"
        value = "http://${azurerm_container_app.teams.name}/"
      }
    }
  }

  secret {
    name  = "jwt-secret"
    value = var.jwt_secret
  }
  secret {
    name  = "redis-connection-string"
    value = var.redis_connection_string
  }
  secret {
    name  = "sentry-dsn-gateway"
    value = var.sentry_dsn_gateway
  }

  ingress {
    external_enabled = true
    target_port      = 8080
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  lifecycle {
    ignore_changes = [template[0].container[0].image]
  }
}

# Frontend
resource "azurerm_container_app" "frontend" {
  name                         = "ca-frontend"
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode                = "Single"

  template {
    container {
      name   = "frontend"
      image  = "mcr.microsoft.com/azuredocs/containerapps-helloworld:latest"
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name  = "GATEWAY_HOST"
        value = "ca-gateway"
      }
    }
  }

  ingress {
    external_enabled = true
    target_port      = 80
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  lifecycle {
    ignore_changes = [template[0].container[0].image]
  }
}

# Teams API
resource "azurerm_container_app" "teams" {
  name                         = "ca-teams"
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode                = "Single"

  template {
    container {
      name   = "teams"
      image  = "mcr.microsoft.com/azuredocs/containerapps-helloworld:latest"
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "Production"
      }
      env {
        name  = "ASPNETCORE_URLS"
        value = "http://+:8080"
      }
      env {
        name        = "ConnectionStrings__DefaultConnection"
        secret_name = "teams-db-connection-string"
      }
      env {
        name        = "RabbitMQ__ConnectionString"
        secret_name = "rabbitmq-connection-string"
      }
      env {
        name        = "Sentry__Dsn"
        secret_name = "sentry-dsn-teams"
      }
    }
  }

  secret {
    name  = "teams-db-connection-string"
    value = var.teams_db_connection_string
  }
  secret {
    name  = "rabbitmq-connection-string"
    value = var.rabbitmq_connection_string
  }
  secret {
    name  = "sentry-dsn-teams"
    value = var.sentry_dsn_teams
  }

  ingress {
    external_enabled = false
    target_port      = 8080
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  lifecycle {
    ignore_changes = [template[0].container[0].image]
  }
}

# Ticket API
resource "azurerm_container_app" "ticket" {
  name                         = "ca-ticket"
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode                = "Single"

  template {
    container {
      name   = "ticket"
      image  = "mcr.microsoft.com/azuredocs/containerapps-helloworld:latest"
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "Production"
      }
      env {
        name  = "ASPNETCORE_URLS"
        value = "http://+:8080"
      }
      env {
        name        = "ConnectionStrings__DefaultConnection"
        secret_name = "ticket-db-connection-string"
      }
      env {
        name        = "RabbitMQ__ConnectionString"
        secret_name = "rabbitmq-connection-string"
      }
      env {
        name        = "Sentry__Dsn"
        secret_name = "sentry-dsn-ticket"
      }
    }
  }

  secret {
    name  = "ticket-db-connection-string"
    value = var.ticket_db_connection_string
  }
  secret {
    name  = "rabbitmq-connection-string"
    value = var.rabbitmq_connection_string
  }
  secret {
    name  = "sentry-dsn-ticket"
    value = var.sentry_dsn_ticket
  }

  ingress {
    external_enabled = false
    target_port      = 8080
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  lifecycle {
    ignore_changes = [template[0].container[0].image]
  }
}
