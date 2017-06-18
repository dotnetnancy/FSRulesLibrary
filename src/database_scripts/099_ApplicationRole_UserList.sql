USE [GenericDotNetRulesStore]
GO

Create Procedure [dbo].[ApplicationRole_UserList]
	@ApplicationID uniqueidentifier,
	@RoleID int
AS
Begin
	SET NOCOUNT ON;

	Select 
		us.UserID, us.FirstName, us.LastName, us.Email
	FROM
		dbo.Application_Role ar inner join Application_User au 
		on au.ApplicationID = ar.ApplicationID inner join dbo.UserRole ur
		on ur.RoleID = ar.RoleID and ur.UserID=au.UserID
		inner join dbo.UserStore us
		on us.UserID = au.UserID
	where
		ar.ApplicationID = @ApplicationID and ar.RoleID = @RoleID and
		ur.RoleID = @RoleID 
End
GO