CREATE PROCEDURE [dbo].[DeleteUserRole]
	@UserID  uniqueidentifier,
	@RoleID  int      
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select RoleID from dbo.[Role] where RoleID = @RoleID )
	begin
		raiserror('Role does not exist',16,1);
		return;
	end
	
	delete from dbo.[UserRole] where RoleID=@RoleID and UserID=@UserID;
END
