CREATE PROC dbo.GetRuleByRuleID
	@RuleID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	select
		dbo.RuleDefinition.RuleID,
		dbo.RuleDefinition.RuleName,
		dbo.RuleDefinition.Definition,
		dbo.RuleDefinition.Paused,
		dbo.RuleDefinition.DateCreated,
		dbo.RuleDefinition.DateUpdated,
		isnull(dbo.UserStore.FirstName,'Unknown') as CreatorFirstName,
		isnull(dbo.UserStore.LastName,'Unknown') as CreatorLastName,
		res.ModifierFirstName,
		res.ModifierLastName
	from dbo.RuleDefinition
		left outer join dbo.UserStore on dbo.RuleDefinition.CreatedBy = dbo.UserStore.UserID,
		( 
			Select 
				isnull(dbo.UserStore.FirstName,'Unknown') as ModifierFirstName,
				isnull(dbo.UserStore.LastName,'Unknown') as ModifierLastName
			from dbo.RuleDefinition left outer join dbo.UserStore on dbo.RuleDefinition.UpdatedBy = dbo.UserStore.UserID
			where dbo.RuleDefinition.RuleID = @RuleID
				and dbo.RuleDefinition.UpdatedBy is not null
		) res
	where dbo.RuleDefinition.RuleID = @RuleID;

END