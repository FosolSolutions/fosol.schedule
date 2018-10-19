PRINT 'INSERT [Subscriptions]'

SET IDENTITY_INSERT dbo.[Subscriptions] ON

INSERT INTO dbo.[Subscriptions] (
	[Id]
	, [Name]
	, [Description]
	, [State]
	, [Key]
) VALUES (
	1
	, 'Free'
	, 'A free subscription'
	, 1
	, NEWID()
)

SET IDENTITY_INSERT dbo.[Subscriptions] OFF
