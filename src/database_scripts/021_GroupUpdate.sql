CREATE PROC [dbo].[GroupUpdate]
	@GroupID uniqueidentifier,
	@GroupName nvarchar(50)
AS
Begin
	SET NOCOUNT ON;

	If exists(Select GroupID from dbo.[Group] where GroupName = @GroupName and GroupID != @GroupID)
	begin
		raiserror('Groupname is already exists', 16,1);
		return;
	end
	Update dbo.[Group] Set GroupName = @GroupName
	where GroupID = @GroupID;
End
