# SamlProxyApp

A modular SAML 2.0 Proxy Web App built in .NET 8 and deployed on Azure Web Apps for Linux. It is designed to securely handle:

- SAML AuthnRequest signing
- Assertion validation (SAMLResponse)
- Single Logout (SAML LogoutRequest)
- Secure certificate retrieval from Azure Key Vault
- Acting as a delegated authentication module callable from IdentityServer4 (on-prem VM)
- Token issuance or session generation to return to main app

## 📦 Technologies

- **.NET 8**
- **Sustainsys.Saml2**
- **Azure Key Vault**
- **Azure Web App for Linux**
- **Terraform (IaC)**
- **GitHub Actions (CI/CD)**
