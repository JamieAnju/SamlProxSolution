# SamlProxyApp

## Overview
SamlProxyApp is a web application designed to handle SAML authentication requests and responses. It utilizes Azure Key Vault for secure certificate management and provides a structured approach to managing SAML interactions.

## Project Structure
The project is organized into the following directories:

- **src/SamlProxyApp**: Contains the main application code.
  - **Controllers**: Houses the `SamlController` class responsible for handling SAML requests.
  - **Models**: Intended for model classes representing data structures (currently empty).
  - **Services**: Intended for service classes encapsulating business logic (currently empty).
  - **appsettings.json**: Configuration settings for the application.
  - **appsettings.Development.json**: Configuration settings specific to the development environment.
  - **Program.cs**: Entry point of the application.
  - **Startup.cs**: Configures services and the application's request pipeline.

- **tests/SamlProxyApp.Tests**: Contains unit tests for the application.
  - **Controllers**: Intended for unit tests related to controllers (currently empty).
  - **Services**: Intended for unit tests related to services (currently empty).

## Setup Instructions
1. Clone the repository:
   ```
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```
   cd SamlProxyApp
   ```
3. Restore the dependencies:
   ```
   dotnet restore
   ```
4. Run the application:
   ```
   dotnet run --project src/SamlProxyApp/SamlProxyApp.csproj
   ```

## Usage
- To initiate a SAML authentication request, navigate to `/saml/initiate`.
- The application will handle the SAML response at `/saml/acs`.
- Metadata can be accessed at `/saml/metadata`.

## Contributing
Contributions are welcome! Please submit a pull request or open an issue for discussion.

## License
This project is licensed under the MIT License. See the LICENSE file for details.