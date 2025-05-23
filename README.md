# Datenlotsen Inventory Management

This repository contains a cross-platform Inventory Management system, including:
- **WPF Desktop Application** (Windows, .NET 8)
- **RESTful API** (ASP.NET Core, .NET 8, Docker-ready)

Automated GitHub Actions workflows are used for building, testing, and publishing releases.

---

## Table of Contents
- [Getting Started](#getting-started)
- [WPF Application](#wpf-application)
- [API Service](#api-service)
- [Docker Image](#docker-image)
- [Docker Compose](#docker-compose)
- [Automated Workflows](#automated-workflows)
- [Development](#development)

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- (For API) [Docker](https://www.docker.com/get-started) (optional, for containerized runs)
- (For WPF) Windows OS

---

## WPF Application

### Run from Source
1. Restore dependencies:dotnet restore Datenlotsen.WPF/Datenlotsen.WPF.csproj. Build and run:dotnet run --project Datenlotsen.WPF/Datenlotsen.WPF.csproj
### Install via MSI
- Download the latest `.msi` installer from the [GitHub Releases](https://github.com/Ibrahimogod/datenlotsen-inventory-management/releases) page.
- Run the installer and follow the prompts.

> The MSI is automatically built and published by the `release-wpf-msi.yml` workflow on new version tags.

---

## API Service

### Run Locally (without Docker)
1. Restore dependencies:dotnet restore Datenlotsen.InventoryManagement.API/Datenlotsen.InventoryManagement.API.csproj. Build and run:dotnet run --project Datenlotsen.InventoryManagement.API/Datenlotsen.InventoryManagement.API.csproj. The API will be available at `http://localhost:5155` by default. You may need to configure your connection string for PostgreSQL in your user secrets or environment variables.

### Run with Docker
1. Pull the latest image from GHCR:docker pull ghcr.io/ibrahimogod/datenlotsen-inventory-management-api:latest. Run the container:docker run -d -p 5155:8080 -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/ibrahimogod/datenlotsen-inventory-management-api:latest
> The Docker image is built and published by the `docker-publish.yml` workflow on pushes to `main` and on versioned releases.

---

## Docker Compose

A `docker-compose.yaml` file is provided to run the API and a PostgreSQL database together.

### Usage
1. Make sure Docker is installed and running.
2. Start all services:docker compose up --build   This will start:
   - The API service (on port 5155)
   - A PostgreSQL database (on port 5432)

3. The API will be available at `http://localhost:5155`.

---

## Automated Workflows

- **Build and Test**: On every push/PR to `main`, all projects are built and tested (`build-and-test.yml`).
- **WPF MSI Release**: On version tags or manual dispatch, builds the WPF app and publishes an MSI installer to GitHub Releases (`release-wpf-msi.yml`).
- **API Docker Publish**: On push to `main` or manual versioned release, builds and pushes the API Docker image to GHCR, and creates a release (`docker-publish.yml`).

---

## Development

- All projects target .NET 8.
- To run tests:dotnet test- To build all projects:dotnet build
---