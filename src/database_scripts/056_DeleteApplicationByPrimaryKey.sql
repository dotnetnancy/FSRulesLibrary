ALTER PROCEDURE [dbo].[DeleteApplicationByPrimaryKey]
	@ApplicationID [uniqueidentifier]
AS
Begin
	SET NOCOUNT ON;
	
	Exec dbo.DeleteUsersByApplication @ApplicationID;

	Delete From Application 
	Where  ApplicationID = @ApplicationID  
END