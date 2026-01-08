FROM mcr.microsoft.com/dotnet/aspnet:10.0.1-alpine3.22@sha256:1be14b20e4ec3dad26aa4932ae16c2193cc990c5499239492e9b697f0cba4e04 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0.101-alpine3.22@sha256:0fba99926e4f12405c78f37941312f00c1aadb178bd63616f5d96fc2af5a26a9 AS restore
WORKDIR /src
COPY ["nuget.config", "global.json", "Directory.Build.props", "Directory.Packages.props", "Evently.slnx", "./"]
COPY ["src/Api/Evently.Api/Evently.Api.csproj", "./src/Api/Evently.Api/"]
COPY ["src/Modules/Events/Evently.Modules.Events.Api/Evently.Modules.Events.Api.csproj", "./src/Modules/Events/Evently.Modules.Events.Api/"]
RUN dotnet restore "./Evently.slnx"

FROM restore AS build
ARG BUILD_CONFIGURATION=Release
COPY . .
RUN dotnet build "./Evently.slnx" \
  -c $BUILD_CONFIGURATION \
  --no-restore \
  --nologo \
  -p:OutputPath=/app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Evently.slnx" \
  -c $BUILD_CONFIGURATION \
  --no-restore \
  --nologo \
  -p:PublishDir=/app/publish \
  -p:UseAppHost=false \
  -p:DebugType=None \
  -p:DebugSymbols=false

FROM base AS prod
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE $ASPNETCORE_HTTP_PORTS
COPY --from=publish /app/publish .
RUN rm /app/publish/appsettings.Development.json
HEALTHCHECK --interval=30s --timeout=10s --start-period=30s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:$ASPNETCORE_HTTP_PORTS/health || exit 1
USER app
ENTRYPOINT ["dotnet", "Evently.Api.dll"]


FROM base AS dev
ENV ASPNETCORE_HTTP_PORTS=5000
ENV ASPNETCORE_HTTPS_PORTS=5001
EXPOSE $ASPNETCORE_HTTP_PORTS
EXPOSE $ASPNETCORE_HTTPS_PORTS
COPY --from=build /app/build .
HEALTHCHECK --interval=10s --timeout=5s --start-period=15s --retries=3 \
  CMD wget --no-verbose --tries=1 --no-check-certificate --spider https://localhost:$ASPNETCORE_HTTPS_PORTS/health || \
  wget --no-verbose --tries=1 --spider http://localhost:$ASPNETCORE_HTTP_PORTS/health || \
  exit 1
COPY ["src/Api/Evently.Api/certificate.pfx", "/https/certificate.pfx"]
RUN chmod 644 /https/certificate.pfx
RUN apk update && apk add curl && curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v vs2019 -l /remote_debugger
USER app
ENTRYPOINT ["dotnet", "Evently.Api.dll"]
