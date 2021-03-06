USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[User_GroupsList]    Script Date: 07/27/2011 15:26:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[User_GroupsList]
	@UserID			uniqueidentifier,
	@ApplicationID	uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
		
	Select g.GroupID, g.GroupName
	from dbo.[Group] g inner join dbo.Group_User gu
		on g.GroupId = gu.GroupID
		inner join dbo.UserStore us 
		on us.UserID = gu.UserID
	Where
		us.UserID = @UserID
		and g.ApplicationID = @ApplicationID;
End


