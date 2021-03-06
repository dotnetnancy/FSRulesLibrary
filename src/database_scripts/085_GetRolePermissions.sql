IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GetRolePermission') 
	DROP PROC [GetRolePermission]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRolePermission]
	@RoleID int
AS
BEGIN
  	SELECT r.[RoleID]
      ,p.[PermissionID]
      ,p.[Description]
	FROM dbo.RolePermission r left join [dbo].[Permission] p on r.PermissionID = p.PermissionID
	WHERE r.RoleID = @RoleID  
	ORDER BY RoleID ASC;  
END



IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GetUserRolePermissions') 
	DROP PROC [GetUserRolePermissions]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserRolePermissions]
	@UserID uniqueidentifier
AS
BEGIN
  	SELECT r.[RoleID]
      ,p.[PermissionID]
      ,p.[Description]
	FROM dbo.[UserRole] ur left join dbo.RolePermission r on ur.RoleID=r.RoleID 
		join [dbo].[Permission] p on r.PermissionID = p.PermissionID
	WHERE ur.UserID = @UserID  
	ORDER BY PermissionID ASC;    
END
