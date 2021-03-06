USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[InsertApplication]    Script Date: 07/21/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[InsertApplication]
	@ApplicationID [uniqueidentifier]= NULL OUTPUT,
	@ApplicationName [nvarchar](50),
	@ApplicationDescription [nvarchar](255),
	@Email varchar(150),
	@Pass varchar(64),
	@CreatorID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	if exists (select ApplicationName from dbo.Application where ApplicationName = @ApplicationName)
	begin
		raiserror('Application with this name already exists.',16,1);
		return;
	end

	BEGIN TRANSACTION		
	if @ApplicationID is null
		Set @ApplicationID = NEWID();

	Insert Into Application 
	( ApplicationID, ApplicationName, ApplicationDescription)
	Values ( @ApplicationID, @ApplicationName, @ApplicationDescription);

	Declare @UserID uniqueidentifier
	Declare @FirstName [nvarchar](255)
	set @FirstName = ltrim(Substring(@Email,1,Charindex('@', @Email)-1))
	Exec dbo.InsertUserStore @UserID out, @ApplicationID, @FirstName,'',@Email, @Pass, @CreatorID, 1;
	
	-- Every application needs to have uniquea admin user
	Insert into dbo.SuperUserStore(ApplicationID, UserID)
			Select @ApplicationID , UserID from dbo.UserStore where IsSuperUser = 1;

	--Insert Default Roles for application
	Exec dbo.InsertApplication_Role null, null, @ApplicationID, @Title='SuperAdmin'
	Exec dbo.InsertApplication_Role null, null, @ApplicationID, @Title='Admin'
	Exec dbo.InsertApplication_Role null, null, @ApplicationID, @Title='Agent'	
	COMMIT TRANSACTION			
END
