USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[DeleteType_UserByPrimaryKey]    Script Date: 07/26/2011 15:26:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteType_UserByPrimaryKey]
	@TypeID [uniqueidentifier],
	@UserID [uniqueidentifier],
	@ApplicationID	uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	Delete From Type_User 
	Where (( TypeID = @TypeID ) 
	And ( UserID = @UserID ) 
	)

	Exec dbo.GetUserTypes @UserID, @ApplicationID;
END

