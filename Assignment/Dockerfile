# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy the project files
COPY Assignment/*.csproj ./Assignment/

# Copy the entrypoint script to the root of the container
COPY entrypoint.sh ./entrypoint.sh

# Restore the dependencies
RUN dotnet nuget locals all --clear
RUN dotnet restore ./Assignment/Assignment.csproj

# Copy the remaining files and build the app
COPY . .

# Build the project
RUN dotnet build ./Assignment/Assignment.csproj -c Release -o /app/build

# Publish the application
RUN dotnet publish ./Assignment/Assignment.csproj -c Release -o /app/publish

# Make entrypoint.sh executable
RUN chmod +x ./entrypoint.sh

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/publish . 

# # Expose the port that the application listens on (only for documentation purposes)
# # Port mapping will be done at runtime.
# # NB: Port mapping can be done in either of the following ways:
#     # Through the docker-compose file (chosen for this application)
#     # Directly by passing port numbers as args when running the docker container.
EXPOSE ${HOST_HTTP_PORT}
EXPOSE ${HOST_HTTPS_PORT}

# Set the ASP.NET Core environment URL to listen on all addresses for HTTP
ENV ASPNETCORE_URLS=http://*:${HOST_HTTP_PORT};

# Set the entry point to run your application
ENTRYPOINT ["dotnet", "Assignment.dll"]


