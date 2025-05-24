# Datenlotsen Inventory Management

[![Build and Test](https://github.com/Ibrahimogod/datenlotsen-inventory-management/actions/workflows/ci.yml/badge.svg)](https://github.com/Ibrahimogod/datenlotsen-inventory-management/actions/workflows/ci.yml)

This repository contains a cross-platform Inventory Management system:
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

### Download and Run (Recommended)
1. Go to the [GitHub Releases](https://github.com/Ibrahimogod/datenlotsen-inventory-management/releases) page.
2. Download the latest `Datenlotsen.zip` file under the most recent release.
3. Extract the ZIP file to a folder of your choice.
4. Run `Datenlotsen.WPF.exe` from the extracted files.

### Run from Source
1. Restore dependencies:dotnet restore Datenlotsen.WPF/Datenlotsen.WPF.csproj. Build and run:dotnet run --project Datenlotsen.WPF/Datenlotsen.WPF.csproj
---

## API Service

### Run Locally (without Docker)
1. Restore dependencies:dotnet restore Datenlotsen.InventoryManagement.API/Datenlotsen.InventoryManagement.API.csproj. Build and run:dotnet run --project Datenlotsen.InventoryManagement.API/Datenlotsen.InventoryManagement.API.csproj   The API will be available at `http://localhost:5155` by default. You may need to configure your connection string for PostgreSQL in your user secrets or environment variables.

### Run with Docker
1. Pull the latest image from GHCR:docker pull ghcr.io/ibrahimogod/datenlotsen-inventory-management-api:latest. Run the container:docker run -d -p 5155:8080 -e ASPNETCORE_ENVIRONMENT=Production ghcr.io/ibrahimogod/datenlotsen-inventory-management-api:latest The Docker image is built and published by the `cd.yml` workflow on pushes to `main` with every new version tag (e.g., `v1.0.0`).

---

## Docker Compose

A `docker-compose.yaml` file is provided to run the API and a PostgreSQL database together. This is the recommended way to run the full stack locally for development or testing.

### Usage
1. Make sure Docker is installed and running.
2. Start all services:docker compose up --build   This will start:
   - The API service (on port 5155)
   - A PostgreSQL database (on port 5432)

3. The API will be available at `http://localhost:5155`.

---

## Automated Workflows

- **Build and Test**: On every push/PR to `main`, all projects are built and tested (`ci.yml`).
- **API Docker and WPF App Release**: On every new version tag (e.g., `v1.0.0`) or manual dispatch, builds the WPF app and publishes a ZIP file and Docker Image as a GitHub Release (`cd.yml`).

---

## Development

- All projects target .NET 8.
- To run tests:dotnet test- To build all projects:dotnet build
---