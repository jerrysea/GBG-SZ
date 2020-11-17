USE [InstinctDRC]
GO

/****** Object:  Table [dbo].[T01_IND_ERA_PAY_STAT_P]    Script Date: 2016/12/30 1:07:25 ******/
DROP TABLE [dbo].[T01_IND_ERA_PAY_STAT_P]
GO

/****** Object:  Table [dbo].[T01_IND_CUST_CORE_INFO_P]    Script Date: 2016/12/30 1:07:25 ******/
DROP TABLE [dbo].[T01_IND_CUST_CORE_INFO_P]
GO

/****** Object:  Table [dbo].[T01_IND_CUST_BIZ_INFO_P]    Script Date: 2016/12/30 1:07:25 ******/
DROP TABLE [dbo].[T01_IND_CUST_BIZ_INFO_P]
GO

/****** Object:  Table [dbo].[T01_CUST_RELATIVE_G]    Script Date: 2016/12/30 1:07:25 ******/
DROP TABLE [dbo].[T01_CUST_RELATIVE_G]
GO

/****** Object:  Table [dbo].[RW_IND_BASIC_INFO_TAB]    Script Date: 2016/12/30 1:07:25 ******/
DROP TABLE [dbo].[RW_IND_BASIC_INFO_TAB]
GO

/****** Object:  Table [dbo].[QZZC_UBNK_LOAN_BIZ]    Script Date: 2016/12/30 1:07:25 ******/
DROP TABLE [dbo].[QZZC_UBNK_LOAN_BIZ]
GO

/****** Object:  Table [dbo].[QZZC_UBNK_CRDT_INFO]    Script Date: 2016/12/30 1:07:25 ******/
DROP TABLE [dbo].[QZZC_UBNK_CRDT_INFO]
GO

/****** Object:  Table [dbo].[QZZC_APPC_ENT_INFO]    Script Date: 2016/12/30 1:07:25 ******/
DROP TABLE [dbo].[QZZC_APPC_ENT_INFO]
GO

/****** Object:  Table [dbo].[A_RWMS_CUSTOMER_STATUS]    Script Date: 2016/12/30 1:07:25 ******/
DROP TABLE [dbo].[A_RWMS_CUSTOMER_STATUS]
GO

/****** Object:  Table [dbo].[A_RWMS_CUSTOMER_STATUS]    Script Date: 2016/12/30 1:07:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[A_RWMS_CUSTOMER_STATUS](
	[DATADATE] [date] NOT NULL,
	[OBJECTTYPE] [nvarchar](18) NOT NULL,
	[OBJECTNO] [nvarchar](60) NOT NULL,
	[OBJECTNAME] [nvarchar](200) NOT NULL,
	[CERTTYPE] [nvarchar](20) NOT NULL,
	[CERTID] [nvarchar](60) NOT NULL,
	[SIGNALTRIGMODE] [nvarchar](10) NOT NULL,
	[RWLEVEL] [nvarchar](10) NOT NULL,
	[RWSTATUS] [nvarchar](10) NOT NULL,
	[SIGNALNO] [nvarchar](40) NOT NULL,
	[SIGNALTOPIC1] [nvarchar](40) NOT NULL,
	[SIGNALTOPIC2] [nvarchar](40) NOT NULL,
	[SIGNALNAME] [nvarchar](1000) NOT NULL,
	[SIGNALTYPE] [nvarchar](10) NOT NULL,
	[SIGNALTRIGDATE] [nvarchar](10) NOT NULL,
	[SIGNALLEVEL] [nvarchar](10) NOT NULL,
	[SIGNALSTATUS] [nvarchar](10) NOT NULL,
	[RULEFACTOR] [nvarchar](800) NOT NULL,
 CONSTRAINT [PK_A_RWMS_CUSTOMER_STATUS] PRIMARY KEY CLUSTERED 
(
	[DATADATE] ASC,
	[OBJECTNO] ASC,
	[SIGNALNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[QZZC_APPC_ENT_INFO]    Script Date: 2016/12/30 1:07:25 ******/
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
 CONSTRAINT [PK_QZZC_APPC_ENT_INFO] PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Appc_ID] ASC,
	[Ent_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[QZZC_UBNK_CRDT_INFO]    Script Date: 2016/12/30 1:07:25 ******/
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
 CONSTRAINT [PK_QZZC_UBNK_CRDT_INFO] PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[QZZC_UBNK_LOAN_BIZ]    Script Date: 2016/12/30 1:07:25 ******/
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
 CONSTRAINT [PK_QZZC_UBNK_LOAN_BIZ] PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC,
	[Biz_Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[RW_IND_BASIC_INFO_TAB]    Script Date: 2016/12/30 1:07:25 ******/
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
 CONSTRAINT [PK_RW_IND_BASIC_INFO_TAB] PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[T01_CUST_RELATIVE_G]    Script Date: 2016/12/30 1:07:25 ******/
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
 CONSTRAINT [PK_T01_CUST_RELATIVE_G] PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC,
	[Rela_Cust_Id] ASC,
	[Rela_Ship] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[T01_IND_CUST_BIZ_INFO_P]    Script Date: 2016/12/30 1:07:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T01_IND_CUST_BIZ_INFO_P](
	[Inputdate] [date] NOT NULL,
	[Updatedate] [date] NULL,
	[From_System] [nvarchar](10) NOT NULL,
	[Change_Dt] [date] NOT NULL,
	[Cert_Type_Cd] [nvarchar](2) NOT NULL,
	[Cert_ID] [nvarchar](20) NOT NULL,
	[Cust_ID] [nvarchar](10) NOT NULL,
	[Cust_Type] [nvarchar](1) NULL,
	[Biz_Type] [nvarchar](10) NULL,
	[Asset_Type] [nvarchar](10) NULL,
	[Asset_Bal] [decimal](15, 2) NULL,
	[Avg_Asset_Bal] [decimal](15, 2) NULL,
	[Vouch_Type] [nvarchar](10) NULL,
	[Card_Type] [nvarchar](10) NULL,
	[If_Vistnt] [nvarchar](1) NULL,
	[Bad_Repay_Rec] [nvarchar](1) NULL,
	[Mavg_Consm_Rec] [decimal](15, 2) NULL,
	[Chrem_Bal] [decimal](15, 2) NULL,
	[Avg_Chrem_Bal] [decimal](15, 2) NULL,
	[Stck_Bal] [decimal](15, 2) NULL,
	[Stck_Chanage_Dt] [date] NULL,
	[Loan_Type] [nvarchar](10) NULL,
	[Loan_Bal] [decimal](15, 2) NULL,
	[Star_Proc_Biz] [nvarchar](50) NULL,
	[Rmb_Curt_Avg_Bal] [decimal](15, 2) NULL,
	[Resv_Bal1] [decimal](15, 2) NULL,
	[Resv_Bal2] [decimal](15, 2) NULL,
	[Resv_Bal3] [decimal](15, 2) NULL,
	[Memo1] [nvarchar](40) NULL,
	[Memo2] [nvarchar](40) NULL,
 CONSTRAINT [PK_T01_IND_CUST_BIZ_INFO_P] PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Change_Dt] ASC,
	[Cert_Type_Cd] ASC,
	[Cert_ID] ASC,
	[Cust_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[T01_IND_CUST_CORE_INFO_P]    Script Date: 2016/12/30 1:07:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T01_IND_CUST_CORE_INFO_P](
	[Inputdate] [date] NOT NULL,
	[Updatedate] [date] NULL,
	[From_System] [nvarchar](10) NOT NULL,
	[Cust_ID] [nvarchar](10) NOT NULL,
	[Cust_Type] [nvarchar](1) NULL,
	[Cust_Seq_ID] [nvarchar](8) NULL,
	[Check_Place] [nvarchar](1) NULL,
	[Ind_Cn_Name] [nvarchar](60) NULL,
	[Ind_En_Name] [nvarchar](60) NULL,
	[Cust_Another_Name] [nvarchar](60) NULL,
	[Crdt_Lvl] [nvarchar](1) NULL,
	[Serv_Lvl] [nvarchar](3) NULL,
	[Biz_Pers_Id] [nvarchar](5) NULL,
	[Loan_Card_Id] [nvarchar](20) NULL,
	[Gold_Cust] [nvarchar](1) NULL,
	[Gender] [nvarchar](1) NULL,
	[Birth_Dt] [date] NULL,
	[Elec_Mail] [nvarchar](50) NULL,
	[Cert_Type] [nvarchar](1) NULL,
	[Cert_ID] [nvarchar](20) NULL,
	[Valid_Dt] [date] NULL,
	[Issue_Cert] [nvarchar](22) NULL,
	[Addr] [nvarchar](150) NULL,
	[Work_Unit] [nvarchar](60) NULL,
	[Tel_Id] [nvarchar](15) NULL,
	[Zip_Cd] [nvarchar](7) NULL,
	[Fax_Id] [nvarchar](15) NULL,
	[Pos] [nvarchar](10) NULL,
	[Career] [nvarchar](10) NULL,
	[En_Addr] [nvarchar](150) NULL,
	[Memo] [nvarchar](256) NULL,
	[Open_Acct_Org] [nvarchar](5) NULL,
	[Open_Acct_Tellr] [nvarchar](5) NULL,
	[Open_Acct_Dt] [date] NULL,
	[Rec_Sts] [nvarchar](1) NULL,
 CONSTRAINT [PK_T01_IND_CUST_CORE_INFO_P] PRIMARY KEY CLUSTERED 
(
	[Inputdate] ASC,
	[From_System] ASC,
	[Cust_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[T01_IND_ERA_PAY_STAT_P]    Script Date: 2016/12/30 1:07:25 ******/
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
 CONSTRAINT [PK_T01_IND_ERA_PAY_STAT_P] PRIMARY KEY CLUSTERED 
(
	[INPUTDATE] ASC,
	[FROM_SYSTEM] ASC,
	[CORE_CUST_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

