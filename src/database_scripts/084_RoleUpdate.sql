CREATE PROC [dbo].[RoleUpdate]
	@RoleID		 int,
	@Title 		 [nvarchar](20),
	@Description [nvarchar](50),
	@Visible 	bit 
AS
Begin
	SET NOCOUNT ON;

	If exists(Select RoleID from dbo.[Role] where Title = @Title and RoleID != @RoleID)
	begin
		raiserror('Role title already exists', 16,1);
		return;
	end
	Update dbo.[Role] 
	Set Title = @Title,
		Description = @Description,
		Visible = @Visible
	where RoleID = @RoleID;
End
