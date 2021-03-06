USE [master]
GO
/****** Object:  Database [ccprstaging_db]    Script Date: 5/21/2015 2:20:45 PM ******/
CREATE DATABASE [ccprstaging_db]
GO
ALTER DATABASE [ccprstaging_db] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ccprstaging_db].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [ccprstaging_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ccprstaging_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ccprstaging_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ccprstaging_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ccprstaging_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [ccprstaging_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ccprstaging_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ccprstaging_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ccprstaging_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ccprstaging_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ccprstaging_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ccprstaging_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ccprstaging_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ccprstaging_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ccprstaging_db] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ccprstaging_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ccprstaging_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ccprstaging_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ccprstaging_db] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [ccprstaging_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ccprstaging_db] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ccprstaging_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ccprstaging_db] SET RECOVERY FULL 
GO
ALTER DATABASE [ccprstaging_db] SET  MULTI_USER 
GO
ALTER DATABASE [ccprstaging_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ccprstaging_db] SET DB_CHAINING OFF 
GO
USE [ccprstaging_db]
GO
/****** Object:  Table [dbo].[BuilderGalleries]    Script Date: 5/21/2015 2:20:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuilderGalleries](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_BuilderGalleries_Active]  DEFAULT ((0)),
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_BuilderGalleries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 5/21/2015 2:20:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Categories_Active]  DEFAULT ((0)),
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Cities]    Script Date: 5/21/2015 2:20:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Comments]    Script Date: 5/21/2015 2:20:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoryId] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Text] [nvarchar](255) NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [nvarchar](100) NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 5/21/2015 2:20:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[From] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Phone] [nvarchar](255) NOT NULL,
	[SchoolId] [int] NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Comments] [nvarchar](max) NOT NULL,
	[IsRead] [bit] NOT NULL CONSTRAINT [DF_Contacts_IsRead]  DEFAULT ((0)),
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Grades]    Script Date: 5/21/2015 2:20:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grades](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Name_EN] [nvarchar](255) NULL,
	[Position] [int] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Grades] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Imagebles]    Script Date: 5/21/2015 2:20:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Imagebles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Imagebles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ImageCategories]    Script Date: 5/21/2015 2:20:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_ImageCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Images]    Script Date: 5/21/2015 2:20:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImagebleId] [int] NOT NULL,
	[Filename] [nvarchar](255) NULL,
	[ContentType] [nvarchar](255) NULL,
	[Size] [int] NULL,
	[Position] [smallint] NULL,
	[Target] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Interests]    Script Date: 5/21/2015 2:20:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Interests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Interests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Pages]    Script Date: 5/21/2015 2:20:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pages](
	[Id] [int] NOT NULL,
	[StoryId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Text] [nvarchar](800) NOT NULL,
	[Position] [int] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[PageTypes]    Script Date: 5/21/2015 2:20:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageTypes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PageTypes_Active]  DEFAULT ((0)),
	[Position] [int] NOT NULL,
 CONSTRAINT [PK_PageTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Ratings]    Script Date: 5/21/2015 2:20:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ratings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoryId] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Rate] [int] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Ratings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[RoleMemberships]    Script Date: 5/21/2015 2:20:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMemberships](
	[UserName] [nvarchar](100) NOT NULL,
	[RoleName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RoleMemberships] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC,
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 5/21/2015 2:20:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Schools]    Script Date: 5/21/2015 2:20:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schools](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Details] [nvarchar](255) NULL,
	[Address1] [nvarchar](255) NULL,
	[Address2] [nvarchar](255) NULL,
	[CityId] [int] NOT NULL,
	[Zip] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Schools] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Stories]    Script Date: 5/21/2015 2:20:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stories](
	[Id] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Summary] [nvarchar](800) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[Featured] [bit] NOT NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [nvarchar](100) NULL,
	[Views] [int] NOT NULL CONSTRAINT [DF_Stories_Views]  DEFAULT ((0)),
	[Pages] [nvarchar](max) NULL,
	[Status] [int] NOT NULL CONSTRAINT [DF_Stories_Status]  DEFAULT ('0'),
 CONSTRAINT [PK_Stories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[StoryCategories]    Script Date: 5/21/2015 2:20:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoryCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoryId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_StoryCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[StoryGrades]    Script Date: 5/21/2015 2:20:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoryGrades](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoryId] [int] NOT NULL,
	[GradeId] [int] NOT NULL,
 CONSTRAINT [PK_StoryGrades] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[StoryInterests]    Script Date: 5/21/2015 2:20:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoryInterests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoryId] [int] NOT NULL,
	[InterestId] [int] NOT NULL,
 CONSTRAINT [PK_StoryInterests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[UserInterests]    Script Date: 5/21/2015 2:20:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInterests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[InterestId] [int] NOT NULL,
 CONSTRAINT [PK_UserInterests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/21/2015 2:20:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[UserName] [nvarchar](100) NOT NULL,
	[PasswordHash] [binary](64) NOT NULL,
	[PasswordSalt] [binary](128) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[IsApproved] [bit] NOT NULL DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[DateLastLogin] [datetime] NULL,
	[DateLastActivity] [datetime] NULL,
	[DateLastPasswordChange] [datetime] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[Age] [int] NULL,
	[GradeId] [int] NULL,
	[SchoolId] [int] NULL,
	[ImageHolders_Id] [int] NULL,
	[Featured] [bit] NOT NULL CONSTRAINT [DF_Users_Featured]  DEFAULT ((0)),
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [nvarchar](100) NULL,
	[Owner] [nvarchar](255) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VersionInfo]    Script Date: 5/21/2015 2:20:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VersionInfo](
	[Version] [bigint] NOT NULL,
	[AppliedOn] [datetime] NULL,
	[Description] [nvarchar](1024) NULL
)

GO
/****** Object:  Index [UC_Version]    Script Date: 5/21/2015 2:20:50 PM ******/
CREATE UNIQUE CLUSTERED INDEX [UC_Version] ON [dbo].[VersionInfo]
(
	[Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
INSERT [dbo].[BuilderGalleries] ([Id], [Name], [Description], [UserName], [Active], [CreatedAt], [UpdatedAt]) VALUES (1, N'Main', N'Imagenes Generales', NULL, 1, CAST(N'2015-05-07 14:51:32.313' AS DateTime), NULL)
INSERT [dbo].[BuilderGalleries] ([Id], [Name], [Description], [UserName], [Active], [CreatedAt], [UpdatedAt]) VALUES (2, N'San Valentin', N'Imagenes para epoca de San Valentin', NULL, 0, CAST(N'2015-05-07 14:51:32.407' AS DateTime), NULL)
INSERT [dbo].[BuilderGalleries] ([Id], [Name], [Description], [UserName], [Active], [CreatedAt], [UpdatedAt]) VALUES (3, N'Navidad', N'Imagenes para epoca de Navidad', NULL, 0, CAST(N'2015-05-07 14:51:32.500' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Cities] ON 

INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (1, N'Adjuntas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (2, N'Aguada', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (3, N'Aguadilla', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (4, N'Aguas Buenas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (5, N'Aibonito', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (6, N'Arecibo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (7, N'Arroyo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (8, N'Añasco', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (9, N'Barceloneta', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (10, N'Barranquitas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (11, N'Bayamón', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (12, N'Cabo Rojo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (13, N'Caguas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (14, N'Camuy', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (15, N'Canóvanas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (16, N'Carolina', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (17, N'Cataño', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (18, N'Cayey', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (19, N'Ceiba', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (20, N'Ciales', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (21, N'Cidra', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (22, N'Coamo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (23, N'Comerío', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (24, N'Corozal', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (25, N'Culebra', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (26, N'Dorado', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (27, N'Fajardo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (28, N'Florida', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (29, N'Guayama', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (30, N'Guayanilla', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (31, N'Guaynabo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (32, N'Gurabo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (33, N'Guánica', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (34, N'Hatillo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (35, N'Hormigueros', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (36, N'Humacao', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (37, N'Isabela', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (38, N'Jayuya', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (39, N'Juana Díaz', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (40, N'Juncos', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (41, N'Lajas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (42, N'Lares', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (43, N'Las Marías', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (44, N'Las Piedras', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (45, N'Loíza', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (46, N'Luquillo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (47, N'Manatí', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (48, N'Maricao', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (49, N'Maunabo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (50, N'Mayagüez', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (51, N'Moca', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (52, N'Morovis', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (53, N'Naguabo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (54, N'Naranjito', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (55, N'Orocovis', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (56, N'Patillas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (57, N'Peñuelas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (58, N'Ponce', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (59, N'Quebradillas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (60, N'Rincón', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (61, N'Río Grande', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (62, N'Sabana Grande', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (63, N'Salinas', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (64, N'San Germán', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (65, N'San Juan', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (66, N'San Lorenzo', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (67, N'San Sebastián', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (68, N'Santa Isabel', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (69, N'Toa Alta', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (70, N'Toa Baja', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (71, N'Trujillo Alto', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (72, N'Utuado', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (73, N'Vega Alta', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (74, N'Vega Baja', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (75, N'Vieques', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (76, N'Villalba', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (77, N'Yabucoa', NULL, NULL)
INSERT [dbo].[Cities] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (78, N'Yauco', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Cities] OFF
SET IDENTITY_INSERT [dbo].[Comments] ON 

INSERT [dbo].[Comments] ([Id], [StoryId], [UserName], [Text], [IsApproved], [CreatedAt], [UpdatedAt], [ApprovedDate], [ApprovedBy]) VALUES (1, 11, N'admin', N'mi comentario', 1, CAST(N'2015-05-13 15:12:55.217' AS DateTime), NULL, CAST(N'2015-05-13 15:14:00.680' AS DateTime), N'admin')
SET IDENTITY_INSERT [dbo].[Comments] OFF
SET IDENTITY_INSERT [dbo].[Grades] ON 

INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (1, N'Primero', NULL, 1, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (2, N'Segundo', NULL, 2, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (3, N'Tercero', NULL, 3, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (4, N'Cuarto', NULL, 4, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (5, N'Quinto', NULL, 5, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (6, N'Sexto', NULL, 6, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (7, N'Septimo', NULL, 7, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (8, N'Octavo', NULL, 8, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (9, N'Noveno', NULL, 9, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (10, N'Décimo', NULL, 10, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (11, N'Undécimo', NULL, 11, NULL, NULL)
INSERT [dbo].[Grades] ([Id], [Name], [Name_EN], [Position], [CreatedAt], [UpdatedAt]) VALUES (12, N'Duodécimo', NULL, 12, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Grades] OFF
SET IDENTITY_INSERT [dbo].[Imagebles] ON 

INSERT [dbo].[Imagebles] ([Id]) VALUES (1)
INSERT [dbo].[Imagebles] ([Id]) VALUES (2)
INSERT [dbo].[Imagebles] ([Id]) VALUES (3)
INSERT [dbo].[Imagebles] ([Id]) VALUES (4)
INSERT [dbo].[Imagebles] ([Id]) VALUES (5)
INSERT [dbo].[Imagebles] ([Id]) VALUES (6)
INSERT [dbo].[Imagebles] ([Id]) VALUES (7)
INSERT [dbo].[Imagebles] ([Id]) VALUES (8)
INSERT [dbo].[Imagebles] ([Id]) VALUES (9)
INSERT [dbo].[Imagebles] ([Id]) VALUES (10)
INSERT [dbo].[Imagebles] ([Id]) VALUES (11)
INSERT [dbo].[Imagebles] ([Id]) VALUES (12)
INSERT [dbo].[Imagebles] ([Id]) VALUES (13)
INSERT [dbo].[Imagebles] ([Id]) VALUES (14)
INSERT [dbo].[Imagebles] ([Id]) VALUES (15)
INSERT [dbo].[Imagebles] ([Id]) VALUES (16)
INSERT [dbo].[Imagebles] ([Id]) VALUES (17)
INSERT [dbo].[Imagebles] ([Id]) VALUES (18)
SET IDENTITY_INSERT [dbo].[Imagebles] OFF
SET IDENTITY_INSERT [dbo].[ImageCategories] ON 

INSERT [dbo].[ImageCategories] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (1, N'Aves', CAST(N'2015-05-07 10:51:32.000' AS DateTime), NULL)
INSERT [dbo].[ImageCategories] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (2, N'Estrellas', CAST(N'2015-05-07 10:51:32.000' AS DateTime), NULL)
INSERT [dbo].[ImageCategories] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (3, N'Alegria', CAST(N'2015-05-07 10:51:32.000' AS DateTime), NULL)
INSERT [dbo].[ImageCategories] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (4, N'Miedo', CAST(N'2015-05-07 10:51:32.000' AS DateTime), NULL)
INSERT [dbo].[ImageCategories] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (5, N'General', CAST(N'2015-05-08 15:28:44.270' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[ImageCategories] OFF
SET IDENTITY_INSERT [dbo].[Images] ON 

INSERT [dbo].[Images] ([Id], [ImagebleId], [Filename], [ContentType], [Size], [Position], [Target], [CreatedAt], [UpdatedAt]) VALUES (1, 1, N'thrall.jpg', N'image/jpeg', 1114766, NULL, N'5', CAST(N'2015-05-08 15:29:52.573' AS DateTime), NULL)
INSERT [dbo].[Images] ([Id], [ImagebleId], [Filename], [ContentType], [Size], [Position], [Target], [CreatedAt], [UpdatedAt]) VALUES (2, 1, N'Screen Shot 2015-05-04 at 5.49.53 PM.png', N'image/png', 29579, NULL, N'5', CAST(N'2015-05-08 15:54:37.597' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Images] OFF
SET IDENTITY_INSERT [dbo].[Interests] ON 

INSERT [dbo].[Interests] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (1, N'Dibujar', NULL, NULL)
INSERT [dbo].[Interests] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (2, N'Pintar', NULL, NULL)
INSERT [dbo].[Interests] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (3, N'Escribir', NULL, NULL)
INSERT [dbo].[Interests] ([Id], [Name], [CreatedAt], [UpdatedAt]) VALUES (4, N'Deportes', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Interests] OFF
INSERT [dbo].[PageTypes] ([Id], [Name], [Description], [Active], [Position]) VALUES (4, N'ImageTopTextBottom', N'Imagen arriba, texto abajo', 1, 0)
INSERT [dbo].[PageTypes] ([Id], [Name], [Description], [Active], [Position]) VALUES (5, N'TextTopImageBottom', N'Texto arriba, imagen abajo', 1, 1)
INSERT [dbo].[PageTypes] ([Id], [Name], [Description], [Active], [Position]) VALUES (6, N'BigImageTextOverlayBottom', N'Imagen grande con texto', 1, 2)
INSERT [dbo].[PageTypes] ([Id], [Name], [Description], [Active], [Position]) VALUES (7, N'TextOnly', N'solo texto', 1, 3)
INSERT [dbo].[PageTypes] ([Id], [Name], [Description], [Active], [Position]) VALUES (8, N'BigImage', N'Solo imagen grande', 1, 4)
INSERT [dbo].[PageTypes] ([Id], [Name], [Description], [Active], [Position]) VALUES (9, N'SmallImage', N'Solo imagen pequena', 1, 5)
INSERT [dbo].[RoleMemberships] ([UserName], [RoleName]) VALUES (N'admin', N'superAdmin')
INSERT [dbo].[RoleMemberships] ([UserName], [RoleName]) VALUES (N'pepito', N'student')
INSERT [dbo].[RoleMemberships] ([UserName], [RoleName]) VALUES (N'schooladmin', N'schoolAdmin')
INSERT [dbo].[RoleMemberships] ([UserName], [RoleName]) VALUES (N'test100', N'student')
INSERT [dbo].[Roles] ([RoleName]) VALUES (N'schoolAdmin')
INSERT [dbo].[Roles] ([RoleName]) VALUES (N'student')
INSERT [dbo].[Roles] ([RoleName]) VALUES (N'superAdmin')
INSERT [dbo].[Schools] ([Id], [Name], [Details], [Address1], [Address2], [CityId], [Zip], [CreatedAt], [UpdatedAt]) VALUES (17, N'Jose s alegria', N'escuaela publica', N'test', N'test2', 10, N'00646', NULL, NULL)
INSERT [dbo].[Stories] ([Id], [UserName], [Name], [Summary], [CreatedAt], [UpdatedAt], [Featured], [ApprovedDate], [ApprovedBy], [Views], [Pages], [Status]) VALUES (11, N'admin', N'MCuento', N' Una prueba ', CAST(N'2015-05-07 15:11:42.473' AS DateTime), NULL, 0, CAST(N'2015-05-08 19:28:48.557' AS DateTime), N'admin', 43, N'[{"Type":"EmptyPage","Text":"","ImageId":0,"ImageUrl":"","Position":0},{"Type":"ImageTopTextBottom","Text":"<h2>Cambiar Titulo_1</h2>","ImageId":1,"ImageUrl":"/Content/dynamic/1/thrall.jpg","Position":1},{"Type":"BigImage","Text":"","ImageId":0,"ImageUrl":"/Content/dynamic/stories/11/Jurassic Park1.jpeg","Position":2},{"Type":"BigImage","Text":"","ImageId":0,"ImageUrl":"/Content/dynamic/stories/11/Jurassic Park2.jpeg","Position":3},{"Type":"BigImage","Text":"","ImageId":0,"ImageUrl":"/Content/dynamic/stories/11/testCuentosPdf1.jpeg","Position":4},{"Type":"BigImage","Text":"","ImageId":0,"ImageUrl":"/Content/dynamic/stories/11/testCuentosPdf2.jpeg","Position":5},{"Type":"BigImage","Text":"","ImageId":0,"ImageUrl":"/Content/dynamic/stories/11/testCuentosPdf3.jpeg","Position":6}]', 2)
INSERT [dbo].[Stories] ([Id], [UserName], [Name], [Summary], [CreatedAt], [UpdatedAt], [Featured], [ApprovedDate], [ApprovedBy], [Views], [Pages], [Status]) VALUES (15, N'admin', N'Segundo Cuento', N' Descripcion del cuento ', CAST(N'2015-05-13 14:56:53.690' AS DateTime), NULL, 0, NULL, NULL, 1, N'[{"Type":"EmptyPage","Text":"","ImageId":0,"ImageUrl":"","Position":0},{"Type":"ImageTopTextBottom","Text":"<h2>Cambiar Titulo_1</h2>","ImageId":0,"ImageUrl":"http://placehold.it/390x280&text=cambiar%20imagen","Position":1},{"Type":"TextTopImageBottom","Text":"<h2>Cambiar Texto_2</h2>","ImageId":0,"ImageUrl":"http://placehold.it/390x280&text=cambiar%20imagen","Position":2},{"Type":"BigImageTextOverlay","Text":"<h2>Cambiar Texto_3</h2>","ImageId":0,"ImageUrl":"http://placehold.it/390x558&text=cambiar%20imagen","Position":3},{"Type":"ImageTopTextBottom","Text":"<h2>Cambiar Texto_4</h2>","ImageId":0,"ImageUrl":"http://placehold.it/390x280&text=cambiar%20imagen","Position":4},{"Type":"BigImage","Text":"<h2>Cambiar Texto_5</h2>","ImageId":0,"ImageUrl":"http://placehold.it/390x558&text=cambiar%20imagen","Position":5}]', 1)
SET IDENTITY_INSERT [dbo].[StoryGrades] ON 

INSERT [dbo].[StoryGrades] ([Id], [StoryId], [GradeId]) VALUES (1, 11, 2)
INSERT [dbo].[StoryGrades] ([Id], [StoryId], [GradeId]) VALUES (2, 11, 2)
INSERT [dbo].[StoryGrades] ([Id], [StoryId], [GradeId]) VALUES (3, 15, 6)
INSERT [dbo].[StoryGrades] ([Id], [StoryId], [GradeId]) VALUES (4, 15, 10)
SET IDENTITY_INSERT [dbo].[StoryGrades] OFF
SET IDENTITY_INSERT [dbo].[StoryInterests] ON 

INSERT [dbo].[StoryInterests] ([Id], [StoryId], [InterestId]) VALUES (1, 11, 3)
INSERT [dbo].[StoryInterests] ([Id], [StoryId], [InterestId]) VALUES (2, 11, 3)
INSERT [dbo].[StoryInterests] ([Id], [StoryId], [InterestId]) VALUES (3, 15, 1)
INSERT [dbo].[StoryInterests] ([Id], [StoryId], [InterestId]) VALUES (4, 15, 2)
INSERT [dbo].[StoryInterests] ([Id], [StoryId], [InterestId]) VALUES (5, 15, 3)
SET IDENTITY_INSERT [dbo].[StoryInterests] OFF
SET IDENTITY_INSERT [dbo].[UserInterests] ON 

INSERT [dbo].[UserInterests] ([Id], [UserName], [InterestId]) VALUES (1, N'test100', 1)
INSERT [dbo].[UserInterests] ([Id], [UserName], [InterestId]) VALUES (2, N'test100', 3)
SET IDENTITY_INSERT [dbo].[UserInterests] OFF
INSERT [dbo].[Users] ([UserName], [PasswordHash], [PasswordSalt], [Email], [Comment], [IsApproved], [DateCreated], [DateLastLogin], [DateLastActivity], [DateLastPasswordChange], [Name], [LastName], [Age], [GradeId], [SchoolId], [ImageHolders_Id], [Featured], [ApprovedDate], [ApprovedBy], [Owner]) VALUES (N'admin', 0x76BF45DA01C710332BD44A6559C08639CE1498245DF1393FF0C6DBA10BF1A889321813EED9C15E9CA0BECB5AADA87699A79B4166A2F8B8BF13D37B86A2EBC0BD, 0x47D97ECBC8C3237939AC77906180599BC91458B0A79501C5CB969F8C4A1628A838678F077068CB72087DE010B1526153D76F01D2CFEFAA26BC63A3B8C47ECD7DB733DB4C3A2B299BA5CE8630F52D9DC63555A8131AE92B4A4D355859146918A82B4F3CECBA891664C20EE90B8654B49F3241F77EDA35D6CB2D4E598A4B7B5876, N'johan@digitaltree.com', NULL, 1, CAST(N'2015-05-07 14:51:23.030' AS DateTime), CAST(N'2015-05-13 15:09:21.650' AS DateTime), NULL, CAST(N'2015-05-07 14:51:23.030' AS DateTime), N'John', N'Smith', NULL, NULL, NULL, 16, 0, NULL, NULL, N'Student')
INSERT [dbo].[Users] ([UserName], [PasswordHash], [PasswordSalt], [Email], [Comment], [IsApproved], [DateCreated], [DateLastLogin], [DateLastActivity], [DateLastPasswordChange], [Name], [LastName], [Age], [GradeId], [SchoolId], [ImageHolders_Id], [Featured], [ApprovedDate], [ApprovedBy], [Owner]) VALUES (N'pepito', 0x76BF45DA01C710332BD44A6559C08639CE1498245DF1393FF0C6DBA10BF1A889321813EED9C15E9CA0BECB5AADA87699A79B4166A2F8B8BF13D37B86A2EBC0BD, 0x47D97ECBC8C3237939AC77906180599BC91458B0A79501C5CB969F8C4A1628A838678F077068CB72087DE010B1526153D76F01D2CFEFAA26BC63A3B8C47ECD7DB733DB4C3A2B299BA5CE8630F52D9DC63555A8131AE92B4A4D355859146918A82B4F3CECBA891664C20EE90B8654B49F3241F77EDA35D6CB2D4E598A4B7B5876, N'johan@digitaltree.com', NULL, 1, CAST(N'2015-05-07 14:51:23.203' AS DateTime), NULL, NULL, CAST(N'2015-05-07 14:51:23.203' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Users] ([UserName], [PasswordHash], [PasswordSalt], [Email], [Comment], [IsApproved], [DateCreated], [DateLastLogin], [DateLastActivity], [DateLastPasswordChange], [Name], [LastName], [Age], [GradeId], [SchoolId], [ImageHolders_Id], [Featured], [ApprovedDate], [ApprovedBy], [Owner]) VALUES (N'schooladmin', 0x76BF45DA01C710332BD44A6559C08639CE1498245DF1393FF0C6DBA10BF1A889321813EED9C15E9CA0BECB5AADA87699A79B4166A2F8B8BF13D37B86A2EBC0BD, 0x47D97ECBC8C3237939AC77906180599BC91458B0A79501C5CB969F8C4A1628A838678F077068CB72087DE010B1526153D76F01D2CFEFAA26BC63A3B8C47ECD7DB733DB4C3A2B299BA5CE8630F52D9DC63555A8131AE92B4A4D355859146918A82B4F3CECBA891664C20EE90B8654B49F3241F77EDA35D6CB2D4E598A4B7B5876, N'johan@digitaltree.com', NULL, 1, CAST(N'2015-05-07 14:51:23.123' AS DateTime), NULL, NULL, CAST(N'2015-05-07 14:51:23.123' AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Users] ([UserName], [PasswordHash], [PasswordSalt], [Email], [Comment], [IsApproved], [DateCreated], [DateLastLogin], [DateLastActivity], [DateLastPasswordChange], [Name], [LastName], [Age], [GradeId], [SchoolId], [ImageHolders_Id], [Featured], [ApprovedDate], [ApprovedBy], [Owner]) VALUES (N'test100', 0xA47660C40B33239E774D676C11B7A114554C4022536FD9CF4760CA47FCAD112BC680C6601A5EDE6050046778B8BA269F1F8C9D716ED8D4D684776D2D0EF7BB26, 0x9FF5D96559817762343E49975468599C1C90AD090CFE7B24866E1823A155244F30B29163AF5F8D4A7EF2E5CB9D201BF7718F4DECE8EEC8ADB47FA470DA3E4B431BB57A9236C66E15B02A5DA5AC5B50B8E177446A06C0A04534CD11C3D2913D225326291E24D3B8C58BB219B3643505DDB1B3294C62FEB86851A1B88E59A2160D, N'test@test.com', NULL, 0, CAST(N'2015-05-13 15:09:05.013' AS DateTime), NULL, NULL, CAST(N'2015-05-13 15:09:05.013' AS DateTime), N'juan', N'padre', 30, 4, 17, 18, 0, NULL, NULL, N'Parent')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1412695474, CAST(N'2015-05-07 14:51:15.000' AS DateTime), N'CreateInitialDatabase')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1412870778, CAST(N'2015-05-07 14:51:24.000' AS DateTime), N'Seed')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1415899231, CAST(N'2015-05-07 14:51:25.000' AS DateTime), N'_1415899231_Add_Featured_Column')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1415906902, CAST(N'2015-05-07 14:51:25.000' AS DateTime), N'AddCategoriesTable')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1416263869, CAST(N'2015-05-07 14:51:26.000' AS DateTime), N'AddFeaturedColumnUsers')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1416318645, CAST(N'2015-05-07 14:51:27.000' AS DateTime), N'AddApprovedDateAndApprovedByColumns')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1416335319, CAST(N'2015-05-07 14:51:29.000' AS DateTime), N'AddTablesStoryCategoriesStoryGrades')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1416346492, CAST(N'2015-05-07 14:51:29.000' AS DateTime), N'AddViewColumnToStories')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1417012531, CAST(N'2015-05-07 14:51:30.000' AS DateTime), N'AddPagesTable')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1417457013, CAST(N'2015-05-07 14:51:30.000' AS DateTime), N'AddContactsTable')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1418753829, CAST(N'2015-05-07 14:51:31.000' AS DateTime), N'AlterStoryTableAddingStatus')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1419000557, CAST(N'2015-05-07 14:51:32.000' AS DateTime), N'AddPageTypesTable')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1419429783, CAST(N'2015-05-07 14:51:32.000' AS DateTime), N'RemoveIsApprovedInStoriesTable')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1419865725, CAST(N'2015-05-07 14:51:34.000' AS DateTime), N'AddBuilderImagesTable')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1421261372, CAST(N'2015-05-07 14:51:35.000' AS DateTime), N'PageTypesTableInheritsImageble')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1421938589, CAST(N'2015-05-07 14:51:35.000' AS DateTime), N'ChangeSummaryLengthStory')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1430838720, CAST(N'2015-05-07 14:51:36.000' AS DateTime), N'CreateStoryInterestsTable')
INSERT [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (1430850522, CAST(N'2015-05-07 14:51:36.000' AS DateTime), N'AddOwnerColumn')
/****** Object:  Index [IX_BuilderGalleries_Id]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_BuilderGalleries_Id] ON [dbo].[BuilderGalleries]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_BuilderGalleries_Name]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_BuilderGalleries_Name] ON [dbo].[BuilderGalleries]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_BuilderGalleries_UserName]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_BuilderGalleries_UserName] ON [dbo].[BuilderGalleries]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Comments_ApprovedBy]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_ApprovedBy] ON [dbo].[Comments]
(
	[ApprovedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Comments_StoryId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_StoryId] ON [dbo].[Comments]
(
	[StoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Comments_UserName]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_UserName] ON [dbo].[Comments]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Contacts_SchoolId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Contacts_SchoolId] ON [dbo].[Contacts]
(
	[SchoolId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ImageCategories_Name]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_ImageCategories_Name] ON [dbo].[ImageCategories]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Pages_StoryId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Pages_StoryId] ON [dbo].[Pages]
(
	[StoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_PageTypes_Id]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_PageTypes_Id] ON [dbo].[PageTypes]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Ratings_StoryId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Ratings_StoryId] ON [dbo].[Ratings]
(
	[StoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Ratings_UserName]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Ratings_UserName] ON [dbo].[Ratings]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Schools_CityId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Schools_CityId] ON [dbo].[Schools]
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Schools_Id]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Schools_Id] ON [dbo].[Schools]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Schools_Name]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Schools_Name] ON [dbo].[Schools]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Stories_ApprovedBy]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Stories_ApprovedBy] ON [dbo].[Stories]
(
	[ApprovedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Stories_Id]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Stories_Id] ON [dbo].[Stories]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Stories_UserName]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Stories_UserName] ON [dbo].[Stories]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_StoryCategories_CategoryId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_StoryCategories_CategoryId] ON [dbo].[StoryCategories]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_StoryCategories_StoryId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_StoryCategories_StoryId] ON [dbo].[StoryCategories]
(
	[StoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_StoryGrades_GradeId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_StoryGrades_GradeId] ON [dbo].[StoryGrades]
(
	[GradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_StoryGrades_StoryId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_StoryGrades_StoryId] ON [dbo].[StoryGrades]
(
	[StoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_StoryInterests_InterestId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_StoryInterests_InterestId] ON [dbo].[StoryInterests]
(
	[InterestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_StoryInterests_StoryId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_StoryInterests_StoryId] ON [dbo].[StoryInterests]
(
	[StoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_UserInterests_InterestId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserInterests_InterestId] ON [dbo].[UserInterests]
(
	[InterestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserInterests_UserName]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserInterests_UserName] ON [dbo].[UserInterests]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Users_ApprovedBy]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Users_ApprovedBy] ON [dbo].[Users]
(
	[ApprovedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Users_GradeId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Users_GradeId] ON [dbo].[Users]
(
	[GradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Users_SchoolId]    Script Date: 5/21/2015 2:20:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_Users_SchoolId] ON [dbo].[Users]
(
	[SchoolId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[BuilderGalleries]  WITH CHECK ADD  CONSTRAINT [FK_BuilderGalleries_Id_Imagebles_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Imagebles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BuilderGalleries] CHECK CONSTRAINT [FK_BuilderGalleries_Id_Imagebles_Id]
GO
ALTER TABLE [dbo].[BuilderGalleries]  WITH CHECK ADD  CONSTRAINT [FK_BuilderGalleries_UserName_Users_UserName] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
GO
ALTER TABLE [dbo].[BuilderGalleries] CHECK CONSTRAINT [FK_BuilderGalleries_UserName_Users_UserName]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_ApprovedBy_Users_UserName] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[Users] ([UserName])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_ApprovedBy_Users_UserName]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_StoryId_Stories_Id] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Stories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_StoryId_Stories_Id]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_UserName_Users_UserName] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_UserName_Users_UserName]
GO
ALTER TABLE [dbo].[Contacts]  WITH CHECK ADD  CONSTRAINT [FK_Contacts_SchoolId_Schools_Id] FOREIGN KEY([SchoolId])
REFERENCES [dbo].[Schools] ([Id])
GO
ALTER TABLE [dbo].[Contacts] CHECK CONSTRAINT [FK_Contacts_SchoolId_Schools_Id]
GO
ALTER TABLE [dbo].[Images]  WITH CHECK ADD  CONSTRAINT [FK_Images_ImagebleId_Imagebles_Id] FOREIGN KEY([ImagebleId])
REFERENCES [dbo].[Imagebles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Images] CHECK CONSTRAINT [FK_Images_ImagebleId_Imagebles_Id]
GO
ALTER TABLE [dbo].[Pages]  WITH CHECK ADD  CONSTRAINT [FK_Pages_Id_Imagebles_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Imagebles] ([Id])
GO
ALTER TABLE [dbo].[Pages] CHECK CONSTRAINT [FK_Pages_Id_Imagebles_Id]
GO
ALTER TABLE [dbo].[Pages]  WITH CHECK ADD  CONSTRAINT [FK_Pages_StoryId_Stories_Id] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Stories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Pages] CHECK CONSTRAINT [FK_Pages_StoryId_Stories_Id]
GO
ALTER TABLE [dbo].[PageTypes]  WITH CHECK ADD  CONSTRAINT [FK_PageTypes_Id_Imagebles_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Imagebles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PageTypes] CHECK CONSTRAINT [FK_PageTypes_Id_Imagebles_Id]
GO
ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_Ratings_StoryId_Stories_Id] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Stories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_Ratings_StoryId_Stories_Id]
GO
ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_Ratings_UserName_Users_UserName] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
GO
ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_Ratings_UserName_Users_UserName]
GO
ALTER TABLE [dbo].[RoleMemberships]  WITH CHECK ADD  CONSTRAINT [FK_RoleMemberships_Roles] FOREIGN KEY([RoleName])
REFERENCES [dbo].[Roles] ([RoleName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMemberships] CHECK CONSTRAINT [FK_RoleMemberships_Roles]
GO
ALTER TABLE [dbo].[RoleMemberships]  WITH CHECK ADD  CONSTRAINT [FK_RoleMemberships_Users] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleMemberships] CHECK CONSTRAINT [FK_RoleMemberships_Users]
GO
ALTER TABLE [dbo].[Schools]  WITH CHECK ADD  CONSTRAINT [FK_Schools_CityId_Cities_Id] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Schools] CHECK CONSTRAINT [FK_Schools_CityId_Cities_Id]
GO
ALTER TABLE [dbo].[Schools]  WITH CHECK ADD  CONSTRAINT [FK_Schools_Id_Imagebles_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Imagebles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Schools] CHECK CONSTRAINT [FK_Schools_Id_Imagebles_Id]
GO
ALTER TABLE [dbo].[Stories]  WITH CHECK ADD  CONSTRAINT [FK_Stories_ApprovedBy_Users_UserName] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[Users] ([UserName])
GO
ALTER TABLE [dbo].[Stories] CHECK CONSTRAINT [FK_Stories_ApprovedBy_Users_UserName]
GO
ALTER TABLE [dbo].[Stories]  WITH CHECK ADD  CONSTRAINT [FK_Stories_Id_Imagebles_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Imagebles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stories] CHECK CONSTRAINT [FK_Stories_Id_Imagebles_Id]
GO
ALTER TABLE [dbo].[Stories]  WITH CHECK ADD  CONSTRAINT [FK_Stories_UserName_Users_UserName] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stories] CHECK CONSTRAINT [FK_Stories_UserName_Users_UserName]
GO
ALTER TABLE [dbo].[StoryCategories]  WITH CHECK ADD  CONSTRAINT [FK_StoryCategories_CategoryId_Categories_Id] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[StoryCategories] CHECK CONSTRAINT [FK_StoryCategories_CategoryId_Categories_Id]
GO
ALTER TABLE [dbo].[StoryCategories]  WITH CHECK ADD  CONSTRAINT [FK_StoryCategories_StoryId_Stories_Id] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Stories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoryCategories] CHECK CONSTRAINT [FK_StoryCategories_StoryId_Stories_Id]
GO
ALTER TABLE [dbo].[StoryGrades]  WITH CHECK ADD  CONSTRAINT [FK_StoryGrades_GradeId_Grades_Id] FOREIGN KEY([GradeId])
REFERENCES [dbo].[Grades] ([Id])
GO
ALTER TABLE [dbo].[StoryGrades] CHECK CONSTRAINT [FK_StoryGrades_GradeId_Grades_Id]
GO
ALTER TABLE [dbo].[StoryGrades]  WITH CHECK ADD  CONSTRAINT [FK_StoryGrades_StoryId_Stories_Id] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Stories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoryGrades] CHECK CONSTRAINT [FK_StoryGrades_StoryId_Stories_Id]
GO
ALTER TABLE [dbo].[StoryInterests]  WITH CHECK ADD  CONSTRAINT [FK_StoryInterests_InterestId_Interests_Id] FOREIGN KEY([InterestId])
REFERENCES [dbo].[Interests] ([Id])
GO
ALTER TABLE [dbo].[StoryInterests] CHECK CONSTRAINT [FK_StoryInterests_InterestId_Interests_Id]
GO
ALTER TABLE [dbo].[StoryInterests]  WITH CHECK ADD  CONSTRAINT [FK_StoryInterests_StoryId_Stories_Id] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Stories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoryInterests] CHECK CONSTRAINT [FK_StoryInterests_StoryId_Stories_Id]
GO
ALTER TABLE [dbo].[UserInterests]  WITH CHECK ADD  CONSTRAINT [FK_UserInterests_InterestId_Interests_Id] FOREIGN KEY([InterestId])
REFERENCES [dbo].[Interests] ([Id])
GO
ALTER TABLE [dbo].[UserInterests] CHECK CONSTRAINT [FK_UserInterests_InterestId_Interests_Id]
GO
ALTER TABLE [dbo].[UserInterests]  WITH CHECK ADD  CONSTRAINT [FK_UserInterests_UserName_Users_UserName] FOREIGN KEY([UserName])
REFERENCES [dbo].[Users] ([UserName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserInterests] CHECK CONSTRAINT [FK_UserInterests_UserName_Users_UserName]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_ApprovedBy_Users_UserName] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[Users] ([UserName])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_ApprovedBy_Users_UserName]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_GradeId_Grades_Id] FOREIGN KEY([GradeId])
REFERENCES [dbo].[Grades] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_GradeId_Grades_Id]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_ImageHolders_Id_Imagebles_Id] FOREIGN KEY([ImageHolders_Id])
REFERENCES [dbo].[Imagebles] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_ImageHolders_Id_Imagebles_Id]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_SchoolId_Schools_Id] FOREIGN KEY([SchoolId])
REFERENCES [dbo].[Schools] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_SchoolId_Schools_Id]
GO
USE [master]
GO
ALTER DATABASE [ccprstaging_db] SET  READ_WRITE 
GO
