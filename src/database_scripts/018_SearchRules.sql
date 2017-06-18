ALTER PROC [dbo].[SearchRules]
	@TypeID uniqueidentifier,
	@ApplicationID uniqueidentifier,
	@CreatorID uniqueidentifier,
	@RuleName nvarchar(50) = null,
	@Paused varchar(2) = null
AS
Begin
	SET NOCOUNT ON;

	declare @pause varchar(1)
	declare @ruletable table
			( 
			RuleID uniqueidentifier, 
			TypeID uniqueidentifier, 
			ApplicationID uniqueidentifier,
			RuleName nvarchar(50), 
			Paused bit, 
			DateCreated datetime,
			CreatedBy uniqueidentifier,
			DateUpdated datetime,
			UpdatedBy uniqueidentifier
			)

	if @Paused is null set @Pause = 0;
		else
			begin
				if @Paused = '1' set @Pause = 1;
				else set @Pause = 0;
			end

	insert into @ruletable
		Select 
			rd.RuleID, rd.TypeID, rd.ApplicationID, rd.RuleName,
			rd.Paused, rd.DateCreated, rd.CreatedBy,
			rd.DateUpdated, rd.UpdatedBy
		from dbo.RuleDefinition rd
		Where rd.TypeID = @TypeID 
			and rd.ApplicationID = @ApplicationID 
			and rd.CreatedBy = @CreatorID
			and (@Paused is null or rd.Paused = @Paused)
			and (@rulename is null or rd.RuleName like '%' + @rulename + '%');

	Select distinct RuleID, TypeID, ApplicationID,RuleName, Paused, 
			DateCreated, CreatedBy,DateUpdated,UpdatedBy 
	from @ruletable;

End