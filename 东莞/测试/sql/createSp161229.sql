USE [InstinctDRC]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_ERA_PAY_STAT_P_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_T01_IND_ERA_PAY_STAT_P_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_BCPInsert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_BCPInsert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_BIZ_INFO_P_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_BIZ_INFO_P_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IC_DEBETOR_C_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_T01_IC_DEBETOR_C_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IC_BREAK_DEBETOR_C_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_T01_IC_BREAK_DEBETOR_C_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IC_ALI_OWE_LOAN_C_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_T01_IC_ALI_OWE_LOAN_C_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_CUST_RELATIVE_G_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_T01_CUST_RELATIVE_G_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_RWMS_CUSTOMER_STATUS_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_RWMS_CUSTOMER_STATUS_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_RW_IND_BASIC_INFO_TAB_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_RW_IND_BASIC_INFO_TAB_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_UBNK_LOAN_BIZ_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_QZZC_UBNK_LOAN_BIZ_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_UBNK_CRDT_INFO_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_QZZC_UBNK_CRDT_INFO_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]    Script Date: 2016/12/30 1:06:15 ******/
DROP PROCEDURE [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;
	--DELETE A FROM 
	--QZZC_APPC_ENT_INFO A(NOLOCK),Staging_QZZC_APPC_ENT_INFO B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Appc_ID = B.Appc_ID AND A.Ent_ID = B.Ent_ID
        TRUNCATE TABLE QZZC_APPC_ENT_INFO;

    -- Insert statements for procedure here
        INSERT  INTO QZZC_APPC_ENT_INFO
                SELECT  [Inputdate] ,
                        [Updatedate] ,
                        [From_System] ,
                        [Appc_ID] ,
                        [Appc_Name] ,
                        [Appc_Cert_Type] ,
                        [Appc_Cert_ID] ,
                        [Ent_ID] ,
                        [Ent_Name] ,
                        [Ent_Char] ,
                        ( CASE WHEN ISNUMERIC([Ent_Rgst_Cap]) = 1
                               THEN [Ent_Rgst_Cap]
                               ELSE NULL
                          END ) AS [Ent_Rgst_Cap] ,
                        [Ent_Found_Dt] ,
                        [Appc_Pos] ,
                        ( CASE WHEN ISNUMERIC([Belong_Ent_Total_Crdt_Amt]) = 1
                               THEN [Belong_Ent_Total_Crdt_Amt]
                               ELSE NULL
                          END ) AS [Belong_Ent_Total_Crdt_Amt] ,
                        ( CASE WHEN ISNUMERIC([Belong_Ent_Total_Loan_Bal]) = 1
                               THEN [Belong_Ent_Total_Loan_Bal]
                               ELSE NULL
                          END ) AS [Belong_Ent_Total_Loan_Bal] ,
                        [Rcnt_Loan_Mature_Dt] ,
                        [High_Five_Lvl_Class] ,
                        ( CASE WHEN ISNUMERIC([Ent_Loan_Two_Years_Ovdue_Term]) = 1
                               THEN [Ent_Loan_Two_Years_Ovdue_Term]
                               ELSE NULL
                          END ) AS [Ent_Loan_Two_Years_Ovdue_Term] ,
                        [If_Blkl]
                FROM    [Staging_QZZC_APPC_ENT_INFO]
                WHERE   Inputdate IS NOT NULL
                        AND From_System IS NOT NULL
                        AND Appc_ID IS NOT NULL
                        AND Appc_Name IS NOT NULL
                        AND Ent_ID IS NOT NULL;

        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;


GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_UBNK_CRDT_INFO_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DRC_QZZC_UBNK_CRDT_INFO_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;       
		
        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;

        TRUNCATE TABLE QZZC_UBNK_CRDT_INFO;
		
    -- Insert statements for procedure here
        INSERT  INTO QZZC_UBNK_CRDT_INFO
                SELECT  [Inputdate] ,
                        [Updatedate] ,
                        [From_System] ,
                        [Cust_ID] ,
                        [Cust_Name] ,
                        ( CASE WHEN ISNUMERIC([Loan_Biz_Qtty]) = 1
                               THEN [Loan_Biz_Qtty]
                               ELSE NULL
                          END ) AS [Loan_Biz_Qtty] ,
                        ( CASE WHEN ISNUMERIC([Loan_Total_Bal]) = 1
                               THEN [Loan_Total_Bal]
                               ELSE NULL
                          END ) AS [Loan_Total_Bal] ,
                        ( CASE WHEN ISNUMERIC([Norm_Bal]) = 1 THEN [Norm_Bal]
                               ELSE NULL
                          END ) AS [Norm_Bal] ,
                        ( CASE WHEN ISNUMERIC([Ovdue_Bal]) = 1
                               THEN [Ovdue_Bal]
                               ELSE NULL
                          END ) AS [Ovdue_Bal] ,
                        [Dubil_Distr_Dt] ,
                        ( CASE WHEN ISNUMERIC([Distr_Amt]) = 1
                               THEN [Distr_Amt]
                               ELSE NULL
                          END ) AS [Distr_Amt] ,
                        [Dubil_Mature_Dt] ,
                        ( CASE WHEN ISNUMERIC([Dubil_Amt]) = 1
                               THEN [Dubil_Amt]
                               ELSE NULL
                          END ) AS [Dubil_Amt] ,
                        ( CASE WHEN ISNUMERIC([Dubil_Bal]) = 1
                               THEN [Dubil_Bal]
                               ELSE NULL
                          END ) AS [Dubil_Bal] ,
                        [Org_ID] ,
                        [Cust_Mgr_Id] ,
                        ( CASE WHEN ISNUMERIC([Expand_Term_Cnt]) = 1
                               THEN [Expand_Term_Cnt]
                               ELSE NULL
                          END ) AS [Expand_Term_Cnt] ,
                        ( CASE WHEN ISNUMERIC([Debt_Regr_Cnt]) = 1
                               THEN [Debt_Regr_Cnt]
                               ELSE NULL
                          END ) AS [Debt_Regr_Cnt] ,
                        ( CASE WHEN ISNUMERIC([Btoaqn_Cnt]) = 1
                               THEN [Btoaqn_Cnt]
                               ELSE NULL
                          END ) AS [Btoaqn_Cnt] ,
                        ( CASE WHEN ISNUMERIC([Ovdue_Qtty]) = 1
                               THEN [Ovdue_Qtty]
                               ELSE NULL
                          END ) AS [Ovdue_Qtty] ,
                        [Lowest_Flvl_Class] ,
                        [If_Wtoff] ,
                        ( CASE WHEN ISNUMERIC([Accum_Ovdue_Cnt_2Y]) = 1
                               THEN [Accum_Ovdue_Cnt_2Y]
                               ELSE NULL
                          END ) AS [Accum_Ovdue_Cnt_2Y] ,
                        [If_Ovdue] ,
                        [Guar_If_Ovdue] ,
                        ( CASE WHEN ISNUMERIC([Assu_Guar_Amt]) = 1
                               THEN [Assu_Guar_Amt]
                               ELSE NULL
                          END ) AS [Assu_Guar_Amt] ,
                        ( CASE WHEN ISNUMERIC([Crdt_Apply_Rej_Sno_Rcnt_6Mon]) = 1
                               THEN [Crdt_Apply_Rej_Sno_Rcnt_6Mon]
                               ELSE NULL
                          END ) AS [Crdt_Apply_Rej_Sno_Rcnt_6Mon] ,
                        [Crdt_Apply_Rej_Sdt_Rcnt_Onc]
                FROM    [Staging_QZZC_UBNK_CRDT_INFO]
                WHERE   Inputdate IS NOT NULL
                        AND From_System IS NOT NULL
                        AND Cust_ID IS NOT NULL;
        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;


GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_UBNK_LOAN_BIZ_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DRC_QZZC_UBNK_LOAN_BIZ_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;

        TRUNCATE TABLE QZZC_UBNK_LOAN_BIZ;
    -- Insert statements for procedure here
        INSERT  INTO QZZC_UBNK_LOAN_BIZ
                SELECT  [Inputdate] ,
                        [Updatedate] ,
                        [From_System] ,
                        [Cust_ID] ,
                        [Cust_Nm] ,
                        [Biz_Type]
                FROM    [Staging_QZZC_UBNK_LOAN_BIZ]
                WHERE   Inputdate IS NOT NULL
                        AND From_System IS NOT NULL
                        AND Cust_ID IS NOT NULL
                        AND Biz_Type IS NOT NULL;
        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_RW_IND_BASIC_INFO_TAB_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DRC_RW_IND_BASIC_INFO_TAB_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;

        --DELETE  A
        --FROM    RW_IND_BASIC_INFO_TAB A ( NOLOCK ) ,
        --        Staging_RW_IND_BASIC_INFO_TAB B ( NOLOCK )
        --WHERE   A.Inputdate = B.Inputdate
        --        AND A.From_System = B.From_System
        --        AND A.Cust_ID = B.Cust_ID; 
        TRUNCATE TABLE RW_IND_BASIC_INFO_TAB;
    -- Insert statements for procedure here
        INSERT  INTO RW_IND_BASIC_INFO_TAB
                SELECT  [Inputdate] ,
                        [Updatedate] ,
                        [From_System] ,
                        [Cust_ID] ,
                        [Cust_Name] ,
                        [Cert_Type] ,
                        [Cert_ID] ,
                        [If_Chur_Rsdnt] ,
                        [Bon_Emply_Flag] ,
                        [Birth_Dt] ,
                        [Gender] ,
                        [Marr_Situ] ,
                        [Prim_Nat] ,
                        [Cont_Tel] ,
                        [Elec_Mail] ,
                        [Family_Addr] ,
                        [Cens_Addr] ,
                        [Edu_Degree] ,
                        [Edu_Degree_Crdt_Card] ,
                        ( CASE WHEN ISNUMERIC([Ind_Asset]) = 1
                               THEN [Ind_Asset]
                               ELSE NULL
                          END ) AS [Ind_Asset] ,
                        [Ethnic_Cd] ,
                        [Work_Unit_Name] ,
                        [Unit_Addr] ,
                        [Career] ,
                        [Pos] ,
                        [Rcnt_24Mon_Pay_Rec] ,
                        [Crdt_Rating] ,
                        [Ind_Cust_Type] ,
                        [Admi_Regi_Encode] ,
                        [Heal_Situ_cd] ,
                        [Unit_Tel] ,
                        [Unit_Tel_113] ,
                        [Unit_Tel_Crdt_Card] ,
                        [Unit_Tel_Sts_114] ,
                        [Guar_Loan_If_Ovdue] ,
                        [Guar_Loan_Gura_Amt] ,
                        [Family_Addr_Zip_Cd] ,
                        [Unit_Addr_Zip_Cd] ,
                        ( CASE WHEN ISNUMERIC([Ind_Mon_Income]) = 1
                               THEN [Ind_Mon_Income]
                               ELSE NULL
                          END ) AS [Ind_Mon_Income] ,
                        [Salry_Open_Acct_Bank] ,
                        [Cert_Prd_Valid] ,
                        [Grad_sch] ,
                        [Grad_Year] ,
                        [High_Deg] ,
                        [Home_Tel] ,
                        [Live_Situ] ,
                        [Pres_Addr_Live_Star_Dt] ,
                        [If_Local_Rpr] ,
                        [Comnctn_Addr] ,
                        [Comnctn_Addr_Zip_Cd] ,
                        [Info_Sts] ,
                        [Unit_Belong_Indus] ,
                        [Corp_Work_Star_Dt] ,
                        [Salry_Acct_Id] ,
                        ( CASE WHEN ISNUMERIC([Family_Mon_Income]) = 1
                               THEN [Family_Mon_Income]
                               ELSE NULL
                          END ) AS [Family_Mon_Income] ,
                        [Scty_Insure_Id] ,
                        [If_Crdt_Farm] ,
                        [Cust_Char] ,
                        [Cust_Type] ,
                        [Cust_Lvl] ,
                        [Loan_Card_Id] ,
                        [Memo] ,
                        [Stdby_Cust_Mgr] ,
                        [Rgst_Pers] ,
                        [Rgst] ,
                        [Rgst_Dt] ,
                        [Updater] ,
                        [Update_Dt] ,
                        [Update_Org] ,
                        [Int_Int] ,
                        [CRDT_RATING_ASS_DT] ,
                        [CRDT_RATING_MATURE_DT] ,
                        [IRE_RATING_RESLT] ,
                        [IRE_RATING_ASS_DT] ,
                        [IRE_RATING_MATURE_DT]
                FROM    [dbo].[Staging_RW_IND_BASIC_INFO_TAB]
                WHERE   Inputdate IS NOT NULL
                        AND From_System IS NOT NULL
                        AND Cust_ID IS NOT NULL;

        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;


GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_RWMS_CUSTOMER_STATUS_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_RWMS_CUSTOMER_STATUS_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;
        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;

        TRUNCATE TABLE [A_RWMS_CUSTOMER_STATUS];
	
    -- Insert statements for procedure here
        INSERT  INTO [A_RWMS_CUSTOMER_STATUS]
                SELECT  [DATADATE] ,
                        [OBJECTTYPE] ,
                        [OBJECTNO] ,
                        [OBJECTNAME] ,
                        [CERTTYPE] ,
                        [CERTID] ,
                        [SIGNALTRIGMODE] ,
                        [RWLEVEL] ,
                        [RWSTATUS] ,
                        [SIGNALNO] ,
                        [SIGNALTOPIC1] ,
                        [SIGNALTOPIC2] ,
                        [SIGNALNAME] ,
                        [SIGNALTYPE] ,
                        [SIGNALTRIGDATE] ,
                        [SIGNALLEVEL] ,
                        [SIGNALSTATUS] ,
                        [RULEFACTOR]
                FROM    [Staging_RWMS_CUSTOMER_STATUS]
                WHERE   DATADATE IS NOT NULL
                        AND OBJECTNO IS NOT NULL
                        AND SIGNALNO IS NOT NULL;

        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_CUST_RELATIVE_G_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_T01_CUST_RELATIVE_G_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;

        TRUNCATE TABLE T01_CUST_RELATIVE_G;
    -- Insert statements for procedure here
        INSERT  INTO T01_CUST_RELATIVE_G
                SELECT  [Inputdate] ,
                        [Updatedate] ,
                        [From_System] ,
                        [Cust_ID] ,
                        [Rela_Cust_Id] ,
                        [Rela_Ship] ,
                        [Cust_Nm] ,
                        [Cert_Type] ,
                        [Cert_ID] ,
                        [Fictitious_Person] ,
                        [Curr_Type] ,
                        ( CASE WHEN ISNUMERIC([Investment_Sum]) = 1
                               THEN [Investment_Sum]
                               ELSE NULL
                          END ) AS [Investment_Sum] ,
                        ( CASE WHEN ISNUMERIC([Ought_Sum]) = 1
                               THEN [Ought_Sum]
                               ELSE NULL
                          END ) AS [Ought_Sum] ,
                        ( CASE WHEN ISNUMERIC([Invest_Ratio]) = 1
                               THEN [Invest_Ratio]
                               ELSE NULL
                          END ) AS [Invest_Ratio] ,
                        [Invest_Tm] ,
                        [Stockcert_No] ,
                        [If_Valid] ,
                        [Valid_Flag] ,
                        [Pos_Tm] ,
                        ( CASE WHEN ISNUMERIC([Indus_Year_Term]) = 1
                               THEN [Indus_Year_Term]
                               ELSE NULL
                          END ) AS [Indus_Year_Term] ,
                        [Rel_Desc] ,
                        [Hold_Stck_Situ] ,
                        [Oth_Pos] ,
                        [Rgst_Pers] ,
                        [Rgst_Org] ,
                        [Rgst_Tm] ,
                        [Update_Tm] ,
                        [Shar_Type] ,
                        [Memo] ,
                        [GROUP_REL]
                FROM    [Staging_T01_CUST_RELATIVE_G]
                WHERE   Inputdate IS NOT NULL
                        AND From_System IS NOT NULL
                        AND Cust_ID IS NOT NULL
                        AND Rela_Cust_Id IS NOT NULL
                        AND Rela_Ship IS NOT NULL;
        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IC_ALI_OWE_LOAN_C_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_T01_IC_ALI_OWE_LOAN_C_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;

        DELETE  A
        FROM    T01_IC_ALI_OWE_LOAN_C A ,
                Staging_T01_IC_ALI_OWE_LOAN_C B ( NOLOCK )
        WHERE   A.Inputdate = B.Inputdate
                AND A.Seq_ID = B.Seq_ID
                AND A.Seq_No = B.Seq_No; 

    -- Insert statements for procedure here
        INSERT  INTO T01_IC_ALI_OWE_LOAN_C
                SELECT  [Inputdate] ,
                        [Seq_ID] ,
                        [Seq_No] ,
                        [Owe_Loan_Cust_Name] ,
                        [Idtty_En_ID] ,
                        [Gender] ,
                        [Age] ,
                        [Prov] ,
                        [Id_Card_Address] ,
                        [Loan_Mature_Tm] ,
                        [Loan_Term] ,
                        [Legal_Rep] ,
                        [Arre_Lim] ,
                        [Tb_Acct] ,
                        [Deflt_Situ]
                FROM    [Staging_T01_IC_ALI_OWE_LOAN_C]
                WHERE   Inputdate IS NOT NULL
                        AND Seq_ID IS NOT NULL
                        AND Seq_No IS NOT NULL;

        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;


GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IC_BREAK_DEBETOR_C_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_T01_IC_BREAK_DEBETOR_C_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;

        DELETE  A
        FROM    T01_IC_BREAK_DEBETOR_C A ,
                Staging_T01_IC_BREAK_DEBETOR_C B ( NOLOCK )
        WHERE   A.Inputdate = B.Inputdate
                AND A.Seq_ID = B.Seq_ID
                AND A.Seq_No = B.Seq_No; 

    -- Insert statements for procedure here
        INSERT  INTO T01_IC_BREAK_DEBETOR_C
                SELECT  [Inputdate] ,
                        [Seq_ID] ,
                        [Seq_No] ,
                        [Case_No] ,
                        [By_Transactor_Name] ,
                        [Break_Man_Type] ,
                        [Gender] ,
                        [Age] ,
                        [ID_Card_Ic_ID] ,
                        [ID_Card_Address] ,
                        [Legal_Rep_Name] ,
                        [Register_Tm] ,
                        [Issue_Tm] ,
                        [Exec_Court] ,
                        [Prov] ,
                        [Exec_Gist_ID] ,
                        [Exec_Gist_Unit] ,
                        [Effect_Law_Duty] ,
                        [Break_Debetor_Behav] ,
                        [Debetor_Fulfill_Situ] ,
                        [Yes_Fulfill] ,
                        [Not_Fulfill] ,
                        [Conc_Cnt] ,
                        [Out_Dt]
                FROM    [Staging_T01_IC_BREAK_DEBETOR_C]
                WHERE   Inputdate IS NOT NULL
                        AND Seq_ID IS NOT NULL
                        AND Seq_No IS NOT NULL;

        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IC_DEBETOR_C_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_T01_IC_DEBETOR_C_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;

        DELETE  A
        FROM    T01_IC_DEBETOR_C A  ,
                Staging_T01_IC_DEBETOR_C B ( NOLOCK )
        WHERE   A.Inputdate = B.Inputdate
                AND A.Seq_ID = B.Seq_ID
                AND A.Seq_No = B.Seq_No; 

    -- Insert statements for procedure here
        INSERT  INTO T01_IC_DEBETOR_C
                SELECT  [Inputdate] ,
                        [Seq_ID] ,
                        [Seq_No] ,
                        [Case_No] ,
                        [By_Transactor_Name] ,
                        [Idtty_En_ID] ,
                        [Gender] ,
                        [Age] ,
                        [Prov] ,
                        [Id_Card_Address] ,
                        [Exec_Court] ,
                        [Register_Tm] ,
                        [Cae_Sts] ,
                        [Exec_Object] ,
                        [Conc_Cnt]
                FROM    [Staging_T01_IC_DEBETOR_C]
                WHERE   Inputdate IS NOT NULL
                        AND Seq_ID IS NOT NULL
                        AND Seq_No IS NOT NULL;

        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_BIZ_INFO_P_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_BIZ_INFO_P_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;
        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;
	--DELETE A FROM 
	--T01_IND_CUST_BIZ_INFO_P A(NOLOCK),Staging_T01_IND_CUST_BIZ_INFO_P B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Change_Dt=b.Change_Dt AND A.Cert_Type_Cd = B.Cert_Type_Cd
	--AND A.Cert_ID = B.Cert_ID AND A.Cust_ID = B.Cust_ID 
        TRUNCATE TABLE T01_IND_CUST_BIZ_INFO_P;
    -- Insert statements for procedure here
        INSERT  INTO T01_IND_CUST_BIZ_INFO_P
                SELECT  [Inputdate] ,
                        [Updatedate] ,
                        [From_System] ,
                        [Change_Dt] ,
                        [Cert_Type_Cd] ,
                        [Cert_ID] ,
                        [Cust_ID] ,
                        [Cust_Type] ,
                        [Biz_Type] ,
                        [Asset_Type] ,
                        ( CASE WHEN ISNUMERIC([Asset_Bal]) = 1
                               THEN [Asset_Bal]
                               ELSE NULL
                          END ) [Asset_Bal] ,
                        ( CASE WHEN ISNUMERIC([Avg_Asset_Bal]) = 1
                               THEN [Avg_Asset_Bal]
                               ELSE NULL
                          END ) AS [Avg_Asset_Bal] ,
                        [Vouch_Type] ,
                        [Card_Type] ,
                        [If_Vistnt] ,
                        [Bad_Repay_Rec] ,
                        ( CASE WHEN ISNUMERIC([Mavg_Consm_Rec]) = 1
                               THEN [Mavg_Consm_Rec]
                               ELSE NULL
                          END ) AS [Mavg_Consm_Rec] ,
                        ( CASE WHEN ISNUMERIC([Chrem_Bal]) = 1
                               THEN [Chrem_Bal]
                               ELSE NULL
                          END ) AS [Chrem_Bal] ,
                        ( CASE WHEN ISNUMERIC([Avg_Chrem_Bal]) = 1
                               THEN [Avg_Chrem_Bal]
                               ELSE NULL
                          END ) AS [Avg_Chrem_Bal] ,
                        ( CASE WHEN ISNUMERIC([Stck_Bal]) = 1 THEN [Stck_Bal]
                               ELSE NULL
                          END ) AS [Stck_Bal] ,
                        [Stck_Chanage_Dt] ,
                        [Loan_Type] ,
                        ( CASE WHEN ISNUMERIC([Loan_Bal]) = 1 THEN [Loan_Bal]
                               ELSE NULL
                          END ) AS [Loan_Bal] ,
                        [Star_Proc_Biz] ,
                        ( CASE WHEN ISNUMERIC([Rmb_Curt_Avg_Bal]) = 1
                               THEN [Rmb_Curt_Avg_Bal]
                               ELSE NULL
                          END ) AS [Rmb_Curt_Avg_Bal] ,
                        ( CASE WHEN ISNUMERIC([Resv_Bal1]) = 1
                               THEN [Resv_Bal1]
                               ELSE NULL
                          END ) AS [Resv_Bal1] ,
                        ( CASE WHEN ISNUMERIC([Resv_Bal2]) = 1
                               THEN [Resv_Bal2]
                               ELSE NULL
                          END ) AS [Resv_Bal2] ,
                        ( CASE WHEN ISNUMERIC([Resv_Bal3]) = 1
                               THEN [Resv_Bal3]
                               ELSE NULL
                          END ) AS [Resv_Bal3] ,
                        [Memo1] ,
                        [Memo2]
                FROM    [Staging_T01_IND_CUST_BIZ_INFO_P]
                WHERE   Inputdate IS NOT NULL
                        AND From_System IS NOT NULL
                        AND Change_Dt IS NOT NULL
                        AND Cert_Type_Cd IS NOT NULL
                        AND Cert_ID IS NOT NULL
                        AND Cust_ID IS NOT NULL;

        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_BCPInsert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_BCPInsert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;
		       
        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;
        BEGIN TRY
    -- Insert statements for procedure here
            DELETE  A
            FROM    [dbo].[Staging_T01_IND_CUST_CORE_INFO_P] AS B ( NOLOCK ) ,
                    [dbo].[T01_IND_CUST_CORE_INFO_P] AS A
            WHERE   REPLACE(REPLACE(B.Inputdate, ' ', ''), '"', '') = A.Inputdate
                    AND REPLACE(REPLACE(B.From_System, ' ', ''), '"', '') = A.From_System
                    AND REPLACE(REPLACE(B.Cust_ID, ' ', ''), '"', '') = A.Cust_ID;

            INSERT  INTO T01_IND_CUST_CORE_INFO_P            
                    SELECT 
                            REPLACE(REPLACE([Inputdate], ' ', ''), '"', '') AS [Inputdate] ,
                            REPLACE(REPLACE([Updatedate], ' ', ''), '"', '') AS [Updatedate] ,
                            REPLACE(REPLACE([From_System], ' ', ''), '"', '') AS [From_System] ,
                            REPLACE(REPLACE([Cust_ID], ' ', ''), '"', '') AS [Cust_ID] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Cust_Type],''), ' ', ''), '"', ''),1,1) AS [Cust_Type] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Cust_Seq_ID],''), ' ', ''), '"', ''),1,8) AS [Cust_Seq_ID] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Check_Place],''), ' ', ''), '"', ''),1,1) AS [Check_Place] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Ind_Cn_Name],''), ' ', ''), '"', ''),1,60) AS [Ind_Cn_Name] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Ind_En_Name],''), ' ', ''), '"', ''),1,60) AS [Ind_En_Name] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Cust_Another_Name],''), ' ', ''), '"',''),1,60) AS [Cust_Another_Name] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Crdt_Lvl],''), ' ', ''), '"', ''),1,1) AS [Crdt_Lvl] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Serv_Lvl],''), ' ', ''), '"', ''),1,3) AS [Serv_Lvl] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Biz_Pers_Id],''), ' ', ''), '"', ''),1,5) AS [Biz_Pers_Id] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Loan_Card_Id],''), ' ', ''), '"', ''),1,20) AS [Loan_Card_Id] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Gold_Cust],''), ' ', ''), '"', ''),1,1) AS [Gold_Cust] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Gender],''), ' ', ''), '"', ''),1,1) AS [Gender] ,
                            (CASE WHEN ISDATE(REPLACE(REPLACE([Birth_Dt], ' ', ''), '"', ''))=1 THEN REPLACE(REPLACE([Birth_Dt], ' ', ''), '"', '') ELSE NULL END ) AS [Birth_Dt] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Elec_Mail],''), ' ', ''), '"', ''),1,50) AS [Elec_Mail] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Cert_Type],''), ' ', ''), '"', ''),1,1) AS [Cert_Type] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Cert_ID],''), ' ', ''), '"', ''),1,20) AS [Cert_ID] ,
                            (CASE WHEN ISDATE(REPLACE(REPLACE([Valid_Dt], ' ', ''), '"', ''))=1 THEN REPLACE(REPLACE([Valid_Dt], ' ', ''), '"', '') ELSE NULL END) AS [Valid_Dt] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Issue_Cert],''), ' ', ''), '"', ''),1,22) AS [Issue_Cert] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Addr],''), ' ', ''), '"', ''),1,150) AS [Addr] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Work_Unit],''), ' ', ''), '"', ''),1,60) AS [Work_Unit] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Tel_Id],''), ' ', ''), '"', ''),1,15) AS [Tel_Id] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Zip_Cd],''), ' ', ''), '"', ''),1,7) AS [Zip_Cd] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Fax_Id],''), ' ', ''), '"', ''),1,15) AS [Fax_Id] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Pos],''), ' ', ''), '"', ''),1,10) AS [Pos] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Career],''), ' ', ''), '"', ''),1,10) AS [Career] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([En_Addr],''), ' ', ''), '"', ''),1,150) AS [En_Addr] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Memo],''), ' ', ''), '"', ''),1,256) AS [Memo] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Open_Acct_Org],''), ' ', ''), '"', ''),1,5) AS [Open_Acct_Org] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Open_Acct_Tellr],''), ' ', ''), '"',''),1,5) AS [Open_Acct_Tellr] ,
                            (CASE WHEN ISDATE(REPLACE(REPLACE([Open_Acct_Dt], ' ', ''), '"', ''))=1 THEN REPLACE(REPLACE([Open_Acct_Dt], ' ', ''), '"', '') ELSE NULL END) AS [Open_Acct_Dt] ,
                            SUBSTRING(REPLACE(REPLACE(ISNULL([Rec_Sts],''), ' ', ''), '"', ''),1,1) AS [Rec_Sts]
                    FROM    [dbo].[Staging_T01_IND_CUST_CORE_INFO_P]
                    WHERE   Inputdate IS NOT NULL
                            AND From_System IS NOT NULL
                            AND Cust_ID IS NOT NULL;
            SELECT  @rowlength = @@ROWCOUNT;
			TRUNCATE TABLE dbo.Staging_T01_IND_CUST_CORE_INFO_P;
            SELECT  @IsContinue = 0;	
        END TRY
        BEGIN CATCH
            PRINT ERROR_MESSAGE();
            IF @@TRANCOUNT > 0
                ROLLBACK;
        END CATCH;			
        RETURN @rowlength;
    END;



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        DECLARE @rowlength BIGINT;        
        SET @rowlength = 0;
        BEGIN TRY
    -- Insert statements for procedure here
            DELETE  A
            FROM    dbo.T01_IND_CUST_CORE_INFO_P A ,
                    dbo.Staging_T01_IND_CUST_CORE_INFO_P B ( NOLOCK )
            WHERE   A.Inputdate = B.Inputdate
                    AND A.From_System = B.From_System
                    AND A.Cust_ID = B.Cust_ID;

            INSERT  INTO T01_IND_CUST_CORE_INFO_P
                    SELECT  [Inputdate] ,
                            [Updatedate] ,
                            [From_System] ,
                            [Cust_ID] ,
                            [Cust_Type] ,
                            [Cust_Seq_ID] ,
                            [Check_Place] ,
                            [Ind_Cn_Name] ,
                            [Ind_En_Name] ,
                            [Cust_Another_Name] ,
                            [Crdt_Lvl] ,
                            [Serv_Lvl] ,
                            [Biz_Pers_Id] ,
                            [Loan_Card_Id] ,
                            [Gold_Cust] ,
                            [Gender] ,
                            [Birth_Dt] ,
                            [Elec_Mail] ,
                            [Cert_Type] ,
                            [Cert_ID] ,
                            [Valid_Dt] ,
                            [Issue_Cert] ,
                            [Addr] ,
                            [Work_Unit] ,
                            [Tel_Id] ,
                            [Zip_Cd] ,
                            [Fax_Id] ,
                            [Pos] ,
                            [Career] ,
                            [En_Addr] ,
                            [Memo] ,
                            [Open_Acct_Org] ,
                            [Open_Acct_Tellr] ,
                            [Open_Acct_Dt] ,
                            [Rec_Sts]
                    FROM    [Staging_T01_IND_CUST_CORE_INFO_P]
                    WHERE   Inputdate IS NOT NULL
                            AND From_System IS NOT NULL
                            AND Cust_ID IS NOT NULL;
            SELECT  @rowlength = @@ROWCOUNT;
			TRUNCATE TABLE dbo.Staging_T01_IND_CUST_CORE_INFO_P;
            SELECT  @IsContinue = 0;		
        END TRY
        BEGIN CATCH
            PRINT ERROR_MESSAGE();
            IF @@TRANCOUNT > 0
                ROLLBACK;
        END CATCH;		
        RETURN @rowlength;
    END;


GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_ERA_PAY_STAT_P_Insert]    Script Date: 2016/12/30 1:06:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_T01_IND_ERA_PAY_STAT_P_Insert]
    @IsContinue BIT = 0 OUTPUT
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;
        DECLARE @rowlength BIGINT;
        
        SET @rowlength = 0;
        TRUNCATE TABLE T01_IND_ERA_PAY_STAT_P;
    -- Insert statements for procedure here
        INSERT  INTO T01_IND_ERA_PAY_STAT_P
                SELECT  [INPUTDATE] ,
                        [UPDATEDATE] ,
                        [FROM_SYSTEM] ,
                        [RCMIS_CUST_ID] ,
                        [CORE_CUST_ID] ,
                        [CUST_NAME] ,
                        [IS_AGENT_DISTR_CUST] ,
                        ( CASE WHEN ISNUMERIC([MON_AGENT_DEDUCT_AMT]) = 1
                               THEN [MON_AGENT_DEDUCT_AMT]
                               ELSE NULL
                          END ) AS [MON_AGENT_DEDUCT_AMT] ,
                        ( CASE WHEN ISNUMERIC([PREV_YEAR_TERM_AG_DED_AMT]) = 1
                               THEN [PREV_YEAR_TERM_AG_DED_AMT]
                               ELSE NULL
                          END ) AS [PREV_YEAR_TERM_AG_DED_AMT] ,
                        ( CASE WHEN ISNUMERIC([THIRD_MON_AG_DED_AMT]) = 1
                               THEN [THIRD_MON_AG_DED_AMT]
                               ELSE NULL
                          END ) AS [THIRD_MON_AG_DED_AMT] ,
                        [RCNT_DISTR_DT] ,
                        [EARLY_DISTR_DT] ,
                        [RCNT_SMON_ADIS_SAL_DAVG] ,
                        ( CASE WHEN ISNUMERIC([SALRY]) = 1 THEN [SALRY]
                               ELSE NULL
                          END ) AS [SALRY] ,
                        ( CASE WHEN ISNUMERIC([THR_MAVG_SALRY]) = 1
                               THEN [THR_MAVG_SALRY]
                               ELSE NULL
                          END ) AS [THR_MAVG_SALRY] ,
                        ( CASE WHEN ISNUMERIC([SIX_MAVG_SALRY]) = 1
                               THEN [SIX_MAVG_SALRY]
                               ELSE NULL
                          END ) AS [SIX_MAVG_SALRY] ,
                        [ENT_NAME]
                FROM    [Staging_T01_IND_ERA_PAY_STAT_P]
                WHERE   INPUTDATE IS NOT NULL
                        AND FROM_SYSTEM IS NOT NULL
                        AND CORE_CUST_ID IS NOT NULL;

        SELECT  @IsContinue = 0;
        RETURN @rowlength;
    END;


GO

