name: Deploy to Azure

on:
  push:
    branches:
      - main

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0

    - name: Build and publish
      run: |
        dotnet restore
        dotnet build --configuration Release
        dotnet publish -c Release -o published/

    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: saml-web-app
        package: ./published/