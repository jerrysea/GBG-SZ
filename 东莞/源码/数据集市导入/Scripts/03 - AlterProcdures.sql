USE [InstinctDRC]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]    Script Date: 09/12/2016 15:08:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_UBNK_CRDT_INFO_Insert]    Script Date: 09/12/2016 15:08:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_DRC_QZZC_UBNK_CRDT_INFO_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_DRC_QZZC_UBNK_CRDT_INFO_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_UBNK_LOAN_BIZ_Insert]    Script Date: 09/12/2016 15:08:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_DRC_QZZC_UBNK_LOAN_BIZ_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_DRC_QZZC_UBNK_LOAN_BIZ_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_RW_IND_BASIC_INFO_TAB_Insert]    Script Date: 09/12/2016 15:08:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_DRC_RW_IND_BASIC_INFO_TAB_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_DRC_RW_IND_BASIC_INFO_TAB_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_CUST_RELATIVE_G_Insert]    Script Date: 09/12/2016 15:08:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_DRC_T01_CUST_RELATIVE_G_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_DRC_T01_CUST_RELATIVE_G_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_BIZ_INFO_P_Insert]    Script Date: 09/12/2016 15:08:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_DRC_T01_IND_CUST_BIZ_INFO_P_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_BIZ_INFO_P_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_Insert]    Script Date: 09/12/2016 15:08:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_Insert]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_ERA_PAY_STAT_P_Insert]    Script Date: 09/12/2016 15:08:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_DRC_T01_IND_ERA_PAY_STAT_P_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USP_DRC_T01_IND_ERA_PAY_STAT_P_Insert]
GO

USE [InstinctDRC]
GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]    Script Date: 09/12/2016 15:08:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]    Script Date: 2016/5/9 13:05:26 ******/
--DROP PROCEDURE [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DELETE A FROM 
	--QZZC_APPC_ENT_INFO A(NOLOCK),Staging_QZZC_APPC_ENT_INFO B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Appc_ID = B.Appc_ID AND A.Ent_ID = B.Ent_ID
	
	truncate table QZZC_APPC_ENT_INFO

    -- Insert statements for procedure here
	INSERT INTO QZZC_APPC_ENT_INFO
	SELECT * FROM Staging_QZZC_APPC_ENT_INFO
END



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_UBNK_CRDT_INFO_Insert]    Script Date: 09/12/2016 15:08:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]    Script Date: 2016/5/9 13:05:26 ******/
--DROP PROCEDURE [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DRC_QZZC_UBNK_CRDT_INFO_Insert]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DELETE A FROM 
	--QZZC_UBNK_CRDT_INFO A(NOLOCK),Staging_QZZC_UBNK_CRDT_INFO B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Cust_ID = B.Cust_ID
	
	truncate table QZZC_UBNK_CRDT_INFO

    -- Insert statements for procedure here
	INSERT INTO QZZC_UBNK_CRDT_INFO
	SELECT * FROM Staging_QZZC_UBNK_CRDT_INFO
END



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_UBNK_LOAN_BIZ_Insert]    Script Date: 09/12/2016 15:08:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]    Script Date: 2016/5/9 13:05:26 ******/
--DROP PROCEDURE [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DRC_QZZC_UBNK_LOAN_BIZ_Insert]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DELETE A FROM 
	--QZZC_UBNK_LOAN_BIZ A(NOLOCK),Staging_QZZC_UBNK_LOAN_BIZ B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Cust_ID = B.Cust_ID AND A.Biz_Type = B.Biz_Type
	
	truncate table QZZC_UBNK_LOAN_BIZ

    -- Insert statements for procedure here
	INSERT INTO QZZC_UBNK_LOAN_BIZ
	SELECT * FROM Staging_QZZC_UBNK_LOAN_BIZ
END



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_RW_IND_BASIC_INFO_TAB_Insert]    Script Date: 09/12/2016 15:08:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]    Script Date: 2016/5/9 13:05:26 ******/
--DROP PROCEDURE [dbo].[USP_DRC_QZZC_APPC_ENT_INFO_Insert]


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_DRC_RW_IND_BASIC_INFO_TAB_Insert]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DELETE A FROM 
	--RW_IND_BASIC_INFO_TAB A(NOLOCK),Staging_RW_IND_BASIC_INFO_TAB B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Cust_ID = B.Cust_ID
	
	truncate table RW_IND_BASIC_INFO_TAB

    -- Insert statements for procedure here
	INSERT INTO RW_IND_BASIC_INFO_TAB
	SELECT * FROM Staging_RW_IND_BASIC_INFO_TAB
END



GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_CUST_RELATIVE_G_Insert]    Script Date: 09/12/2016 15:08:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_T01_CUST_RELATIVE_G_Insert]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DELETE A FROM 
	--T01_CUST_RELATIVE_G A(NOLOCK),Staging_T01_CUST_RELATIVE_G B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Cust_ID=b.Cust_ID AND A.Rela_Cust_Id=B.Rela_Cust_Id
	--AND A.Rela_Ship = B.Rela_Ship
	
	truncate table T01_CUST_RELATIVE_G

    -- Insert statements for procedure here
	INSERT INTO T01_CUST_RELATIVE_G
	SELECT * FROM Staging_T01_CUST_RELATIVE_G
END




GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_BIZ_INFO_P_Insert]    Script Date: 09/12/2016 15:08:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_BIZ_INFO_P_Insert]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DELETE A FROM 
	--T01_IND_CUST_BIZ_INFO_P A(NOLOCK),Staging_T01_IND_CUST_BIZ_INFO_P B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Change_Dt=b.Change_Dt AND A.Cert_Type_Cd = B.Cert_Type_Cd
	--AND A.Cert_ID = B.Cert_ID AND A.Cust_ID = B.Cust_ID 
	
	truncate table T01_IND_CUST_BIZ_INFO_P

    -- Insert statements for procedure here
	INSERT INTO T01_IND_CUST_BIZ_INFO_P
	SELECT * FROM Staging_T01_IND_CUST_BIZ_INFO_P
END





GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_Insert]    Script Date: 09/12/2016 15:08:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[USP_DRC_T01_IND_CUST_CORE_INFO_P_Insert]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DELETE A FROM 
	--T01_IND_CUST_CORE_INFO_P A(NOLOCK),Staging_T01_IND_CUST_CORE_INFO_P B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Cust_ID = B.Cust_ID 
	
	truncate table T01_IND_CUST_CORE_INFO_P

    -- Insert statements for procedure here
	INSERT INTO T01_IND_CUST_CORE_INFO_P
	SELECT * FROM Staging_T01_IND_CUST_CORE_INFO_P
END





GO

/****** Object:  StoredProcedure [dbo].[USP_DRC_T01_IND_ERA_PAY_STAT_P_Insert]    Script Date: 09/12/2016 15:08:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[USP_DRC_T01_IND_ERA_PAY_STAT_P_Insert]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DELETE A FROM 
	--T01_IND_ERA_PAY_STAT_P A(NOLOCK),Staging_T01_IND_ERA_PAY_STAT_P B(NOLOCK)
	--WHERE A.Inputdate = b.Inputdate AND A.From_System = B.From_System 
	--AND A.Core_Cust_ID = B.Core_Cust_ID 
	
	truncate table T01_IND_ERA_PAY_STAT_P

    -- Insert statements for procedure here
	INSERT INTO T01_IND_ERA_PAY_STAT_P
	SELECT * FROM Staging_T01_IND_ERA_PAY_STAT_P
END





GO

