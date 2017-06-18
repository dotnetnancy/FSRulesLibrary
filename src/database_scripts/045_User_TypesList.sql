USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[User_TypesList]    Script Date: 09/15/2010 09:25:08 ******/
IF EXISTS(SELECT * FROM sysobjects WHERE NAME ='User_TypesList') 
	DROP PROC [User_TypesList]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[User_TypesList]
	@ApplicationID	uniqueidentifier,
	@UserID			uniqueidentifier
AS
Begin
	SET NOCOUNT ON;
		
	Select t.TypeID, t.TypeFullName
	from dbo.[Type] t inner join dbo.Type_User tu
		on t.TypeID = tu.TypeID
		inner join dbo.UserStore us 
		on us.UserID = tu.UserID
		inner join dbo.Application_User au
		on us.UserID = au.UserID 
		inner join dbo.[Application] a
		on a.ApplicationID = au.ApplicationID 
	Where
		us.UserID = @UserID;
End