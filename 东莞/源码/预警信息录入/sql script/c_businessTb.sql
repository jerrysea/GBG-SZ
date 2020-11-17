/****** Object:  Table [dbo].[A_Rwms_customer_status]    ******/
BEGIN TRY 

	IF EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'[A_Rwms_customer_status]') AND OBJECTPROPERTY(ID, 'IsTable') = 1) 
	BEGIN
	RAISERROR ('the db has contained the table ''A_Rwms_customer_status''' , 16, 1)
	END

	CREATE TABLE [dbo].[A_Rwms_customer_status](
		[DATADATE] [varchar](10) NOT NULL,
		[OBJECTTYPE] [varchar](18) NOT NULL,
		[OBJECTNO] [varchar](60) NOT NULL,
		[OBJECTNAME] [nvarchar](200) NOT NULL,
		[CERTTYPE] [varchar](20) NOT NULL,
		[CERTID] [varchar](60) NOT NULL,
		[SIGNALTRIGMODE] [varchar](10) NOT NULL,
		[RWLEVEL] [varchar](10) NOT NULL,
		[RWSTATUS] [varchar](10) NOT NULL,
		[SIGNALNO] [varchar](40) NOT NULL,
		[SIGNALTOPIC1] [nvarchar](40) NOT NULL,
		[SIGNALTOPIC2] [nvarchar](40) NOT NULL,
		[SIGNALNAME] [nvarchar](1000) NOT NULL,
		[SIGNALTYPE] [varchar](10) NOT NULL,
		[SIGNALTRIGDATE] [varchar](10) NOT NULL,
		[SIGNALLEVEL] [varchar](10) NOT NULL,
		[SIGNALSTATUS] [varchar](10) NOT NULL,
		[RULEFACTOR] [nvarchar](800) NOT NULL
	) ON [PRIMARY]
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



