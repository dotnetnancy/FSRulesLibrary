USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[SuperUser_ApplicationsList]    Script Date: 02/07/2011 11:48:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SuperUser_ApplicationsList]
	@UserID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
		
	Select a.ApplicationID, a.ApplicationName
	from dbo.Application a inner join dbo.SuperUserStore su
		on a.ApplicationID = su.ApplicationID
		inner join dbo.UserStore us 
		on us.UserID = su.UserID
	Where
		us.UserID = @UserID;
End


