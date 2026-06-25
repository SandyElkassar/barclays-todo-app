# Barclays TODO Application

A small full-stack TODO application built with React + TypeScript and ASP.NET Core.

## Features

- List existing tasks
- Add tasks
- Edit tasks
- Delete completed tasks only
- Client-side and server-side validation
- In-memory server-side storage
- Backend tests with xUnit
- Frontend tests with Vitest

## Validation Rules

- Every task must have a name
- Task names must be unique, case-insensitive
- Every task has a priority
- Priority must be a non-negative number
- Every task has a status:
  - Not started
  - In progress
  - Completed
- Only completed tasks can be deleted

## Tech Stack

### Backend

- ASP.NET Core
- Minimal APIs
- xUnit
- In-memory repository
- No Entity Framework

### Frontend

- React
- TypeScript
- Vite
- Vitest
- React Testing Library

## Running the Backend

From the root folder:

```bash
cd backend
dotnet restore
dotnet build
dotnet test
dotnet run --project BarclaysTodo.Api

The API runs on the port shown in the terminal.

Example:

http://localhost:5000
Running the Frontend

In a second terminal:

cd frontend/barclays-todo-ui
npm install
npm run dev

Open:

http://localhost:5173

If the backend runs on a different port, update the API URL in:

frontend/barclays-todo-ui/src/features/todos/todoApi.ts
Running Frontend Tests
cd frontend/barclays-todo-ui
npm run test:run
Design Notes

The backend uses ASP.NET Core Minimal APIs because the API surface is small.

Business logic, validation, and in-memory storage are separated into services to keep the code testable and easy to extend.

The in-memory repository is registered as a singleton so data is kept while the API is running. The repository protects the in-memory collection with locking.

Validation is implemented on both client and server. Server-side validation is the source of truth.

The validator is separated from the service because business may ask for more validation rules in the future.

The frontend is organized by feature because the application currently has one main feature: TODO management. If the application grew, shared API clients, reusable components, and common types could be extracted into shared folders.

Assumptions
In-memory data is reset when the backend application restarts.
Task name uniqueness is case-insensitive.
Priority must be zero or a positive number.
Delete is allowed only when the task status is completed.

Then commit and push:

```bash
git add README.md
git commit -m "Add README"
git push