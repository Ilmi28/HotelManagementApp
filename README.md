# Hotel Management App

Modern hotel management system built with .NET 8 and Blazor WebAssembly.

## Core Features

- **Reservations** - Room booking, parking, services
- **User Management** - Admin, Manager, Staff, Guest roles
- **Payments & Loyalty** - Payment processing, points system, VIP perks
- **Room Management** - Types, pricing, availability tracking

## Tech Stack

- .NET 8, ASP.NET Core API
- Blazor WebAssembly
- Entity Framework
- SQL Server/SQLite

## Quick Start

```bash
git clone https://github.com/Ilmi28/HotelManagementApp.git
cd HotelManagementApp
dotnet restore
dotnet run --project src/HotelManagementApp.API
```

## Structure
```
src/
├── API          # Backend API
├── Blazor       # Frontend UI
├── Core         # Domain models
├── Application  # Business logic
└── Infrastructure # External dependencies layer
```
