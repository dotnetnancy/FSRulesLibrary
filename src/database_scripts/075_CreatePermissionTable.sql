CREATE TABLE [dbo].[Permission](
	[PermissionID] [varchar](4) NOT NULL,
	[Description] [varchar](40) NOT NULL,
	[Visible] [bit] NOT NULL,
	CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
	(
		[PermissionID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

