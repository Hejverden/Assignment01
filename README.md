# Assignment Project

## Introduction
This project is a web application built with ASP.NET Core. It's structured to separate concerns between user interactions (Controllers), data models (Models), business logic (Services), and views (Views). It also includes configuration files and resources (wwwroot) for front-end assets.

### Conceptual Design
HERE I need a sketch, ...


![Flowchart](FlowChart.png)

## Getting Started

### Prerequisites
Ensure you have the following installed on your machine:

- Visual Studio Code : https://code.visualstudio.com/download
- C# for Visual Studio Code (latest version): https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp
- .NET 8.0 SDK: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

### Setting Up the Project
Choose either of the options below to open the project in your preferred IDE that supports ASP.NET Core development eg. Visual Studio Code (VS Code)

- Unzip the project file --> using cmd in Windows (terminal in mac os) navigate to the root directory of the project --> type "code .". This will open the project in VS Code.
- Clone the git repository from ???? --> using cmd in Windows (terminal in mac os) navigate to the root directory of the project --> type "code .". This will open the project in VS Code.

### Installing Dependencies
To install the required packages for this project, run the following command in your terminal in VS Code:
*dotnet restore*

Using *dotnet restore`* is generally sufficient for ensuring that all necessary dependencies are installed. The command will install the following packages (as listed inside the .csproj file):

- DotNetEnv
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Json
- Microsoft.Extensions.Options
- Newtonsoft.Json
- Serilog.AspNetCore
- Serilog.Sinks.Console
- Serilog.Sinks.File
- Microsoft.NET.Test.Sdk
- Microsoft.AspNetCore.Mvc.Testing
- xunit
- xunit.runner.visualstudio
- Moq

For manual installation, use;
*dotnet add package _packagename_*

### Setting Environment for Running the Application
To run the application in different environments, you need to set the ASPNETCORE_ENVIRONMENT environment variable by using either of the following options:

#### Linux/MacOS
- export ASPNETCORE_ENVIRONMENT=Production   # For Production environment
- export ASPNETCORE_ENVIRONMENT=Development  # For Development environment (default)
- export ASPNETCORE_ENVIRONMENT=Test         # For Test environment

#### Windows (PowerShell)
- $Env:ASPNETCORE_ENVIRONMENT = "Production"   # For Production environment
- $Env:ASPNETCORE_ENVIRONMENT = "Development"  # For Development environment (default)
- $Env:ASPNETCORE_ENVIRONMENT = "Test"         # For Test environment

Notes:
- Production Environment: Use this setting when deploying your application to a production server.
- Development Environment: Default setting for local development. Provides detailed error messages and other developer-friendly features.
- Test Environment: Use this setting for running automated tests. It can have specific configurations for testing purposes.

### Environment Variables
To send requests to the Flickr API, you need to have your own API key and API secret. Since these are sensitive information, I have chosen to securely save them as environment variables inside an .env file in the project root directory. For the purpose of this project, the following environment variables are defined inside the .env file in the root directory of the solution:

- FLICKR_API_KEY: API key from Flickr obtained upon Flickr account creation.
- FLICKR_API_SECRET: API secret from Flickr obtained together with API key upon Flickr account creation.
- HOST_HTTP_PORT
- CONTAINER_HTTP_PORT
- HOST_HTTPS_PORT
- CONTAINER_HTTPS_PORT

The values of *FLICKR_API_KEY* and *FLICKR_API_SECRET* are simply placeholders in order to avoid revealing my personal Flickr API key and secret. Thus, you only need to replace the two placeholders with your actual Flickr API key and secret values. 

**NB:**
*These values are also excluded from my git commits. However, in order to get the CI/CD pipeline to run successfully, I have added them as secrets in Github Action.* 


### Run unit tests
To run unit-/integration tests, navigate to the root directory of the solution project, type:
- *dotnet test* will run all tests in the entire solution. 

### Run the application in Debug mode ###
To build and run the application do either of the following options:
- Press F5 (for debug) or ctrl+F5 (no debug)
- Navigate to the application project directory (Assignment)
  1. *dotnet build* will build the application
  2. *dotnet run* will start the application on a local server accissible at https://localhost:7044 or http://localhost:5129 (manually navigate to that).

### Run the application in Docker ###
This project includes a Docker setup that allows you to build and run the application in isolated containers. Using Docker simplifies the environment setup and ensures consistency across different machines.

#### Prerequisites #### 
Make sure you have Docker installed on your machine. You can download it from Docker's official site: https://www.docker.com/

#### Run #### 
- Navigate to the root directory of the whole solution
1. *docker-compose build* will build the images for all 3 services.
2. *docker-compose up* runs the container
3. Access the Application: Open your web browser and navigate to:
  - http://localhost:5129 for HTTP
  - https://localhost:7044 for HTTPS
  - You can also click on the URL's from inside of the web application container. 

*Notes*
- Ensure your .env file is set up correctly with your Flickr API key and other required environment variables before running the Docker containers.
- The application is configured to run in the Development environment by default. You can modify this in the docker-compose.yml file if needed.




**Assignment**
├──  **Assignment/**
├────── Dockerfile
├────── Assignment.csproj
├────── Program.cs
├────── ...
├──  **Assignment.UnitTests/**
├────── Dockerfile.UnitTests
├────── Assignment.UnitTests.csproj
├────── ...
├──  **Assignment.IntegrationTests/**
├──────Dockerfile.IntegrationTests
├────── Assignment.IntegrationTests.csproj
├────── ...
├──  **docker-compose.yml**


## Project Structure

- Controllers/**
  - `HomeController.cs`: Handles requests for the homepage, contact, and privacy pages.
  - `PhotosController.cs`: Manages photo-related actions.

- Models/**
  - `ErrorViewModel.cs`: Represents the structure for error messages.
  - `FlickrApiSettings.cs`: Contains settings for the Flickr API integration.
  - `Photo.cs`: Represents the photo model.

- Services/**
  - `FetchService.cs`: Service for fetching data from external APIs.

- Views/**
  - Home/: Contains views for the homepage, contact, and privacy pages.
  - Shared/: Contains shared views like layout and validation scripts.
  - `_ViewImports.cshtml`: Contains common Razor directives that are imported into other view files.
  - `_ViewStart.cshtml`: Sets common view properties.

- wwwroot/:**
  - Contains static files like CSS, JavaScript, and the index.html.

- Properties/**
  - `launchSettings.json`: Configures the project's launch settings for development.

- Root Directory Files:**
  - `appsettings.json` and `appsettings.Development.json`: Configuration files for the application.
  - `Program.cs`: Entry point for the application.
  - `Assignment.csproj` and `Assignment.sln`: Project and solution files for the ASP.NET Core application.

## Regarding running the tests when containerizing:
- Running Tests Inside the Dockerfile
- Defining Tests as Separate Services: 
  - one could consider each of the projects in the solution as a service each with their own Dockerfile
  - A single docker-compose.yml file at the solution directory takes care of all those Docker files and sets all services up. 


## Why the test services depend on the main application service?
- Integration tests may need the main application to be up to interact with its endpoints or to access its environment (like environment variables or configuration files).
- Even if they mock some parts, they often depend on the application being available in a certain state (fx. running)

- Mocking vs. Integration: Clearly define which tests require the application to be up and running. Mocking should be used extensively in unit tests to reduce dependencies on external services, while integration tests can validate real interactions with the application.

If your unit tests are purely mock-based and don't require the application's runtime, they can run independently of the main application.

Integration tests may depend on the application being reachable and operational. However, if they only require specific endpoints or services (like APIs), you might consider running those components in Docker Compose along with the tests.





