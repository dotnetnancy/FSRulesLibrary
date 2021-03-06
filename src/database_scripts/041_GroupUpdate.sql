IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GroupUpdate') 
	DROP PROC [GroupUpdate]
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
CREATE PROCEDURE [dbo].[GroupUpdate]
	@GroupID UNIQUEIDENTIFIER
	, @GroupName NVARCHAR(50)
	, @IsRunTime BIT = NULL
	, @IsPreProcess BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT GroupID FROM dbo.[Group] WHERE GroupName = @GroupName AND GroupID != @GroupID)
	BEGIN
		RAISERROR('Groupname is already exists', 16,1);
		RETURN;
	END
	UPDATE dbo.[Group] 
		SET GroupName = @GroupName
			, IsRunTime = @IsRunTime
			, IsPreProcess = @IsPreProcess
	WHERE GroupID = @GroupID;
END
