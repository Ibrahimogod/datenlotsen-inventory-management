FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY ["Datenlotsen.InventoryManagement.API/Datenlotsen.InventoryManagement.API.csproj", "Datenlotsen.InventoryManagement.API/"]
COPY ["Datenlotsen.InventoryManagement/Datenlotsen.InventoryManagement.csproj", "Datenlotsen.InventoryManagement/"]
COPY ["Datenlotsen.InventoryManagement.Data/Datenlotsen.InventoryManagement.Data.csproj", "Datenlotsen.InventoryManagement.Data/"]
COPY ["Datenlotsen.InventoryManagement.Shared.Data/Datenlotsen.InventoryManagement.Shared.Data.csproj", "Datenlotsen.InventoryManagement.Shared.Data/"]
RUN dotnet restore "Datenlotsen.InventoryManagement.API/Datenlotsen.InventoryManagement.API.csproj" --runtime linux-musl-x64

COPY . .
RUN dotnet publish "Datenlotsen.InventoryManagement.API/Datenlotsen.InventoryManagement.API.csproj" -c Release -o /app/publish --runtime linux-musl-x64 /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Datenlotsen.InventoryManagement.API.dll"]