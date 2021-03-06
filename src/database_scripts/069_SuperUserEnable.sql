USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[SuperUserEnable]    Script Date: 02/14/2011 11:04:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[SuperUserEnable]
	@UserID			uniqueidentifier,
	@ApplicationID	uniqueidentifier,
	@Enable			bit
AS
Begin
	SET NOCOUNT ON;

	if @Enable = 1
	begin
		Declare @AID uniqueidentifier
		
		Select @AID = au.ApplicationID 
		from dbo.UserStore us inner join dbo.Application_User au
			on us.UserID = au.UserID
			inner join dbo.Application a
			on a.ApplicationID = au.ApplicationID
		where us.UserID = @UserID;
		
		UPDATE dbo.UserStore SET IsSuperUser = 1 WHERE UserID = @UserID;
		
		exec dbo.SuperUser_ApplicationInsert @AID, @UserID;
		if(@AID <> @ApplicationID)
			exec dbo.SuperUser_ApplicationInsert @ApplicationID, @UserID;
	end
	else 
	begin
		DELETE FROM dbo.SuperUserStore WHERE UserID = @UserID;
		
		UPDATE dbo.UserStore SET IsSuperUser = 0 WHERE UserID = @UserID;
	end
End
