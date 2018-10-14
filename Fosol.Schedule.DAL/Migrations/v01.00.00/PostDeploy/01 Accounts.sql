PRINT 'INSERT [Accounts]'

SET IDENTITY_INSERT dbo.[Accounts] ON

INSERT INTO dbo.[Accounts] (
	[Id]
	, [Key]
	, [OwnerId]
	, [State]
	, [Kind]
	, [SubscriptionId]
	, [AddedById]
) VALUES (
	1
	, NEWID()
	, 1
	, 1
	, 0
	, 1
	, 1
)

SET IDENTITY_INSERT dbo.[Accounts] OFF
