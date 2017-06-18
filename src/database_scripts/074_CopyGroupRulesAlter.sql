ALTER Proc [dbo].[CopyGroupRules]
	@RuleIDs		varchar(4000),
	@CopyGroupID	uniqueidentifier,
	@TypeID			uniqueidentifier,
	@ApplicationID  uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
	
	insert into dbo.Group_RuleDefinition(GroupID, RuleID)
	Select @CopyGroupID, rd.RuleID from (dbo.RuleDefinition rd join dbo.uf_SplitIntoTable(@RuleIDs,',') rIds on rd.RuleID= rIds.Item) 
	where  
		(rd.RuleID not in (Select grd.RuleID from dbo.Group_RuleDefinition grd
										where grd.GroupID = @CopyGroupID))
		and rd.TypeID = @TypeID
		and rd.ApplicationID = @ApplicationID
		and rd.Deleted = 0;	
End

