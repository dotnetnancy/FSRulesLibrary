CREATE PROC [dbo].[Group_UserDelete]
	@GroupID uniqueidentifier,
	@UserID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
	
	Delete from dbo.Group_User where GroupID = @GroupID and UserID = @UserID;

	exec dbo.User_GroupsList @UserID;
End
