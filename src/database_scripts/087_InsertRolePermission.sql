IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='InsertRolePermission') 
	DROP PROC [InsertRolePermission]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertRolePermission]
	@RoleID int,
	@PermissionID [nvarchar](4)
AS
BEGIN
	SET NOCOUNT ON;
	
	if not exists (select RoleID from dbo.[Role] where RoleID = @RoleID)
	begin
		raiserror('',16,1);
		return;
	end
		
	INSERT INTO [dbo].[RolePermission] ([RoleID]
      ,[PermissionID])
    VALUES ( @RoleID, @PermissionID)
END
GO

-- Exec InsertRolePermission @Role=1, @PermissionID="APPM"

IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='DeleteRolePermission') 
	DROP PROC [DeleteRolePermission]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRolePermission]
	@RoleID int,
	@PermissionID [nvarchar](4)
AS
BEGIN
	SET NOCOUNT ON;
	
	if not exists (select RoleID from dbo.[Role] where RoleID = @RoleID)
	begin
		raiserror('',16,1);
		return;
	end
		
	Delete [dbo].[RolePermission] 
    Where RoleID=@RoleID and PermissionID=@PermissionID
END
GO