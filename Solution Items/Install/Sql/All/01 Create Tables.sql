SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Create Table [dbo].[Person]
CREATE TABLE [dbo].[Person](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](200) NOT NULL,
	[LastName] [nvarchar](200) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Person] ADD CONSTRAINT [DF_Person_Active] DEFAULT ((0)) FOR [Active]
GO

-- Create Table [dbo].[CommonMeal]
CREATE TABLE [dbo].[CommonMeal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_CommonMeal] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Create Table [dbo].[CommonMealRegistration]

CREATE TABLE [dbo].[CommonMealRegistration](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[CommonMealId] [int] NOT NULL,
	[Attending] [bit] NOT NULL,
 CONSTRAINT [PK_CommonMealRegistration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CommonMealRegistration] ADD  CONSTRAINT [DF_CommonMealRegistration_Attending]  DEFAULT ((0)) FOR [Attending]
GO

ALTER TABLE [dbo].[CommonMealRegistration]  WITH CHECK ADD  CONSTRAINT [FK_CommonMealRegistration_CommonMeal] FOREIGN KEY([CommonMealId])
REFERENCES [dbo].[CommonMeal] ([Id])
GO

ALTER TABLE [dbo].[CommonMealRegistration] CHECK CONSTRAINT [FK_CommonMealRegistration_CommonMeal]
GO

ALTER TABLE [dbo].[CommonMealRegistration]  WITH CHECK ADD  CONSTRAINT [FK_CommonMealRegistration_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([Id])
GO

ALTER TABLE [dbo].[CommonMealRegistration] CHECK CONSTRAINT [FK_CommonMealRegistration_Person]
GO
