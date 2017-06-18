ALTER PROCEDURE [dbo].[InsertType_User]
	@TypeID [uniqueidentifier],
	@UserID [uniqueidentifier]
AS
BEGIN
	SET NOCOUNT ON;

	Insert Into Type_User 
	( TypeID, UserID)
	Values ( @TypeID, @UserID) 
	
	Exec dbo.GetUserTypes @UserID;
END

