# HabitTracker

A modern full-stack habit tracking web application built with .NET 9, Entity Framework Core, and JWT authentication. Features a clean REST API with Swagger documentation for comprehensive habit management.

## 🚀 Features

- **🔐 Secure Authentication** - JWT-based authentication with ASP.NET Core Identity
- **📝 Habit Management** - Create, update, delete, and track daily habits
- **🔌 RESTful API** - Clean, documented API endpoints with Swagger integration
- **💾 Data Persistence** - SQL Server database with Entity Framework Core
- **🌐 Cross-Platform** - Runs on Windows, macOS, and Linux
- **📊 Progress Tracking** - Monitor habit completion and view statistics

## 🛠️ Tech Stack

**Backend**
- .NET 9 / ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger/OpenAPI

**Frontend**
- React - Typescript
- HTML/CSS

## 📋 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express/LocalDB)
- [Node.js](https://nodejs.org/) (for frontend client)

## ⚡ Quick Start

### 1. Clone the Repository
```
git clone https://github.com/UnderKitten/HabitTracker.git
cd HabitTracker
```
### 2. Backend Setup
#### Configure Database Connection
```
{
"ConnectionStrings": {
"DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=HabitTrackerDb;Trusted_Connection=true;"
},
"JwtKey": "your-super-secret-jwt-key-here"
}
```
#### Install Dependencies & Run Migrations
```
cd HabitTracker
dotnet restore
dotnet ef database update
```
The API will be available at:
- **HTTPS**: `https://localhost:7113`
- **HTTP**: `http://localhost:5113`
- **Swagger UI**: `https://localhost:7113/swagger`
### 3. Frontend Client Setup (In progress)
```
cd habit-tracker-client
npm install
npm start
```
## 🔧 Development

### Database Migrations
```
Create new migration
dotnet ef migrations add MigrationName

Update database
dotnet ef database update

Remove last migration
dotnet ef migrations remove
```
### API Testing
1. Navigate to `https://localhost:7113/swagger`
2. Register a new user via `/auth/register`
3. Login via `/auth/login` to get JWT token
4. Click **Authorize** button and enter your token
5. Test habit endpoints

### Common Commands
```
Clean and rebuild
dotnet clean
dotnet build

Run with hot reload
dotnet watch run

Clear NuGet cache (if package issues)
dotnet nuget locals all --clear
```

## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/auth/register` | User registration |
| POST | `/auth/login` | User login |
| GET | `/habits` | Get user habits |
| POST | `/habits` | Create new habit |
| PUT | `/habits/{id}` | Update habit |
| DELETE | `/habits/{id}` | Delete habit |

## 🔐 Authentication

The API uses JWT Bearer token authentication:

1. **Register**: Create account via `/auth/register`
2. **Login**: Get JWT token via `/auth/login`
3. **Authorize**: Include token in requests:
```
Authorization: Bearer <your-jwt-token>
```
## 📂 Project Structure
```
HabitTracker/
├── Controllers/ # API controllers
├── Models/ # Data models
├── Data/ # Database context
├── Endpoints/ # Minimal API endpoints
├── Migrations/ # EF Core migrations
├── appsettings.json # Configuration
└── Program.cs # Application startup

habit-tracker-client/
├── src/ # Frontend source code
├── public/ # Static assets
└── package.json # Node dependencies
```
