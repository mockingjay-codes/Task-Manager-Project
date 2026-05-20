# HMCTS Task Manager

A task management system for caseworkers to create and manage tasks.

## Tech Stack
- **Backend:** ASP.NET Core Web API, Entity Framework Core, SQLite
- **Frontend:** Blazor Server (.NET 10)
- **Tests:** xUnit

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

Verify installation:
```
dotnet --version
```

## Running the Application

The backend API and frontend are separate projects. **Open two terminals and run both at the same time.**

**Terminal 1 — API**
```
cd TaskManager.Api
dotnet run
```
Runs at `http://localhost:5282`

**Terminal 2 — Frontend**
```
cd TaskManager
dotnet run
```
Runs at `http://localhost:5026` — browser opens automatically. If not, navigate there manually.

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tasks` | Get all tasks |
| GET | `/api/tasks/{id}` | Get task by ID |
| POST | `/api/tasks` | Create a task |
| PATCH | `/api/tasks/{id}/status` | Update task status |
| DELETE | `/api/tasks/{id}` | Delete a task |

## Running Tests

```
cd TaskManager.Tests
dotnet test
```

## Why Blazor?

Although HMCTS provided starter templates for a Java backend and a Node.js frontend, the brief allowed flexibility in choosing other technologies. I decided to take a different approach, using ASP.NET Core for the API and Blazor Server for the frontend—keeping the entire stack in C#.

While I have some experience with JavaScript, I had recently completed a course where I built a REST API integrated with a Blazor frontend, and I wanted to apply that knowledge in a real project. Working within the .NET ecosystem also allowed me to move quickly and maintain consistent patterns across the API, frontend, and tests without needing to switch between languages or toolchains.

## App Demo

### Empty State
When no tasks exist, the application displays an empty state prompt encouraging the user to create their first task.

![Empty State](screenshots/empty-state.png)

### Creating a Task
Navigate to the Create Task page to add a new task. The form accepts a title (required), an optional description, and an optional due date. The **Create** button remains disabled until a title has been entered, preventing empty submissions. A success message confirms the task was created, and the form resets automatically.

![Create Task](screenshots/create-task.png)

### Managing Tasks
All tasks are displayed in a table ordered by due date. From here, caseworkers can:

- **Update status** — use the dropdown to change a task between Pending, InProgress, Completed, and Cancelled
- **Delete a task** — permanently removes the task from the system

![Manage Tasks](screenshots/manage-tasks.png)