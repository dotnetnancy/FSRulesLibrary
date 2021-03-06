CREATE PROC [dbo].[Group_UserInsert]
	@GroupID uniqueidentifier,
	@UserID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	INSERT INTO dbo.Group_User(GroupID, UserID) VALUES (@GroupID, @UserID);
	
	exec dbo.User_GroupsList @UserID;
End
