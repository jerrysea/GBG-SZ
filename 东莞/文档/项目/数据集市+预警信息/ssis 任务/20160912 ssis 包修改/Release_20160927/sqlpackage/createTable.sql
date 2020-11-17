USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[A_RWMS_CUSTOMER_STATUS]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[A_RWMS_CUSTOMER_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[A_RWMS_CUSTOMER_STATUS]
GO

/****** Object:  Table [dbo].[QZZC_APPC_ENT_INFO]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QZZC_APPC_ENT_INFO]') AND type in (N'U'))
DROP TABLE [dbo].[QZZC_APPC_ENT_INFO]
GO

/****** Object:  Table [dbo].[QZZC_UBNK_CRDT_INFO]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QZZC_UBNK_CRDT_INFO]') AND type in (N'U'))
DROP TABLE [dbo].[QZZC_UBNK_CRDT_INFO]
GO

/****** Object:  Table [dbo].[QZZC_UBNK_LOAN_BIZ]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QZZC_UBNK_LOAN_BIZ]') AND type in (N'U'))
DROP TABLE [dbo].[QZZC_UBNK_LOAN_BIZ]
GO

/****** Object:  Table [dbo].[RW_IND_BASIC_INFO_TAB]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RW_IND_BASIC_INFO_TAB]') AND type in (N'U'))
DROP TABLE [dbo].[RW_IND_BASIC_INFO_TAB]
GO

/****** Object:  Table [dbo].[Staging_QZZC_APPC_ENT_INFO]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_QZZC_APPC_ENT_INFO]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_QZZC_APPC_ENT_INFO]
GO

/****** Object:  Table [dbo].[Staging_QZZC_UBNK_CRDT_INFO]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_QZZC_UBNK_CRDT_INFO]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_QZZC_UBNK_CRDT_INFO]
GO

/****** Object:  Table [dbo].[Staging_QZZC_UBNK_LOAN_BIZ]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_QZZC_UBNK_LOAN_BIZ]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_QZZC_UBNK_LOAN_BIZ]
GO

/****** Object:  Table [dbo].[Staging_RW_IND_BASIC_INFO_TAB]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_RW_IND_BASIC_INFO_TAB]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_RW_IND_BASIC_INFO_TAB]
GO

/****** Object:  Table [dbo].[Staging_RWMS_CUSTOMER_STATUS]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_RWMS_CUSTOMER_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_RWMS_CUSTOMER_STATUS]
GO

/****** Object:  Table [dbo].[Staging_T01_CUST_RELATIVE_G]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_T01_CUST_RELATIVE_G]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_T01_CUST_RELATIVE_G]
GO

/****** Object:  Table [dbo].[Staging_T01_IC_ALI_OWE_LOAN_C]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_T01_IC_ALI_OWE_LOAN_C]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_T01_IC_ALI_OWE_LOAN_C]
GO

/****** Object:  Table [dbo].[Staging_T01_IC_BREAK_DEBETOR_C]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_T01_IC_BREAK_DEBETOR_C]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_T01_IC_BREAK_DEBETOR_C]
GO

/****** Object:  Table [dbo].[Staging_T01_IC_DEBETOR_C]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_T01_IC_DEBETOR_C]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_T01_IC_DEBETOR_C]
GO

/****** Object:  Table [dbo].[Staging_T01_IND_CUST_BIZ_INFO_P]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_T01_IND_CUST_BIZ_INFO_P]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_T01_IND_CUST_BIZ_INFO_P]
GO

/****** Object:  Table [dbo].[Staging_T01_IND_CUST_CORE_INFO_P]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_T01_IND_CUST_CORE_INFO_P]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_T01_IND_CUST_CORE_INFO_P]
GO

/****** Object:  Table [dbo].[Staging_T01_IND_ERA_PAY_STAT_P]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Staging_T01_IND_ERA_PAY_STAT_P]') AND type in (N'U'))
DROP TABLE [dbo].[Staging_T01_IND_ERA_PAY_STAT_P]
GO

/****** Object:  Table [dbo].[T01_CUST_RELATIVE_G]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T01_CUST_RELATIVE_G]') AND type in (N'U'))
DROP TABLE [dbo].[T01_CUST_RELATIVE_G]
GO

/****** Object:  Table [dbo].[T01_IC_ALI_OWE_LOAN_C]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T01_IC_ALI_OWE_LOAN_C]') AND type in (N'U'))
DROP TABLE [dbo].[T01_IC_ALI_OWE_LOAN_C]
GO

/****** Object:  Table [dbo].[T01_IC_BREAK_DEBETOR_C]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T01_IC_BREAK_DEBETOR_C]') AND type in (N'U'))
DROP TABLE [dbo].[T01_IC_BREAK_DEBETOR_C]
GO

/****** Object:  Table [dbo].[T01_IC_DEBETOR_C]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T01_IC_DEBETOR_C]') AND type in (N'U'))
DROP TABLE [dbo].[T01_IC_DEBETOR_C]
GO

/****** Object:  Table [dbo].[T01_IND_CUST_BIZ_INFO_P]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T01_IND_CUST_BIZ_INFO_P]') AND type in (N'U'))
DROP TABLE [dbo].[T01_IND_CUST_BIZ_INFO_P]
GO

/****** Object:  Table [dbo].[T01_IND_CUST_CORE_INFO_P]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T01_IND_CUST_CORE_INFO_P]') AND type in (N'U'))
DROP TABLE [dbo].[T01_IND_CUST_CORE_INFO_P]
GO

/****** Object:  Table [dbo].[T01_IND_ERA_PAY_STAT_P]    Script Date: 09/26/2016 22:26:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T01_IND_ERA_PAY_STAT_P]') AND type in (N'U'))
DROP TABLE [dbo].[T01_IND_ERA_PAY_STAT_P]
GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[A_RWMS_CUSTOMER_STATUS]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[A_RWMS_CUSTOMER_STATUS](
	[DATADATE] [date] NOT NULL,
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
	[RULEFACTOR] [nvarchar](800) NOT NULL,
 CONSTRAINT [PK_A_RWMS_CUSTOMER_STATUS] PRIMARY KEY CLUSTERED 
(
	[DATADATE] ASC,
	[OBJECTNO] ASC,
	[SIGNALNO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[QZZC_APPC_ENT_INFO]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QZZC_APPC_ENT_INFO](
	[Inputdate] [date] NOT NULL,
	[Updatedate] [date] NULL,
	[From_System] [nvarchar](20) NOT NULL,
	[Appc_ID] [nvarchar](60) NOT NULL,
	[Appc_Name] [nvarchar](80) NULL,
	[Appc_Cert_Type] [nvarchar](18) NULL,
	[Appc_Cert_ID] [nvarchar](32) NULL,
	[Ent_ID] [nvarchar](60) NOT NULL,
	[Ent_Name] [nvarchar](100) NULL,
	[Ent_Char] [nvarchar](20) NULL,
	[Ent_Rgst_Cap] [decimal](24, 6) NULL,
	[Ent_Found_Dt] [date] NULL,
	[Appc_Pos] [nvarchar](20) NULL,
	[Belong_Ent_Total_Crdt_Amt] [decimal](24, 6) NULL,
	[Belong_Ent_Total_Loan_Bal] [decimal](24, 6) NULL,
	[Rcnt_Loan_Mature_Dt] [date] NULL,
	[High_Five_Lvl_Class] [nvarchar](20) NULL,
	[Ent_Loan_Two_Years_Ovdue_Term] [int] NULL,
	[If_Blkl] [nvarchar](4) NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Appc_ID] ASC,
	[Ent_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[QZZC_UBNK_CRDT_INFO]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QZZC_UBNK_CRDT_INFO](
	[Inputdate] [date] NOT NULL,
	[Updatedate] [date] NULL,
	[From_System] [nvarchar](20) NOT NULL,
	[Cust_ID] [nvarchar](60) NOT NULL,
	[Cust_Name] [nvarchar](100) NULL,
	[Loan_Biz_Qtty] [int] NULL,
	[Loan_Total_Bal] [decimal](24, 6) NULL,
	[Norm_Bal] [decimal](24, 6) NULL,
	[Ovdue_Bal] [decimal](24, 6) NULL,
	[Dubil_Distr_Dt] [date] NULL,
	[Distr_Amt] [decimal](24, 6) NULL,
	[Dubil_Mature_Dt] [date] NULL,
	[Dubil_Amt] [decimal](24, 6) NULL,
	[Dubil_Bal] [decimal](24, 6) NULL,
	[Org_ID] [nvarchar](20) NULL,
	[Cust_Mgr_Id] [nvarchar](40) NULL,
	[Expand_Term_Cnt] [int] NULL,
	[Debt_Regr_Cnt] [int] NULL,
	[Btoaqn_Cnt] [int] NULL,
	[Ovdue_Qtty] [int] NULL,
	[Lowest_Flvl_Class] [nvarchar](20) NULL,
	[If_Wtoff] [nvarchar](4) NULL,
	[Accum_Ovdue_Cnt_2Y] [int] NULL,
	[If_Ovdue] [nvarchar](4) NULL,
	[Guar_If_Ovdue] [nvarchar](4) NULL,
	[Assu_Guar_Amt] [decimal](24, 6) NULL,
	[Crdt_Apply_Rej_Sno_Rcnt_6Mon] [int] NULL,
	[Crdt_Apply_Rej_Sdt_Rcnt_Onc] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[QZZC_UBNK_LOAN_BIZ]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QZZC_UBNK_LOAN_BIZ](
	[Inputdate] [date] NOT NULL,
	[Updatedate] [date] NULL,
	[From_System] [nvarchar](10) NOT NULL,
	[Cust_ID] [nvarchar](32) NOT NULL,
	[Cust_Nm] [nvarchar](80) NULL,
	[Biz_Type] [nvarchar](60) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC,
	[Biz_Type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[RW_IND_BASIC_INFO_TAB]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RW_IND_BASIC_INFO_TAB](
	[Inputdate] [date] NOT NULL,
	[Updatedate] [date] NULL,
	[From_System] [nvarchar](10) NOT NULL,
	[Cust_ID] [nvarchar](60) NOT NULL,
	[Cust_Name] [nvarchar](100) NULL,
	[Cert_Type] [nvarchar](20) NULL,
	[Cert_ID] [nvarchar](60) NULL,
	[If_Chur_Rsdnt] [nvarchar](4) NULL,
	[Bon_Emply_Flag] [nvarchar](4) NULL,
	[Birth_Dt] [date] NULL,
	[Gender] [nvarchar](4) NULL,
	[Marr_Situ] [nvarchar](4) NULL,
	[Prim_Nat] [nvarchar](20) NULL,
	[Cont_Tel] [nvarchar](32) NULL,
	[Elec_Mail] [nvarchar](60) NULL,
	[Family_Addr] [nvarchar](200) NULL,
	[Cens_Addr] [nvarchar](200) NULL,
	[Edu_Degree] [nvarchar](80) NULL,
	[Edu_Degree_Crdt_Card] [nvarchar](80) NULL,
	[Ind_Asset] [decimal](24, 6) NULL,
	[Ethnic_Cd] [nvarchar](20) NULL,
	[Work_Unit_Name] [nvarchar](100) NULL,
	[Unit_Addr] [nvarchar](200) NULL,
	[Career] [nvarchar](100) NULL,
	[Pos] [nvarchar](20) NULL,
	[Rcnt_24Mon_Pay_Rec] [nvarchar](100) NULL,
	[Crdt_Rating] [nvarchar](20) NULL,
	[Ind_Cust_Type] [nvarchar](20) NULL,
	[Admi_Regi_Encode] [nvarchar](20) NULL,
	[Heal_Situ_cd] [nvarchar](20) NULL,
	[Unit_Tel] [nvarchar](20) NULL,
	[Unit_Tel_113] [nvarchar](20) NULL,
	[Unit_Tel_Crdt_Card] [nvarchar](20) NULL,
	[Unit_Tel_Sts_114] [nvarchar](20) NULL,
	[Guar_Loan_If_Ovdue] [nvarchar](4) NULL,
	[Guar_Loan_Gura_Amt] [decimal](24, 6) NULL,
	[Family_Addr_Zip_Cd] [nvarchar](20) NULL,
	[Unit_Addr_Zip_Cd] [nvarchar](20) NULL,
	[Ind_Mon_Income] [decimal](24, 6) NULL,
	[Salry_Open_Acct_Bank] [nvarchar](50) NULL,
	[Cert_Prd_Valid] [date] NULL,
	[Grad_sch] [nvarchar](32) NULL,
	[Grad_Year] [nvarchar](10) NULL,
	[High_Deg] [nvarchar](18) NULL,
	[Home_Tel] [nvarchar](32) NULL,
	[Live_Situ] [nvarchar](18) NULL,
	[Pres_Addr_Live_Star_Dt] [nvarchar](10) NULL,
	[If_Local_Rpr] [nvarchar](100) NULL,
	[Comnctn_Addr] [nvarchar](200) NULL,
	[Comnctn_Addr_Zip_Cd] [nvarchar](10) NULL,
	[Info_Sts] [nvarchar](18) NULL,
	[Unit_Belong_Indus] [nvarchar](18) NULL,
	[Corp_Work_Star_Dt] [nvarchar](10) NULL,
	[Salry_Acct_Id] [nvarchar](32) NULL,
	[Family_Mon_Income] [decimal](24, 6) NULL,
	[Scty_Insure_Id] [nvarchar](32) NULL,
	[If_Crdt_Farm] [nvarchar](18) NULL,
	[Cust_Char] [nvarchar](100) NULL,
	[Cust_Type] [nvarchar](10) NULL,
	[Cust_Lvl] [nvarchar](10) NULL,
	[Loan_Card_Id] [nvarchar](32) NULL,
	[Memo] [nvarchar](400) NULL,
	[Stdby_Cust_Mgr] [nvarchar](32) NULL,
	[Rgst_Pers] [nvarchar](32) NULL,
	[Rgst] [nvarchar](32) NULL,
	[Rgst_Dt] [date] NULL,
	[Updater] [nvarchar](32) NULL,
	[Update_Dt] [date] NULL,
	[Update_Org] [nvarchar](32) NULL,
	[Int_Int] [nvarchar](400) NULL,
	[CRDT_RATING_ASS_DT] [date] NULL,
	[CRDT_RATING_MATURE_DT] [date] NULL,
	[IRE_RATING_RESLT] [nvarchar](20) NULL,
	[IRE_RATING_ASS_DT] [date] NULL,
	[IRE_RATING_MATURE_DT] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_QZZC_APPC_ENT_INFO]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_QZZC_APPC_ENT_INFO](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Updatedate] [nvarchar](255) NULL,
	[From_System] [nvarchar](255) NULL,
	[Appc_ID] [nvarchar](255) NULL,
	[Appc_Name] [nvarchar](255) NULL,
	[Appc_Cert_Type] [nvarchar](255) NULL,
	[Appc_Cert_ID] [nvarchar](255) NULL,
	[Ent_ID] [nvarchar](255) NULL,
	[Ent_Name] [nvarchar](255) NULL,
	[Ent_Char] [nvarchar](255) NULL,
	[Ent_Rgst_Cap] [nvarchar](255) NULL,
	[Ent_Found_Dt] [nvarchar](255) NULL,
	[Appc_Pos] [nvarchar](255) NULL,
	[Belong_Ent_Total_Crdt_Amt] [nvarchar](255) NULL,
	[Belong_Ent_Total_Loan_Bal] [nvarchar](255) NULL,
	[Rcnt_Loan_Mature_Dt] [nvarchar](255) NULL,
	[High_Five_Lvl_Class] [nvarchar](255) NULL,
	[Ent_Loan_Two_Years_Ovdue_Term] [nvarchar](255) NULL,
	[If_Blkl] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_QZZC_APPC_ENT_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_QZZC_UBNK_CRDT_INFO]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_QZZC_UBNK_CRDT_INFO](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Updatedate] [nvarchar](255) NULL,
	[From_System] [nvarchar](255) NULL,
	[Cust_ID] [nvarchar](255) NULL,
	[Cust_Name] [nvarchar](255) NULL,
	[Loan_Biz_Qtty] [nvarchar](255) NULL,
	[Loan_Total_Bal] [nvarchar](255) NULL,
	[Norm_Bal] [nvarchar](255) NULL,
	[Ovdue_Bal] [nvarchar](255) NULL,
	[Dubil_Distr_Dt] [nvarchar](255) NULL,
	[Distr_Amt] [nvarchar](255) NULL,
	[Dubil_Mature_Dt] [nvarchar](255) NULL,
	[Dubil_Amt] [nvarchar](255) NULL,
	[Dubil_Bal] [nvarchar](255) NULL,
	[Org_ID] [nvarchar](255) NULL,
	[Cust_Mgr_Id] [nvarchar](255) NULL,
	[Expand_Term_Cnt] [nvarchar](255) NULL,
	[Debt_Regr_Cnt] [nvarchar](255) NULL,
	[Btoaqn_Cnt] [nvarchar](255) NULL,
	[Ovdue_Qtty] [nvarchar](255) NULL,
	[Lowest_Flvl_Class] [nvarchar](255) NULL,
	[If_Wtoff] [nvarchar](255) NULL,
	[Accum_Ovdue_Cnt_2Y] [nvarchar](255) NULL,
	[If_Ovdue] [nvarchar](255) NULL,
	[Guar_If_Ovdue] [nvarchar](255) NULL,
	[Assu_Guar_Amt] [nvarchar](255) NULL,
	[Crdt_Apply_Rej_Sno_Rcnt_6Mon] [nvarchar](255) NULL,
	[Crdt_Apply_Rej_Sdt_Rcnt_Onc] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_QZZC_UBNK_CRDT_INFO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_QZZC_UBNK_LOAN_BIZ]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_QZZC_UBNK_LOAN_BIZ](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Updatedate] [nvarchar](255) NULL,
	[From_System] [nvarchar](255) NULL,
	[Cust_ID] [nvarchar](255) NULL,
	[Cust_Nm] [nvarchar](255) NULL,
	[Biz_Type] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_QZZC_UBNK_LOAN_BIZ] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_RW_IND_BASIC_INFO_TAB]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_RW_IND_BASIC_INFO_TAB](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Updatedate] [nvarchar](255) NULL,
	[From_System] [nvarchar](255) NULL,
	[Cust_ID] [nvarchar](255) NULL,
	[Cust_Name] [nvarchar](255) NULL,
	[Cert_Type] [nvarchar](255) NULL,
	[Cert_ID] [nvarchar](255) NULL,
	[If_Chur_Rsdnt] [nvarchar](255) NULL,
	[Bon_Emply_Flag] [nvarchar](255) NULL,
	[Birth_Dt] [nvarchar](255) NULL,
	[Gender] [nvarchar](255) NULL,
	[Marr_Situ] [nvarchar](255) NULL,
	[Prim_Nat] [nvarchar](255) NULL,
	[Cont_Tel] [nvarchar](255) NULL,
	[Elec_Mail] [nvarchar](255) NULL,
	[Family_Addr] [nvarchar](255) NULL,
	[Cens_Addr] [nvarchar](255) NULL,
	[Edu_Degree] [nvarchar](255) NULL,
	[Edu_Degree_Crdt_Card] [nvarchar](255) NULL,
	[Ind_Asset] [nvarchar](255) NULL,
	[Ethnic_Cd] [nvarchar](255) NULL,
	[Work_Unit_Name] [nvarchar](255) NULL,
	[Unit_Addr] [nvarchar](255) NULL,
	[Career] [nvarchar](255) NULL,
	[Pos] [nvarchar](255) NULL,
	[Rcnt_24Mon_Pay_Rec] [nvarchar](255) NULL,
	[Crdt_Rating] [nvarchar](255) NULL,
	[Ind_Cust_Type] [nvarchar](255) NULL,
	[Admi_Regi_Encode] [nvarchar](255) NULL,
	[Heal_Situ_cd] [nvarchar](255) NULL,
	[Unit_Tel] [nvarchar](255) NULL,
	[Unit_Tel_113] [nvarchar](255) NULL,
	[Unit_Tel_Crdt_Card] [nvarchar](255) NULL,
	[Unit_Tel_Sts_114] [nvarchar](255) NULL,
	[Guar_Loan_If_Ovdue] [nvarchar](255) NULL,
	[Guar_Loan_Gura_Amt] [nvarchar](255) NULL,
	[Family_Addr_Zip_Cd] [nvarchar](255) NULL,
	[Unit_Addr_Zip_Cd] [nvarchar](255) NULL,
	[Ind_Mon_Income] [nvarchar](255) NULL,
	[Salry_Open_Acct_Bank] [nvarchar](255) NULL,
	[Cert_Prd_Valid] [nvarchar](255) NULL,
	[Grad_sch] [nvarchar](255) NULL,
	[Grad_Year] [nvarchar](255) NULL,
	[High_Deg] [nvarchar](255) NULL,
	[Home_Tel] [nvarchar](255) NULL,
	[Live_Situ] [nvarchar](255) NULL,
	[Pres_Addr_Live_Star_Dt] [nvarchar](255) NULL,
	[If_Local_Rpr] [nvarchar](255) NULL,
	[Comnctn_Addr] [nvarchar](255) NULL,
	[Comnctn_Addr_Zip_Cd] [nvarchar](255) NULL,
	[Info_Sts] [nvarchar](255) NULL,
	[Unit_Belong_Indus] [nvarchar](255) NULL,
	[Corp_Work_Star_Dt] [nvarchar](255) NULL,
	[Salry_Acct_Id] [nvarchar](255) NULL,
	[Family_Mon_Income] [nvarchar](255) NULL,
	[Scty_Insure_Id] [nvarchar](255) NULL,
	[If_Crdt_Farm] [nvarchar](255) NULL,
	[Cust_Char] [nvarchar](255) NULL,
	[Cust_Type] [nvarchar](255) NULL,
	[Cust_Lvl] [nvarchar](255) NULL,
	[Loan_Card_Id] [nvarchar](255) NULL,
	[Memo] [nvarchar](400) NULL,
	[Stdby_Cust_Mgr] [nvarchar](255) NULL,
	[Rgst_Pers] [nvarchar](255) NULL,
	[Rgst] [nvarchar](255) NULL,
	[Rgst_Dt] [nvarchar](255) NULL,
	[Updater] [nvarchar](255) NULL,
	[Update_Dt] [nvarchar](255) NULL,
	[Update_Org] [nvarchar](255) NULL,
	[Int_Int] [nvarchar](400) NULL,
	[CRDT_RATING_ASS_DT] [nvarchar](255) NULL,
	[CRDT_RATING_MATURE_DT] [nvarchar](255) NULL,
	[IRE_RATING_RESLT] [nvarchar](255) NULL,
	[IRE_RATING_ASS_DT] [nvarchar](255) NULL,
	[IRE_RATING_MATURE_DT] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_RW_IND_BASIC_INFO_TAB] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_RWMS_CUSTOMER_STATUS]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_RWMS_CUSTOMER_STATUS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DATADATE] [nvarchar](255) NULL,
	[OBJECTTYPE] [nvarchar](255) NULL,
	[OBJECTNO] [nvarchar](255) NULL,
	[OBJECTNAME] [nvarchar](255) NULL,
	[CERTTYPE] [nvarchar](255) NULL,
	[CERTID] [nvarchar](255) NULL,
	[SIGNALTRIGMODE] [nvarchar](255) NULL,
	[RWLEVEL] [nvarchar](255) NULL,
	[RWSTATUS] [nvarchar](255) NULL,
	[SIGNALNO] [nvarchar](255) NULL,
	[SIGNALTOPIC1] [nvarchar](255) NULL,
	[SIGNALTOPIC2] [nvarchar](255) NULL,
	[SIGNALNAME] [nvarchar](1000) NULL,
	[SIGNALTYPE] [nvarchar](255) NULL,
	[SIGNALTRIGDATE] [nvarchar](255) NULL,
	[SIGNALLEVEL] [nvarchar](255) NULL,
	[SIGNALSTATUS] [nvarchar](255) NULL,
	[RULEFACTOR] [nvarchar](800) NULL,
 CONSTRAINT [PK_Staging_Rwms_customer_status] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_T01_CUST_RELATIVE_G]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_T01_CUST_RELATIVE_G](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Updatedate] [nvarchar](255) NULL,
	[From_System] [nvarchar](255) NULL,
	[Cust_ID] [nvarchar](255) NULL,
	[Rela_Cust_Id] [nvarchar](255) NULL,
	[Rela_Ship] [nvarchar](255) NULL,
	[Cust_Nm] [nvarchar](255) NULL,
	[Cert_Type] [nvarchar](255) NULL,
	[Cert_ID] [nvarchar](255) NULL,
	[Fictitious_Person] [nvarchar](255) NULL,
	[Curr_Type] [nvarchar](255) NULL,
	[Investment_Sum] [nvarchar](255) NULL,
	[Ought_Sum] [nvarchar](255) NULL,
	[Invest_Ratio] [nvarchar](255) NULL,
	[Invest_Tm] [nvarchar](255) NULL,
	[Stockcert_No] [nvarchar](255) NULL,
	[If_Valid] [nvarchar](255) NULL,
	[Valid_Flag] [nvarchar](255) NULL,
	[Pos_Tm] [nvarchar](255) NULL,
	[Indus_Year_Term] [nvarchar](255) NULL,
	[Rel_Desc] [nvarchar](1000) NULL,
	[Hold_Stck_Situ] [nvarchar](255) NULL,
	[Oth_Pos] [nvarchar](255) NULL,
	[Rgst_Pers] [nvarchar](255) NULL,
	[Rgst_Org] [nvarchar](255) NULL,
	[Rgst_Tm] [nvarchar](255) NULL,
	[Update_Tm] [nvarchar](255) NULL,
	[Shar_Type] [nvarchar](255) NULL,
	[Memo] [nvarchar](255) NULL,
	[GROUP_REL] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_T01_CUST_RELATIVE_G] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_T01_IC_ALI_OWE_LOAN_C]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_T01_IC_ALI_OWE_LOAN_C](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Seq_ID] [nvarchar](255) NULL,
	[Seq_No] [nvarchar](255) NULL,
	[Owe_Loan_Cust_Name] [nvarchar](300) NULL,
	[Idtty_En_ID] [nvarchar](255) NULL,
	[Gender] [nvarchar](255) NULL,
	[Age] [nvarchar](255) NULL,
	[Prov] [nvarchar](300) NULL,
	[Id_Card_Address] [nvarchar](300) NULL,
	[Loan_Mature_Tm] [nvarchar](255) NULL,
	[Loan_Term] [nvarchar](255) NULL,
	[Legal_Rep] [nvarchar](300) NULL,
	[Arre_Lim] [nvarchar](255) NULL,
	[Tb_Acct] [nvarchar](255) NULL,
	[Deflt_Situ] [nvarchar](3000) NULL,
 CONSTRAINT [PK_Staging_T01_IC_ALI_OWE_LOAN_C] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_T01_IC_BREAK_DEBETOR_C]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_T01_IC_BREAK_DEBETOR_C](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Seq_ID] [nvarchar](255) NULL,
	[Seq_No] [nvarchar](255) NULL,
	[Case_No] [nvarchar](255) NULL,
	[By_Transactor_Name] [nvarchar](300) NULL,
	[Break_Man_Type] [nvarchar](255) NULL,
	[Gender] [nvarchar](255) NULL,
	[Age] [nvarchar](255) NULL,
	[ID_Card_Ic_ID] [nvarchar](255) NULL,
	[ID_Card_Address] [nvarchar](300) NULL,
	[Legal_Rep_Name] [nvarchar](300) NULL,
	[Register_Tm] [nvarchar](255) NULL,
	[Issue_Tm] [nvarchar](255) NULL,
	[Exec_Court] [nvarchar](300) NULL,
	[Prov] [nvarchar](255) NULL,
	[Exec_Gist_ID] [nvarchar](255) NULL,
	[Exec_Gist_Unit] [nvarchar](300) NULL,
	[Effect_Law_Duty] [nvarchar](4000) NULL,
	[Break_Debetor_Behav] [nvarchar](4000) NULL,
	[Debetor_Fulfill_Situ] [nvarchar](4000) NULL,
	[Yes_Fulfill] [nvarchar](300) NULL,
	[Not_Fulfill] [nvarchar](300) NULL,
	[Conc_Cnt] [nvarchar](255) NULL,
	[Out_Dt] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_T01_IC_BREAK_DEBETOR_C] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_T01_IC_DEBETOR_C]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_T01_IC_DEBETOR_C](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Seq_ID] [nvarchar](255) NULL,
	[Seq_No] [nvarchar](255) NULL,
	[Case_No] [nvarchar](255) NULL,
	[By_Transactor_Name] [nvarchar](300) NULL,
	[Idtty_En_ID] [nvarchar](255) NULL,
	[Gender] [nvarchar](255) NULL,
	[Age] [nvarchar](255) NULL,
	[Prov] [nvarchar](255) NULL,
	[Id_Card_Address] [nvarchar](300) NULL,
	[Exec_Court] [nvarchar](300) NULL,
	[Register_Tm] [nvarchar](255) NULL,
	[Cae_Sts] [nvarchar](255) NULL,
	[Exec_Object] [nvarchar](255) NULL,
	[Conc_Cnt] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_T01_IC_DEBETOR_C] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_T01_IND_CUST_BIZ_INFO_P]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_T01_IND_CUST_BIZ_INFO_P](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Updatedate] [nvarchar](255) NULL,
	[From_System] [nvarchar](255) NULL,
	[Change_Dt] [nvarchar](255) NULL,
	[Cert_Type_Cd] [nvarchar](255) NULL,
	[Cert_ID] [nvarchar](255) NULL,
	[Cust_ID] [nvarchar](255) NULL,
	[Cust_Type] [nvarchar](255) NULL,
	[Biz_Type] [nvarchar](255) NULL,
	[Asset_Type] [nvarchar](255) NULL,
	[Asset_Bal] [nvarchar](255) NULL,
	[Avg_Asset_Bal] [nvarchar](255) NULL,
	[Vouch_Type] [nvarchar](255) NULL,
	[Card_Type] [nvarchar](255) NULL,
	[If_Vistnt] [nvarchar](255) NULL,
	[Bad_Repay_Rec] [nvarchar](255) NULL,
	[Mavg_Consm_Rec] [nvarchar](255) NULL,
	[Chrem_Bal] [nvarchar](255) NULL,
	[Avg_Chrem_Bal] [nvarchar](255) NULL,
	[Stck_Bal] [nvarchar](255) NULL,
	[Stck_Chanage_Dt] [nvarchar](255) NULL,
	[Loan_Type] [nvarchar](255) NULL,
	[Loan_Bal] [nvarchar](255) NULL,
	[Star_Proc_Biz] [nvarchar](255) NULL,
	[Rmb_Curt_Avg_Bal] [nvarchar](255) NULL,
	[Resv_Bal1] [nvarchar](255) NULL,
	[Resv_Bal2] [nvarchar](255) NULL,
	[Resv_Bal3] [nvarchar](255) NULL,
	[Memo1] [nvarchar](255) NULL,
	[Memo2] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_T01_IND_CUST_BIZ_INFO_P] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_T01_IND_CUST_CORE_INFO_P]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_T01_IND_CUST_CORE_INFO_P](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Inputdate] [nvarchar](255) NULL,
	[Updatedate] [nvarchar](255) NULL,
	[From_System] [nvarchar](255) NULL,
	[Cust_ID] [nvarchar](255) NULL,
	[Cust_Type] [nvarchar](255) NULL,
	[Cust_Seq_ID] [nvarchar](255) NULL,
	[Check_Place] [nvarchar](255) NULL,
	[Ind_Cn_Name] [nvarchar](255) NULL,
	[Ind_En_Name] [nvarchar](255) NULL,
	[Cust_Another_Name] [nvarchar](255) NULL,
	[Crdt_Lvl] [nvarchar](255) NULL,
	[Serv_Lvl] [nvarchar](255) NULL,
	[Biz_Pers_Id] [nvarchar](255) NULL,
	[Loan_Card_Id] [nvarchar](255) NULL,
	[Gold_Cust] [nvarchar](255) NULL,
	[Gender] [nvarchar](255) NULL,
	[Birth_Dt] [nvarchar](255) NULL,
	[Elec_Mail] [nvarchar](255) NULL,
	[Cert_Type] [nvarchar](255) NULL,
	[Cert_ID] [nvarchar](255) NULL,
	[Valid_Dt] [nvarchar](255) NULL,
	[Issue_Cert] [nvarchar](255) NULL,
	[Addr] [nvarchar](255) NULL,
	[Work_Unit] [nvarchar](255) NULL,
	[Tel_Id] [nvarchar](255) NULL,
	[Zip_Cd] [nvarchar](255) NULL,
	[Fax_Id] [nvarchar](255) NULL,
	[Pos] [nvarchar](255) NULL,
	[Career] [nvarchar](255) NULL,
	[En_Addr] [nvarchar](255) NULL,
	[Memo] [nvarchar](260) NULL,
	[Open_Acct_Org] [nvarchar](255) NULL,
	[Open_Acct_Tellr] [nvarchar](255) NULL,
	[Open_Acct_Dt] [nvarchar](255) NULL,
	[Rec_Sts] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_T01_IND_CUST_CORE_INFO_P] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[Staging_T01_IND_ERA_PAY_STAT_P]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging_T01_IND_ERA_PAY_STAT_P](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[INPUTDATE] [nvarchar](255) NULL,
	[UPDATEDATE] [nvarchar](255) NULL,
	[FROM_SYSTEM] [nvarchar](255) NULL,
	[RCMIS_CUST_ID] [nvarchar](255) NULL,
	[CORE_CUST_ID] [nvarchar](255) NULL,
	[CUST_NAME] [nvarchar](255) NULL,
	[IS_AGENT_DISTR_CUST] [nvarchar](255) NULL,
	[MON_AGENT_DEDUCT_AMT] [nvarchar](255) NULL,
	[PREV_YEAR_TERM_AG_DED_AMT] [nvarchar](255) NULL,
	[THIRD_MON_AG_DED_AMT] [nvarchar](255) NULL,
	[RCNT_DISTR_DT] [nvarchar](255) NULL,
	[EARLY_DISTR_DT] [nvarchar](255) NULL,
	[RCNT_SMON_ADIS_SAL_DAVG] [nvarchar](255) NULL,
	[SALRY] [nvarchar](255) NULL,
	[THR_MAVG_SALRY] [nvarchar](255) NULL,
	[SIX_MAVG_SALRY] [nvarchar](255) NULL,
	[ENT_NAME] [nvarchar](255) NULL,
 CONSTRAINT [PK_Staging_T01_IND_ERA_PAY_STAT_P] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[T01_CUST_RELATIVE_G]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T01_CUST_RELATIVE_G](
	[Inputdate] [date] NOT NULL,
	[Updatedate] [date] NULL,
	[From_System] [nvarchar](10) NOT NULL,
	[Cust_ID] [nvarchar](32) NOT NULL,
	[Rela_Cust_Id] [nvarchar](32) NOT NULL,
	[Rela_Ship] [nvarchar](18) NOT NULL,
	[Cust_Nm] [nvarchar](80) NULL,
	[Cert_Type] [nvarchar](18) NULL,
	[Cert_ID] [nvarchar](32) NULL,
	[Fictitious_Person] [nvarchar](80) NULL,
	[Curr_Type] [nvarchar](18) NULL,
	[Investment_Sum] [decimal](24, 0) NULL,
	[Ought_Sum] [decimal](24, 0) NULL,
	[Invest_Ratio] [decimal](10, 0) NULL,
	[Invest_Tm] [date] NULL,
	[Stockcert_No] [nvarchar](32) NULL,
	[If_Valid] [nvarchar](18) NULL,
	[Valid_Flag] [nvarchar](1) NULL,
	[Pos_Tm] [date] NULL,
	[Indus_Year_Term] [int] NULL,
	[Rel_Desc] [nvarchar](1000) NULL,
	[Hold_Stck_Situ] [nvarchar](200) NULL,
	[Oth_Pos] [nvarchar](100) NULL,
	[Rgst_Pers] [nvarchar](80) NULL,
	[Rgst_Org] [nvarchar](80) NULL,
	[Rgst_Tm] [date] NULL,
	[Update_Tm] [date] NULL,
	[Shar_Type] [nvarchar](2) NULL,
	[Memo] [nvarchar](200) NULL,
	[GROUP_REL] [nvarchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC,
	[Rela_Cust_Id] ASC,
	[Rela_Ship] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[T01_IC_ALI_OWE_LOAN_C]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T01_IC_ALI_OWE_LOAN_C](
	[Inputdate] [date] NOT NULL,
	[Seq_ID] [nvarchar](40) NOT NULL,
	[Seq_No] [nvarchar](40) NOT NULL,
	[Owe_Loan_Cust_Name] [nvarchar](300) NULL,
	[Idtty_En_ID] [nvarchar](100) NULL,
	[Gender] [nvarchar](64) NULL,
	[Age] [nvarchar](50) NULL,
	[Prov] [nvarchar](300) NULL,
	[Id_Card_Address] [nvarchar](300) NULL,
	[Loan_Mature_Tm] [date] NULL,
	[Loan_Term] [nvarchar](50) NULL,
	[Legal_Rep] [nvarchar](300) NULL,
	[Arre_Lim] [nvarchar](50) NULL,
	[Tb_Acct] [nvarchar](100) NULL,
	[Deflt_Situ] [nvarchar](3000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[Seq_ID] ASC,
	[Seq_No] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[T01_IC_BREAK_DEBETOR_C]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T01_IC_BREAK_DEBETOR_C](
	[Inputdate] [date] NOT NULL,
	[Seq_ID] [nvarchar](40) NOT NULL,
	[Seq_No] [nvarchar](40) NOT NULL,
	[Case_No] [nvarchar](100) NULL,
	[By_Transactor_Name] [nvarchar](300) NULL,
	[Break_Man_Type] [nvarchar](64) NULL,
	[Gender] [nvarchar](64) NULL,
	[Age] [nvarchar](50) NULL,
	[ID_Card_Ic_ID] [nvarchar](50) NULL,
	[ID_Card_Address] [nvarchar](300) NULL,
	[Legal_Rep_Name] [nvarchar](300) NULL,
	[Register_Tm] [date] NULL,
	[Issue_Tm] [date] NULL,
	[Exec_Court] [nvarchar](300) NULL,
	[Prov] [nvarchar](200) NULL,
	[Exec_Gist_ID] [nvarchar](100) NULL,
	[Exec_Gist_Unit] [nvarchar](300) NULL,
	[Effect_Law_Duty] [nvarchar](4000) NULL,
	[Break_Debetor_Behav] [nvarchar](4000) NULL,
	[Debetor_Fulfill_Situ] [nvarchar](4000) NULL,
	[Yes_Fulfill] [nvarchar](300) NULL,
	[Not_Fulfill] [nvarchar](300) NULL,
	[Conc_Cnt] [nvarchar](50) NULL,
	[Out_Dt] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[Seq_ID] ASC,
	[Seq_No] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[T01_IC_DEBETOR_C]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T01_IC_DEBETOR_C](
	[Inputdate] [date] NOT NULL,
	[Seq_ID] [nvarchar](40) NOT NULL,
	[Seq_No] [nvarchar](40) NOT NULL,
	[Case_No] [nvarchar](100) NULL,
	[By_Transactor_Name] [nvarchar](300) NULL,
	[Idtty_En_ID] [nvarchar](100) NULL,
	[Gender] [nvarchar](64) NULL,
	[Age] [nvarchar](50) NULL,
	[Prov] [nvarchar](200) NULL,
	[Id_Card_Address] [nvarchar](300) NULL,
	[Exec_Court] [nvarchar](300) NULL,
	[Register_Tm] [date] NULL,
	[Cae_Sts] [nvarchar](64) NULL,
	[Exec_Object] [nvarchar](50) NULL,
	[Conc_Cnt] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[Seq_ID] ASC,
	[Seq_No] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[T01_IND_CUST_BIZ_INFO_P]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T01_IND_CUST_BIZ_INFO_P](
	[Inputdate] [date] NOT NULL,
	[Updatedate] [date] NULL,
	[From_System] [nvarchar](10) NOT NULL,
	[Change_Dt] [date] NOT NULL,
	[Cert_Type_Cd] [char](2) NOT NULL,
	[Cert_ID] [char](20) NOT NULL,
	[Cust_ID] [char](10) NOT NULL,
	[Cust_Type] [char](1) NULL,
	[Biz_Type] [char](10) NULL,
	[Asset_Type] [char](10) NULL,
	[Asset_Bal] [decimal](15, 2) NULL,
	[Avg_Asset_Bal] [decimal](15, 2) NULL,
	[Vouch_Type] [char](10) NULL,
	[Card_Type] [char](10) NULL,
	[If_Vistnt] [char](1) NULL,
	[Bad_Repay_Rec] [char](1) NULL,
	[Mavg_Consm_Rec] [decimal](15, 2) NULL,
	[Chrem_Bal] [decimal](15, 2) NULL,
	[Avg_Chrem_Bal] [decimal](15, 2) NULL,
	[Stck_Bal] [decimal](15, 2) NULL,
	[Stck_Chanage_Dt] [date] NULL,
	[Loan_Type] [char](10) NULL,
	[Loan_Bal] [decimal](15, 2) NULL,
	[Star_Proc_Biz] [char](50) NULL,
	[Rmb_Curt_Avg_Bal] [decimal](15, 2) NULL,
	[Resv_Bal1] [decimal](15, 2) NULL,
	[Resv_Bal2] [decimal](15, 2) NULL,
	[Resv_Bal3] [decimal](15, 2) NULL,
	[Memo1] [nvarchar](40) NULL,
	[Memo2] [nvarchar](40) NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Change_Dt] ASC,
	[Cert_Type_Cd] ASC,
	[Cert_ID] ASC,
	[Cust_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[T01_IND_CUST_CORE_INFO_P]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T01_IND_CUST_CORE_INFO_P](
	[Inputdate] [date] NOT NULL,
	[Updatedate] [date] NULL,
	[From_System] [nvarchar](10) NOT NULL,
	[Cust_ID] [char](10) NOT NULL,
	[Cust_Type] [char](1) NULL,
	[Cust_Seq_ID] [char](8) NULL,
	[Check_Place] [char](1) NULL,
	[Ind_Cn_Name] [nvarchar](60) NULL,
	[Ind_En_Name] [nvarchar](60) NULL,
	[Cust_Another_Name] [nvarchar](60) NULL,
	[Crdt_Lvl] [char](1) NULL,
	[Serv_Lvl] [char](3) NULL,
	[Biz_Pers_Id] [char](5) NULL,
	[Loan_Card_Id] [char](20) NULL,
	[Gold_Cust] [char](1) NULL,
	[Gender] [char](1) NULL,
	[Birth_Dt] [date] NULL,
	[Elec_Mail] [nvarchar](50) NULL,
	[Cert_Type] [char](1) NULL,
	[Cert_ID] [char](20) NULL,
	[Valid_Dt] [date] NULL,
	[Issue_Cert] [char](22) NULL,
	[Addr] [nvarchar](150) NULL,
	[Work_Unit] [nvarchar](60) NULL,
	[Tel_Id] [char](15) NULL,
	[Zip_Cd] [char](7) NULL,
	[Fax_Id] [char](15) NULL,
	[Pos] [char](10) NULL,
	[Career] [char](10) NULL,
	[En_Addr] [nvarchar](150) NULL,
	[Memo] [nvarchar](256) NULL,
	[Open_Acct_Org] [char](5) NULL,
	[Open_Acct_Tellr] [char](5) NULL,
	[Open_Acct_Dt] [date] NULL,
	[Rec_Sts] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[T01_IND_ERA_PAY_STAT_P]    Script Date: 09/26/2016 22:26:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T01_IND_ERA_PAY_STAT_P](
	[INPUTDATE] [date] NOT NULL,
	[UPDATEDATE] [date] NULL,
	[FROM_SYSTEM] [nvarchar](10) NOT NULL,
	[RCMIS_CUST_ID] [nvarchar](60) NULL,
	[CORE_CUST_ID] [nvarchar](60) NOT NULL,
	[CUST_NAME] [nvarchar](60) NULL,
	[IS_AGENT_DISTR_CUST] [nvarchar](1) NULL,
	[MON_AGENT_DEDUCT_AMT] [decimal](24, 6) NULL,
	[PREV_YEAR_TERM_AG_DED_AMT] [decimal](24, 6) NULL,
	[THIRD_MON_AG_DED_AMT] [decimal](24, 6) NULL,
	[RCNT_DISTR_DT] [date] NULL,
	[EARLY_DISTR_DT] [date] NULL,
	[RCNT_SMON_ADIS_SAL_DAVG] [date] NULL,
	[SALRY] [decimal](24, 6) NULL,
	[THR_MAVG_SALRY] [decimal](24, 6) NULL,
	[SIX_MAVG_SALRY] [decimal](24, 6) NULL,
	[ENT_NAME] [nvarchar](60) NULL,
PRIMARY KEY CLUSTERED 
(
	[INPUTDATE] ASC,
	[FROM_SYSTEM] ASC,
	[CORE_CUST_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

