CREATE TABLE [dbo].[UserRole](
	[UserID] [uniqueidentifier] NOT NULL,
	[RoleID] [int] NOT NULL,
	CONSTRAINT [PK_UserRole] PRIMARY KEY
	(
		UserID
		, RoleID
	)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO

ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Role]
GO

ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_UserStore] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserStore] ([UserID])
GO

ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_UserStore]
GO
