output "resource_group_name" {
  value = azurerm_resource_group.saml.name
}

output "app_service_name" {
  value = azurerm_app_service.saml.name
}
