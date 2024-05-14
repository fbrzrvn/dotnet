FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/FreeStuff.Api/FreeStuff.Api.csproj", "src/FreeStuff.Api/"]
COPY ["src/FreeStuff.Contracts/FreeStuff.Contracts.csproj", "src/FreeStuff.Contracts/"]
COPY ["src/FreeStuff/FreeStuff.csproj", "src/FreeStuff/"]
RUN dotnet restore "src/FreeStuff.Api/FreeStuff.Api.csproj"
COPY . .
WORKDIR "/src/src/FreeStuff.Api"
RUN dotnet build "FreeStuff.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FreeStuff.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FreeStuff.Api.dll"]
