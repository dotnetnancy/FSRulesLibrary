IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GetRole') 
	DROP PROC [GetRole]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRole]
	@RoleID int
AS
BEGIN
	SELECT [RoleID]
      ,[Title]
      ,[Description]
      ,[Visible]
	FROM [dbo].[Role]
	WHERE RoleID = @RoleID
END

