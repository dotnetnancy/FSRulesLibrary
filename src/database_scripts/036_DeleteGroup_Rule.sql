IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='DeleteGroup_Rule') 
	DROP PROC [DeleteGroup_Rule]
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
CREATE PROCEDURE [dbo].[DeleteGroup_Rule]
	@GroupID UNIQUEIDENTIFIER
	, @RuleID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM Group_RuleDefinition 
	WHERE GroupID = @GroupID
	AND RuleID = @RuleID

END 