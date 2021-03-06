USE [master]
GO
/****** Object:  Database [GenericDotNetRulesStore]    Script Date: 10/28/2009 12:46:48 ******/
CREATE DATABASE [GenericDotNetRulesStore] ON  PRIMARY 
( NAME = N'GenericDotNetRulesStore', FILENAME = N'C:\GenericDotNetRulesStore.mdf' , SIZE = 280576KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'GenericDotNetRulesStore_log', FILENAME = N'C:\GenericDotNetRulesStore_1.ldf' , SIZE = 6912KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'GenericDotNetRulesStore', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GenericDotNetRulesStore].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [GenericDotNetRulesStore] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET ARITHABORT OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET  READ_WRITE 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET  MULTI_USER 
GO
ALTER DATABASE [GenericDotNetRulesStore] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GenericDotNetRulesStore] SET DB_CHAINING OFF 

GO

use [GenericDotNetRulesStore]
GO


use GenericDotNetRulesStore
GO
/****** Object:  User [GenericRulesUser]    Script Date: 12/07/2010 13:11:57 ******/


use GenericDotNetRulesStore
GO
/****** Object:  User [GenericRulesUser]    Script Date: 12/07/2010 13:11:57 ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'GenericRulesUser')
	CREATE LOGIN [GenericRulesUser] WITH PASSWORD = 'C3ntr@lRul3sUs3r';
GO
	
use GenericDotNetRulesStore
CREATE USER [GenericRulesUser] FOR LOGIN [GenericRulesUser] WITH DEFAULT_SCHEMA = dbo;
GO




use GenericDotNetRulesStore
GRANT SELECT ON SCHEMA :: dbo TO GenericRulesUser WITH GRANT OPTION;
go

use GenericDotNetRulesStore
GRANT EXECUTE ON SCHEMA ::dbo TO GenericRulesUser WITH GRANT OPTION;
go

--use GenericDotNetRulesStore
--GRANT INSERT ON SCHEMA ::dbo TO GenericRulesUser WITH GRANT OPTION;
--go
--
--use GenericDotNetRulesStore
--GRANT UPDATE ON SCHEMA ::dbo TO GenericRulesUser WITH GRANT OPTION;
--go
--
--use GenericDotNetRulesStore
--GRANT DELETE ON SCHEMA ::dbo TO GenericRulesUser WITH GRANT OPTION;
--go


IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'GenericRulesUser')
CREATE USER [GenericRulesUser] FOR LOGIN [GenericRulesUser] WITH DEFAULT_SCHEMA=[dbo]
GO


GRANT EXECUTE ON SCHEMA::dbo TO GenericRulesUser;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Application]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Application](
	[ApplicationID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Application_ApplicationID]  DEFAULT (newid()),
	[ApplicationName] [nvarchar](50) NOT NULL,
	[ApplicationDescription] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [Unique_Constraint_Application_Name] UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Type]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Type](
	[TypeID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Type_TypeID]  DEFAULT (newid()),
	[TypeFullName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [Unique_Constraint_TypeFullName] UNIQUE NONCLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigurationType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConfigurationType](
	[ConfigurationTypeID] [int] NOT NULL,
	[ConfigurationTypeDescription] [char](4) NOT NULL,
 CONSTRAINT [PK_ConfigurationType] PRIMARY KEY CLUSTERED 
(
	[ConfigurationTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [Unique_Constraint_ConfigurationType] UNIQUE NONCLUSTERED 
(
	[ConfigurationTypeDescription] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserStore]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserStore](
	[UserID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UserStore_UserID]  DEFAULT (newid()),
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[Password] [varchar](64) NOT NULL,
	[LastLogin] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_UserStore_DateCreated]  DEFAULT (getdate()),
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[IsSuperUser] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInformationSchemaTableConstraints]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetInformationSchemaTableConstraints]
	@TableName [nvarchar](255) = null
AS



   select tc.*
	from Information_Schema.Tables t
	inner join Information_Schema.Table_Constraints tc
	on t.Table_Name = tc.Table_Name
	where (t.Table_Name = @TableName or @TableName is null)
	and t.Table_Type IN (''BASE TABLE'') 
	and t.Table_Name <> ''dtproperties''
    and t.Table_Name <> ''sysdiagrams''

	Order by t.Table_Name	

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInformationSchemaColumns]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetInformationSchemaColumns]
	@TableName [nvarchar](255) = null
AS


  
	SELECT c.*
	FROM Information_Schema.Tables t
	inner join Information_Schema.Columns c
	on c.Table_Name = t.Table_Name
	--WHERE t.TABLE_TYPE IN (''BASE TABLE'', ''VIEW'') 
	WHERE (t.Table_Name = @TableName or @TableName is null)
	and t.TABLE_TYPE IN (''BASE TABLE'') 
	and t.Table_Name <> ''dtproperties''
    and t.Table_Name <> ''sysdiagrams''

	ORDER BY t.TABLE_NAME

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInformationSchemaColumnUsage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetInformationSchemaColumnUsage]
	@TableName [nvarchar](255) = null
AS
select ccu.*
from Information_Schema.Tables t
inner join Information_Schema.Constraint_Column_Usage ccu
on ccu.Table_Name = t.Table_Name
where (t.Table_Name = @TableName or @TableName is null)
	and t.Table_Type IN (''BASE TABLE'') 
	and t.Table_Name <> ''dtproperties''
    and t.Table_Name <> ''sysdiagrams''

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RuleStatistic]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RuleStatistic](
	[RuleStatisticID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_RuleStatistic_RuleStatisticID]  DEFAULT (newid()),
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[TypeID] [uniqueidentifier] NOT NULL,
	[RuleID] [uniqueidentifier] NOT NULL,
	[RuleName] [nvarchar](50) NOT NULL,
	[DateInserted] [datetime] NOT NULL CONSTRAINT [DF_RuleStatistic_DateInserted]  DEFAULT (getdate()),
	[ReferenceID] [uniqueidentifier] NULL,
	[Result] [bit] NOT NULL,
 CONSTRAINT [PK_RuleStatistic] PRIMARY KEY CLUSTERED 
(
	[RuleStatisticID] ASC,
	[ApplicationID] ASC,
	[TypeID] ASC,
	[RuleID] ASC,
	[DateInserted] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInformationSchemaTables]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetInformationSchemaTables]
	@TableName [nvarchar](255) = null
AS


  SELECT t.*
	from Information_Schema.Tables t
where (t.Table_Name = @TableName or @TableName is null)
and t.Table_Name <> ''dtproperties''
and t.Table_Name <> ''sysdiagrams''

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsIdentity]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[IsIdentity]
	@TableName [nvarchar](255) = null,
	@ColumnName [nvarchar](255) = null,
	@IsIdentity [bit] = 0 OUTPUT
AS
if exists(select *
		from information_schema.columns 
		where 
		table_schema = ''dbo'' 
		and columnproperty(object_id(@TableName), @ColumnName,''IsIdentity'') = 1 
		)
			set @IsIdentity = 1
else
			set @IsIdentity = 0' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateApplication_TypeByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateApplication_TypeByPrimaryKey]
	@ApplicationID [uniqueidentifier],
	@TypeID [uniqueidentifier]
AS
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateType_UserByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateType_UserByPrimaryKey]
	@TypeID [uniqueidentifier],
	@UserID [uniqueidentifier]
AS
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateApplication_UserByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateApplication_UserByPrimaryKey]
	@ApplicationID [uniqueidentifier],
	@UserID [uniqueidentifier]
AS
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Query]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Query](
	[RuleID] [uniqueidentifier] NOT NULL,
	[TypeID] [varchar](36) NOT NULL,
	[ApplicationID] [varchar](36) NOT NULL,
	[RuleName] [nvarchar](50) NOT NULL,
	[Definition] [ntext] NOT NULL,
	[Paused] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[CreatedBy] [varchar](36) NOT NULL,
	[Deleted] [bit] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[UpdatedBy] [varchar](36) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RuleDefinition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RuleDefinition](
	[RuleID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_RuleDefinition_RuleID]  DEFAULT (newid()),
	[TypeID] [uniqueidentifier] NOT NULL,
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[RuleName] [nvarchar](50) NOT NULL,
	[Definition] [xml] NOT NULL,
	[Paused] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_RuleDefinition_DateCreated]  DEFAULT (getdate()),
	[CreatedBy] [uniqueidentifier] NULL,
	[Deleted] [bit] NOT NULL,
	[DateUpdated] [datetime] NOT NULL CONSTRAINT [DF_RuleDefinition_DateUpdated]  DEFAULT (getdate()),
	[UpdatedBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[RuleID] ASC,
	[TypeID] ASC,
	[ApplicationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Application_User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Application_User](
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Application_User] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Application_Type]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Application_Type](
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[TypeID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Application_Type] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC,
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Type_User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Type_User](
	[TypeID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Type_User] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigurationFile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConfigurationFile](
	[TypeID] [uniqueidentifier] NOT NULL,
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[ConfigurationTypeID] [int] NOT NULL,
	[ConfigurationFile] [xml] NOT NULL,
 CONSTRAINT [PK_ConfigurationFile] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC,
	[ApplicationID] ASC,
	[ConfigurationTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplicationByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplicationByPrimaryKey]
	@ApplicationID [uniqueidentifier]
AS
Select ApplicationID, ApplicationName, ApplicationDescription
From Application 
Where  ApplicationID = @ApplicationID  
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateApplicationByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateApplicationByPrimaryKey]
	@ApplicationID [uniqueidentifier],
	@ApplicationName [nvarchar](50),
	@ApplicationDescription [nvarchar](255)
AS
Update Application 
Set ApplicationName = @ApplicationName, 
ApplicationDescription = @ApplicationDescription 
Where  ApplicationID = @ApplicationID  

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteApplicationByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteApplicationByPrimaryKey]
	@ApplicationID [uniqueidentifier]
AS
Delete From Application 
Where  ApplicationID = @ApplicationID  

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplication]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplication]
AS
Select ApplicationID, ApplicationName, ApplicationDescription
From Application' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplicationByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplicationByCriteriaFuzzy]
	@ApplicationID [uniqueidentifier] = null,
	@ApplicationName [nvarchar](50) = null,
	@ApplicationDescription [nvarchar](255) = null
AS
Select ApplicationID, ApplicationName, ApplicationDescription
From Application 
Where ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( ApplicationName Like @ApplicationName + ''%'' Or @ApplicationName = null ) 
And ( ApplicationDescription Like @ApplicationDescription + ''%'' Or @ApplicationDescription = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplicationByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplicationByCriteriaExact]
	@ApplicationID [uniqueidentifier] = null,
	@ApplicationName [nvarchar](50) = null,
	@ApplicationDescription [nvarchar](255) = null
AS
Select ApplicationID, ApplicationName, ApplicationDescription
From Application 
Where ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( ApplicationName = @ApplicationName Or @ApplicationName = null ) 
And ( ApplicationDescription = @ApplicationDescription Or @ApplicationDescription = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertApplication]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertApplication]
	@ApplicationID [uniqueidentifier] OUTPUT,
	@ApplicationName [nvarchar](50),
	@ApplicationDescription [nvarchar](255)
AS
Insert Into Application 
( ApplicationID, ApplicationName, ApplicationDescription)
Values ( @ApplicationID, @ApplicationName, @ApplicationDescription) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetTypeByPrimaryKey]
	@TypeID [uniqueidentifier]
AS
Select TypeID, TypeFullName
From Type 
Where  TypeID = @TypeID  
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateTypeByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateTypeByPrimaryKey]
	@TypeID [uniqueidentifier],
	@TypeFullName [nvarchar](255)
AS
Update Type 
Set TypeFullName = @TypeFullName 
Where  TypeID = @TypeID  

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteTypeByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteTypeByPrimaryKey]
	@TypeID [uniqueidentifier]
AS
Delete From Type 
Where  TypeID = @TypeID  

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetType]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetType]
AS
Select TypeID, TypeFullName
From Type' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetTypeByCriteriaFuzzy]
	@TypeID [uniqueidentifier] = null,
	@TypeFullName [nvarchar](255) = null
AS
Select TypeID, TypeFullName
From Type 
Where ( TypeID = @TypeID Or @TypeID = null ) 
And ( TypeFullName Like @TypeFullName + ''%'' Or @TypeFullName = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTypeByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetTypeByCriteriaExact]
	@TypeID [uniqueidentifier] = null,
	@TypeFullName [nvarchar](255) = null
AS
Select TypeID, TypeFullName
From Type 
Where ( TypeID = @TypeID Or @TypeID = null ) 
And ( TypeFullName = @TypeFullName Or @TypeFullName = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertType]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertType]
	@TypeID [uniqueidentifier] OUTPUT,
	@TypeFullName [nvarchar](255)
AS
Insert Into Type 
( TypeID, TypeFullName)
Values ( @TypeID, @TypeFullName) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplication_TypeByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplication_TypeByPrimaryKey]
	@ApplicationID [uniqueidentifier],
	@TypeID [uniqueidentifier]
AS
Select ApplicationID, TypeID
From Application_Type 
Where (( ApplicationID = @ApplicationID ) 
And ( TypeID = @TypeID ) 
)' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteApplication_TypeByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteApplication_TypeByPrimaryKey]
	@ApplicationID [uniqueidentifier],
	@TypeID [uniqueidentifier]
AS
Delete From Application_Type 
Where (( ApplicationID = @ApplicationID ) 
And ( TypeID = @TypeID ) 
)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplication_Type]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplication_Type]
AS
Select ApplicationID, TypeID
From Application_Type' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplication_TypeByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplication_TypeByCriteriaFuzzy]
	@ApplicationID [uniqueidentifier] = null,
	@TypeID [uniqueidentifier] = null
AS
Select ApplicationID, TypeID
From Application_Type 
Where ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( TypeID = @TypeID Or @TypeID = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplication_TypeByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplication_TypeByCriteriaExact]
	@ApplicationID [uniqueidentifier] = null,
	@TypeID [uniqueidentifier] = null
AS
Select ApplicationID, TypeID
From Application_Type 
Where ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( TypeID = @TypeID Or @TypeID = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertApplication_Type]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertApplication_Type]
	@ApplicationID [uniqueidentifier] OUTPUT,
	@TypeID [uniqueidentifier] OUTPUT
AS
Insert Into Application_Type 
( ApplicationID, TypeID)
Values ( @ApplicationID, @TypeID) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetConfigurationTypeByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetConfigurationTypeByPrimaryKey]
	@ConfigurationTypeID [int]
AS
Select ConfigurationTypeID, ConfigurationTypeDescription
From ConfigurationType 
Where  ConfigurationTypeID = @ConfigurationTypeID  
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateConfigurationTypeByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateConfigurationTypeByPrimaryKey]
	@ConfigurationTypeID [int],
	@ConfigurationTypeDescription [char](4)
AS
Update ConfigurationType 
Set ConfigurationTypeDescription = @ConfigurationTypeDescription 
Where  ConfigurationTypeID = @ConfigurationTypeID  

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteConfigurationTypeByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteConfigurationTypeByPrimaryKey]
	@ConfigurationTypeID [int]
AS
Delete From ConfigurationType 
Where  ConfigurationTypeID = @ConfigurationTypeID  

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetConfigurationType]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetConfigurationType]
AS
Select ConfigurationTypeID, ConfigurationTypeDescription
From ConfigurationType' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetConfigurationTypeByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetConfigurationTypeByCriteriaFuzzy]
	@ConfigurationTypeID [int] = null,
	@ConfigurationTypeDescription [char](4) = null
AS
Select ConfigurationTypeID, ConfigurationTypeDescription
From ConfigurationType 
Where ( ConfigurationTypeID = @ConfigurationTypeID Or @ConfigurationTypeID = null ) 
And ( ConfigurationTypeDescription Like @ConfigurationTypeDescription + ''%'' Or @ConfigurationTypeDescription = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetConfigurationTypeByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetConfigurationTypeByCriteriaExact]
	@ConfigurationTypeID [int] = null,
	@ConfigurationTypeDescription [char](4) = null
AS
Select ConfigurationTypeID, ConfigurationTypeDescription
From ConfigurationType 
Where ( ConfigurationTypeID = @ConfigurationTypeID Or @ConfigurationTypeID = null ) 
And ( ConfigurationTypeDescription = @ConfigurationTypeDescription Or @ConfigurationTypeDescription = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertConfigurationType]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertConfigurationType]
	@ConfigurationTypeID [int] OUTPUT,
	@ConfigurationTypeDescription [char](4)
AS
Insert Into ConfigurationType 
( ConfigurationTypeID, ConfigurationTypeDescription)
Values ( @ConfigurationTypeID, @ConfigurationTypeDescription) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserStoreByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetUserStoreByPrimaryKey]
	@UserID [uniqueidentifier]
AS
Select UserID, FirstName, LastName, Email, Password, LastLogin, DateCreated, CreatedBy, IsSuperUser
From UserStore 
Where  UserID = @UserID  
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserStoreByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateUserStoreByPrimaryKey]
	@UserID [uniqueidentifier],
	@FirstName [nvarchar](255),
	@LastName [nvarchar](255),
	@Email [varchar](150),
	@Password [varchar](64),
	@LastLogin [datetime],
	@DateCreated [datetime],
	@CreatedBy [uniqueidentifier],
	@IsSuperUser [bit]
AS
Update UserStore 
Set FirstName = @FirstName, 
LastName = @LastName, 
Email = @Email, 
Password = @Password, 
LastLogin = @LastLogin, 
DateCreated = @DateCreated, 
CreatedBy = @CreatedBy, 
IsSuperUser = @IsSuperUser 
Where  UserID = @UserID  

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteUserStoreByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteUserStoreByPrimaryKey]
	@UserID [uniqueidentifier]
AS
Delete From UserStore 
Where  UserID = @UserID  

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserStore]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetUserStore]
AS
Select UserID, FirstName, LastName, Email, Password, LastLogin, DateCreated, CreatedBy, IsSuperUser
From UserStore' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserStoreByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetUserStoreByCriteriaFuzzy]
	@UserID [uniqueidentifier] = null,
	@FirstName [nvarchar](255) = null,
	@LastName [nvarchar](255) = null,
	@Email [varchar](150) = null,
	@Password [varchar](64) = null,
	@LastLogin [datetime] = null,
	@DateCreated [datetime] = null,
	@CreatedBy [uniqueidentifier] = null,
	@IsSuperUser [bit] = null
AS
Select UserID, FirstName, LastName, Email, Password, LastLogin, DateCreated, CreatedBy, IsSuperUser
From UserStore 
Where ( UserID = @UserID Or @UserID = null ) 
And ( FirstName Like @FirstName + ''%'' Or @FirstName = null ) 
And ( LastName Like @LastName + ''%'' Or @LastName = null ) 
And ( Email Like @Email + ''%'' Or @Email = null ) 
And ( Password Like @Password + ''%'' Or @Password = null ) 
And ( LastLogin = @LastLogin Or @LastLogin = null ) 
And ( DateCreated = @DateCreated Or @DateCreated = null ) 
And ( CreatedBy = @CreatedBy Or @CreatedBy = null ) 
And ( IsSuperUser = @IsSuperUser Or @IsSuperUser = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserStoreByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetUserStoreByCriteriaExact]
	@UserID [uniqueidentifier] = null,
	@FirstName [nvarchar](255) = null,
	@LastName [nvarchar](255) = null,
	@Email [varchar](150) = null,
	@Password [varchar](64) = null,
	@LastLogin [datetime] = null,
	@DateCreated [datetime] = null,
	@CreatedBy [uniqueidentifier] = null,
	@IsSuperUser [bit] = null
AS
Select UserID, FirstName, LastName, Email, Password, LastLogin, DateCreated, CreatedBy, IsSuperUser
From UserStore 
Where ( UserID = @UserID Or @UserID = null ) 
And ( FirstName = @FirstName Or @FirstName = null ) 
And ( LastName = @LastName Or @LastName = null ) 
And ( Email = @Email Or @Email = null ) 
And ( Password = @Password Or @Password = null ) 
And ( LastLogin = @LastLogin Or @LastLogin = null ) 
And ( DateCreated = @DateCreated Or @DateCreated = null ) 
And ( CreatedBy = @CreatedBy Or @CreatedBy = null ) 
And ( IsSuperUser = @IsSuperUser Or @IsSuperUser = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertUserStore]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertUserStore]
	@UserID [uniqueidentifier] OUTPUT,
	@FirstName [nvarchar](255),
	@LastName [nvarchar](255),
	@Email [varchar](150),
	@Password [varchar](64),
	@LastLogin [datetime],
	@DateCreated [datetime],
	@CreatedBy [uniqueidentifier],
	@IsSuperUser [bit]
AS
Insert Into UserStore 
( UserID, FirstName, LastName, Email, Password, LastLogin, DateCreated, CreatedBy, IsSuperUser)
Values ( @UserID, @FirstName, @LastName, @Email, @Password, @LastLogin, @DateCreated, @CreatedBy, @IsSuperUser) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetUser]
	@Email [varchar](150),
	@Password [varchar](64),
	@ApplicationID uniqueidentifier,
	@TypeID uniqueidentifier
AS
Select u.UserID, u.FirstName, u.LastName, u.Email, u.Password, u.LastLogin, u.DateCreated, u.CreatedBy, u.IsSuperUser
From UserStore u 

inner join Application_User au
on au.UserID = u.UserID

inner join Type_User tu
on tu.UserID = u.UserID

Where u.Email = @Email
And u.Password = @Password
and au.ApplicationID = @ApplicationID
and tu.TypeID = @TypeID' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertType_User]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertType_User]
	@TypeID [uniqueidentifier] OUTPUT,
	@UserID [uniqueidentifier] OUTPUT
AS
Insert Into Type_User 
( TypeID, UserID)
Values ( @TypeID, @UserID) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetType_UserByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetType_UserByPrimaryKey]
	@TypeID [uniqueidentifier],
	@UserID [uniqueidentifier]
AS
Select TypeID, UserID
From Type_User 
Where (( TypeID = @TypeID ) 
And ( UserID = @UserID ) 
)' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteType_UserByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteType_UserByPrimaryKey]
	@TypeID [uniqueidentifier],
	@UserID [uniqueidentifier]
AS
Delete From Type_User 
Where (( TypeID = @TypeID ) 
And ( UserID = @UserID ) 
)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetType_User]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetType_User]
AS
Select TypeID, UserID
From Type_User' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetType_UserByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetType_UserByCriteriaFuzzy]
	@TypeID [uniqueidentifier] = null,
	@UserID [uniqueidentifier] = null
AS
Select TypeID, UserID
From Type_User 
Where ( TypeID = @TypeID Or @TypeID = null ) 
And ( UserID = @UserID Or @UserID = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetType_UserByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetType_UserByCriteriaExact]
	@TypeID [uniqueidentifier] = null,
	@UserID [uniqueidentifier] = null
AS
Select TypeID, UserID
From Type_User 
Where ( TypeID = @TypeID Or @TypeID = null ) 
And ( UserID = @UserID Or @UserID = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInformationSchema]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetInformationSchema]
	@TableName [nvarchar](255) = null
AS


if (@TableName is null)
	begin
		Exec GetInformationSchemaTables
		Exec GetInformationSchemaTableConstraints
		Exec GetInformationSchemaColumns
		Exec GetInformationSchemaColumnUsage
   end

if(@TableName is not null)
	begin
		Exec GetInformationSchemaTables @TableName
		Exec GetInformationSchemaTableConstraints @TableName
		Exec GetInformationSchemaColumns @TableName
		Exec GetInformationSchemaColumnUsage @TableName
    end
	

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteApplication_UserByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteApplication_UserByPrimaryKey]
	@ApplicationID [uniqueidentifier],
	@UserID [uniqueidentifier]
AS
Delete From Application_User 
Where (( ApplicationID = @ApplicationID ) 
And ( UserID = @UserID ) 
)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplication_User]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplication_User]
AS
Select ApplicationID, UserID
From Application_User' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplication_UserByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplication_UserByCriteriaFuzzy]
	@ApplicationID [uniqueidentifier] = null,
	@UserID [uniqueidentifier] = null
AS
Select ApplicationID, UserID
From Application_User 
Where ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( UserID = @UserID Or @UserID = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplication_UserByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplication_UserByCriteriaExact]
	@ApplicationID [uniqueidentifier] = null,
	@UserID [uniqueidentifier] = null
AS
Select ApplicationID, UserID
From Application_User 
Where ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( UserID = @UserID Or @UserID = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertApplication_User]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertApplication_User]
	@ApplicationID [uniqueidentifier] OUTPUT,
	@UserID [uniqueidentifier] OUTPUT
AS
Insert Into Application_User 
( ApplicationID, UserID)
Values ( @ApplicationID, @UserID) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplication_UserByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetApplication_UserByPrimaryKey]
	@ApplicationID [uniqueidentifier],
	@UserID [uniqueidentifier]
AS
Select ApplicationID, UserID
From Application_User 
Where (( ApplicationID = @ApplicationID ) 
And ( UserID = @UserID ) 
)' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleStatisticByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetRuleStatisticByPrimaryKey]
	@RuleStatisticID [uniqueidentifier],
	@ApplicationID [uniqueidentifier],
	@TypeID [uniqueidentifier],
	@RuleID [uniqueidentifier],
	@DateInserted [datetime]
AS
Select RuleStatisticID, ApplicationID, TypeID, RuleID, RuleName, DateInserted, ReferenceID, Result
From RuleStatistic 
Where (( RuleStatisticID = @RuleStatisticID ) 
And ( ApplicationID = @ApplicationID ) 
And ( TypeID = @TypeID ) 
And ( RuleID = @RuleID ) 
And ( DateInserted = @DateInserted ) 
)' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleStatisticByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetRuleStatisticByCriteriaExact]
	@RuleStatisticID [uniqueidentifier] = null,
	@ApplicationID [uniqueidentifier] = null,
	@TypeID [uniqueidentifier] = null,
	@RuleID [uniqueidentifier] = null,
	@RuleName [nvarchar](50) = null,
	@DateInserted [datetime] = null,
	@ReferenceID [uniqueidentifier] = null,
	@Result [bit] = null
AS
Select RuleStatisticID, ApplicationID, TypeID, RuleID, RuleName, DateInserted, ReferenceID, Result
From RuleStatistic 
Where ( RuleStatisticID = @RuleStatisticID Or @RuleStatisticID = null ) 
And ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( TypeID = @TypeID Or @TypeID = null ) 
And ( RuleID = @RuleID Or @RuleID = null ) 
And ( RuleName = @RuleName Or @RuleName = null ) 
And ( DateInserted = @DateInserted Or @DateInserted = null ) 
And ( ReferenceID = @ReferenceID Or @ReferenceID = null ) 
And ( Result = @Result Or @Result = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleStatisticByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetRuleStatisticByCriteriaFuzzy]
	@RuleStatisticID [uniqueidentifier] = null,
	@ApplicationID [uniqueidentifier] = null,
	@TypeID [uniqueidentifier] = null,
	@RuleID [uniqueidentifier] = null,
	@RuleName [nvarchar](50) = null,
	@DateInserted [datetime] = null,
	@ReferenceID [uniqueidentifier] = null,
	@Result [bit] = null
AS
Select RuleStatisticID, ApplicationID, TypeID, RuleID, RuleName, DateInserted, ReferenceID, Result
From RuleStatistic 
Where ( RuleStatisticID = @RuleStatisticID Or @RuleStatisticID = null ) 
And ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( TypeID = @TypeID Or @TypeID = null ) 
And ( RuleID = @RuleID Or @RuleID = null ) 
And ( RuleName Like @RuleName + ''%'' Or @RuleName = null ) 
And ( DateInserted = @DateInserted Or @DateInserted = null ) 
And ( ReferenceID = @ReferenceID Or @ReferenceID = null ) 
And ( Result = @Result Or @Result = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleStatistic]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetRuleStatistic]
AS
Select RuleStatisticID, ApplicationID, TypeID, RuleID, RuleName, DateInserted, ReferenceID, Result
From RuleStatistic' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteRuleStatisticByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteRuleStatisticByPrimaryKey]
	@RuleStatisticID [uniqueidentifier],
	@ApplicationID [uniqueidentifier],
	@TypeID [uniqueidentifier],
	@RuleID [uniqueidentifier],
	@DateInserted [datetime]
AS
Delete From RuleStatistic 
Where (( RuleStatisticID = @RuleStatisticID ) 
And ( ApplicationID = @ApplicationID ) 
And ( TypeID = @TypeID ) 
And ( RuleID = @RuleID ) 
And ( DateInserted = @DateInserted ) 
)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateRuleStatisticByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateRuleStatisticByPrimaryKey]
	@RuleStatisticID [uniqueidentifier],
	@ApplicationID [uniqueidentifier],
	@TypeID [uniqueidentifier],
	@RuleID [uniqueidentifier],
	@RuleName [nvarchar](50),
	@DateInserted [datetime],
	@ReferenceID [uniqueidentifier],
	@Result [bit]
AS
Update RuleStatistic 
Set RuleName = @RuleName, 
ReferenceID = @ReferenceID, 
Result = @Result 
Where (( RuleStatisticID = @RuleStatisticID ) 
And ( ApplicationID = @ApplicationID ) 
And ( TypeID = @TypeID ) 
And ( RuleID = @RuleID ) 
And ( DateInserted = @DateInserted ) 
)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleStatistics]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetRuleStatistics]
	@ApplicationID [uniqueidentifier],
	@TypeID [uniqueidentifier]
AS
Select RuleStatisticID, ApplicationID, TypeID, RuleID, RuleName, DateInserted, ReferenceID, Result
From RuleStatistic 
Where ApplicationID = @ApplicationID 
and	  TypeID = @TypeID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertRuleStatistic]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertRuleStatistic]
	@RuleStatisticID [uniqueidentifier] OUTPUT,
	@ApplicationID [uniqueidentifier] OUTPUT,
	@TypeID [uniqueidentifier] OUTPUT,
	@RuleID [uniqueidentifier] OUTPUT,
	@RuleName [nvarchar](50),
	@DateInserted [datetime] OUTPUT,
	@ReferenceID [uniqueidentifier],
	@Result [bit]
AS
Insert Into RuleStatistic 
( RuleStatisticID, ApplicationID, TypeID, RuleID, RuleName, DateInserted, ReferenceID, Result)
Values ( @RuleStatisticID, @ApplicationID, @TypeID, @RuleID, @RuleName, @DateInserted, @ReferenceID, @Result) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetConfigurationFileByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetConfigurationFileByCriteriaExact]
	@TypeID [uniqueidentifier] = null,
	@ApplicationID [uniqueidentifier] = null,
	@ConfigurationTypeID [int] = null
AS
Select TypeID, ApplicationID, ConfigurationTypeID, ConfigurationFile
From ConfigurationFile 
Where ( TypeID = @TypeID Or @TypeID = null ) 
And ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( ConfigurationTypeID = @ConfigurationTypeID Or @ConfigurationTypeID = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertConfigurationFile]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertConfigurationFile]
	@TypeID [uniqueidentifier] OUTPUT,
	@ApplicationID [uniqueidentifier] OUTPUT,
	@ConfigurationTypeID [int] OUTPUT,
	@ConfigurationFile [xml]
AS
Insert Into ConfigurationFile 
( TypeID, ApplicationID, ConfigurationTypeID, ConfigurationFile)
Values ( @TypeID, @ApplicationID, @ConfigurationTypeID, @ConfigurationFile) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetConfigurationFileByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetConfigurationFileByPrimaryKey]
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier],
	@ConfigurationTypeID [int]
AS
Select TypeID, ApplicationID, ConfigurationTypeID, ConfigurationFile
From ConfigurationFile 
Where (( TypeID = @TypeID ) 
And ( ApplicationID = @ApplicationID ) 
And ( ConfigurationTypeID = @ConfigurationTypeID ) 
)' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateConfigurationFileByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateConfigurationFileByPrimaryKey]
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier],
	@ConfigurationTypeID [int],
	@ConfigurationFile [xml]
AS
Update ConfigurationFile 
Set ConfigurationFile = @ConfigurationFile 
Where (( TypeID = @TypeID ) 
And ( ApplicationID = @ApplicationID ) 
And ( ConfigurationTypeID = @ConfigurationTypeID ) 
)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteConfigurationFileByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteConfigurationFileByPrimaryKey]
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier],
	@ConfigurationTypeID [int]
AS
Delete From ConfigurationFile 
Where (( TypeID = @TypeID ) 
And ( ApplicationID = @ApplicationID ) 
And ( ConfigurationTypeID = @ConfigurationTypeID ) 
)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetConfigurationFile]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetConfigurationFile]
AS
Select TypeID, ApplicationID, ConfigurationTypeID, ConfigurationFile
From ConfigurationFile' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetConfigurationFileByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetConfigurationFileByCriteriaFuzzy]
	@TypeID [uniqueidentifier] = null,
	@ApplicationID [uniqueidentifier] = null,
	@ConfigurationTypeID [int] = null
AS
Select TypeID, ApplicationID, ConfigurationTypeID, ConfigurationFile
From ConfigurationFile 
Where ( TypeID = @TypeID Or @TypeID = null ) 
And ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( ConfigurationTypeID = @ConfigurationTypeID Or @ConfigurationTypeID = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MarkRuleDefinitionDeleted]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[MarkRuleDefinitionDeleted]
	@RuleID [uniqueidentifier],
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier]
AS
Update RuleDefinition 
set Deleted = 1
Where RuleID = @RuleID 
And TypeID = @TypeID 
And ApplicationID = @ApplicationID 

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleDefinitionByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetRuleDefinitionByPrimaryKey]
	@RuleID [uniqueidentifier],
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier]
AS
Select RuleID, TypeID, ApplicationID, RuleName, Definition, Paused, DateCreated, CreatedBy, Deleted, DateUpdated, UpdatedBy
From RuleDefinition 
Where (( RuleID = @RuleID ) 
And ( TypeID = @TypeID ) 
And ( ApplicationID = @ApplicationID ) 
)' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateRuleDefinitionByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UpdateRuleDefinitionByPrimaryKey]
	@RuleID [uniqueidentifier],
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier],
	@RuleName [nvarchar](50),
	@Definition [xml],
	@Paused [bit],
	@DateCreated [datetime],
	@CreatedBy [uniqueidentifier],
	@Deleted [bit],
	@DateUpdated [datetime],
	@UpdatedBy [uniqueidentifier]
AS
Update RuleDefinition 
Set RuleName = @RuleName, 
Definition = @Definition, 
Paused = @Paused, 
DateCreated = @DateCreated, 
CreatedBy = @CreatedBy, 
Deleted = @Deleted, 
DateUpdated = @DateUpdated, 
UpdatedBy = @UpdatedBy 
Where (( RuleID = @RuleID ) 
And ( TypeID = @TypeID ) 
And ( ApplicationID = @ApplicationID ) 
)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteRuleDefinitionByPrimaryKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[DeleteRuleDefinitionByPrimaryKey]
	@RuleID [uniqueidentifier],
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier]
AS
Delete From RuleDefinition 
Where (( RuleID = @RuleID ) 
And ( TypeID = @TypeID ) 
And ( ApplicationID = @ApplicationID ) 
)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleDefinition]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetRuleDefinition]
AS
Select RuleID, TypeID, ApplicationID, RuleName, Definition, Paused, DateCreated, CreatedBy, Deleted, DateUpdated, UpdatedBy
From RuleDefinition' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleDefinitionByCriteriaFuzzy]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetRuleDefinitionByCriteriaFuzzy]
	@RuleID [uniqueidentifier] = null,
	@TypeID [uniqueidentifier] = null,
	@ApplicationID [uniqueidentifier] = null,
	@RuleName [nvarchar](50) = null,
	@Paused [bit] = null,
	@DateCreated [datetime] = null,
	@CreatedBy [uniqueidentifier] = null,
	@Deleted [bit] = null,
	@DateUpdated [datetime] = null,
	@UpdatedBy [uniqueidentifier] = null
AS
Select RuleID, TypeID, ApplicationID, RuleName, Definition, Paused, DateCreated, CreatedBy, Deleted, DateUpdated, UpdatedBy
From RuleDefinition 
Where ( RuleID = @RuleID Or @RuleID = null ) 
And ( TypeID = @TypeID Or @TypeID = null ) 
And ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( RuleName Like @RuleName + ''%'' Or @RuleName = null ) 
And ( Paused = @Paused Or @Paused = null ) 
And ( DateCreated = @DateCreated Or @DateCreated = null ) 
And ( CreatedBy = @CreatedBy Or @CreatedBy = null ) 
And ( Deleted = @Deleted Or @Deleted = null ) 
And ( DateUpdated = @DateUpdated Or @DateUpdated = null ) 
And ( UpdatedBy = @UpdatedBy Or @UpdatedBy = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleDefinitionByCriteriaExact]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetRuleDefinitionByCriteriaExact]
	@RuleID [uniqueidentifier] = null,
	@TypeID [uniqueidentifier] = null,
	@ApplicationID [uniqueidentifier] = null,
	@RuleName [nvarchar](50) = null,
	@Paused [bit] = null,
	@DateCreated [datetime] = null,
	@CreatedBy [uniqueidentifier] = null,
	@Deleted [bit] = null,
	@DateUpdated [datetime] = null,
	@UpdatedBy [uniqueidentifier] = null
AS
Select RuleID, TypeID, ApplicationID, RuleName, Definition, Paused, DateCreated, CreatedBy, Deleted, DateUpdated, UpdatedBy
From RuleDefinition 
Where ( RuleID = @RuleID Or @RuleID = null ) 
And ( TypeID = @TypeID Or @TypeID = null ) 
And ( ApplicationID = @ApplicationID Or @ApplicationID = null ) 
And ( RuleName = @RuleName Or @RuleName = null ) 
And ( Paused = @Paused Or @Paused = null ) 
And ( DateCreated = @DateCreated Or @DateCreated = null ) 
And ( CreatedBy = @CreatedBy Or @CreatedBy = null ) 
And ( Deleted = @Deleted Or @Deleted = null ) 
And ( DateUpdated = @DateUpdated Or @DateUpdated = null ) 
And ( UpdatedBy = @UpdatedBy Or @UpdatedBy = null ) 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRuleDefinitions]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[GetRuleDefinitions]
	@TypeID [uniqueidentifier],
	@ApplicationID [uniqueidentifier]
AS
Select RuleID, TypeID, ApplicationID, RuleName, Definition, Paused, DateCreated, CreatedBy, Deleted, DateUpdated, UpdatedBy
From RuleDefinition 
Where TypeID = @TypeID 
And ApplicationID = @ApplicationID
And Deleted = 0
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertRuleDefinition]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[InsertRuleDefinition]
	@RuleID [uniqueidentifier] OUTPUT,
	@TypeID [uniqueidentifier] OUTPUT,
	@ApplicationID [uniqueidentifier] OUTPUT,
	@RuleName [nvarchar](50),
	@Definition [xml],
	@Paused [bit],
	@DateCreated [datetime],
	@CreatedBy [uniqueidentifier],
	@Deleted [bit],
	@DateUpdated [datetime],
	@UpdatedBy [uniqueidentifier]
AS
Insert Into RuleDefinition 
( RuleID, TypeID, ApplicationID, RuleName, Definition, Paused, DateCreated, CreatedBy, Deleted, DateUpdated, UpdatedBy)
Values ( @RuleID, @TypeID, @ApplicationID, @RuleName, @Definition, @Paused, @DateCreated, @CreatedBy, @Deleted, @DateUpdated, @UpdatedBy) 
' 
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Rule_Application]') AND parent_object_id = OBJECT_ID(N'[dbo].[RuleDefinition]'))
ALTER TABLE [dbo].[RuleDefinition]  WITH CHECK ADD  CONSTRAINT [FK_Rule_Application] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Application] ([ApplicationID])
GO
ALTER TABLE [dbo].[RuleDefinition] CHECK CONSTRAINT [FK_Rule_Application]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Rule_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[RuleDefinition]'))
ALTER TABLE [dbo].[RuleDefinition]  WITH CHECK ADD  CONSTRAINT [FK_Rule_Type] FOREIGN KEY([TypeID])
REFERENCES [dbo].[Type] ([TypeID])
GO
ALTER TABLE [dbo].[RuleDefinition] CHECK CONSTRAINT [FK_Rule_Type]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Application_User_Application]') AND parent_object_id = OBJECT_ID(N'[dbo].[Application_User]'))
ALTER TABLE [dbo].[Application_User]  WITH CHECK ADD  CONSTRAINT [FK_Application_User_Application] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Application] ([ApplicationID])
GO
ALTER TABLE [dbo].[Application_User] CHECK CONSTRAINT [FK_Application_User_Application]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Application_User_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Application_User]'))
ALTER TABLE [dbo].[Application_User]  WITH CHECK ADD  CONSTRAINT [FK_Application_User_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserStore] ([UserID])
GO
ALTER TABLE [dbo].[Application_User] CHECK CONSTRAINT [FK_Application_User_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Application_Type_Application]') AND parent_object_id = OBJECT_ID(N'[dbo].[Application_Type]'))
ALTER TABLE [dbo].[Application_Type]  WITH CHECK ADD  CONSTRAINT [FK_Application_Type_Application] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Application] ([ApplicationID])
GO
ALTER TABLE [dbo].[Application_Type] CHECK CONSTRAINT [FK_Application_Type_Application]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Application_Type_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[Application_Type]'))
ALTER TABLE [dbo].[Application_Type]  WITH CHECK ADD  CONSTRAINT [FK_Application_Type_Type] FOREIGN KEY([TypeID])
REFERENCES [dbo].[Type] ([TypeID])
GO
ALTER TABLE [dbo].[Application_Type] CHECK CONSTRAINT [FK_Application_Type_Type]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Type_User_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[Type_User]'))
ALTER TABLE [dbo].[Type_User]  WITH CHECK ADD  CONSTRAINT [FK_Type_User_Type] FOREIGN KEY([TypeID])
REFERENCES [dbo].[Type] ([TypeID])
GO
ALTER TABLE [dbo].[Type_User] CHECK CONSTRAINT [FK_Type_User_Type]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Type_User_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Type_User]'))
ALTER TABLE [dbo].[Type_User]  WITH CHECK ADD  CONSTRAINT [FK_Type_User_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserStore] ([UserID])
GO
ALTER TABLE [dbo].[Type_User] CHECK CONSTRAINT [FK_Type_User_User]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ConfigurationFile_ConfigurationType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ConfigurationFile]'))
ALTER TABLE [dbo].[ConfigurationFile]  WITH CHECK ADD  CONSTRAINT [FK_ConfigurationFile_ConfigurationType] FOREIGN KEY([ConfigurationTypeID])
REFERENCES [dbo].[ConfigurationType] ([ConfigurationTypeID])
GO
ALTER TABLE [dbo].[ConfigurationFile] CHECK CONSTRAINT [FK_ConfigurationFile_ConfigurationType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ConfigurationFile_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[ConfigurationFile]'))
ALTER TABLE [dbo].[ConfigurationFile]  WITH CHECK ADD  CONSTRAINT [FK_ConfigurationFile_Type] FOREIGN KEY([TypeID])
REFERENCES [dbo].[Type] ([TypeID])
GO
ALTER TABLE [dbo].[ConfigurationFile] CHECK CONSTRAINT [FK_ConfigurationFile_Type]
GO
