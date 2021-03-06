
CREATE PROCEDURE [dbo].[User_Login]
	@Email varchar(150),
	@Password varchar(64),
	@ApplicationID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	Declare @UserID uniqueidentifier
	
	select @UserID = dbo.UserStore.UserID
	from dbo.UserStore inner join dbo.Application_User 
		on dbo.UserStore.UserID = dbo.Application_User.UserID
		inner join dbo.Application 
		on dbo.Application.ApplicationID = dbo.Application_User.ApplicationID
	where dbo.UserStore.Email = @Email
		and dbo.UserStore.Password = @Password
		and dbo.Application_User.ApplicationID = @ApplicationID;

	if @UserID is null
		begin
			raiserror('',16,1);
			return;
		end
	else
		begin
			update dbo.UserStore
			set dbo.UserStore.LastLogin = getutcdate()
			where dbo.UserStore.UserID = @UserID;
			
			Select @UserID UserID, @ApplicationID ApplicationID;
		end
END