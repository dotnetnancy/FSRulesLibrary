CREATE PROCEDURE [dbo].[DeleteApplicationRolesByApplication]
	@ApplicationID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	--if not exists(select RoleID from dbo.[Role] where RoleID = @RoleID )
	--begin
	--	raiserror('',16,1);
	--	return;
	--end
	
	delete from dbo.[Application_Role] where ApplicationID = @ApplicationID;
END
