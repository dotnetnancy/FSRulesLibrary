IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='UpdateRuleDefinitionByPrimaryKey') 
	DROP PROC [UpdateRuleDefinitionByPrimaryKey]
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

CREATE PROCEDURE [dbo].[UpdateRuleDefinitionByPrimaryKey]
	@RuleID UNIQUEIDENTIFIER
	, @TypeID UNIQUEIDENTIFIER
	, @ApplicationID UNIQUEIDENTIFIER
	, @RuleName NVARCHAR(50)
	, @Definition XML
	, @Paused BIT
	, @Priority INT = NULL
	, @UpdatedBy UNIQUEIDENTIFIER = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE RuleDefinition 
		SET RuleName = @RuleName
			, Definition = @Definition
			, Paused = @Paused
			, DateUpdated = getutcdate()
			, Priority = @Priority
			, UpdatedBy = @UpdatedBy 
	WHERE RuleID = @RuleID 
	And TypeID = @TypeID
	And ApplicationID = @ApplicationID
END