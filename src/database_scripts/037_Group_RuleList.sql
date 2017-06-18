IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='Group_RuleList') 
	DROP PROC [Group_RuleList]
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
CREATE PROCEDURE [dbo].[Group_RuleList]
	@RuleID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	SELECT GrpR.GroupID
		, Grp.GroupName
	FROM Group_RuleDefinition AS GrpR
		INNER JOIN [Group] AS Grp
			ON GrpR.GroupID = Grp.GroupID
	WHERE GrpR.RuleID = @RuleID

END 