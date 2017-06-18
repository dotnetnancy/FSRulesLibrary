
CREATE PROC dbo.GetTypes
	@ApplicationID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	Select 
		t.TypeID, t.TypeFullName 
	from dbo.Type t inner join dbo.Application_Type at
		on t.TypeID = at.TypeID
		inner join dbo.Application a
		on a.ApplicationID = at.ApplicationID
	where 
		a.ApplicationID = @ApplicationID;
End
