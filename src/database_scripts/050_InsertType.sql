USE [GenericDotNetRulesStore]
GO

/****** Object:  StoredProcedure [dbo].[InsertType]    Script Date: 10/28/2010 13:24:01 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[InsertType]
	@TypeID			uniqueidentifier = NULL OUTPUT,
	@TypeFullName	nvarchar(255),
	@ApplicationID	uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS(SELECT TypeID FROM dbo.[Type] WHERE TypeFullName = @TypeFullName)
	BEGIN
		RAISERROR('TypeName is already exists',16,1);
		RETURN
	END
	SET @TypeID = newid();	
	Insert Into Type ( TypeID, TypeFullName)
	Values ( @TypeID, @TypeFullName); 
	
	Exec dbo.InsertApplication_Type @ApplicationID, @TypeID ;
END
GO


