ALTER PROCEDURE [dbo].[DeleteUserStoreByPrimaryKey]
	@UserID [uniqueidentifier]
AS
BEGIN
	SET NOCOUNT ON;

	Delete from dbo.Application_User
	where UserID = @UserID;
	
	Delete from dbo.Type_User
	where UserID = @UserID;

	Delete From UserStore 
	Where  UserID = @UserID  
END

