FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FreeStuff.Identity.Api/FreeStuff.Identity.Api.csproj", "src/FreeStuff.Identity.Api/"]
COPY ["src/FreeStuff.Contracts/FreeStuff.Contracts.csproj", "src/FreeStuff.Contracts/"]
RUN dotnet restore "src/FreeStuff.Identity.Api/FreeStuff.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/FreeStuff.Identity.Api"
RUN dotnet build "FreeStuff.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FreeStuff.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FreeStuff.Identity.Api.dll"]
