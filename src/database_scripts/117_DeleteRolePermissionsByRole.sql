USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[DeleteRolePermissionsByRole]    Script Date: 08/04/2011 11:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteRolePermissionsByRole]
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
	
	Declare @RoleTitle varchar(15);
	Select @RoleTitle = Title from dbo.Role where RoleID = @RoleID;
	
	If @RoleTitle = 'SuperAdmin'
	Begin
		Insert into dbo.RolePermission(RoleID, PermissionID) 
		values(@RoleID, 'APPM');
	End
	
END
