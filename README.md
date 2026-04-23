# Employee Management System

This is an ASP.NET Core MVC project built with .NET 8 and Entity Framework Core.

The project can be used with either of these database setups:

- XAMPP with MySQL or MariaDB
- SQL Server

Right now, the project is configured to use MySQL in `EmployeeManagementSystem/Program.cs`.

## Requirements

- .NET 8 SDK
- Entity Framework Core CLI tool: `dotnet-ef`
- One database option:
  - XAMPP with MySQL or MariaDB
  - SQL Server / SQL Server Express / LocalDB

## Project Structure

- `EmployeeManagementSystem.sln` - solution file
- `EmployeeManagementSystem/` - ASP.NET Core MVC web app
- `Employeedb.sql` - sample SQL export for the employee table

## First-Time Setup

Open PowerShell in the project root:

```powershell
cd "c:\Users\Fahad\3D Objects\crud\employee"
```

Restore and build:

```powershell
dotnet restore
dotnet build
```

If `dotnet ef` is not installed, install it once:

```powershell
dotnet tool install --global dotnet-ef --version 8.*
```

If it is already installed but outdated:

```powershell
dotnet tool update --global dotnet-ef --version 8.*
```

## Important Migration Note

Use one database provider at a time.

If you create migrations for MySQL, keep using MySQL for that migration set.
If you later switch to SQL Server, remove the old provider-specific migrations first and then create a fresh initial migration for SQL Server.

In short:

- MySQL migrations should be created while `UseMySql(...)` is active
- SQL Server migrations should be created while `UseSqlServer(...)` is active

## Connection Strings

The current `EmployeeManagementSystem/appsettings.json` contains:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=Employeedb;user=root;password=",
  "SqlServerConnection": "Server=.; Database=EmployeeDB; User Id=sa; Password=aptech; TrustServerCertificate=True"
}
```

You can update these values to match your machine before running migrations.

## Option 1: XAMPP / MySQL Setup With Migrations

### 1. Start XAMPP

Open XAMPP Control Panel and start:

- `Apache`
- `MySQL`

### 2. Create the database

Open `http://localhost/phpmyadmin` and create a database named:

```text
Employeedb
```

### 3. Keep MySQL enabled in `Program.cs`

For XAMPP/MySQL, `EmployeeManagementSystem/Program.cs` should use:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
```

### 4. Create the migration

Run this if you are creating the database from the model for the first time:

```powershell
dotnet ef migrations add InitialCreate `
  --project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --startup-project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --context ApplicatonDbContext
```

If you already have a valid MySQL migration, you do not need to create it again.

### 5. Apply the migration

```powershell
dotnet ef database update `
  --project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --startup-project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --context ApplicatonDbContext
```

### 6. Run the app

```powershell
dotnet run --project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" --launch-profile http
```

Open:

```text
http://localhost:5232/Employees
```

## Option 2: SQL Server Setup With Migrations

### 1. Make sure SQL Server is available

Use one of these:

- SQL Server
- SQL Server Express
- LocalDB

Create a database named `EmployeeDB`, or let EF create it during `database update`.

### 2. Switch `Program.cs` to SQL Server

Change `EmployeeManagementSystem/Program.cs` to use the SQL Server connection string:

```csharp
var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
options.UseSqlServer(connectionString);
```

That means replacing the current MySQL line for the SQL Server setup.

### 3. Update the SQL Server connection string if needed

Example from `appsettings.json`:

```json
"SqlServerConnection": "Server=.; Database=EmployeeDB; User Id=sa; Password=aptech; TrustServerCertificate=True"
```

Change server name, username, or password to match your SQL Server instance.

### 4. Create the migration

Run this when creating the SQL Server database from the model:

```powershell
dotnet ef migrations add InitialCreate `
  --project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --startup-project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --context ApplicatonDbContext
```

If you previously created MySQL migrations, remove them first before creating the SQL Server initial migration.

### 5. Apply the migration

```powershell
dotnet ef database update `
  --project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --startup-project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --context ApplicatonDbContext
```

### 6. Run the app

```powershell
dotnet run --project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" --launch-profile http
```

Open:

```text
http://localhost:5232/Employees
```

## When The Model Changes

After you add or change properties in the model, create a new migration and apply it:

```powershell
dotnet ef migrations add YourMigrationName `
  --project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --startup-project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --context ApplicatonDbContext

dotnet ef database update `
  --project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --startup-project ".\EmployeeManagementSystem\EmployeeManagementSystem.csproj" `
  --context ApplicatonDbContext
```

## Run In Visual Studio

1. Open `EmployeeManagementSystem.sln`
2. Set `EmployeeManagementSystem` as the startup project
3. Press `F5` or click `Run`

## Notes

- The current code defaults to MySQL unless you switch `Program.cs` to SQL Server.
- If `dotnet ef` says it cannot be found, install `dotnet-ef` first.
- If MySQL fails to connect, check that XAMPP MySQL is running and the connection string is correct.
- If SQL Server fails to connect, check the server name, login, password, and whether the SQL Server service is running.
