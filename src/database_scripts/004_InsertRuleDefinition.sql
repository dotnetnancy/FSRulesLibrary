ALTER PROCEDURE [dbo].[InsertRuleDefinition]
	@RuleID [uniqueidentifier] = null out,
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier],
	@RuleName [nvarchar](50),
	@Definition [xml],
	@Paused [bit],
	@CreatedBy [uniqueidentifier]
AS
Begin
	SET NOCOUNT ON;
	
	set @RuleID = newid();

	Insert Into RuleDefinition 
	( RuleID, TypeID, ApplicationID, RuleName, Definition, Paused, DateCreated, CreatedBy, Deleted, DateUpdated, UpdatedBy)
	Values ( @RuleID, @TypeID, @ApplicationID, @RuleName, @Definition, @Paused, getutcdate(), @CreatedBy, 0, getutcdate(), @CreatedBy) 

End

