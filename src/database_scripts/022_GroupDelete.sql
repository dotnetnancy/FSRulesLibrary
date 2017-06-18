CREATE proc [dbo].[GroupDelete]
	@GroupID uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	Delete from dbo.Group_User where GroupID = @GroupID;

	Delete from dbo.[Group] where GroupID = @GroupID;
End
