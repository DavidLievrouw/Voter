CREATE TYPE [security].[T_User] AS TABLE (
  [UniqueId] UNIQUEIDENTIFIER NOT NULL, 
  [Login] NVARCHAR(100) NULL, 
  [Password] NVARCHAR(1000) NULL, 
  [Salt] NVARCHAR(1000) NULL, 
  [FirstName] NVARCHAR(50) NOT NULL, 
  [LastName] NVARCHAR(50) NOT NULL, 
  [LastNamePrefix] NVARCHAR(50) NULL, 
  [Type] NCHAR(1) NOT NULL, 
  [ExternalCorrelationId] NVARCHAR(50) NULL
);
