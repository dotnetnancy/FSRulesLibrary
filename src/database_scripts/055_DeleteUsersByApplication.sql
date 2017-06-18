
CREATE PROCEDURE dbo.DeleteUsersByApplication
	@ApplicationID		uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
	
	Delete from dbo.Application_User where ApplicationID = @ApplicationID;	
End
GO