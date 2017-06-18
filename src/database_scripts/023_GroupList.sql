Create proc [dbo].[GroupList]
	@ApplicationID uniqueidentifier
AS
Begin
	Select GroupID, GroupName, CreatedDate, CreatedBy 
	From dbo.[Group] where ApplicationID = @ApplicationID
	order by CreatedDate asc;
End

