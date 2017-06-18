Create Proc dbo.Group_UserList
	@GroupID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	Select 
		us.UserID, us.FirstName, us.LastName, us.Email
	from 
		dbo.UserStore us inner join dbo.Group_User gu 
		on us.UserID = gu.UserID
		inner join dbo.[Group] g
		on g.GroupID = gu.GroupID
	where
		gu.GroupID = @GroupID;
End