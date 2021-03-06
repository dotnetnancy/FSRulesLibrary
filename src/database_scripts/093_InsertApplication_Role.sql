IF EXISTS (SELECT * FROM sysobjects WHERE NAME ='InsertApplication_Role') 
	DROP PROC [InsertApplication_Role]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertApplication_Role]
	@RoleID int = null out,
	@UserID uniqueidentifier = null,
	@ApplicationID uniqueidentifier,
	@Title [nvarchar](20)
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (@RoleID is null)
	BEGIN
		IF NOT EXISTS (select RoleID from dbo.[Role] where UPPER(Title) = UPPER(@Title) )
		BEGIN
			INSERT INTO [dbo].[Role] ([Title] ,[Description] ,[Visible])
			VALUES (@Title, '', 1)
			SET @RoleID = @@IDENTITY
		END
	END

	select @RoleID=RoleID from dbo.[Role] where UPPER(Title) = UPPER(@Title)
	IF (@RoleID is not null)
	BEGIN
		INSERT INTO [dbo].[Application_Role]
			   ([ApplicationID], [RoleID])
		VALUES (@ApplicationID, @RoleID)
	END
	ELSE 
	begin
			raiserror('Invalid Role',16,1);
			return;
	end
		
	-- Since roles are specific to application user/role is associated
	IF (@UserID is not null)   
	BEGIN
		Exec InsertUserRole @UserID, @RoleID;
	END
	
END
GO

-- Exec InsertApplication_Role null, null, @ApplicationID='guid', @RoleID=1