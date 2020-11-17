USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[IndexFragmentationMessage]    Script Date: 2017-08-07 15:38:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IndexFragmentationMessage](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[DBName] [NVARCHAR](200) NULL,
	[SchemaName] [NVARCHAR](100) NULL,
	[TableName] [NVARCHAR](500) NULL,
	[IndexName] [NVARCHAR](500) NULL,
	[IndexType] [NVARCHAR](100) NULL,
	[Avg_Fragmentation_In_Percent] [DECIMAL](18, 5) NULL,
	[Page_Count] [INT] NULL,
	[Lob_Data_Space_Id] [INT] NULL,
	[Is_Disabled] [BIT] NULL,
	[Allow_Page_Locks] [BIT] NULL,
	[Exec_Alter_Index] [NVARCHAR](MAX) NULL,
	[MessageList] [NVARCHAR](2000) NULL,
	[AddTime] [DATETIME] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[IndexFragmentationMessage] ADD  DEFAULT (GETDATE()) FOR [AddTime]
GO


