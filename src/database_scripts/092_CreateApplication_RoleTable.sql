USE [GenericDotNetRulesStore]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Application_Role_Application]') AND parent_object_id = OBJECT_ID(N'[dbo].[Application_Role]'))
ALTER TABLE [dbo].[Application_Role] DROP CONSTRAINT [FK_Application_Role_Application]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Application_Role_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[Application_Role]'))
ALTER TABLE [dbo].[Application_Role] DROP CONSTRAINT [FK_Application_Role_Role]
GO

USE [GenericDotNetRulesStore]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Application_Role]') AND type in (N'U'))
DROP TABLE [dbo].[Application_Role]
GO

USE [GenericDotNetRulesStore]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Application_Role](
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[RoleID] [int] NOT NULL,
	CONSTRAINT [PK_Application_Role] PRIMARY KEY
		(
			ApplicationID
			, RoleID
		)		
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Application_Role]  WITH CHECK ADD  CONSTRAINT [FK_Application_Role_Application] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Application] ([ApplicationID])
GO

ALTER TABLE [dbo].[Application_Role] CHECK CONSTRAINT [FK_Application_Role_Application]
GO

ALTER TABLE [dbo].[Application_Role]  WITH CHECK ADD  CONSTRAINT [FK_Application_Role_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO

ALTER TABLE [dbo].[Application_Role] CHECK CONSTRAINT [FK_Application_Role_Role]
GO

