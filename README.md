# HMCTS Task Manager

A task management system for caseworkers to create and manage tasks.

## Tech Stack
- **Backend:** ASP.NET Core Web API, Entity Framework Core, SQLite
- **Frontend:** Blazor Server (.NET 10)
- **Tests:** xUnit

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
check if it's installed:
dotnet --version


Running the Application
The backend API and frontend are separate projects. Open 2 separate terminals to run both at the same time.

Terminal 1 — Start the API

cd TaskManager.Api
dotnet run
API will be available at http://localhost:5282

Terminal 2 — Start the Frontend

cd TaskManager
dotnet run
Frontend will be available at http://localhost:5026 — your browser should open automatically. If it does not, visit http://localhost:5026.

API Endpoints
GET	/api/tasks
GET	/api/tasks/{id}
POST	/api/tasks
PATCH	/api/tasks/{id}/status
DELETE	/api/tasks/{id}	

Running Tests

Open another terminal
cd TaskManager.Tests
dotnet test