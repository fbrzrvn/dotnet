FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Identity/src/Identity.Api/Identity.Api.csproj", "Identity/src/Identity.Api/"]
COPY ["Identity/src/Identity/Identity.csproj", "Identity/src/Identity/"]
COPY ["Shared/src/Shared/Shared.csproj", "Shared/src/Shared/"]
RUN dotnet restore "Identity/src/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/Identity/src/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]
