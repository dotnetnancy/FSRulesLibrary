use [GenericDotNetRulesStore]
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[Permission]([PermissionID],[Description],[Visible])
     VALUES ('APPM','Applications Mangement', 0)
GO
INSERT INTO [GenericDotNetRulesStore].[dbo].[Permission]([PermissionID],[Description],[Visible])
     VALUES ('TYPM','Types Management', 1)
GO
INSERT INTO [GenericDotNetRulesStore].[dbo].[Permission]([PermissionID],[Description],[Visible])
     VALUES ('GRPM','Groups Mangement', 1)
GO
INSERT INTO [GenericDotNetRulesStore].[dbo].[Permission]([PermissionID],[Description],[Visible])
     VALUES ('CONF','Configurations Mangement', 1)
GO
INSERT INTO [GenericDotNetRulesStore].[dbo].[Permission]([PermissionID],[Description],[Visible])
     VALUES ('RULE','Rules Mangement', 1)
GO

INSERT INTO [GenericDotNetRulesStore].[dbo].[Permission]([PermissionID],[Description],[Visible])
     VALUES ('ROLE','Roles Mangement', 1)
GO

INSERT INTO [GenericDotNetRulesStore].[dbo].[Permission]([PermissionID],[Description],[Visible])
     VALUES ('USER','Users Mangement', 1)
GO

Exec InsertRole @Title="SuperAdmin", @Description="Default Super Admin", @Visible=True
Exec InsertRolePermission @RoleID=1, @PermissionID="APPM"
Exec InsertRolePermission @RoleID=1, @PermissionID="TYPM"
Exec InsertRolePermission @RoleID=1, @PermissionID="GRPM"
Exec InsertRolePermission @RoleID=1, @PermissionID="CONF"
Exec InsertRolePermission @RoleID=1, @PermissionID="RULE"
Exec InsertRolePermission @RoleID=1, @PermissionID="ROLE"
Exec InsertRolePermission @RoleID=1, @PermissionID="USER"

Exec InsertRole @Title="Admin", @Description="Application Admin", @Visible=True
Exec InsertRolePermission @RoleID=2, @PermissionID="TYPM"
Exec InsertRolePermission @RoleID=2, @PermissionID="GRPM"
Exec InsertRolePermission @RoleID=2, @PermissionID="CONF"
Exec InsertRolePermission @RoleID=2, @PermissionID="RULE"
Exec InsertRolePermission @RoleID=2, @PermissionID="USER"

Exec InsertRole @Title="Agent", @Description="Application Agnent/User", @Visible=True
Exec InsertRolePermission @RoleID=3, @PermissionID="GRPM"
Exec InsertRolePermission @RoleID=3, @PermissionID="RULE"
Exec InsertRolePermission @RoleID=3, @PermissionID="USER"

Exec InsertUserRole @RoleID=1, @UserID='EAAD5439-08AE-4E12-8307-138AFD0B6D9F'
Exec dbo.InsertApplication_Role @RoleID=null, @UserID=null, @ApplicationID='3428A138-4738-4324-BD1F-134732B26CE0', @Title='SuperAdmin'