IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='InsertGroup_Rule') 
	DROP PROC [InsertGroup_Rule]
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
CREATE PROCEDURE [dbo].[InsertGroup_Rule]
	@GroupID UNIQUEIDENTIFIER
	, @RuleID UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Group_RuleDefinition (GroupID, RuleID)
	VALUES(@GroupID, @RuleID)

END 