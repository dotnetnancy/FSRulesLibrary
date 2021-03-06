create PROC [dbo].[SearchRules]
	@TypeID uniqueidentifier,
	@ApplicationID uniqueidentifier,
	@CreatorID uniqueidentifier,
	@Top varchar(4),
	@Field varchar(50),
	@Direction varchar(4),
	@Row varchar(12)
AS
Begin
	SET NOCOUNT ON;

	Declare @S nvarchar(4000),
			@P nvarchar(1000);

	Set @P = N'
			@TypeID uniqueidentifier,
			@ApplicationID uniqueidentifier,
			@CreatorID uniqueidentifier';

	Set @S = N'
		Select top ' + @TOP + ' 
			RuleID, TypeID, ApplicationID, RuleName,
			Paused, DateCreated
		from
		(
			Select 
				Row_number() over(order by rd.' + @Field + ' ' + @Direction + ') RowNumber,
				rd.RuleID, rd.TypeID, rd.ApplicationID, rd.RuleName,
				rd.Definition, rd.Paused, rd.DateCreated, rd.CreatedBy,
				rd.DateUpdated, rd.UpdatedBy
			from dbo.RuleDefinition rd
			Where rd.TypeID = @TypeID 
				and rd.ApplicationID = @ApplicationID 
				and rd.CreatedBy = @CreatorID
				and rd.Deleted = 0
		) s
		where RowNumber>' + @ROW + ';'

	Set @S = @S + N'
			Select 
				count(rd.RuleID) TotalRows
			from dbo.RuleDefinition rd
			Where rd.TypeID = @TypeID 
				and rd.ApplicationID = @ApplicationID 
				and rd.CreatedBy = @CreatorID
				and rd.Deleted = 0;'

	Exec dbo.sp_executesql @S,@P, @TypeID, @ApplicationID, @CreatorID;

--	Select 
--		rd.RuleID, rd.TypeID, rd.ApplicationID, rd.RuleName,
--		rd.Definition, rd.Paused, rd.DateCreated, rd.CreatedBy,
--		rd.DateUpdated, rd.UpdatedBy
--	from dbo.RuleDefinition rd
--	Where rd.TypeID = @TypeID 
--		and rd.ApplicationID = @ApplicationID 
--		and rd.CreatedBy = @CreatorID
--		and rd.Deleted = 0;
End



