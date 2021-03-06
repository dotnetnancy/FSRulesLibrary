USE [GenericDotNetRulesStore]
GO

IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GetApplicationRoles') 
	DROP PROC [GetApplicationRoles]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetApplicationRoles]
	@ApplicationID uniqueidentifier
AS
BEGIN
	SELECT ar.[RoleID]
      ,[Title]
      ,[Description]
      ,[Visible]
	FROM dbo.[Application_Role] ar left join [dbo].[Role] r on ar.RoleID = r.RoleID
	WHERE ar.ApplicationID = @ApplicationID and r.Visible=1
	ORDER BY RoleID ASC;  
END
Go

IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='ApplicationRole_SearchUsers') 
	DROP PROC [ApplicationRole_SearchUsers]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationRole_SearchUsers]
	@RoleID			int,
	@ApplicationID	uniqueidentifier,
	@Criteria		nvarchar(30)
AS
BEGIN
	SET NOCOUNT ON;
	select
		us.UserID,
		us.FirstName,
		us.LastName,
		us.Email
	from dbo.UserStore us join [dbo].[Application_User] au on us.UserID = au.UserID
	where (us.FirstName like '%'+dbo.uf_EscapeSearch(@Criteria)+'%'
		or us.LastName like '%'+dbo.uf_EscapeSearch(@Criteria)+'%'
		or us.Email like '%'+dbo.uf_EscapeSearch(@Criteria)+'%')
		and us.UserID not in 
		(select UserID from dbo.UserRole where RoleID = @RoleID)
		and au.ApplicationID = @ApplicationID
	order by us.LastName, us.FirstName;
END
Go

IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GetApplicationRolePermissions') 
	DROP PROC [GetApplicationRolePermissions]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetApplicationRolePermissions]
	@RoleID			int,
	@ApplicationID uniqueidentifier
AS
BEGIN
	SELECT ar.[RoleID]
      ,p.[PermissionID]
      ,p.[Description]
	FROM dbo.[Application_Role] ar left join dbo.[RolePermission] r on ar.RoleID = r.RoleID 
		 join [dbo].[Permission] p on r.PermissionID = p.PermissionID
	WHERE ar.ApplicationID = @ApplicationID 
	and p.Visible=1
	and  ar.RoleID = @RoleID
	ORDER BY PermissionID ASC;  
END
Go
