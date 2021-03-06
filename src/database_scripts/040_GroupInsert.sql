IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GroupInsert') 
	DROP PROC [GroupInsert]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:		Phani Prathap
-- Create date: 
-- Updated By : Pranot Bhosale
-- Updated date: 04-01-2010
-- Description:	
-- ==========================================================================================
CREATE PROC [dbo].[GroupInsert]
	@GroupID UNIQUEIDENTIFIER = NULL OUTPUT
	, @ApplicationID UNIQUEIDENTIFIER
	, @GroupName NVARCHAR(50)
	, @CreatedBy UNIQUEIDENTIFIER
	, @IsRunTime BIT = NULL
	, @IsPreProcess BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT GroupID FROM dbo.[Group] WHERE GroupName = @GroupName)
	BEGIN
		RAISERROR('GroupName is already exists',16,1);
		RETURN
	END
	SET @GroupID = newid();	
	INSERT INTO dbo.[Group]
		(GroupID
		, ApplicationID
		, GroupName
		, CreatedBy
		, IsRunTime
		, IsPreProcess)
	VALUES(@GroupID
		, @ApplicationID
		, @GroupName
		, @CreatedBy
		, @IsRunTime
		, @IsPreProcess);	
END