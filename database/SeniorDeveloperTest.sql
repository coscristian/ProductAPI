USE [master];
GO

BEGIN TRY
    IF DB_ID(N'SeniorDeveloperTest') IS NULL
    BEGIN
        PRINT 'Creating database [SeniorDeveloperTest]...';

        CREATE DATABASE [SeniorDeveloperTest];

        PRINT 'Database [SeniorDeveloperTest] created successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Database [SeniorDeveloperTest] already exists.';
    END
END TRY
BEGIN CATCH
    PRINT 'ERROR: Failed to create database [SeniorDeveloperTest].';
    PRINT 'Error ' + CAST(ERROR_NUMBER() AS NVARCHAR(10))
        + ': ' + ERROR_MESSAGE();

    THROW;
END CATCH;
GO

USE [SeniorDeveloperTest];
GO

BEGIN TRY

    IF OBJECT_ID(N'[dbo].[Products]', N'U') IS NULL
    BEGIN
        PRINT 'Creating table [Products]...';

        CREATE TABLE [dbo].[Products]
        (
            [Id] INT IDENTITY(1,1) NOT NULL,
            [Name] NVARCHAR(200) NOT NULL,
            [Description] NVARCHAR(1000) NULL,
            [Price] DECIMAL(18,2) NOT NULL,
            [CreatedDate] DATETIME2 NOT NULL,
            [IsDeleted] BIT NOT NULL
                CONSTRAINT [DF_Products_IsDeleted] DEFAULT 0,
            [DeletedDate] DATETIME2 NULL,

            CONSTRAINT [PK_Products]
                PRIMARY KEY ([Id]),

            CONSTRAINT [CK_Products_Price]
                CHECK ([Price] > 0)
        );

        CREATE INDEX [IX_Products_IsDeleted_CreatedDate]
            ON [dbo].[Products] ([IsDeleted], [CreatedDate] DESC);

        PRINT 'Table [Products] created successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Table [Products] already exists.';
    END

END TRY
BEGIN CATCH
    PRINT 'ERROR: Failed to create table [Products].';
    PRINT 'Error ' + CAST(ERROR_NUMBER() AS NVARCHAR(10))
        + ': ' + ERROR_MESSAGE();

    THROW;
END CATCH;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Product_Create]
    @Name NVARCHAR(200),
    @Description NVARCHAR(1000),
    @Price DECIMAL(18,2),
    @CreatedDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Products]
    (
        [Name],
        [Description],
        [Price],
        [CreatedDate],
        [IsDeleted]
    )
    OUTPUT
        INSERTED.[Id],
        INSERTED.[Name],
        INSERTED.[Description],
        INSERTED.[Price],
        INSERTED.[CreatedDate],
        INSERTED.[IsDeleted],
        INSERTED.[DeletedDate]
    VALUES
    (
        @Name,
        @Description,
        @Price,
        @CreatedDate,
        0
    );
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Product_GetById]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [Id],
        [Name],
        [Description],
        [Price],
        [CreatedDate],
        [IsDeleted],
        [DeletedDate]
    FROM [dbo].[Products]
    WHERE [Id] = @Id
      AND [IsDeleted] = 0;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Product_GetPaged]
    @PageNumber INT,
    @PageSize INT,
    @Search NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT
        [Id],
        [Name],
        [Description],
        [Price],
        [CreatedDate],
        [IsDeleted],
        [DeletedDate]
    FROM [dbo].[Products]
    WHERE [IsDeleted] = 0
      AND
      (
          @Search IS NULL
          OR [Name] LIKE '%' + @Search + '%'
          OR [Description] LIKE '%' + @Search + '%'
      )
    ORDER BY [CreatedDate] DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(1)
    FROM [dbo].[Products]
    WHERE [IsDeleted] = 0
      AND
      (
          @Search IS NULL
          OR [Name] LIKE '%' + @Search + '%'
          OR [Description] LIKE '%' + @Search + '%'
      );
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Product_Update]
    @Id INT,
    @Name NVARCHAR(200),
    @Description NVARCHAR(1000),
    @Price DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Products]
    SET
        [Name] = @Name,
        [Description] = @Description,
        [Price] = @Price
    WHERE [Id] = @Id
      AND [IsDeleted] = 0;

    SELECT
        [Id],
        [Name],
        [Description],
        [Price],
        [CreatedDate],
        [IsDeleted],
        [DeletedDate]
    FROM [dbo].[Products]
    WHERE [Id] = @Id
      AND [IsDeleted] = 0;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Product_Delete]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Products]
    SET
        [IsDeleted] = 1,
        [DeletedDate] = SYSUTCDATETIME()
    WHERE [Id] = @Id
      AND [IsDeleted] = 0;

    SELECT @@ROWCOUNT AS [RowsAffected];
END;
GO

PRINT '==================================================';
PRINT 'Database initialization completed successfully.';
PRINT 'Database: [SeniorDeveloperTest]';
PRINT '==================================================';
GO