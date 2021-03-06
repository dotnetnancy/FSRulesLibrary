USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[User_TypesList]    Script Date: 07/27/2011 09:54:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[User_TypesList]
	@ApplicationID	uniqueidentifier,
	@UserID			uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
	
	Select t.TypeID, t.TypeFullName
	From dbo.[Type] t inner join dbo.Type_User tu
		on t.TypeID = tu.TypeID
		inner join dbo.UserStore us
		on us.UserID = tu.UserID
		inner join dbo.Application_Type at
		on t.TypeID = at.TypeID
		inner join dbo.Application a
		on a.ApplicationID = at.ApplicationID 
	where
		us.UserID = @UserID
		and a.ApplicationID = @ApplicationID;	

	--Select t.TypeID, t.TypeFullName
	--from dbo.[Type] t inner join dbo.Type_User tu
	--	on t.TypeID = tu.TypeID
	--	inner join dbo.UserStore us 
	--	on us.UserID = tu.UserID
	--	inner join dbo.Application_User au
	--	on us.UserID = au.UserID 
	--	inner join dbo.[Application] a
	--	on a.ApplicationID = au.ApplicationID 
	--Where
	--	us.UserID = @UserID;
End