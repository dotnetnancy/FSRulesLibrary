
-- ==========================================================================================
-- Author:		Phani Prathap
-- Create date: 
-- Updated By : Pranot Bhosale
-- Updated date: 03-25-2010
-- Description:	
-- ==========================================================================================
ALTER PROCEDURE [dbo].[SearchRules]
	@TypeID UNIQUEIDENTIFIER
	, @ApplicationID UNIQUEIDENTIFIER
	, @CreatorID UNIQUEIDENTIFIER
	, @Group UNIQUEIDENTIFIER = NULL
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

--	SELECT rd.RuleID
--		, rd.TypeID
--		, rd.ApplicationID
--		, rd.RuleName
--		, rd.Paused
--		, rd.DateCreated
--		, rd.Priority
--		, rd.CreatedBy
--		, rd.DateUpdated
--		, rd.UpdatedBy
--	FROM dbo.RuleDefinition rd
--	WHERE rd.TypeID = @TypeID 
--		AND rd.ApplicationID = @ApplicationID 
--		AND rd.CreatedBy = @CreatorID
--		AND (@Paused IS NULL OR rd.Paused = @Paused)
--		AND (@rulename IS NULL OR rd.RuleName LIKE '%' + @rulename + '%')
--		AND rd.Deleted = 0
--	ORDER BY rd.Priority;

--	Phani : Modified to filter rules by Group 05/05/2010
	SELECT distinct rd.RuleID
		, rd.TypeID
		, rd.ApplicationID
		, rd.RuleName
		, rd.Paused
		, rd.DateCreated
		, rd.Priority
		, rd.CreatedBy
		, rd.DateUpdated
		, rd.UpdatedBy
	FROM dbo.RuleDefinition rd left join dbo.Group_RuleDefinition grd
		on rd.RuleID = grd.RuleID
	WHERE rd.TypeID = @TypeID 
		AND rd.ApplicationID = @ApplicationID 
		AND rd.CreatedBy = @CreatorID
		AND (@Paused IS NULL OR rd.Paused = @Paused)
		AND (@rulename IS NULL OR rd.RuleName LIKE '%' + @rulename + '%')
		AND rd.Deleted = 0
		AND ( (@Group is null)
			  OR (@Group = CAST(cast(0 as binary) as uniqueidentifier) and rd.RuleID not in(Select RuleID from dbo.Group_RuleDefinition))
			  OR (@Group is not NULL and @Group != CAST(cast(0 as binary) as uniqueidentifier) and grd.GroupID = @Group))
	ORDER BY rd.Priority;
END

