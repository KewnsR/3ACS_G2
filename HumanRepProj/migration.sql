IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Employees] (
    [EmployeeID] int NOT NULL IDENTITY,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [Email] nvarchar(255) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [DepartmentID] int NOT NULL,
    [Position] nvarchar(100) NOT NULL,
    [Salary] decimal(18,2) NOT NULL,
    [DateHired] datetime2 NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([EmployeeID])
);

CREATE TABLE [Logins] (
    [LoginID] int NOT NULL IDENTITY,
    [EmployeeID] int NOT NULL,
    [Username] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [LastLogin] datetime2 NULL,
    [FailedAttempts] int NOT NULL,
    [IsLocked] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Logins] PRIMARY KEY ([LoginID]),
    CONSTRAINT [FK_Logins_Employees_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employees] ([EmployeeID]) ON DELETE CASCADE
);

CREATE INDEX [IX_Logins_EmployeeID] ON [Logins] ([EmployeeID]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250416014728_AddDepartmentAndEmployeeRelationships', N'9.0.4');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logins]') AND [c].[name] = N'Username');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Logins] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Logins] ALTER COLUMN [Username] nvarchar(50) NOT NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logins]') AND [c].[name] = N'UpdatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Logins] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Logins] ADD DEFAULT (GETDATE()) FOR [UpdatedAt];

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logins]') AND [c].[name] = N'Password');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Logins] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Logins] ALTER COLUMN [Password] nvarchar(255) NOT NULL;

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logins]') AND [c].[name] = N'IsLocked');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Logins] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Logins] ADD DEFAULT CAST(0 AS bit) FOR [IsLocked];

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logins]') AND [c].[name] = N'FailedAttempts');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Logins] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Logins] ADD DEFAULT 0 FOR [FailedAttempts];

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logins]') AND [c].[name] = N'CreatedAt');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Logins] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Logins] ADD DEFAULT (GETDATE()) FOR [CreatedAt];

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'Status');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Employees] ADD DEFAULT N'Active' FOR [Status];

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'PhoneNumber');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var7 + '];');
UPDATE [Employees] SET [PhoneNumber] = N'' WHERE [PhoneNumber] IS NULL;
ALTER TABLE [Employees] ALTER COLUMN [PhoneNumber] nvarchar(20) NOT NULL;
ALTER TABLE [Employees] ADD DEFAULT N'' FOR [PhoneNumber];

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'Email');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var8 + '];');
UPDATE [Employees] SET [Email] = N'' WHERE [Email] IS NULL;
ALTER TABLE [Employees] ALTER COLUMN [Email] nvarchar(255) NOT NULL;
ALTER TABLE [Employees] ADD DEFAULT N'' FOR [Email];

CREATE TABLE [Departments] (
    [DepartmentID] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([DepartmentID])
);

CREATE INDEX [IX_Employees_DepartmentID] ON [Employees] ([DepartmentID]);

ALTER TABLE [Employees] ADD CONSTRAINT [FK_Employees_Departments_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [Departments] ([DepartmentID]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250416050929_AddDepartmentNameAndDescription', N'9.0.4');

COMMIT;
GO

