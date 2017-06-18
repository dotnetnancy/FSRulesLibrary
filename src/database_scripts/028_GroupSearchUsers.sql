
CREATE PROCEDURE [dbo].[Group_SearchUsers]
	@GroupID uniqueidentifier,
	@ApplicationID uniqueidentifier,
	@Criteria nvarchar(30)
AS
BEGIN
	SET NOCOUNT ON;
	select
		us.UserID,
		us.FirstName,
		us.LastName,
		us.Email
	from dbo.UserStore us
	where (us.FirstName like '%'+dbo.uf_EscapeSearch(@Criteria)+'%'
		or us.LastName like '%'+dbo.uf_EscapeSearch(@Criteria)+'%'
		or us.Email like '%'+dbo.uf_EscapeSearch(@Criteria)+'%')
		and us.UserID not in
		(select UserID from dbo.Group_User where GroupID = @GroupID)
	order by us.LastName, us.FirstName;
END
