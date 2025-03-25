CREATE TABLE Events (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Type INT,
    Metadata VARCHAR(MAX),
    CreatedAt DATETIME2(7),
    UpdatedAt DATETIME2(7)
);
GO

DROP TABLE Events;
GO