CREATE PROC dbo.GetUserTypes 
	@UserID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	Select 
		dbo.Type.TypeID,
		dbo.Type.TypeFullName
	From dbo.Type inner join dbo.Type_User
		on dbo.Type.TypeID = dbo.Type_User.TypeID
	where
		dbo.Type_User.UserID =@UserID;
End
