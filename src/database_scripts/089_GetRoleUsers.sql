IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GetRoleUsers') 
	DROP PROC [GetRoleUsers]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRoleUsers]
	@RoleID int
AS
BEGIN
  	SELECT r.[RoleID]
	  ,u.[UserID]
      ,[FirstName]
      ,[LastName] 
      ,[Email]
	FROM dbo.[UserRole] r left join [dbo].[UserStore] u on r.UserID = u.UserID
	WHERE r.RoleID = @RoleID  
	ORDER BY RoleID ASC;  
END

