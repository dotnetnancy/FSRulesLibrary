USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[SuperUserEnable]    Script Date: 02/08/2011 16:41:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SuperUserEnable]
	@UserID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	UPDATE dbo.UserStore SET IsSuperUser = 1 WHERE UserID = @UserID;
End
