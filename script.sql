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
CREATE TABLE [Companies] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(20) NOT NULL,
    [LeaderId] int NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
);

CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(30) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Phone] nvarchar(30) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [CompanyId] int NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employees_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Divisions] (
    [Id] int NOT NULL IDENTITY,
    [CompanyId] int NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(20) NOT NULL,
    [LeaderId] int NULL,
    CONSTRAINT [PK_Divisions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Divisions_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Divisions_Employees_LeaderId] FOREIGN KEY ([LeaderId]) REFERENCES [Employees] ([Id])
);

CREATE TABLE [Projects] (
    [Id] int NOT NULL IDENTITY,
    [DivisionId] int NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(20) NOT NULL,
    [LeaderId] int NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Projects_Divisions_DivisionId] FOREIGN KEY ([DivisionId]) REFERENCES [Divisions] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Projects_Employees_LeaderId] FOREIGN KEY ([LeaderId]) REFERENCES [Employees] ([Id])
);

CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [ProjectId] int NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(20) NOT NULL,
    [LeaderId] int NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Departments_Employees_LeaderId] FOREIGN KEY ([LeaderId]) REFERENCES [Employees] ([Id]),
    CONSTRAINT [FK_Departments_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX [IX_Companies_Code] ON [Companies] ([Code]);

CREATE INDEX [IX_Companies_LeaderId] ON [Companies] ([LeaderId]);

CREATE INDEX [IX_Departments_LeaderId] ON [Departments] ([LeaderId]);

CREATE UNIQUE INDEX [IX_Departments_ProjectId_Code] ON [Departments] ([ProjectId], [Code]);

CREATE UNIQUE INDEX [IX_Divisions_CompanyId_Code] ON [Divisions] ([CompanyId], [Code]);

CREATE INDEX [IX_Divisions_LeaderId] ON [Divisions] ([LeaderId]);

CREATE INDEX [IX_Employees_CompanyId] ON [Employees] ([CompanyId]);

CREATE UNIQUE INDEX [IX_Projects_DivisionId_Code] ON [Projects] ([DivisionId], [Code]);

CREATE INDEX [IX_Projects_LeaderId] ON [Projects] ([LeaderId]);

ALTER TABLE [Companies] ADD CONSTRAINT [FK_Companies_Employees_LeaderId] FOREIGN KEY ([LeaderId]) REFERENCES [Employees] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260514175225_InitialCreate', N'9.0.15');

COMMIT;
GO

