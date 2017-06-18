IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='CopyRuleSetToGroup') 
	DROP PROC [CopyRuleSetToGroup]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ==========================================================================================
-- Author:		Pranot Bhosale
-- Create date: 03-30-2010
-- Updated date: 03-30-2010
-- Description:	
-- =========================================================================================
CREATE PROCEDURE [dbo].[CopyRuleSetToGroup]
	@GroupID UNIQUEIDENTIFIER
	, @TypeID UNIQUEIDENTIFIER
	, @ApplicationID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Group_RuleDefinition (GroupID, RuleID)
	SELECT DISTINCT @GroupID
	, RulD.RuleID
	FROM RuleDefinition AS RulD
	WHERE TypeID = @TypeID
	AND ApplicationID = @ApplicationID

END 