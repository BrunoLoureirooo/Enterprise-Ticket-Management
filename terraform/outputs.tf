output "frontend_url" {
  description = "Public URL of the Angular frontend"
  value       = "https://${azurerm_container_app.frontend.ingress[0].fqdn}"
}

output "gateway_url" {
  description = "Public URL of the API gateway"
  value       = "https://${azurerm_container_app.gateway.ingress[0].fqdn}"
}
