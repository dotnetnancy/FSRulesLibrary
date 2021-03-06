IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GetPermissionList') 
	DROP PROC [GetPermissionList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPermissionList]
AS
BEGIN
	SELECT [PermissionID]
	  ,[Description]
	  ,[Visible]
	FROM [dbo].[Permission]
	Where Visible=1
	ORDER BY PermissionID ASC;  
END

