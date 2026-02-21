![.NET](https://img.shields.io/badge/-.NET%2010.0-blueviolet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?logo=postgresql&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-ff463a?logo=redis&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-ff6600?logo=rabbitmq&logoColor=white)
![KeyCloak](https://img.shields.io/badge/KeyCloak-008aaa?logo=keycloak&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=fff)
![OpenAPI](https://img.shields.io/badge/-Swagger-85EA2D?style=flat&logo=swagger&logoColor=white)

# Evently API

## Table of Contents

<ol>
  <li>
    <a href="#-resumen">Resumen</a>
  </li>
  <li>
    <a href="#-tecnologías-utilizadas">Tecnologías Utilizadas</a>
  </li>
  <li>
    <a href="#-características">Características</a>
  </li>
  <li>
    <a href="#-primeros-pasos">Primeros Pasos</a>
    <ul>
      <li><a href="#-requisitos-previos">Requisitos Previos</a></li>
      <li><a href="#-instalación">Instalación</a></li>
    </ul>
  </li>
  <li>
    <a href="#-configuración-del-entorno">Configuración del Entorno</a>
    <ul>
      <li><a href="#-desarrollo-en-local">Desarrollo en Local</a></li>
      <li><a href="#-docker-para-desarrollo">Docker para Desarrollo</a></li>
      <li><a href="#-docker-para-producción">Docker para Producción</a></li>
    </ul>
  </li>
  <li>
    <a href="#-pruebas">Pruebas</a>
  </li>
  <li>
    <a href="#-documentación-de-la-api">Documentación de la API</a>
  </li>
</ol>

## 🎯 Resumen

Evently API es un sistema de gestión de eventos desarrollado con .NET 10 que utiliza una arquitectura monolítica modular y un diseño basado en eventos. Ofrece autenticación segura OpenID Connect mediante Keycloak, garantiza la fiabilidad de los mensajes mediante los patrones de Inbox/Outbox y proporciona información detallada con OpenTelemetry. La infraestructura está completamente contenedorizada con Docker, utilizando PostgreSQL para la persistencia, Redis para el almacenamiento en caché distribuido y RabbitMQ como intermediario de mensajes.

<div align="center">
  <img width="1011" height="784" alt="diagrama de la aplicación" src="https://github.com/user-attachments/assets/2a3374b7-b5e4-4ef7-bb34-0bd407602fdb" />
</div>

Este proyecto fue desarrollado siguiendo el curso [Modular Monolith](https://www.milanjovanovic.tech/modular-monolith-architecture) de [Milan Jovanović](https://github.com/m-jovanovic), usando las últimas características de ASP.NET Core y buenas prácticas recomendadas.

## 🔧 Tecnologías Utilizadas

- [.NET 10](https://dotnet.microsoft.com/) – Framework moderno para construir APIs web escalables
- [PostgreSQL](https://www.postgresql.org/) – Base de datos relacional de código abierto
- [Redis](https://redis.io/) – Almacenamiento de datos en memoria utilizado como caché distribuido
- [KeyCloak](https://www.keycloak.org/) – Gestión de identidad y acceso de código abierto
- [RabbitMQ](https://www.rabbitmq.com/) – Broker de mensajería maduro y confiable para la comunicación asíncrona entre microservicios
- [MassTransit](https://masstransit.io/) – Framework de aplicaciones distribuidas para .NET que simplifica el trabajo con buses de mensajes
- [Yarp](https://dotnet.github.io/yarp/) – Un conjunto de herramientas para construir servidores proxy HTTP de alto rendimiento utilizando la infraestructura de .NET
- [Dapper](https://www.learndapper.com/) – Micro-ORM de alto rendimiento para .NET utilizado para un acceso a datos eficiente basado en SQL puro
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) – ORM para el acceso a datos en .NET
- [Docker](https://www.docker.com/) – Contenerización e infraestructura local
- [Swagger (Swashbuckle)](https://swagger.io/) – Documentación interactiva de OpenAPI
- [FluentValidation](https://docs.fluentvalidation.net/) – Framework de validación de modelos
- [Quartz.NET](https://www.quartz-scheduler.net/) – Tareas en segundo plano y programación de horarios (scheduling)
- [OpenTelemetry](https://opentelemetry.io/) – Trazabilidad distribuida y observabilidad
- [Seq](https://datalust.co/seq) – Agregación de registros (logs) centralizada
- [Jaeger](https://www.jaegertracing.io/) – Sistema de trazado distribuido de extremo a extremo de código abierto
- [xUnit](https://xunit.net/) – Framework de pruebas unitarias
- [Testcontainers](https://dotnet.testcontainers.org/) – Pruebas de integración con contenedores
- [SonarAnalyzer](https://rules.sonarsource.com/csharp) – Análisis estático de código

## ✨ Características

#### ⚙️ Infraestructura e Integración
- PostgreSQL con EF Core, Dapper y convenciones de nomenclatura snake_case
- Tareas en segundo plano (background jobs) con Quartz
- Redis como caché distribuido y almacenamiento de clave-valor de alto rendimiento
- RabbitMQ para mensajería asíncrona y comunicación entre servicios
- YARP (Yet Another Reverse Proxy) para enrutamiento y funcionalidad de API Gateway
- Documentación interactiva de API con Swagger

#### 🔐 Seguridad
- Integración de proveedor de identidad OpenID Connect (OIDC) con Keycloak
- Identidad basada en Claims transformada a partir de los scopes del perfil OIDC
- Control de Acceso Basado en Roles (RBAC) mediante políticas y roles personalizados

#### 📈 Observabilidad
- Trazado distribuido basado en OpenTelemetry
- Soporte para verificaciones de salud (Health checks)
- Registro de eventos (Logging) con Seq
- Trazado distribuido con Jaeger

#### 🧪 Stack de Pruebas
- Pruebas unitarias con xUnit
- Pruebas de reglas de arquitectura con NetArchTest
- Pruebas de integración con Testcontainers, PostgreSQL, Redis, Keycloak y RabbitMQ

#### 📦 Despliegue y DevOps
- Soporte para desarrollo local con Docker Compose
- Versionado de paquetes centralizado con MSBuild
- Soporte para imágenes de producción dockerizadas

## 🚀 Primeros Pasos

### 📋 Requisitos Previos

Asegúrate de tener instalado el CLI de .NET en tu sistema. Puedes verificarlo ejecutando:

```bash
dotnet --version
```

Esto debería mostrar la versión instalada del CLI de .NET. Si no está instalado, descárgelo desde la [pagina oficial de .NET](https://dotnet.microsoft.com/download).

Para verificar qué versiones del SDK están instalados:

```bash
dotnet --list-sdks
```

> [!IMPORTANT]
> La versión mínima requerida del SDK de .NET es **10.0.0**

Adicionalmente, el proyecto utiliza Docker para ejecutar servicios de soporte (por ejemplo, PostgreSQL, Redis, RabbitMQ, Seq). Necesitará:

- [**Docker**](https://www.docker.com/products/docker-desktop): Se recomienda instalar Docker Desktop.
- [**Docker  Compose**](https://docs.docker.com/compose/): Usualmente incluido con Docker Desktop.

Para comprobar que Docker está instalado y en funcionamiento:

```bash
docker --version
docker compose version
```

Si estos comandos fallan o arrojan errores, consulte la [guía de instalación de Docker](https://docs.docker.com/get-docker/).

---

### 📥 Instalación

Para comenzar, clona el repositorio y configura el entorno:

1. Clone el repositorio:

```bash
git clone https://github.com/jaimejaramilloperez/evently.git
```

2. Navege al directorio del proyecto:

```bash
cd evently
```

3. Genere y confíe en el certificado de desarrollo HTTPS:

```bash
dotnet dev-certs https -ep ./src/Evently.Api/certificate.pfx -p Test1234!
dotnet dev-certs https --trust
```

4. Copie la plantilla de entorno y configúrelo:

```bash
cp .env.template .env
# Edit the .env file as needed
```

Después de la instalación, estarás listo para ejecutar la aplicación localmente o usando Docker. Consulta las secciones de [Desarrollo en local](#-desarrollo-en-local) o [Docker](#-docker-para-desarrollo) para más detalles.

## 💻 Configuración del Entorno

Configura tu entorno para ejecutar la API de Evently ya sea localmente o con Docker, según tu flujo de trabajo.

> [!NOTE]
> Los valores de configuración mostrados (por ejemplo, contraseñas, puertos, claves, cadenas de conexión) se proporcionan solo con fines demostrativos. Puede modificarlos según sea necesario — especialmente para entornos de producción.

> [!TIP]
> Si la API se ejecuta dentro de un contenedor Docker, `localhost` hace referencia al propio contenedor. En ese caso, reemplace `localhost` con el nombre del servicio definido en la red de Docker (por ejemplo, `evently.seq` o `evently.jaeger`).

---

### 🧑‍💻 Desarrollo en Local

Puede ejecutar la API localmente utilizando la CLI de .NET, mientras los servicios de soporte (PostgreSQL, Redis, RabbitMQ, Seq, etc.) se ejecutan a través de Docker Compose.

> [!IMPORTANT]
> Los valores sensibles deben almacenarse de forma segura mediante [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=linux)

1. Revise y actualice los siguientes archivos de configuración según sea necesario:

- [Evently.Api - appsettings.Development.json](./src/Api/Evently.Api/appsettings.Development.json)
- [Evently.Ticketing.Api - appsettings.Development.json](./src/Api/Evently.Ticketing.Api/appsettings.Development.json)
- [Evently.Gateway - appsettings.Development.json](./src/Api/Evently.Gateway/appsettings.Development.json)
- [Evently.Api - modules.attendance.Development.json](./src/Api/Evently.Api/modules.attendance.Development.json)
- [Evently.Api - modules.events.Development.json](./src/Api/Evently.Api/modules.events.Development.json)
- [Evently.Api - modules.users.Development.json](./src/Api/Evently.Api/modules.users.Development.json)
- [Evently.Ticketing.Api - modules.ticketing.Development.json](./src/Api/Evently.Ticketing.Api/modules.ticketing.Development.json)

2. Configure las variables de entorno (archivo `.env`):

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

3. Inicie los servicios de Docker:

```bash
docker compose up -d
```

4. Ejecute las APIs:

```bash
dotnet run --project src/Api/Evently.Api
dotnet run --project src/Api/Evently.Ticketing.Api
dotnet run --project src/Api/Evently.Gateway
# o con HTTPS
dotnet run --launch-profile https --project src/Api/Evently.Api
dotnet run --launch-profile https --project src/Api/Evently.Ticketing.Api
dotnet run --launch-profile https --project src/Api/Evently.Gateway
```

---

### 🐳 Docker para Desarrollo

Este modo ejecuta tanto la aplicación como los servicios en contenedores Docker utilizando una imagen de desarrollo.

1. Revise y actualice el archivo `.env`:

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

2. Inicie los contenedores:

```bash
docker compose -f ./docker-compose-debug.yml up -d
```

#### 🐞Depuración en el contenedor

> [!IMPORTANT]
> La depuración dentro de contenedores requiere el depurador `vsdbg`. Si aún no está instalado, consulte la [guía oficial de configuración](https://github.com/microsoft/MIEngine/wiki/Offroad-Debugging-of-.NET-Core-on-Linux---OSX-from-Visual-Studio) para instrucciones sobre cómo instalarlo manualmente.

Si está utilizando Visual Studio Code, puede depurar la aplicación que se ejecuta dentro del contenedor con la opción **Containers .NET Attach**. Esta opción más sencilla permite adjuntar el depurador a un contenedor en ejecución, sin necesidad de configuración adicional más allá de iniciar los contenedores como se describe en el paso 2 de la sección [docker para desarrollo](#-docker-para-desarrollo).

---

### 📦 Docker para Producción

Para compilar y ejecutar la API en modo producción utilizando una imagen Docker mínima:

1. Actualice el archivo `.env` con los siguientes valores requeridos:

```dotenv
DOCKER_BUILD_TARGET="prod"
BUILD_CONFIGURATION="Release"
ENVIRONMENT="Production"
```

2. Ejecute el archivo `docker-compose-debug.yml` con docker de nuevo o construya las imágenes manualmente

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

Este proyecto incluye pruebas unitarias, de integración y de arquitectura

> [!NOTE]
> Las pruebas se encuentran en el directorio raíz `test/` y dentro de cada modulo.

#### Tecnologías utilizadas:

- **xUnit**: Framework de pruebas.
- **NetArchTest.Rules**: Pruebas de arquitectura.
- **Testcontainers**: Pruebas de integración con instancias efímeras de contenedores.
- **Microsoft.AspNetCore.Mvc.Testing**: Pruebas funcionales de extremo a extremo.

#### Ejecutar todas las pruebas

```bash
dotnet test
```

## 📘 Documentación de la API

Evently API ofrece documentación interactiva a través de Swagger con soporte para endpoints versionados y autenticación JWT.

Una vez las APIs estén en ejecución:

### Api

- **OpenAPI spec (JSON)**: `https://localhost:5001/openapi/v1.json`
- **Swagger UI**: `https://localhost:5001/swagger`

### Ticketing Api

- **OpenAPI spec (JSON)**: `https://localhost:5101/openapi/v1.json`
- **Swagger UI**: `https://localhost:5101/swagger`

> [!NOTE]
> Reemplace `5001` y `5101` con los puertos HTTPS correspondientes si utiliza unos distintos.
