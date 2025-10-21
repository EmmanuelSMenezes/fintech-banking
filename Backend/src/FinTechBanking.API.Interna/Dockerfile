# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/FinTechBanking.API.Interna/FinTechBanking.API.Interna.csproj", "src/FinTechBanking.API.Interna/"]
COPY ["src/FinTechBanking.Core/FinTechBanking.Core.csproj", "src/FinTechBanking.Core/"]
COPY ["src/FinTechBanking.Data/FinTechBanking.Data.csproj", "src/FinTechBanking.Data/"]
COPY ["src/FinTechBanking.Services/FinTechBanking.Services.csproj", "src/FinTechBanking.Services/"]
COPY ["src/FinTechBanking.Banking/FinTechBanking.Banking.csproj", "src/FinTechBanking.Banking/"]

# Restore dependencies
RUN dotnet restore "src/FinTechBanking.API.Interna/FinTechBanking.API.Interna.csproj"

# Copy all source code
COPY . .

# Build
WORKDIR "/src/src/FinTechBanking.API.Interna"
RUN dotnet build "FinTechBanking.API.Interna.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FinTechBanking.API.Interna.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "FinTechBanking.API.Interna.dll"]

