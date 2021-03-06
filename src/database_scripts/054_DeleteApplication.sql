CREATE PROCEDURE [dbo].[DeleteApplication]
	@ApplicationID	uniqueidentifier,
	@UserID			uniqueidentifier,
	@Password		varchar(64)
AS
BEGIN
	SET NOCOUNT ON;
	if not exists(select UserID from dbo.UserStore where UserID = @UserID and Password = @Password)
	begin
		raiserror('',16,1);
		return;
	end
	
	Exec dbo.DeleteUsersByApplication @ApplicationID;
	
	delete from dbo.Application where ApplicationID = @ApplicationID;
END
