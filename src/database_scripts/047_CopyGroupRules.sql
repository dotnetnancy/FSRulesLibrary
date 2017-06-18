CREATE Proc dbo.CopyGroupRules
	@GroupID		uniqueidentifier = null,
	@CopyGroupID	uniqueidentifier,
	@TypeID			uniqueidentifier,
	@ApplicationID  uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
	
	insert into dbo.Group_RuleDefinition(GroupID, RuleID)
	Select @CopyGroupID, rd.RuleID from dbo.RuleDefinition rd left join dbo.Group_RuleDefinition grd
		on rd.RuleID = grd.RuleID
	where  
		((@GroupID is null and rd.RuleID not in (Select rd.RuleID from dbo.RuleDefinition rd left join dbo.Group_RuleDefinition grd
															on rd.RuleID = grd.RuleID
												 where grd.GroupID = @CopyGroupID))
		OR (@GroupID = CAST(cast(0 as binary) as uniqueidentifier) and rd.RuleID not in(Select RuleID from dbo.Group_RuleDefinition) )		
		OR (@GroupID != CAST(cast(0 as binary) as uniqueidentifier) and grd.GroupID = @GroupID
		and rd.RuleID not in (Select rd.RuleID from dbo.RuleDefinition rd left join dbo.Group_RuleDefinition grd
												on rd.RuleID = grd.RuleID
							  where grd.GroupID = @CopyGroupID)))
		and rd.TypeID = @TypeID
		and rd.ApplicationID = @ApplicationID
		and rd.Deleted = 0;	
End