CREATE PROCEDURE [dbo].[DeleteApplicationRole]
	@ApplicationID uniqueidentifier,
	@RoleID int
AS
BEGIN
	SET NOCOUNT ON;
	delete from dbo.[Application_Role] where ApplicationID = @ApplicationID and RoleID=@RoleID;
	--If this role is specific to this application. Delete Role.
	--System default Roles cannot be deleted ie @RoleID<=3
	if (@RoleID>3 AND not exists(select RoleID from dbo.[Application_Role] where ApplicationID != @ApplicationID )) 
	begin
	  delete from dbo.[UserRole] where RoleID=@RoleID;
	  delete from [dbo].[RolePermission] where RoleID=@RoleID;
	  delete from dbo.[Role] where RoleID=@RoleID;
	end
END
	
