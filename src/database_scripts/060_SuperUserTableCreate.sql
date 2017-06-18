USE [GenericDotNetRulesStore]
GO

/****** Object:  Table [dbo].[SuperUserStore]    Script Date: 02/07/2011 11:46:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuperUserStore](
	[UserID] [uniqueidentifier] NOT NULL,
	[ApplicationID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SuperUserStore] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[ApplicationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuperUserStore]  WITH CHECK ADD  CONSTRAINT [FK_SuperUserStore_Application] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Application] ([ApplicationID])
GO

ALTER TABLE [dbo].[SuperUserStore] CHECK CONSTRAINT [FK_SuperUserStore_Application]
GO

ALTER TABLE [dbo].[SuperUserStore]  WITH CHECK ADD  CONSTRAINT [FK_SuperUserStore_UserStore] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserStore] ([UserID])
GO

ALTER TABLE [dbo].[SuperUserStore] CHECK CONSTRAINT [FK_SuperUserStore_UserStore]
GO


