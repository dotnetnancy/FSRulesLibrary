USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[User_Login]    Script Date: 02/15/2011 14:46:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[User_Login]
	@Email varchar(150),
	@Password varchar(64),
	@ApplicationID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	Declare @UserID uniqueidentifier
	
	select @UserID = us.UserID
	from dbo.UserStore us left join dbo.SuperUserStore sus
		on us.UserID = sus.UserID
	where us.Email = @Email
		and us.Password = @Password
		and ((us.IsSuperUser = 1 and us.UserID in 
				(Select su.UserID from dbo.SuperUserStore su where su.ApplicationID = @ApplicationID))
			or (us.IsSuperUser = 0 and us.UserID in 
					(Select au.UserID 
					 from dbo.Application_User au inner join dbo.Application a
						on a.ApplicationID = au.ApplicationID 
						and us.UserID = au.UserID
					 where a.ApplicationID = @ApplicationID)));
					 
	--select @UserID = dbo.UserStore.UserID
	--from dbo.UserStore inner join dbo.Application_User 
	--	on dbo.UserStore.UserID = dbo.Application_User.UserID
	--	inner join dbo.Application 
	--	on dbo.Application.ApplicationID = dbo.Application_User.ApplicationID
	--where dbo.UserStore.Email = @Email
	--	and dbo.UserStore.Password = @Password
	--	and dbo.Application_User.ApplicationID = @ApplicationID;

	if @UserID is null
		begin
			raiserror('',16,1);
			return;
		end
	else
		begin
			update dbo.UserStore
			set dbo.UserStore.LastLogin = getutcdate()
			where dbo.UserStore.UserID = @UserID;
			
			Select @UserID UserID, @ApplicationID ApplicationID;
		end
END