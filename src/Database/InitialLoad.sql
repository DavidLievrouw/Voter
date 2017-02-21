INSERT INTO [security].[User] ([Login], [Password], [Salt], [FirstName], [LastName]) VALUES (
  'dali',
  'TBA',
  'TBA',
  'David',
  'Lievrouw'
);
GO

INSERT INTO [dbo].[version] ([Number], [Creation]) VALUES (1, GETDATE());
GO