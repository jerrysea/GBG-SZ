/****** Object:  Table [dbo].[Staging_Rwms_customer_status]   ******/
BEGIN TRY 
	IF EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'[Staging_Rwms_customer_status]') AND OBJECTPROPERTY(ID, 'IsTable') = 1) 
	BEGIN
	RAISERROR ('the db has contained the table ''Staging_Rwms_customer_status''' , 16, 1)
	END

	CREATE TABLE [dbo].[Staging_Rwms_customer_status](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[DATADATE] [varchar](max) NULL,
		[OBJECTTYPE] [varchar](max) NULL,
		[OBJECTNO] [varchar](max) NULL,
		[OBJECTNAME] [nvarchar](max) NULL,
		[CERTTYPE] [varchar](max) NULL,
		[CERTID] [varchar](max) NULL,
		[SIGNALTRIGMODE] [varchar](max) NULL,
		[RWLEVEL] [varchar](max) NULL,
		[RWSTATUS] [varchar](max) NULL,
		[SIGNALNO] [varchar](max) NULL,
		[SIGNALTOPIC1] [nvarchar](max) NULL,
		[SIGNALTOPIC2] [nvarchar](max) NULL,
		[SIGNALNAME] [nvarchar](max) NULL,
		[SIGNALTYPE] [varchar](max) NULL,
		[SIGNALTRIGDATE] [varchar](max) NULL,
		[SIGNALLEVEL] [varchar](max) NULL,
		[SIGNALSTATUS] [varchar](max) NULL,
		[RULEFACTOR] [nvarchar](max) NULL,
	 CONSTRAINT [PK_Staging_Rwms_customer_status] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END TRY 

BEGIN CATCH 
DECLARE @ErrorMessage NVARCHAR(4000); 
DECLARE @ErrorSeverity INT; 
DECLARE @ErrorState INT; 

SELECT 
@ErrorMessage = ERROR_MESSAGE(), 
@ErrorSeverity = ERROR_SEVERITY(), 
@ErrorState = ERROR_STATE(); 

RAISERROR (@ErrorMessage, -- Message text. 
@ErrorSeverity, -- Severity. 
@ErrorState -- State. 
); 
END CATCH
GO




