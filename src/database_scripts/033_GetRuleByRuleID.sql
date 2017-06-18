IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='GetRuleByRuleID') 
	DROP PROC [GetRuleByRuleID]
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
CREATE PROC dbo.GetRuleByRuleID
	@RuleID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	SELECT
		RulD.RuleID
		, RulD.RuleName
		, RulD.Definition
		, RulD.Paused
		, RulD.DateCreated
		, RulD.DateUpdated
		, RulD.Priority
		, ISNULL(UsrS.FirstName,'Unknown') AS CreatorFirstName
		, ISNULL(UsrS.LastName,'Unknown') AS CreatorLastName
		, ISNULL(UsrSt.FirstName,'Unknown') AS ModifierFirstName
		, ISNULL(UsrSt.LastName,'Unknown') AS ModifierLastName
	FROM RuleDefinition AS RulD
		LEFT OUTER JOIN dbo.UserStore AS UsrS
				ON RulD.CreatedBy = UsrS.UserID
		LEFT OUTER JOIN dbo.UserStore AS UsrSt
				ON RulD.UpdatedBy = UsrSt.UserID
	WHERE RulD.RuleID = @RuleID;

END