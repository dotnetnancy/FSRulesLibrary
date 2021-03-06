ALTER PROCEDURE [dbo].[DeleteType_UserByPrimaryKey]
	@TypeID [uniqueidentifier],
	@UserID [uniqueidentifier]
AS
BEGIN
	SET NOCOUNT ON;

	Delete From Type_User 
	Where (( TypeID = @TypeID ) 
	And ( UserID = @UserID ) 
	)

	Exec dbo.GetUserTypes @UserID;
END

