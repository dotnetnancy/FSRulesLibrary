IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GroupList') 
	DROP PROC [GroupList]
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
CREATE PROCEDURE [dbo].[GroupList]
	@ApplicationID UNIQUEIDENTIFIER
AS
BEGIN
	SELECT GroupID
		, GroupName
		, CreatedDate
		, CreatedBy 
		, IsRunTime
		, IsPreProcess
	FROM dbo.[Group] 
	WHERE ApplicationID = @ApplicationID
	ORDER BY CreatedDate ASC;
END

