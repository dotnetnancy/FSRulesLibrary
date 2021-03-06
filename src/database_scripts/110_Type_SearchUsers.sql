USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[Type_SearchUsers]    Script Date: 07/26/2011 14:52:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Type_SearchUsers]
	@TypeID			uniqueidentifier,
	@ApplicationID	uniqueidentifier,
	@Criteria		nvarchar(30)
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
		(select UserID from dbo.Type_User where TypeID = @TypeID)
		and us.UserID in
		(Select UserID from dbo.Application_User where ApplicationID = @ApplicationID)
	order by us.LastName, us.FirstName;
END

