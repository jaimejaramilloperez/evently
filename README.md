![.NET](https://img.shields.io/badge/-.NET%2010.0-blueviolet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?logo=postgresql&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-ff463a?logo=redis&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-ff6600?logo=rabbitmq&logoColor=white)
![KeyCloak](https://img.shields.io/badge/KeyCloak-008aaa?logo=keycloak&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=fff)
![OpenAPI](https://img.shields.io/badge/-Swagger-85EA2D?style=flat&logo=swagger&logoColor=white)

# Evently API

> [!TIP]
> 📘 This project documentation is also available in [Spanish](./README-ES.md).

## Table of Contents

<ol>
  <li>
    <a href="#-overview">Overview</a>
  </li>
  <li>
    <a href="#-technologies">Technologies</a>
  </li>
  <li>
    <a href="#-features">Features</a>
  </li>
  <li>
    <a href="#-getting-started">Getting Started</a>
    <ul>
      <li><a href="#-prerequisites">Prerequisites</a></li>
      <li><a href="#-installation">Installation</a></li>
    </ul>
  </li>
  <li>
    <a href="#-environment-setup">Environment Setup</a>
    <ul>
      <li><a href="#-local-development">Local Development</a></li>
      <li><a href="#-docker-for-development">Docker for Development</a></li>
      <li><a href="#-docker-for-production">Docker for Production</a></li>
    </ul>
  </li>
  <li>
    <a href="#-testing">Testing</a>
  </li>
  <li>
    <a href="#-api-documentation">API Documentation</a>
  </li>
</ol>

## 🎯 Overview

Evently API is an event management system built with .NET 10 using a Modular Monolith architecture and Event-Driven Design. It features secure OpenID Connect authentication via Keycloak, ensures reliable messaging through Outbox/Inbox patterns, and provides deep insights with OpenTelemetry. The infrastructure is fully containerized with Docker, leveraging PostgreSQL for persistence, Redis for distributed caching, and RabbitMQ as the message broker.

<div align="center">
  <img width="1011" height="784" alt="app diagram" src="https://github.com/user-attachments/assets/2a3374b7-b5e4-4ef7-bb34-0bd407602fdb" />
</div>

This project was built following [Milan Jovanović](https://github.com/m-jovanovic)'s course [Modular Monolith](https://www.milanjovanovic.tech/modular-monolith-architecture), using the latest ASP.NET Core features and best practices.

## 🔧 Technologies

- [.NET 10](https://dotnet.microsoft.com/) – Modern framework for building scalable web APIs
- [PostgreSQL](https://www.postgresql.org/) – Open-source relational database
- [Redis](https://redis.io/) – In-memory data store used as a distributed cache
- [KeyCloak](https://www.keycloak.org/) – Open-source identity and access management
- [RabbitMQ](https://www.rabbitmq.com/) – Reliable and mature message broker for asynchronous communication between microservices
- [MassTransit](https://masstransit.io/) – Distributed application framework for .NET that simplifies working with message buses
- [Yarp](https://dotnet.github.io/yarp/) – A toolkit for building high-performance HTTP proxy servers using .NET infrastructure
- [Dapper](https://www.learndapper.com/) – High-performance micro-ORM for .NET used for efficient, raw SQL-based data access
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) – ORM for data access for .NET
- [Docker](https://www.docker.com/) – Containerization and local infrastructure
- [Swagger (Swashbuckle)](https://swagger.io/) – OpenAPI interactive documentation
- [FluentValidation](https://docs.fluentvalidation.net/) – Model validation framework
- [Quartz.NET](https://www.quartz-scheduler.net/) – Background jobs and scheduling
- [OpenTelemetry](https://opentelemetry.io/) – Distributed tracing and observability
- [Seq](https://datalust.co/seq) – Optional centralized log aggregation
- [Jaeger](https://www.jaegertracing.io/) – Open-source, end-to-end distributed tracing system
- [xUnit](https://xunit.net/) – Unit testing framework
- [Testcontainers](https://dotnet.testcontainers.org/) – Integration testing with containers
- [SonarAnalyzer](https://rules.sonarsource.com/csharp) – Static code analysis

## ✨ Features

#### ⚙️ Infrastructure & Integration
- PostgreSQL with EF Core, Dapper, and snake_case naming conventions
- Background jobs with Quartz
- Redis as a distributed cache and high-performance key-value store
- RabbitMQ for asynchronous messaging and service-to-service communication
- YARP (Yet Another Reverse Proxy) for routing and API Gateway functionality
- Interactive API docs with Swagger

#### 🔐 Security
- OpenID Connect (OIDC) identity provider integration with Keycloak
- Claims-based identity transformed from OIDC profile scopes
- Role-based Access Control (RBAC) through custom policies and roles

#### 📈 Observability
- OpenTelemetry-based distributed tracing
- Health checks support
- Logging with Seq
- Distributed tracing with Jaeger

#### 🧪 Testing Stack
- Unit testing with xUnit
- Architectural testing with NetArchTest
- Integration testing with Testcontainers, PostgreSQL, Redis, KeyCloak and RabbitMq

#### 📦 Deployment & DevOps
- Local development support with Docker Compose
- Centralized package versioning with MSBuild
- Dockerized production image support

## 🚀 Getting Started

### 📋 Prerequisites

Make sure you have .NET CLI installed on your system. You can check if it's available by running:

```bash
dotnet --version
```

This should print the installed version of the .NET CLI. If it's not installed, download it from the [official .NET site](https://dotnet.microsoft.com/download).

To verify which SDK versions are installed:

```bash
dotnet --list-sdks
```

> [!IMPORTANT]
> The minimum .NET SDK version required is **10.0.0**

Additionally, the project uses Docker for running supporting services (e.g., PostgreSQL, Redis, RabbitMQ, Seq). You’ll need:

- [**Docker**](https://www.docker.com/products/docker-desktop): Recommended to install Docker Desktop.
- [**Docker  Compose**](https://docs.docker.com/compose/): Typically included with Docker Desktop.

To check that Docker is installed and running:

```bash
docker --version
docker compose version
```

If these commands fail or return errors, refer to the [Docker installation guide](https://docs.docker.com/get-docker/).

---

### 📥 Installation

To get started, clone the repository and set up the environment configuration:

1. Clone the repository:

```bash
git clone https://github.com/jaimejaramilloperez/evently.git
```

2. Navigate to the project directory:

```bash
cd evently
```

3. Generate and trust the HTTPS development certificate:

```bash
dotnet dev-certs https -ep ./src/Evently.Api/certificate.pfx -p Test1234!
dotnet dev-certs https --trust
```

4. Copy the environment template and configure it:

```bash
cp .env.template .env
# Edit the .env file as needed
```

After installation, you're ready to run the app either locally or using Docker. See the [Local Development](#-local-development) or [Docker](#-docker-for-development) sections for details.

## 💻 Environment SetUp

Set up your environment to run the Evently API either locally or with Docker, depending on your workflow.

> [!NOTE]
> The configuration values shown (e.g., passwords, ports, keys, connection strings) are provided for demonstration purposes only. You are free to modify them as needed — especially for production environments.

> [!TIP]
> If the APIs runs inside a Docker container, `localhost` refers to the container itself. In that case, replace `localhost` with the service name defined in your Docker network (e.g., `evently.seq` and `evently.jaeger`).

---

### 🧑‍💻 Local Development

You can run the API locally using the .NET CLI and supporting services (PostgreSQL, Redis, RabbitMQ, Seq, etc.) via Docker Compose.

> [!IMPORTANT]
> Sensitive values should be stored securely using [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=linux)

1. Review and update the following configuration files as needed:

- [Evently.Api - appsettings.Development.json](./src/Api/Evently.Api/appsettings.Development.json)
- [Evently.Ticketing.Api - appsettings.Development.json](./src/Api/Evently.Ticketing.Api/appsettings.Development.json)
- [Evently.Gateway - appsettings.Development.json](./src/Api/Evently.Gateway/appsettings.Development.json)
- [Evently.Api - modules.attendance.Development.json](./src/Api/Evently.Api/modules.attendance.Development.json)
- [Evently.Api - modules.events.Development.json](./src/Api/Evently.Api/modules.events.Development.json)
- [Evently.Api - modules.users.Development.json](./src/Api/Evently.Api/modules.users.Development.json)
- [Evently.Ticketing.Api - modules.ticketing.Development.json](./src/Api/Evently.Ticketing.Api/modules.ticketing.Development.json)

2. Configure environment variables (`.env` file):

```dotenv
# postgresql
POSTGRES_DB="evently"
POSTGRES_USER="evently"
POSTGRES_PASSWORD="123456"

# redis
REDIS_PASSWORD="123456"

# pgadmin
PGADMIN_EMAIL="user@mail.com"
PGADMIN_PASSWORD="123456"

# seq
SEQ_SERVER_URL="http://evently.seq:5341"
SEQ_PASSWORD="12345678"

# keycloak
KEYCLOAK_USERNAME="admin"
KEYCLOAK_PASSWORD="admin"

# OpenTelemetry
OTEL_EXPORTER_OTLP_ENDPOINT="http://evently.jaeger:4317"
OTEL_EXPORTER_OTLP_PROTOCOL="grpc"

# RabbitMq
RABBITMQ_USER="guest"
RABBITMQ_PASSWORD="guest"
```

3. Start docker services:

```bash
docker compose up -d
```

4. Run the APIs and the gateway:

```bash
dotnet run --project src/Api/Evently.Api
dotnet run --project src/Api/Evently.Ticketing.Api
dotnet run --project src/Api/Evently.Gateway
# or with HTTPS
dotnet run --launch-profile https --project src/Api/Evently.Api
dotnet run --launch-profile https --project src/Api/Evently.Ticketing.Api
dotnet run --launch-profile https --project src/Api/Evently.Gateway
```

---

### 🐳 Docker for Development

This mode runs both the application and services in Docker containers using a development image.

1. Review and update the `.env` file:

```dotenv
# docker
COMPOSE_PROJECT_NAME="evently"
DOCKER_REGISTRY=""
DOCKER_IMAGE_TAG="dev"
DOCKER_BUILD_TARGET="dev"

# dotnet
BUILD_CONFIGURATION="Debug"
ENVIRONMENT="Development"
HTTP_PORT="5000"
HTTPS_PORT="5001"
CERTIFICATE_PATH="/https/certificate.pfx"
CERTIFICATE_PASSWORD="Test1234!"

# evently api
DATABASE_CONNECTION_STRING="Host=evently.database;Port=5432;Database=evently;Username=evently;Password=123456;"
CACHE_CONNECTION_STRING="evently.cache:6379,password=123456"

AUTHENTICATION_AUDIENCE="account"
AUTHENTICATION_VALID_ISSUERS_1="http://evently.identity:8080/realms/evently"
AUTHENTICATION_VALID_ISSUERS_2="http://host.docker.internal:18000/realms/evently"
AUTHENTICATION_METADATA_ADDRESS="http://evently.identity:8080/realms/evently/.well-known/openid-configuration"
AUTHENTICATION_REQUIRE_HTTPS_METADATA="false"
KEYCLOAK_HEALTH_Url="http://evently.identity:9000/health"

USERS_KEYCLOAK_ADMIN_URL="http://evently.identity:8080/admin/realms/evently/"
USERS_KEYCLOAK_TOKEN_URL="http://evently.identity:8080/realms/evently/protocol/openid-connect/token"
USERS_KEYCLOAK_CONFIDENTIAL_CLIENT_ID="evently-confidential-client"
USERS_KEYCLOAK_CONFIDENTIAL_CLIENT_SECRET="eHPYDdH5j8eutm54aApbgb4khT3vQXPM"
USERS_KEYCLOAK_PUBLIC_CLIENT_ID="evently-public-client"

# evently ticketing api
TICKETING_HTTP_PORT="5100"
TICKETING_HTTPS_PORT="5101"
TICKETING_CERTIFICATE_PATH="/https/certificate.pfx"
TICKETING_CERTIFICATE_PASSWORD="Test1234!"

# Gateway
GATEWAY_HTTP_PORT="3000"
GATEWAY_HTTPS_PORT="3001"
GATEWAY_CERTIFICATE_PATH="/https/certificate.pfx"
GATEWAY_CERTIFICATE_PASSWORD="Test1234!"

# postgresql
POSTGRES_DB="evently"
POSTGRES_USER="evently"
POSTGRES_PASSWORD="123456"

# redis
REDIS_PASSWORD="123456"

# pgadmin
PGADMIN_EMAIL="user@mail.com"
PGADMIN_PASSWORD="123456"

# seq
SEQ_SERVER_URL="http://evently.seq:5341"
SEQ_PASSWORD="12345678"

# keycloak
KEYCLOAK_USERNAME="admin"
KEYCLOAK_PASSWORD="admin"

# OpenTelemetry
OTEL_EXPORTER_OTLP_ENDPOINT="http://evently.jaeger:4317"
OTEL_EXPORTER_OTLP_PROTOCOL="grpc"

# RabbitMq
RABBITMQ_USER="guest"
RABBITMQ_PASSWORD="guest"
```

2. Start the containers:

```bash
docker compose -f ./docker-compose-debug.yml up -d
```

#### 🐞 Debugging in container

> [!IMPORTANT]
> Debugging inside containers requires the `vsdbg` debugger. If it’s not already installed, refer to the [official setup guide](https://github.com/microsoft/MIEngine/wiki/Offroad-Debugging-of-.NET-Core-on-Linux---OSX-from-Visual-Studio) for instructions on how to install it manually.

If you're using Visual Studio Code, you can debug the application running inside a container with the **Containers .NET Attach** option. This option attaches the debugger to a running container — no extra configuration required beyond starting the containers as shown above in the step 2 of [docker for development](#-docker-for-development).

---

### 📦 Docker for Production

To build and run the API in production mode using a minimal Docker image:

1. Update the `.env` file with the required following values:

```dotenv
DOCKER_BUILD_TARGET="prod"
BUILD_CONFIGURATION="Release"
ENVIRONMENT="Production"
```

2. Run the `docker-compose-debug.yml` file with docker again or build the images manually

```bash
docker buildx build \
  --platform linux/amd64 \
  -f src/Api/Evently.Api/Dockerfile \
  --target prod \
  -t evently.api:latest .
```

```bash
docker buildx build \
  --platform linux/amd64 \
  -f src/Api/Evently.Ticketing.Api/Dockerfile \
  --target prod \
  -t evently.ticketing.api:latest .
```

```bash
docker buildx build \
  --platform linux/amd64 \
  -f src/Api/Evently.Gateway/Dockerfile \
  --target prod \
  -t evently.gateway:latest .
```

## 🧪 Testing

This project includes unit, integration and architectural tests.

> [!NOTE]
> Tests are located in the root `/test` directory and the inside each module.

#### Testing Technologies:

- **xUnit**: Test framework.
- **NetArchTest.Rules**: Architectural tests.
- **Testcontainers**: Integration testing with ephemeral container instances.
- **Microsoft.AspNetCore.Mvc.Testing**: End-to-end and functional tests.

#### Running all tests

```bash
dotnet test
```

## 📘 Api Documentation

Evently API provides interactive documentation via Swagger with support for versioned endpoints and JWT authentication.

Once the APIs are running:

### Api

- **OpenAPI spec (JSON)**: `https://localhost:5001/openapi/v1.json`
- **Swagger UI**: `https://localhost:5001/swagger`

### Ticketing Api

- **OpenAPI spec (JSON)**: `https://localhost:5101/openapi/v1.json`
- **Swagger UI**: `https://localhost:5101/swagger`

> [!NOTE]
> Replace `5001` and `5101` with your actual HTTPS ports if different.
