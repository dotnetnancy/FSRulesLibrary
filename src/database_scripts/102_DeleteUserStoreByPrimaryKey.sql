USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserStoreByPrimaryKey]    Script Date: 07/20/2011 09:46:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteUserStoreByPrimaryKey]
	@UserID [uniqueidentifier]
AS
BEGIN
	SET NOCOUNT ON;

	Delete from dbo.Application_User
	where UserID = @UserID;
	
	Delete from dbo.Type_User
	where UserID = @UserID;

	Delete From UserStore 
	Where  UserID = @UserID;
	
	Delete from UserRole
	where UserID = @UserID;
END

