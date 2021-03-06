CREATE PROC [dbo].[GroupInsert]
	@GroupID uniqueidentifier = null out,
	@ApplicationID uniqueidentifier,
	@GroupName nvarchar(50),
	@CreatedBy uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	If exists(Select GroupID from dbo.[Group] where GroupName = @GroupName)
	begin
		raiserror('GroupName is already exists',16,1);
		return;
	end
	Set @GroupID = newid();	
	INSERT INTO dbo.[Group](GroupID,ApplicationID,GroupName,CreatedBy) VALUES(@GroupID,@ApplicationID,@GroupName,@CreatedBy);	
End

