# Azure Key Vault Strategy and Best Practices

This guide outlines how to securely use Azure Key Vault to manage certificates for the SamlProxyApp in production.

---

## 🎯 Objectives

- Avoid hardcoding credentials or secrets
- Ensure secure, role-based access to certs
- Enable safe cert rotation without downtime
- Leverage Managed Identity instead of client secrets

---

## 🔐 Certificate Storage Guidelines

### Certificate Format

- Upload **PFX (Personal Information Exchange)** files that include private keys.
- Azure Key Vault stores the cert as:
  - A certificate resource (public part)
  - A secret (base64 PFX)
  - A key (if backed by HSM)

> For SAML2, use the **secret (PFX)** to load an `X509Certificate2`.

---

## 📦 Uploading a Certificate

### Via CLI

```bash
az keyvault certificate import \
  --vault-name samlKeyVault \
  --name SamlSigningCert \
  --file ./saml-signing-cert.pfx
```

> Ensure the cert has an exportable private key.

---

## 🔐 Secure Access with Managed Identity

### Why Managed Identity?

- No secrets stored in code
- RBAC-enforced
- Rotates credentials automatically

### Setup Steps

1. Enable System-Assigned Managed Identity on the Azure Web App
2. Grant permissions to Key Vault:

```bash
az keyvault set-policy \
  --name samlKeyVault \
  --object-id <web-app-principal-id> \
  --secret-permissions get \
  --certificate-permissions get
```

> You can find the principal ID in the App Service's "Identity" section.

---

## 🧑‍💻 Accessing in .NET

```csharp
var client = new SecretClient(new Uri("https://saml-keyvault.vault.azure.net/"), new DefaultAzureCredential());
var secret = await client.GetSecretAsync("SamlSigningCert");
var bytes = Convert.FromBase64String(secret.Value.Value);
var cert = new X509Certificate2(bytes, (string)null, X509KeyStorageFlags.MachineKeySet);
```

---

## 🔁 Certificate Rotation Best Practices

- Use versioned secrets or upload new certs under a new name
- Update config to point to new cert without downtime
- Monitor expiration and rotate at least 30 days prior

---

## 🧪 Testing Key Vault Integration

- Use `az login` and `DefaultAzureCredential()` locally
- Test retrieval with:
  - Correct Key Vault URI
  - Correct certificate name
  - Role assignment confirmed

---

## 🛡️ Summary

| Task                        | Best Practice                                  |
|-----------------------------|-----------------------------------------------|
| Cert storage                | Base64 PFX in Key Vault secret                |
| Access method               | SecretClient + DefaultAzureCredential         |
| Identity method             | System-assigned Managed Identity              |
| Permissions                 | `get` on secret and certificate               |
| Rotation                    | Manual with versioned names or auto tools     |
| Audit access                | Use Key Vault diagnostic logging              |
