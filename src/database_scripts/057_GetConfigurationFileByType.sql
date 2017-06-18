CREATE PROCEDURE [dbo].[GetConfigurationFileByType]
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier]
AS
BEGIN
	SET NOCOUNT ON;
	
	Select TypeID, ApplicationID, ConfigurationTypeID, ConfigurationFile
	From ConfigurationFile 
	Where TypeID = @TypeID AND ApplicationID = @ApplicationID;	
END