provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "saml" {
  name     = "saml-rg"
  location = "East US"
}

resource "azurerm_app_service_plan" "saml" {
  name                = "saml-app-service-plan"
  location            = azurerm_resource_group.saml.location
  resource_group_name = azurerm_resource_group.saml.name
  kind                = "Linux"
  reserved            = true
  sku {
    tier = "Basic"
    size = "B1"
  }
}

resource "azurerm_app_service" "saml" {
  name                = "saml-web-app"
  location            = azurerm_resource_group.saml.location
  resource_group_name = azurerm_resource_group.saml.name
  app_service_plan_id = azurerm_app_service_plan.saml.id
  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_key_vault" "saml" {
  name                = "saml-keyvault"
  location            = azurerm_resource_group.saml.location
  resource_group_name = azurerm_resource_group.saml.name
  tenant_id           = data.azurerm_client_config.current.tenant_id
  sku_name            = "standard"
}

resource "azurerm_key_vault_access_policy" "saml" {
  key_vault_id = azurerm_key_vault.saml.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_app_service.saml.identity[0].principal_id

  secret_permissions = ["get"]
  certificate_permissions = ["get"]
}
