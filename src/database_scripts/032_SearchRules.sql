IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='SearchRules') 
	DROP PROC [SearchRules]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ==========================================================================================
-- Author:		Phani Prathap
-- Create date: 
-- Updated By : Pranot Bhosale
-- Updated date: 03-25-2010
-- Description:	
-- ==========================================================================================
CREATE PROCEDURE [dbo].[SearchRules]
	@TypeID UNIQUEIDENTIFIER
	, @ApplicationID UNIQUEIDENTIFIER
	, @CreatorID UNIQUEIDENTIFIER
	, @RuleName NVARCHAR(50) = NULL
	, @Paused VARCHAR(2) = NULL
AS
Begin
	SET NOCOUNT ON;

	DECLARE @pause VARCHAR(1)
	IF @Paused IS NULL 
		SET @Pause = 0;
	ELSE
		BEGIN
			IF @Paused = '1' 
				SET @Pause = 1;
			ELSE
				SET @Pause = 0;
		END

	SELECT rd.RuleID
		, rd.TypeID
		, rd.ApplicationID
		, rd.RuleName
		, rd.Paused
		, rd.DateCreated
		, rd.Priority
		, rd.CreatedBy
		, rd.DateUpdated
		, rd.UpdatedBy
	FROM dbo.RuleDefinition rd
	WHERE rd.TypeID = @TypeID 
		AND rd.ApplicationID = @ApplicationID 
		AND rd.CreatedBy = @CreatorID
		AND (@Paused IS NULL OR rd.Paused = @Paused)
		AND (@rulename IS NULL OR rd.RuleName LIKE '%' + @rulename + '%');

END