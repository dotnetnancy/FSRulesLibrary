/****** Object:  Table [dbo].[Group_User]    Script Date: 03/04/2010 11:08:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group_User](
	[GroupID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Group_User] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC,
	[UserID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
USE [GenericDotNetRulesStore]
GO
ALTER TABLE [dbo].[Group_User]  WITH CHECK ADD  CONSTRAINT [FK_Group_User_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([GroupID])
GO
ALTER TABLE [dbo].[Group_User]  WITH CHECK ADD  CONSTRAINT [FK_Group_User_UserStore] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserStore] ([UserID])