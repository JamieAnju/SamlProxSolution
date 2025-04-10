# SAML2 Security Best Practices

This document outlines best practices for secure implementation of SAML 2.0 in the SamlProxyApp.

---

## 🔐 Certificate and Signature Handling

- Sign all outgoing `AuthnRequest` and `LogoutRequest` messages using a private key from Azure Key Vault.
- Use **X.509 certificates** with private keys stored as **base64-encoded PFX** in Key Vault.
- Load certificates using Azure SDK's `SecretClient` and convert to `X509Certificate2` in code.
- Apply `X509KeyStorageFlags.MachineKeySet | Exportable | PersistKeySet` for compatibility.

---

## ✅ Inbound Assertion Validation

- Always verify SAMLResponse:
  - Signature is present
  - Signature is valid and from known IdP
  - Certificate matches metadata
- Validate:
  - `Issuer` matches IdP
  - `Audience` matches service provider EntityId
  - `Destination` matches ACS URL
  - `NotBefore` and `NotOnOrAfter` claims are valid
  - `InResponseTo` matches request

---

## 🚫 Common Pitfalls

- ❌ Never skip signature validation in dev/test
- ❌ Avoid hardcoding metadata or certificates
- ❌ Don’t use self-signed certs in production
- ❌ Avoid trusting unsigned assertions

---

## 🧾 Logging Recommendations

- Log every:
  - Signing operation (cert thumbprint, timestamp, algorithm)
  - Signature validation success/failure
  - Metadata load/update event
- Include:
  - Request ID, Session ID
  - IP address (where possible)
  - User identifier (if known)

---

## 🔄 Rotation & Maintenance

- Use versioned secrets in Azure Key Vault for cert rotation.
- Re-fetch signing cert if token validation fails with current.
- Monitor cert expiration and alert 30 days before.
