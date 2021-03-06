ALTER PROCEDURE [dbo].[InsertUserStore]
	@UserID [uniqueidentifier] out,
	@ApplicationID [uniqueidentifier],
	@FirstName [nvarchar](255),
	@LastName [nvarchar](255),
	@Email [varchar](150),
	@Password [varchar](64),
	@CreatedBy [uniqueidentifier]
AS
BEGIN
	SET NOCOUNT ON;

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
END
