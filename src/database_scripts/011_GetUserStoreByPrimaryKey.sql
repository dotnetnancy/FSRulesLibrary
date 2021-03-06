
ALTER PROCEDURE [dbo].[GetUserStoreByPrimaryKey]
	@UserID [uniqueidentifier],
	@ApplicationID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	Select us.UserID, us.FirstName, us.LastName, us.Email, us.Password, us.LastLogin, us.DateCreated, us.CreatedBy, us.IsSuperUser,
		a.ApplicationID, a.ApplicationName,
		isnull(crt.FirstName,'Unknown') as CreatorFirstName,
		isnull(crt.LastName,'Unknown') as CreatorLastName
	From dbo.UserStore us inner join dbo.Application_User au 
		on us.UserID = au.UserID
		inner join dbo.Application a 
		on a.ApplicationID = au.ApplicationID
		inner join dbo.UserStore crt 
		on us.CreatedBy = crt.UserID
	Where  us.UserID = @UserID  and 
			a.ApplicationID = @ApplicationID;
END

