USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[SearchNonSuperUsers]    Script Date: 02/09/2011 12:54:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SearchNonSuperUsers]
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
		(select UserID from dbo.SuperUserStore where ApplicationID = @ApplicationID)
		and us.IsSuperUser <> 1
	order by us.LastName, us.FirstName;
END
