-- ============================================================================================
-- Author:		Pranot Bhosale
-- Create date: 03-30-2010
-- Updated date: 03-30-2010
-- Description:	We haven't added FOREIGN KEY constraint on RuleId column because RuleDefinition
--			table has candidate Key as RuleID, TypeID and ApplicationID 
-- ============================================================================================
CREATE TABLE Group_RuleDefinition 
(
	GroupID UNIQUEIDENTIFIER CONSTRAINT [FK_Group_RuleDefinition_Group] REFERENCES [Group](GroupId)
	, RuleID UNIQUEIDENTIFIER 
	, CONSTRAINT [PK_Group_RuleDefinition] PRIMARY KEY
		(
			GroupID
			, RuleID
		)
)