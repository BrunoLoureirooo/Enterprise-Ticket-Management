variable "subscription_id" {
  description = "Azure subscription ID"
  type        = string
}

variable "app_name" {
  description = "Application name used for naming all resources"
  type        = string
  default     = "ticket-mgmt"
}

variable "location" {
  description = "Azure region"
  type        = string
  default     = "westeurope"
}

variable "github_owner" {
  description = "GitHub username or org (for ghcr.io image paths)"
  type        = string
}

variable "db_connection_string" {
  description = "Neon PostgreSQL connection string (Npgsql format)"
  type        = string
  sensitive   = true
}

variable "redis_connection_string" {
  description = "Upstash Redis connection string (StackExchange.Redis format)"
  type        = string
  sensitive   = true
}

variable "jwt_secret" {
  description = "JWT signing secret"
  type        = string
  sensitive   = true
}

variable "sentry_dsn_identity" {
  description = "Sentry DSN for identity service"
  type        = string
  sensitive   = true
  default     = ""
}

variable "sentry_dsn_gateway" {
  description = "Sentry DSN for gateway service"
  type        = string
  sensitive   = true
  default     = ""
}

variable "ticket_db_connection_string" {
  description = "Neon PostgreSQL connection string for ticket service"
  type        = string
  sensitive   = true
}

variable "sentry_dsn_ticket" {
  description = "Sentry DSN for ticket service"
  type        = string
  sensitive   = true
  default     = ""
}

variable "teams_db_connection_string" {
  description = "Neon PostgreSQL connection string for teams service"
  type        = string
  sensitive   = true
}

variable "rabbitmq_connection_string" {
  description = "AMQP connection string (e.g. CloudAMQP URL: amqps://user:pass@host/vhost)"
  type        = string
  sensitive   = true
}

variable "sentry_dsn_teams" {
  description = "Sentry DSN for teams service"
  type        = string
  sensitive   = true
  default     = ""
}
