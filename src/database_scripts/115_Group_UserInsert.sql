USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[Group_UserInsert]    Script Date: 07/27/2011 16:26:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[Group_UserInsert]
	@GroupID		uniqueidentifier,
	@UserID			uniqueidentifier,
	@ApplicationID	uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	INSERT INTO dbo.Group_User(GroupID, UserID) VALUES (@GroupID, @UserID);
	
	exec dbo.User_GroupsList @UserID, @ApplicationID;
End
