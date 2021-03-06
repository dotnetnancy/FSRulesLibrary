USE [GenericDotNetRulesStore]
GO
/****** Object:  StoredProcedure [dbo].[GetUserTypes]    Script Date: 07/20/2011 11:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[GetUserTypes] 
	@UserID			uniqueidentifier,
	@ApplicationID	uniqueidentifier
AS
Begin
	SET NOCOUNT ON;

	If( (Select IsSuperUser from UserStore where UserID = @UserID) = 1)
	begin
		Select
			t.TypeID,
			t.TypeFullName
		From dbo.Type t inner join dbo.Application_Type at
			on t.TypeID = at.TypeID	
			inner join dbo.Application a
			on a.ApplicationID = at.ApplicationID
		where
			a.ApplicationID = @ApplicationID;
	End
	else
	Begin
		Select 
				dbo.Type.TypeID,
				dbo.Type.TypeFullName
			From dbo.Type inner join dbo.Type_User
				on dbo.Type.TypeID = dbo.Type_User.TypeID
			where
				dbo.Type_User.UserID =@UserID;
	End
End
