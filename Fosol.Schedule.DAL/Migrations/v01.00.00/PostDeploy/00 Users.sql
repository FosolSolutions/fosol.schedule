PRINT 'INSERT [Users]'

SET IDENTITY_INSERT dbo.[Users] ON

INSERT INTO dbo.Users (
	[Id]
	, [Key]
	, [Email]
	, [State]
) VALUES (
	1
	, NEWID()
	, 'admin@fosol.ca'
	, 1
)

SET IDENTITY_INSERT dbo.[Users] OFF