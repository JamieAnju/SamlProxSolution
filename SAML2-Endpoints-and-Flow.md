# SAML2 Endpoints and Authentication Flow

This document outlines the expected endpoints and control flow for the SamlProxyApp.

---

## 🔁 Core Endpoints

| Endpoint         | Method | Description                                  |
|------------------|--------|----------------------------------------------|
| `/saml/initiate` | GET    | Starts login flow, builds signed AuthnRequest |
| `/saml/acs`      | POST   | Assertion Consumer Service: receives and validates SAMLResponse |
| `/saml/logout`   | GET/POST | Optional logout endpoint |
| `/saml/metadata` | GET    | Returns SAML 2.0 Service Provider metadata   |

---

## 🧭 Authentication Flow

1. **User Login Attempt**
   - VM-hosted app redirects to:  
     `https://saml.pubstrat.com/saml/initiate?returnUrl=https://customer.pubstrat.com/callback`

2. **SAML AuthnRequest**
   - SamlProxyApp:
     - Loads SAML settings
     - Retrieves signing cert from Azure Key Vault
     - Signs AuthnRequest
     - Redirects to Azure Entra ID (IdP)

3. **SAMLResponse**
   - IdP POSTs signed assertion to `/saml/acs`
   - App verifies:
     - Signature
     - Timestamps
     - Audience
     - Destination
   - Issues local session or signed JWT

4. **Final Redirect**
   - Redirects user to `returnUrl` with session or token

---

## 🧪 Metadata Use

- Metadata is fetched from IdP (Azure Entra ID) at startup or on-demand.
- SamlProxyApp exposes `/saml/metadata` to enable IdP setup.

---

## ⚙️ Additional Notes

- AuthnRequest should include:
  - `Destination`
  - `AssertionConsumerServiceURL`
  - `Issuer`
  - `ID`, `IssueInstant`

- Optional parameters like `ForceAuthn`, `NameIDPolicy` can be configured if needed.
