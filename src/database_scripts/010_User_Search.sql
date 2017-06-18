CREATE PROC dbo.User_Search
	@ApplicationID	uniqueidentifier,
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
		N'@AID uniqueidentifier,
		@FN nvarchar(30),
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
			where au.ApplicationID = @AID ';
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
		where au.ApplicationID = @AID ';
	set @S = @S + @W;

	print @S;

	exec dbo.sp_executesql
		@S,@P,
		@AID = @ApplicationID,
		@FN = @FirstName,
		@LN = @LastName,
		@EM = @Email;

End