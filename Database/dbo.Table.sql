CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NULL, 
    [Lastname] VARCHAR(50) NULL, 
    [Email] VARCHAR(50) NULL, 
    [Password] VARCHAR(MAX) NULL 
)
