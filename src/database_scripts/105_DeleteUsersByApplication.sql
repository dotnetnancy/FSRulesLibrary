USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[DeleteUsersByApplication]    Script Date: 07/20/2011 14:22:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[DeleteUsersByApplication]
	@ApplicationID		uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
	
	Declare @users table(userid uniqueidentifier);
	
	Insert @users Select us.UserID from UserStore us inner join dbo.Application_User au 
							on us.UserID = au.UserID inner join dbo.Application a
							on a.ApplicationID = au.ApplicationID
						Where a.ApplicationID = @ApplicationID;
	
	Delete from dbo.Application_User where ApplicationID = @ApplicationID;	
	
	Delete from dbo.UserRole where UserID in (Select UserId from @users);
	
	Delete from dbo.UserStore where UserID in (Select userid from @users);
	
End
