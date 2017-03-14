INSERT INTO [security].[User] ([Login], [Password], [Salt], [FirstName], [LastName], [Type]) VALUES (
  'dali',
  'TBA',
  'TBA',
  'David',
  'Lievrouw',
  'L'
);
GO

INSERT INTO [dbo].[version] ([Number], [Creation]) VALUES (1, GETDATE());
GO