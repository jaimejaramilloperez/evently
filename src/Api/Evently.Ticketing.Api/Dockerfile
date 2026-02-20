FROM mcr.microsoft.com/dotnet/aspnet:10.0.3-alpine3.22@sha256:e0fd89188698106364ea47cd6bbedc1fdea99363766932a1f4c256c87631ec53 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0.3-alpine3.22@sha256:e0fd89188698106364ea47cd6bbedc1fdea99363766932a1f4c256c87631ec53 AS base-dev
WORKDIR /app
COPY ["src/Api/Evently.Ticketing.Api/certificate.pfx", "/https/certificate.pfx"]
RUN chmod 644 /https/certificate.pfx
RUN apk update && apk add curl && curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v vs2019 -l /remote_debugger


FROM mcr.microsoft.com/dotnet/sdk:10.0.103-alpine3.22@sha256:bb9890f2f2dbefa91b161feecfe6f85fd4c8c9e3933158c54287f37f611d5ed5 AS restore
WORKDIR /src
COPY ["nuget.config", "global.json", "Directory.Build.props", "Directory.Packages.props", "Evently.slnx", "./"]
COPY --parents ["src/**/*.csproj", "./"]
RUN dotnet restore "./src/Api/Evently.Ticketing.Api/Evently.Ticketing.Api.csproj"

FROM restore AS build
ARG BUILD_CONFIGURATION=Release
COPY . .
RUN dotnet build "./src/Api/Evently.Ticketing.Api/Evently.Ticketing.Api.csproj" \
  -c $BUILD_CONFIGURATION \
  --no-restore \
  --nologo \
  -p:OutputPath=/app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./src/Api/Evently.Ticketing.Api/Evently.Ticketing.Api.csproj" \
  -c $BUILD_CONFIGURATION \
  --no-restore \
  --no-build \
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
ENTRYPOINT ["dotnet", "Evently.Ticketing.Api.dll"]


FROM base-dev AS dev
ENV ASPNETCORE_HTTP_PORTS=5000
ENV ASPNETCORE_HTTPS_PORTS=5001
EXPOSE $ASPNETCORE_HTTP_PORTS
EXPOSE $ASPNETCORE_HTTPS_PORTS
COPY --from=build /app/build .
HEALTHCHECK --interval=10s --timeout=5s --start-period=15s --retries=3 \
  CMD wget --no-verbose --tries=1 --no-check-certificate --spider https://localhost:$ASPNETCORE_HTTPS_PORTS/health || \
  wget --no-verbose --tries=1 --spider http://localhost:$ASPNETCORE_HTTP_PORTS/health || \
  exit 1
USER app
ENTRYPOINT ["dotnet", "Evently.Ticketing.Api.dll"]
