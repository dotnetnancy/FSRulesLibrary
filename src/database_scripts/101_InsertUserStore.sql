USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[InsertUserStore]    Script Date: 07/20/2011 11:07:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[InsertUserStore]
	@UserID [uniqueidentifier] = null out,
	@ApplicationID [uniqueidentifier],
	@FirstName [nvarchar](255),
	@LastName [nvarchar](255),
	@Email [varchar](150),
	@Password [varchar](64),
	@CreatedBy [uniqueidentifier],
	@IsAdmin bit = null
AS
BEGIN
	SET NOCOUNT ON;
	Declare @RoleID int;

	if exists (select dbo.UserStore.UserID from dbo.UserStore where dbo.UserStore.Email = @Email)
		begin
			raiserror('',16,1);
			return;
		end
	if @UserID is null set @UserID = newid();

	Insert Into UserStore 
	( UserID, FirstName, LastName, Email, Password, LastLogin, DateCreated, CreatedBy, IsSuperUser)
	Values ( @UserID, @FirstName, @LastName, @Email, @Password, getutcdate(), getutcdate(), @CreatedBy, 0) 

	Insert into Application_User(ApplicationID, UserID)
	Values(@ApplicationID, @UserID);
	
	if(@IsAdmin = 1)
		Set @RoleID = (Select RoleID from dbo.Role where UPPER(Title) = UPPER('Admin'));
	else
		Set @RoleID = (Select RoleID from dbo.Role where UPPER(Title) = UPPER('Agent'));
	
	if(@RoleID is not null)
		Exec dbo.InsertUserRole @UserID, @RoleID;
END
