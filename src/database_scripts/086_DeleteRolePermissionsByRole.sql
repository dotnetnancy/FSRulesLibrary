CREATE PROCEDURE [dbo].[DeleteRolePermissionsByRole]
	@RoleID			int
AS
BEGIN
	SET NOCOUNT ON;
	--if not exists(select RoleID from dbo.[Role] where RoleID = @RoleID )
	--begin
	--	raiserror('',16,1);
	--	return;
	--end
	
	delete from dbo.[RolePermission] where RoleID = @RoleID;
END
