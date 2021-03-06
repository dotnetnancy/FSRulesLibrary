USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[UpdateApplicationByPrimaryKey]    Script Date: 11/03/2010 11:44:58 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[UpdateApplicationByPrimaryKey]
	@ApplicationID [uniqueidentifier],
	@ApplicationName [nvarchar](50),
	@ApplicationDescription [nvarchar](255)
AS
BEGIN
	SET NOCOUNT ON;
	
	if exists (Select ApplicationID from Application where ApplicationName = @ApplicationName and ApplicationID <> @ApplicationID)
	begin
		raiserror('Application with this name already exists.',16,1);
		return;
	end
	
	Update Application 
	Set ApplicationName = @ApplicationName, 
	ApplicationDescription = @ApplicationDescription 
	Where  ApplicationID = @ApplicationID  
END
