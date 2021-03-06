USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[DeleteTypeByPrimaryKey]    Script Date: 07/20/2011 15:51:13 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[DeleteTypeByPrimaryKey]
	@TypeID [uniqueidentifier]
AS
Begin
	SET NOCOUNT ON
	
	BEGIN TRANSACTION
	
	Delete from dbo.Application_Type where TypeID = @TypeID
	
	Delete from dbo.Type_User where TypeID = @TypeID
	
	Delete from dbo.ConfigurationFile where TypeID = @TypeID
	
	Delete from [dbo].[RuleDefinition] where TypeID = @TypeID
	
	Delete from [dbo].[Type] where TypeID = @TypeID 
	
	COMMIT TRANSACTION
End
