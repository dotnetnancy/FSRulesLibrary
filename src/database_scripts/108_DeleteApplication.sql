USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[DeleteApplication]    Script Date: 07/21/2011 11:53:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteApplication]
	@ApplicationID	uniqueidentifier,
	@UserID			uniqueidentifier,
	@Password		varchar(64)
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select UserID from dbo.UserStore where UserID = @UserID and Password = @Password)
	begin
		raiserror('',16,1);
		return;
	end

	if exists(select ApplicationID from dbo.SuperUserStore where ApplicationID = @ApplicationID)
	begin
		-- To delete superuser permission from SuperUserStore table
		delete from dbo.SuperUserStore where ApplicationID = @ApplicationID;
	end
	
	Exec dbo.DeleteUsersByApplication @ApplicationID;
	
	Exec dbo.DeleteApplicationRolesByApplication @ApplicationID
	Delete from dbo.Application where ApplicationID = @ApplicationID;
END
