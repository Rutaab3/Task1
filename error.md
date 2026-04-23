# EmployeeManagementSystem Error Notes

## What was broken

When I tried to build the project on April 23, 2026, the first blocking error was:

```text
CS0246: The type or namespace name 'ApplicatonDbContext' could not be found
```

It came from the migration files because the project had been renamed from `StudentManagementSystem` to `EmployeeManagementSystem`, but several files were still pointing to the old namespace.

There was also an older runtime problem recorded in `EmployeeManagementSystem/watch.out.log`. That error looked like this:

```text
MySqlConnector.MySqlException: Unknown column 's.StudentId' in 'field list'
```

Similar errors also appeared for `ClassName` and `Rollnumber`.

After the build was fixed, the next startup error was:

```text
The entity type 'Employee' requires a primary key to be defined.
```

During the HTTP smoke test, there was one more runtime failure:

```text
Cannot open log for source '.NET Runtime'. You may not have write access.
```

## Why it happened

Two different versions of the app were mixed together:

- The current code model is for employees:
  `EmpId`, `Name`, `EmpNo`, `CabinName`, `Age`
- Some startup, view, and migration files still expected the older student model:
  `StudentId`, `Name`, `Rollnumber`, `ClassName`, `Age`

Because of that mix:

- The build failed when old namespace references could not find the current DbContext.
- The old runtime tried to query the `Students` table in `studentdb`.
- Your current SQL file and model use the `Employees` table in `Employeedb`.
- EF Core also could not infer a key from `EmpId` automatically, because its built-in convention prefers names like `Id` or `EmployeeId`.
- The app was also trying to use Windows Event Log logging in an environment where that log source was not writable.

## What I changed

- Renamed the remaining `StudentManagementSystem` references to `EmployeeManagementSystem`.
- Updated the home page and navbar links from `Students` to `Employees`.
- Changed the default MySQL database in `EmployeeManagementSystem/appsettings.json` from `studentdb` to `Employeedb`.
- Updated the old migration metadata so it matches the current employee model instead of the removed student model.
- Marked `EmpId` with `[Key]` so Entity Framework knows it is the primary key.
- Switched logging to console/debug providers so HTTP requests no longer fail because of Windows Event Log permissions.
- Fixed the README so the run instructions now match this project.

## How to think about this simply

Entity Framework expects your C# model and your database table columns to describe the same shape.

This now matches:

- C# model: `EmpId`, `EmpNo`, `CabinName`
- MySQL table: `EmpId`, `EmpNo`, `CabinName`

This old version did not match:

- C# query expected: `StudentId`, `Rollnumber`, `ClassName`
- MySQL table actually had: `EmpId`, `EmpNo`, `CabinName`

When those names do not match, MySQL throws an "Unknown column" error.

## How to run it

From the project root:

```powershell
dotnet build .\EmployeeManagementSystem\EmployeeManagementSystem.csproj
dotnet run --project .\EmployeeManagementSystem\EmployeeManagementSystem.csproj --launch-profile http
```

Then open:

```text
http://localhost:5232
```

## If it still fails on your machine

The next thing to check is MySQL itself:

- Make sure XAMPP MySQL is running.
- Make sure the database name is `Employeedb`.
- If needed, import `Employeedb.sql`.
- If your `root` user has a password, add it in `EmployeeManagementSystem/appsettings.json`.
