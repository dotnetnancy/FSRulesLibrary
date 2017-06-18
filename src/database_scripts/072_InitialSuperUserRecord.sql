
UPDATE [GenericDotNetRulesStore].[dbo].[UserStore]
SET IsSuperUser =1
WHERE UserID = 'eaad5439-08ae-4e12-8307-138afd0b6d9f'

GO

INSERT INTO [GenericDotNetRulesStore].[dbo].[SuperUserStore] 
	([ApplicationID]
	,[UserID])
	VALUES
	('3428a138-4738-4324-bd1f-134732b26ce0','eaad5439-08ae-4e12-8307-138afd0b6d9f')
go

