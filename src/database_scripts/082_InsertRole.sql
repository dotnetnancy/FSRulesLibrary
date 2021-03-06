IF EXISTS (SELECT * FROM sysobjects WHERE NAME ='InsertRole') 
	DROP PROC [InsertRole]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertRole]
	@RoleID int = null out,
	@Title [nvarchar](20),
	@Description [nvarchar](50),
	@Visible bit = 1
AS
BEGIN
	SET NOCOUNT ON;
	
	if exists (select Title from dbo.Role where Title = @Title)
	begin
		raiserror('Role with this Title already exists.',16,1);
		return;
	end
		
	INSERT INTO [dbo].[Role] ([Title] ,[Description] ,[Visible])
    VALUES (@Title, @Description, @Visible)
	
	SET @RoleID = @@IDENTITY
END
GO

-- Exec InsertRole @Title="TestRole", @Description="Test"