# Deployment & GitHub Actions Strategy

This guide provides a GitHub Actions-based deployment strategy for the SamlProxyApp. It includes Terraform infrastructure deployment, .NET publish, and Azure Web App deployment.

---

## 🚀 GitHub Actions Overview

GitHub Actions enables:

- Automatic deploys on `push` to `main`
- Terraform infrastructure provisioning
- .NET build and publish
- Secure deployment to Azure using OIDC authentication (no secrets)

---

## 📂 Recommended Directory Layout

```text
.github/
└── workflows/
    └── deploy.yml

infra/
├── main.tf
├── provider.tf
├── variables.tf
├── outputs.tf

app/
└── SamlProxyApp/
    ├── Program.cs
    ├── Controllers/
    └── ...
```

---

## ✅ GitHub Workflow (deploy.yml) Key Stages

1. **Checkout Repo**
2. **Terraform Init & Apply**
3. **.NET Publish**
4. **Deploy to Azure Web App**

---

## 🔐 Authentication with OIDC

Use workload identity federation instead of secrets:

```yaml
permissions:
  id-token: write
  contents: read
```

Configure Azure credentials:

```yaml
- name: Azure Login
  uses: azure/login@v1
  with:
    client-id: ${{ secrets.AZURE_CLIENT_ID }}
    tenant-id: ${{ secrets.AZURE_TENANT_ID }}
    subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}
```

---

## ⚙️ Key Terraform Best Practices

- Store state remotely (use `azurerm_backend`)
- Use `variables.tf` for reusable config
- Use `outputs.tf` to expose values to workflows
- Separate staging and production with workspaces or environments

---

## 🧪 .NET Best Practices for CI/CD

- Run `dotnet restore` and `dotnet build --configuration Release`
- Publish to a zip artifact:
  ```bash
  dotnet publish -c Release -o published/
  ```
- Deploy with:
  ```yaml
  uses: azure/webapps-deploy@v2
  with:
    app-name: saml-web-app
    package: ./published/
  ```

---

## 🧠 Recommended Secrets in GitHub

- `AZURE_CLIENT_ID`
- `AZURE_TENANT_ID`
- `AZURE_SUBSCRIPTION_ID`

> Copilot will use these names to suggest correct GitHub Action templates.

---

## 📈 Monitoring

- Enable Application Insights
- Log deployment failures in GitHub Actions
- Alert on Key Vault access denials or cert expiration

---

## 🧼 Clean Up

- Use `terraform destroy` or `az group delete` for teardown
- Clean artifacts on failure to avoid zombie deployments

---

## ✅ Summary Table

| Area              | Best Practice                            |
|-------------------|-------------------------------------------|
| Deployment        | GitHub Actions (push to `main`)           |
| Auth to Azure     | OIDC with `azure/login`                   |
| Infra management  | Terraform with `azurerm` provider         |
| Cert access       | Managed Identity + Key Vault              |
| App publish       | `dotnet publish` + zip deployment         |
| Secret handling   | Use GitHub secrets, not hardcoded values  |
