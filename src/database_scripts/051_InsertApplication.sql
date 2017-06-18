
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
	
	if @ApplicationID is null
		Set @ApplicationID = NEWID();
		
	Insert Into Application 
	( ApplicationID, ApplicationName, ApplicationDescription)
	Values ( @ApplicationID, @ApplicationName, @ApplicationDescription);
	
	Declare @UserID uniqueidentifier
	Exec dbo.InsertUserStore @UserID out, @ApplicationID, '','',@Email, @Pass, @CreatorID;
END
GO