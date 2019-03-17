USE [master]
GO
/****** Object:  Database [WebApidb]    Script Date: 2/7/2019 5:15:49 PM ******/
CREATE DATABASE [WebApidb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WebApidb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\WebApidb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WebApidb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\WebApidb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [WebApidb] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WebApidb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WebApidb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WebApidb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WebApidb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WebApidb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WebApidb] SET ARITHABORT OFF 
GO
ALTER DATABASE [WebApidb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WebApidb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WebApidb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WebApidb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WebApidb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WebApidb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WebApidb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WebApidb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WebApidb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WebApidb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WebApidb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WebApidb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WebApidb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WebApidb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WebApidb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WebApidb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WebApidb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WebApidb] SET RECOVERY FULL 
GO
ALTER DATABASE [WebApidb] SET  MULTI_USER 
GO
ALTER DATABASE [WebApidb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WebApidb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WebApidb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WebApidb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WebApidb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'WebApidb', N'ON'
GO
ALTER DATABASE [WebApidb] SET QUERY_STORE = OFF
GO
USE [WebApidb]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [WebApidb]
GO
/****** Object:  Table [dbo].[AppRoleClaims]    Script Date: 2/7/2019 5:15:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppRoleClaim] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppRoles]    Script Date: 2/7/2019 5:15:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[Trashed] [bit] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[RecordStatus] [int] NOT NULL,
 CONSTRAINT [PK_AppRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserClaims]    Script Date: 2/7/2019 5:15:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppUserClaim] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserLogins]    Script Date: 2/7/2019 5:15:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AppUserLogin] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserRoles]    Script Date: 2/7/2019 5:15:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[Status] [bit] NOT NULL,
	[Trashed] [bit] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[RecordStatus] [int] NOT NULL,
 CONSTRAINT [PK_AppUserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUsers]    Script Date: 2/7/2019 5:15:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[PictureUrl] [nvarchar](max) NULL,
	[TokenNumber] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[Trashed] [bit] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[RecordStatus] [int] NOT NULL,
 CONSTRAINT [PK_AppUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserTokens]    Script Date: 2/7/2019 5:15:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppUserToken] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ELMAH_Error]    Script Date: 2/7/2019 5:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ELMAH_Error](
	[ErrorId] [uniqueidentifier] NOT NULL,
	[Application] [nvarchar](60) NOT NULL,
	[Host] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
	[Source] [nvarchar](60) NOT NULL,
	[Message] [nvarchar](500) NOT NULL,
	[User] [nvarchar](50) NOT NULL,
	[StatusCode] [int] NOT NULL,
	[TimeUtc] [datetime] NOT NULL,
	[Sequence] [int] IDENTITY(1,1) NOT NULL,
	[AllXml] [ntext] NOT NULL,
 CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestRepo]    Script Date: 2/7/2019 5:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestRepo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EmailAddress] [nvarchar](100) NOT NULL,
	[Status] [bit] NOT NULL,
	[Trashed] [bit] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[RecordStatus] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AppRoleClaim_RoleId]    Script Date: 2/7/2019 5:15:50 PM ******/
CREATE NONCLUSTERED INDEX [IX_AppRoleClaim_RoleId] ON [dbo].[AppRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 2/7/2019 5:15:50 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AppRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AppUserClaim_UserId]    Script Date: 2/7/2019 5:15:50 PM ******/
CREATE NONCLUSTERED INDEX [IX_AppUserClaim_UserId] ON [dbo].[AppUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AppUserLogin_UserId]    Script Date: 2/7/2019 5:15:50 PM ******/
CREATE NONCLUSTERED INDEX [IX_AppUserLogin_UserId] ON [dbo].[AppUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AppUserRole_RoleId]    Script Date: 2/7/2019 5:15:50 PM ******/
CREATE NONCLUSTERED INDEX [IX_AppUserRole_RoleId] ON [dbo].[AppUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 2/7/2019 5:15:50 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AppUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 2/7/2019 5:15:50 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AppUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ELMAH_Error_App_Time_Seq]    Script Date: 2/7/2019 5:15:50 PM ******/
CREATE NONCLUSTERED INDEX [IX_ELMAH_Error_App_Time_Seq] ON [dbo].[ELMAH_Error]
(
	[Application] ASC,
	[TimeUtc] DESC,
	[Sequence] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ELMAH_Error] ADD  CONSTRAINT [DF_ELMAH_Error_ErrorId]  DEFAULT (newid()) FOR [ErrorId]
GO
ALTER TABLE [dbo].[AppRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AppRoleClaim_AppRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AppRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppRoleClaims] CHECK CONSTRAINT [FK_AppRoleClaim_AppRoles_RoleId]
GO
ALTER TABLE [dbo].[AppUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AppUserClaim_AppUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AppUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserClaims] CHECK CONSTRAINT [FK_AppUserClaim_AppUsers_UserId]
GO
ALTER TABLE [dbo].[AppUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AppUserLogin_AppUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AppUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserLogins] CHECK CONSTRAINT [FK_AppUserLogin_AppUsers_UserId]
GO
ALTER TABLE [dbo].[AppUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AppUserRole_AppRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AppRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserRoles] CHECK CONSTRAINT [FK_AppUserRole_AppRoles_RoleId]
GO
ALTER TABLE [dbo].[AppUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AppUserRole_AppUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AppUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserRoles] CHECK CONSTRAINT [FK_AppUserRole_AppUsers_UserId]
GO
ALTER TABLE [dbo].[AppUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AppUserToken_AppUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AppUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserTokens] CHECK CONSTRAINT [FK_AppUserToken_AppUsers_UserId]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorsXml]    Script Date: 2/7/2019 5:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml]
(
    @Application NVARCHAR(60),
    @PageIndex INT = 0,
    @PageSize INT = 15,
    @TotalCount INT OUTPUT
)
AS 

    SET NOCOUNT ON

    DECLARE @FirstTimeUTC DATETIME
    DECLARE @FirstSequence INT
    DECLARE @StartRow INT
    DECLARE @StartRowIndex INT

    SELECT 
        @TotalCount = COUNT(1) 
    FROM 
        [ELMAH_Error]
    WHERE 
        [Application] = @Application

    -- Get the ID of the first error for the requested page

    SET @StartRowIndex = @PageIndex * @PageSize + 1

    IF @StartRowIndex <= @TotalCount
    BEGIN

        SET ROWCOUNT @StartRowIndex

        SELECT  
            @FirstTimeUTC = [TimeUtc],
            @FirstSequence = [Sequence]
        FROM 
            [ELMAH_Error]
        WHERE   
            [Application] = @Application
        ORDER BY 
            [TimeUtc] DESC, 
            [Sequence] DESC

    END
    ELSE
    BEGIN

        SET @PageSize = 0

    END

    -- Now set the row count to the requested page size and get
    -- all records below it for the pertaining application.

    SET ROWCOUNT @PageSize

    SELECT 
        errorId     = [ErrorId], 
        application = [Application],
        host        = [Host], 
        type        = [Type],
        source      = [Source],
        message     = [Message],
        [user]      = [User],
        statusCode  = [StatusCode], 
        time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'
    FROM 
        [ELMAH_Error] error
    WHERE
        [Application] = @Application
    AND
        [TimeUtc] <= @FirstTimeUTC
    AND 
        [Sequence] <= @FirstSequence
    ORDER BY
        [TimeUtc] DESC, 
        [Sequence] DESC
    FOR
        XML AUTO

GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorXml]    Script Date: 2/7/2019 5:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
(
    @Application NVARCHAR(60),
    @ErrorId UNIQUEIDENTIFIER
)
AS

    SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
        [ELMAH_Error]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application

GO
/****** Object:  StoredProcedure [dbo].[ELMAH_LogError]    Script Date: 2/7/2019 5:15:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ELMAH_LogError]
(
    @ErrorId UNIQUEIDENTIFIER,
    @Application NVARCHAR(60),
    @Host NVARCHAR(30),
    @Type NVARCHAR(100),
    @Source NVARCHAR(60),
    @Message NVARCHAR(500),
    @User NVARCHAR(50),
    @AllXml NTEXT,
    @StatusCode INT,
    @TimeUtc DATETIME
)
AS

    SET NOCOUNT ON

    INSERT
    INTO
        [ELMAH_Error]
        (
            [ErrorId],
            [Application],
            [Host],
            [Type],
            [Source],
            [Message],
            [User],
            [AllXml],
            [StatusCode],
            [TimeUtc]
        )
    VALUES
        (
            @ErrorId,
            @Application,
            @Host,
            @Type,
            @Source,
            @Message,
            @User,
            @AllXml,
            @StatusCode,
            @TimeUtc
        )

GO
USE [master]
GO
ALTER DATABASE [WebApidb] SET  READ_WRITE 
GO

----------------------------------------------------------------------------------------------------
-----Create Procedure for code creation
----------------------------------------------------------------------------------------------------

CREATE PROC CreateC#ModelFromTable   
@TableName sysname  
AS  
  
  DECLARE @schema VARCHAR(30), @table VARCHAR(100);
  SELECT TOP 1 @schema =  value FROM dbo.SplitByCommas(@TableName,'.')
  SELECT @table = value FROM(SELECT TOP 2 * FROM dbo.SplitByCommas(@TableName,'.') EXCEPT SELECT TOP 1 * FROM dbo.SplitByCommas(@TableName,'.')) tn


DECLARE @Result varchar(max) = '    [Table("'+ @table +'", Schema = "'+@schema+'")]'+CHAR(10)+'    public class ' + @table + 'Model : CommonColumns  
    {'  
  
--SELECT @Result = @Result + '  
-- [Column("'+ ColumnName +'")]  
--    public ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }  
--'  
SELECT @Result =  
 CASE   
 WHEN t.IsPrimaryKey = 1 THEN   
  @Result + '
        [Identity, Key]  
        public ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }  
'  
 WHEN t.ColumnName = 'Status' THEN   
  @Result + '  
        public new ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }  
'  
 ELSE   
  @Result + '  
        public ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }  
'  
end  
from  
(  
    select   
        replace(col.name, ' ', '_') ColumnName,  
        column_id ColumnId,  
        case typ.name   
            when 'bigint' then 'long'  
            when 'binary' then 'byte[]'  
            when 'bit' then 'bool'  
            when 'char' then 'string'  
            when 'date' then 'DateTime'  
            when 'datetime' then 'DateTime'  
            when 'datetime2' then 'DateTime'  
            when 'datetimeoffset' then 'DateTimeOffset'  
            when 'decimal' then 'decimal'  
            when 'float' then 'float'  
            when 'image' then 'byte[]'  
            when 'int' then 'int'  
            when 'money' then 'decimal'  
            when 'nchar' then 'char'  
            when 'ntext' then 'string'  
            when 'numeric' then 'decimal'  
            when 'nvarchar' then 'string'  
            when 'real' then 'double'  
            when 'smalldatetime' then 'DateTime'  
            when 'smallint' then 'short'  
            when 'smallmoney' then 'decimal'  
            when 'text' then 'string'  
            when 'time' then 'TimeSpan'  
            when 'timestamp' then 'DateTime'  
            when 'tinyint' then 'byte'  
            when 'uniqueidentifier' then 'Guid'  
            when 'varbinary' then 'byte[]'  
            when 'varchar' then 'string'  
            else 'UNKNOWN_' + typ.name  
        end ColumnType,  
        case   
            when col.is_nullable = 1 and typ.name in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifie
r')   
            then '?'   
            else ''   
        end NullableSign,  
CASE   
    WHEN EXISTS (  
 SELECT *  
 FROM  
  sys.indexes AS I   
  INNER JOIN   
  sys.index_columns AS IC   
  ON IC.object_id = I.object_id   
  AND IC.index_id = I.index_id  
 WHERE    
  I.object_id = col.object_id  
  AND IC.object_id = col.object_id  
  AND I.is_primary_key = 1  
  AND IC.index_id = I.index_id   
  AND IC.column_id = col.column_id  
 )    
 THEN 1   
    ELSE 0   
    END AS IsPrimaryKey  
    from sys.columns col  
        join sys.types typ on  
            col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id  
    where object_id = object_id(@TableName)  
) t  
order by ColumnId  
  
set @Result = @Result  + '  
    }'  
  
PRINT @Result  
select @Result  AS ModelClass
  

GO


CREATE PROC CreateC#ViewModelFromTable   
@TableName sysname  
AS  

  DECLARE @schema VARCHAR(30), @table VARCHAR(100);
  SELECT TOP 1 @schema =  value FROM dbo.SplitByCommas(@TableName,'.')
  SELECT @table = value FROM(SELECT TOP 2 * FROM dbo.SplitByCommas(@TableName,'.') EXCEPT SELECT TOP 1 * FROM dbo.SplitByCommas(@TableName,'.')) tn
  
DECLARE @Result varchar(max) = '    public class ' + @table + 'ViewModel : DefaultViewModel  
    {'  
SELECT @Result =   
 CASE   
 WHEN t.ColumnName = 'Status' THEN   
  @Result + '  
        public new ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }  
'  
 ELSE   
  @Result + '  
        public ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }  
'  
end  
  
from  
(  
    select   
        replace(col.name, ' ', '_') ColumnName,  
        column_id ColumnId,  
        case typ.name   
            when 'bigint' then 'long'  
            when 'binary' then 'byte[]'  
            when 'bit' then 'bool'  
            when 'char' then 'string'  
            when 'date' then 'DateTime'  
            when 'datetime' then 'DateTime'  
            when 'datetime2' then 'DateTime'  
            when 'datetimeoffset' then 'DateTimeOffset'  
            when 'decimal' then 'decimal'  
            when 'float' then 'float'  
            when 'image' then 'byte[]'  
            when 'int' then 'int'  
            when 'money' then 'decimal'  
            when 'nchar' then 'char'  
            when 'ntext' then 'string'  
            when 'numeric' then 'decimal'  
            when 'nvarchar' then 'string'  
            when 'real' then 'double'  
            when 'smalldatetime' then 'DateTime'  
            when 'smallint' then 'short'  
            when 'smallmoney' then 'decimal'  
            when 'text' then 'string'  
            when 'time' then 'TimeSpan'  
            when 'timestamp' then 'DateTime'  
            when 'tinyint' then 'byte'  
            when 'uniqueidentifier' then 'Guid'  
            when 'varbinary' then 'byte[]'  
            when 'varchar' then 'string'  
            else 'UNKNOWN_' + typ.name  
        end ColumnType,  
        case   
            when col.is_nullable = 1 and typ.name in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifie
r')   
            then '?'   
            else ''   
        end NullableSign  
    from sys.columns col  
        join sys.types typ on  
            col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id  
    where object_id = object_id(@TableName)  
) t  
order by ColumnId  
  
set @Result = @Result  + '  
    }'  
  
print @Result  
  SELECT @Result AS ModelClass
  
GO


Create FUNCTION [dbo].[SplitByCommas](@MYSTR VARCHAR(MAX), @DELIMITER CHAR(1))
RETURNS @MYTBL  TABLE (value varchar(MAX))
AS 
BEGIN
 DECLARE @RET VARCHAR(500)
 DECLARE @INDEX INT
 DECLARE @COUNTER smallint
 
 --Get the first position of delimiter in the main string
 SET @INDEX = CHARINDEX(@DELIMITER,@MYSTR)
 SET @COUNTER = 0
 
 --Loop if delimiter exists in the main string
 WHILE @INDEX > 0
 BEGIN
  --extract the result substring before the delimiter found
  SET @RET = SUBSTRING(@MYSTR,1, @INDEX-1 )
  --set mainstring right part after the delimiter found
  SET @MYSTR = SUBSTRING(@MYSTR,@INDEX+1 , LEN(@MYSTR) - @INDEX )
  --increase the counter
  SET @COUNTER = @COUNTER  + 1 
  --add the result substring to the table
  INSERT INTO @MYTBL (value)
  VALUES ( @RET)
  --Get the next position of delimiter in the main string
  SET @INDEX = CHARINDEX(@DELIMITER,@MYSTR)
 END
 
 --if no delimiter is found then simply add the mainstring to the table
 IF @INDEX = 0 
 BEGIN
  SET @COUNTER = @COUNTER  + 1
  INSERT INTO @MYTBL ( value)
  VALUES ( @MYSTR)
 END 
 RETURN   
END
 
 
GO

ALTER PROCEDURE AddDefaultColumns
@tableName sysname
AS
  DECLARE @schema VARCHAR(30), @table VARCHAR(100);  
  SELECT TOP 1 @schema =  value FROM dbo.SplitByCommas(@tableName,'.')  
  SELECT @table = value FROM(SELECT TOP 2 * FROM dbo.SplitByCommas(@tableName,'.') EXCEPT SELECT TOP 1 * FROM dbo.SplitByCommas(@tableName,'.')) tn  
  
  DECLARE @sqlCommand nvarchar(max)
SET @sqlCommand = N'
ALTER TABLE '+@tableName+'
ADD [Status] [bit] NOT NULL DEFAULT (1)

ALTER TABLE '+@tableName+'
ADD [Trashed] [bit] NOT NULL DEFAULT (0)

ALTER TABLE '+@tableName+'
ADD [RowVersion] [timestamp] NULL

ALTER TABLE '+@tableName+'
ADD [CreatedDate] [datetime2](7) NOT NULL DEFAULT (GETDATE())

ALTER TABLE '+@tableName+'
ADD [ModifiedDate] [datetime2](7) NULL
CONSTRAINT '+@table+'_ModifiedDate_Default_Values DEFAULT (GETDATE())
WITH VALUES

ALTER TABLE '+@tableName+'
ADD [CreatedBy] [nvarchar](max) NULL

ALTER TABLE '+@tableName+'
ADD [ModifiedBy] [nvarchar](max) NULL

ALTER TABLE '+@tableName+'
ADD [RecordStatus] [int] NOT NULL DEFAULT (2)
';
EXEC (@sqlCommand)

GO
