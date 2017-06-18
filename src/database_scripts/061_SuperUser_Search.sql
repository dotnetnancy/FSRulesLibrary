USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[SuperUser_Search]    Script Date: 02/02/2011 15:33:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SuperUser_Search]
	@FirstName nvarchar(30) = null,
	@LastName nvarchar(30) = null,
	@Email varchar(150) = null,
	@Top varchar(4),
	@Field varchar(50),
	@Direction varchar(4),
	@Row varchar(12)
AS
Begin
	SET NOCOUNT ON;

	declare @S nvarchar(4000),@W nvarchar(4000),@P nvarchar(1000);
	declare @F varchar(6),@L varchar(6);
	
	set @F = '%';
	set @L = '%';
	set @W = '';
	set @P = 
		N'@FN nvarchar(30),
		@LN nvarchar(30),
		@EM varchar(150)';
	set @S =
		N'
		select top ' + @Top + N' * from
		(
			select
				row_number() over
				(
					order by
						dbo.UserStore.' + @Field + N' ' + @Direction + N',
						dbo.UserStore.DateCreated ' + @Direction + N'
				) as RowNumber,
				dbo.UserStore.UserID,
				dbo.UserStore.FirstName,
				dbo.UserStore.LastName,
				dbo.UserStore.Email
			from dbo.UserStore inner join dbo.Application_User au
				on dbo.UserStore.UserID = au.UserID
				inner join dbo.Application a
				on a.ApplicationID = au.ApplicationID
				inner join dbo.SuperUserStore sus
				on a.ApplicationID = sus.ApplicationID
				and dbo.UserStore.UserID = sus.UserID
			where dbo.UserStore.IsSuperUser = 1 ';
	if @FirstName is not null
		set @W = @W + N' and dbo.UserStore.FirstName like ''' + @F + N''' + dbo.uf_EscapeSearch(@FN) + ''' + @L + N''' '
	if @LastName is not null
		set @W = @W + N' and dbo.UserStore.LastName like ''' + @F + N''' + dbo.uf_EscapeSearch(@LN) + ''' + @L + N''' '
	if @Email is not null
		set @W = @W + N' and dbo.UserStore.Email like ''' + @F + N''' + dbo.uf_EscapeSearch(@EM) + ''' + @L + N''' '
	set @S = @S + @W;
	set @S = @S + N')
		searchResults where searchResults.RowNumber > ' + @Row + N';
		select count(dbo.UserStore.UserID) as TotalRows
		from dbo.UserStore inner join dbo.Application_User au
			on dbo.UserStore.UserID = au.UserID
			inner join dbo.Application a
			on a.ApplicationID = au.ApplicationID
			inner join dbo.SuperUserStore sus
			on a.ApplicationID = sus.ApplicationID
			and dbo.UserStore.UserID = sus.UserID
		where dbo.UserStore.IsSuperUser = 1  ';
	set @S = @S + @W;

	print @S;

	exec dbo.sp_executesql
		@S,@P,
		@FN = @FirstName,
		@LN = @LastName,
		@EM = @Email;

End