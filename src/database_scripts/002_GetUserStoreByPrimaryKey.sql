set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[GetUserStoreByPrimaryKey]
	@UserID [uniqueidentifier],
	@ApplicationID uniqueidentifier
AS
Select us.UserID, us.FirstName, us.LastName, us.Email, us.Password, us.LastLogin, us.DateCreated, us.CreatedBy, us.IsSuperUser,
	a.ApplicationID, a.ApplicationName
From dbo.UserStore us inner join dbo.Application_User au 
	on us.UserID = au.UserID
	inner join dbo.Application a 
	on a.ApplicationID = au.ApplicationID
Where  us.UserID = @UserID  and 
		a.ApplicationID = @ApplicationID;

