CREATE DATABASE SeniorDeveloperTest;
GO

USE SeniorDeveloperTest;
GO

CREATE TABLE Products
(
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    Price DECIMAL(18,2) NOT NULL,
    CreatedDate DATETIME2 NOT NULL,
    IsDeleted BIT NOT NULL CONSTRAINT DF_Products_IsDeleted DEFAULT 0,
    DeletedDate DATETIME2 NULL,

    CONSTRAINT PK_Products PRIMARY KEY (Id),
    CONSTRAINT CK_Products_Price CHECK (Price > 0)
);
GO

CREATE INDEX IX_Products_IsDeleted_CreatedDate
    ON Products (IsDeleted, CreatedDate DESC);
GO

-- Procedures

-- sp_Product_Create
CREATE OR ALTER PROCEDURE sp_Product_Create
    @Name NVARCHAR(200),
    @Description NVARCHAR(1000),
    @Price DECIMAL(18,2),
    @CreatedDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Products
    (
        Name,
        Description,
        Price,
        CreatedDate,
        IsDeleted
    )
    OUTPUT
        INSERTED.Id,
        INSERTED.Name,
        INSERTED.Description,
        INSERTED.Price,
        INSERTED.CreatedDate,
        INSERTED.IsDeleted,
        INSERTED.DeletedDate
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

-- sp_Product_GetById
CREATE OR ALTER PROCEDURE sp_Product_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Name,
        Description,
        Price,
        CreatedDate,
        IsDeleted,
        DeletedDate
    FROM Products
    WHERE Id = @Id
      AND IsDeleted = 0;
END;
GO

--sp_Product_GetPaged
CREATE OR ALTER PROCEDURE sp_Product_GetPaged
    @PageNumber INT,
    @PageSize INT,
    @Search NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT
        Id,
        Name,
        Description,
        Price,
        CreatedDate,
        IsDeleted,
        DeletedDate
    FROM Products
    WHERE IsDeleted = 0
      AND
      (
          @Search IS NULL
          OR Name LIKE '%' + @Search + '%'
          OR Description LIKE '%' + @Search + '%'
      )
    ORDER BY CreatedDate DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(1)
    FROM Products
    WHERE IsDeleted = 0
      AND
      (
          @Search IS NULL
          OR Name LIKE '%' + @Search + '%'
          OR Description LIKE '%' + @Search + '%'
      );
END;
GO

-- sp_Product_Update
CREATE OR ALTER PROCEDURE sp_Product_Update
    @Id INT,
    @Name NVARCHAR(200),
    @Description NVARCHAR(1000),
    @Price DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Products
    SET
        Name = @Name,
        Description = @Description,
        Price = @Price
    WHERE Id = @Id
      AND IsDeleted = 0;

    SELECT
        Id,
        Name,
        Description,
        Price,
        CreatedDate,
        IsDeleted,
        DeletedDate
    FROM Products
    WHERE Id = @Id
      AND IsDeleted = 0;
END;
GO

-- sp_Product_Delete
CREATE OR ALTER PROCEDURE sp_Product_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Products
    SET
        IsDeleted = 1,
        DeletedDate = SYSUTCDATETIME()
    WHERE Id = @Id
      AND IsDeleted = 0;

    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO