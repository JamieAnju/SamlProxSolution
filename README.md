# SamlProxyApp

A modular SAML 2.0 Proxy Web App built in .NET 8 and deployed on Azure Web Apps for Linux. It is designed to securely handle:

- SAML AuthnRequest signing
- Assertion validation (SAMLResponse)
- Single Logout (SAML LogoutRequest)
- Secure certificate retrieval from Azure Key Vault
- Acting as a delegated authentication module callable from IdentityServer4 (on-prem VM)
- Token issuance or session generation to return to main app

---

## 🧱 Architecture

```mermaid
graph TD
    IIS[Web App on VM]
    AzureApp[Azure App Service (saml.pubstrat.com)]
    KeyVault[Azure Key Vault]
    IdP[Azure Entra ID (SAML IdP)]
    
    IIS -->|redirect| AzureApp
    AzureApp -->|AuthnRequest| IdP
    IdP -->|SAMLResponse| AzureApp
    AzureApp -->|token/assertion| IIS
    AzureApp --> KeyVault
```

---

## 📦 Technologies

- **.NET 8**
- **Sustainsys.Saml2**
- **Azure Key Vault**
- **Azure Web App for Linux**
- **Terraform (IaC)**
- **GitHub Actions (CI/CD)**

---

## 🔐 KeyVault Certificate Setup

- Upload your certificate as PFX with private key
- Store under secret name: `SamlSigningCert`
- Assign access policy to the Azure Web App's managed identity

---

## 🔧 App Features

- `/saml/initiate`: Builds and signs AuthnRequest
- `/saml/acs`: Assertion consumer service for receiving SAMLResponse
- `/saml/logout`: Handles SAML logout
- `/saml/metadata`: Exposes SAML metadata
- All SAML messages are signed with cert pulled from Azure Key Vault
- Certs never touch disk

---

## ⚙️ Infrastructure

Provisioned via Terraform:

- Resource Group
- Azure Key Vault
- Azure App Service Plan
- Azure Linux Web App
- Managed Identity
- SSL Binding to saml.pubstrat.com

---

## 🌐 Configuration

In `appsettings.json`:

```json
{
  "KeyVault": {
    "Uri": "https://saml-keyvault.vault.azure.net/"
  },
  "Saml": {
    "EntityId": "https://saml.pubstrat.com/Saml2",
    "IdpMetadata": "https://login.microsoftonline.com/<TENANT-ID>/federationmetadata/2007-06/federationmetadata.xml"
  }
}
```

---

## 🚀 CI/CD (GitHub Actions)

Located at `.github/workflows/deploy.yml`:

- Runs on `push` to `main`
- Executes `dotnet publish`
- Deploys to Azure Web App via OIDC
- Terraform applies infra if needed

---

## ✅ Future Enhancements

- Token validation endpoint `/token/validate`
- Session management with Redis or table storage
- Multi-tenant support
- Audit logging to Azure Monitor or App Insights

---

## 🧪 Development

```bash
dotnet restore
dotnet build
dotnet run
```

---

## 📥 Deployment

Terraform + GitHub Actions auto-deploy on push.

Ensure secrets are stored in GitHub or Azure Key Vault and not committed.

---

## 🧠 Maintainers

This repo supports delegated auth logic for secure, auditable, centralized SAML handling across any app that supports token handoff.
