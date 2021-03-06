CREATE PROCEDURE [dbo].[User_UpdatePassword]
	@UserID uniqueidentifier,
	@OldPass varchar(64),
	@NewPass varchar(64)
AS
BEGIN
	SET NOCOUNT ON;
	if not exists
	(
		select dbo.UserStore.UserID
		from dbo.UserStore
		where dbo.UserStore.UserID = @UserID
			and dbo.UserStore.Password = @OldPass
	)
	begin
		raiserror('',16,1);
		return;
	end
	update dbo.UserStore
	set dbo.UserStore.Password = @NewPass
	where dbo.UserStore.UserID = @UserID;
END

