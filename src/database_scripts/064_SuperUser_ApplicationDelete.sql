USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[SuperUser_ApplicationDelete]    Script Date: 02/08/2011 09:06:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SuperUser_ApplicationDelete]
	@UserID			uniqueidentifier,
	@ApplicationID	uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
	
	Delete from dbo.SuperUserStore where ApplicationID = @ApplicationID and UserID = @UserID;

	exec dbo.SuperUser_ApplicationsList @UserID;
End
