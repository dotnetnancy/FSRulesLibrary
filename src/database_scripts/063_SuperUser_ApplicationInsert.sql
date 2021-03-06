USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[SuperUser_ApplicationInsert]    Script Date: 02/07/2011 11:44:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SuperUser_ApplicationInsert]
	@ApplicationID uniqueidentifier,
	@UserID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	INSERT INTO dbo.SuperUserStore(UserID,ApplicationID) VALUES (@UserID, @ApplicationID);
	
	exec dbo.SuperUser_ApplicationsList @UserID;
End
