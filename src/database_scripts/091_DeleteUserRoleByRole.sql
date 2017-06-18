CREATE PROCEDURE [dbo].[DeleteUserRoleByRole]
	@RoleID int      
AS
BEGIN
	SET NOCOUNT ON;
	--if not exists(select RoleID from dbo.[Role] where RoleID = @RoleID )
	--begin
	--	raiserror('',16,1);
	--	return;
	--end
	
	delete from dbo.[UserRole] where RoleID = @RoleID;
END
