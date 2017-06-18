USE [GenericDotNetRulesStore]
GO

Create Proc [dbo].[Type_UserList]
	@TypeID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	Select 
		us.UserID, us.FirstName, us.LastName, us.Email
	from 
		dbo.UserStore us inner join dbo.Type_User tu 
		on us.UserID = tu.UserID
		inner join dbo.Type t
		on t.TypeID = tu.TypeID
	where
		tu.TypeID = @TypeID;
End
GO


