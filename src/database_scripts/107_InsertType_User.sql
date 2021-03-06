USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[InsertType_User]    Script Date: 07/20/2011 16:00:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[InsertType_User]
	@TypeID [uniqueidentifier],
	@UserID [uniqueidentifier],
	@ApplicationID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	Insert Into Type_User 
	( TypeID, UserID)
	Values ( @TypeID, @UserID) 
	
	Exec dbo.GetUserTypes @UserID, @ApplicationID;
END

