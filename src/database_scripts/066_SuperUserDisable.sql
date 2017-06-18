USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[SuperUserDisable]    Script Date: 02/08/2011 16:41:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SuperUserDisable]
	@UserID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	DELETE FROM dbo.SuperUserStore WHERE UserID = @UserID;
	
	UPDATE dbo.UserStore SET IsSuperUser = 0 WHERE UserID = @UserID;
End
