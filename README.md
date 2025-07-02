# Hotel Management App

Modern hotel management system built with .NET 8 and Blazor WebAssembly.

## Core Features

- **Reservations** - Room booking, parking, services
- **User Management** - Admin, Manager, Staff, Guest roles
- **Payments & Loyalty** - Payment processing, points system, VIP perks
- **Room Management** - Types, pricing, availability tracking
- **Discount system** - Ability to apply scoped discounts for certain period of time
- **Blacklist** - Blacklisting users
- **VIP list** - Adding users to VIP list for various benefits
- **Cities API** - Integration with public api for cities with population >1000
- **Weather API** - Integration with weather api for accessing information about weather through geographic coordinates
- **Mail system** - Integration with Azure Communication Services for sending various emails
- **PDF documents generation** - Generating bills in PDF format after successful reservation.

## Tech Stack

- .NET 8, ASP.NET Core API
- Blazor WebAssembly
- Entity Framework
- SQLite

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
