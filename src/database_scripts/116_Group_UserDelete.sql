USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[Group_UserDelete]    Script Date: 07/27/2011 16:27:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[Group_UserDelete]
	@GroupID		uniqueidentifier,
	@UserID			uniqueidentifier,
	@ApplicationID	uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
	
	Delete from dbo.Group_User where GroupID = @GroupID and UserID = @UserID;

	exec dbo.User_GroupsList @UserID, @ApplicationID;
End
