IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GetRolesList') 
	DROP PROC [GetRolesList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRolesList]
	@Visible bit
AS
BEGIN
	SELECT [RoleID]
      ,[Title]
      ,[Description]
      ,[Visible]
	FROM [dbo].[Role]
	WHERE Visible = @Visible
	ORDER BY RoleID ASC;  
END

