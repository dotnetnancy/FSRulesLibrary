CREATE procedure [dbo].[User_GroupsList]
	@UserID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
		
	Select g.GroupID, g.GroupName
	from dbo.[Group] g inner join dbo.Group_User gu
		on g.GroupId = gu.GroupID
		inner join dbo.UserStore us 
		on us.UserID = gu.UserID
	Where
		us.UserID = @UserID;
End


