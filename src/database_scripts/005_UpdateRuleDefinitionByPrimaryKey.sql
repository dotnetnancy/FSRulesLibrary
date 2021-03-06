ALTER PROCEDURE [dbo].[UpdateRuleDefinitionByPrimaryKey]
	@RuleID [uniqueidentifier],
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier],
	@RuleName [nvarchar](50),
	@Definition [xml],
	@Paused [bit],
	@UpdatedBy [uniqueidentifier]
AS
Update RuleDefinition 
Set RuleName = @RuleName, 
Definition = @Definition, 
Paused = @Paused, 
DateUpdated = getutcdate(), 
UpdatedBy = @UpdatedBy 
Where (( RuleID = @RuleID ) 
And ( TypeID = @TypeID ) 
And ( ApplicationID = @ApplicationID ) 
)

