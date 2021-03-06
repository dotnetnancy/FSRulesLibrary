CREATE PROCEDURE [dbo].[DeleteRole]
	@RoleID			int
AS
BEGIN
	SET NOCOUNT ON;
	
	Exec dbo.DeleteRolePermissionByRole @RoleID;
	Exec dbo.DeleteUserRoleByRole @RoleID;
	Exec dbo.DeleteApplicationRolesByRole @RoleID;
	
	delete from dbo.[Role] where RoleID = @RoleID;
END
