IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='InsertUserRole') 
	DROP PROC [InsertUserRole]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertUserRole]
	@UserID uniqueidentifier,
	@RoleID int
AS
BEGIN
	SET NOCOUNT ON;
	
	if not exists (select RoleID from dbo.[Role] where RoleID = @RoleID) 
	begin	
		raiserror('Role does not exist',16,1);
		return;
	end
	
	if not exists (select RoleID from [dbo].[UserRole] where RoleID=@RoleID and UserID=@UserID)
	begin 
		INSERT INTO [dbo].[UserRole] ([RoleID],[UserID])
		VALUES (@RoleID, @UserID)
	end 
END
GO

-- Exec InsertRolePermission @Role=1, @UserID='Guid'