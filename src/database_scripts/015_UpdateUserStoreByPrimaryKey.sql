ALTER PROCEDURE [dbo].[UpdateUserStoreByPrimaryKey]
	@UserID [uniqueidentifier],
	@FirstName [nvarchar](255),
	@LastName [nvarchar](255),
	@Email [varchar](150)
AS
BEGIN
	SET NOCOUNT ON;

	if exists (select dbo.UserStore.UserID from dbo.UserStore where dbo.UserStore.Email = @Email and dbo.UserStore.UserID <> @UserID)
		begin
			raiserror('',16,1);
			return;
		end

	Update UserStore 
	Set FirstName = @FirstName, 
		LastName = @LastName, 
		Email = @Email
	Where  UserID = @UserID;
END


