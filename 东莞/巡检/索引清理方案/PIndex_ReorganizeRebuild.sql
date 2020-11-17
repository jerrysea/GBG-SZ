USE [InstinctDRC]
GO

/****** Object:  StoredProcedure [dbo].[PIndex_ReorganizeRebuild]    Script Date: 2017-08-07 15:39:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- Author:  LvZhongFeng  
-- Create date: 2010-12-10  
-- Description: 自定义索引重建- 重组阀值 

CREATE PROCEDURE [dbo].[PIndex_ReorganizeRebuild]
	@DB_Name NVARCHAR(200),
	@Index_ReorganizeValue FLOAT=5,
	@Index_RebuildValue FLOAT=30
AS


/*************在变量中，自定义重组 OR 重建索引的阀值***********/
SET NOCOUNT ON;
DECLARE @DeleteTime DATETIME
DECLARE @BeginTime DATETIME
DECLARE @EndTime DATETIME
SET @DeleteTime=CAST(CONVERT(VARCHAR(10),GETDATE()-60,120) AS DATETIME) 
SET @BeginTime=CAST(CONVERT(VARCHAR(10),GETDATE(),120) AS DATETIME)
SET @EndTime=CAST(CONVERT(VARCHAR(10),GETDATE(),120) AS DATETIME)+1
--select @DeleteTime,@BeginTime,@EndTime

DELETE FROM IndexFragmentationMessage WHERE AddTime<=@DeleteTime
DELETE FROM IndexFragmentationMessage WHERE AddTime>=@BeginTime AND AddTime<@EndTime

INSERT INTO IndexFragmentationMessage
(
	DBName,
	SchemaName,
	TableName,
	IndexName,
	IndexType,
	Avg_Fragmentation_In_Percent,
	Page_Count,
	Lob_Data_Space_Id,
	Is_Disabled,
	[Allow_Page_Locks],
	AddTime
)

SELECT 
		@DB_Name AS DBName,
		SCHEMA_NAME(ST.Schema_id) AS SchemaName, 
		OBJECT_NAME(DPS.OBJECT_ID) AS TableName, 
		SI.Name AS IndexName,
		DPS.Index_Type_Desc AS IndexType,
		DPS.Avg_Fragmentation_In_Percent,
		DPS.Page_Count,
		ST.Lob_Data_Space_Id,
		SI.Is_Disabled,
		SI.Allow_Page_Locks,
		GETDATE() AS AddTime

FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL , N'LIMITED') DPS 
INNER JOIN sys.tables ST ON DPS.OBJECT_ID = ST.OBJECT_ID 
INNER JOIN sys.indexes SI ON DPS.OBJECT_ID = SI.OBJECT_ID AND DPS.index_id = SI.index_id
WHERE avg_fragmentation_in_percent>@Index_ReorganizeValue AND SI.index_id>0 AND DPS.Page_Count>=6000


/****Avg_Fragmentation_In_Percent From 5 to 30 Reorganize. If Allow_page_Locks=OFF, Rebuild.If Allow_page_Locks=On, Reorganize ***/

UPDATE IndexFragmentationMessage 
SET Exec_Alter_Index= 'Alter Index '+IndexName+' On '+SchemaName+'.'+TableName+' Rebuild'
WHERE (AddTime>=@BeginTime AND AddTime<@EndTime) 
		AND(Avg_Fragmentation_In_Percent BETWEEN @Index_ReorganizeValue AND @Index_RebuildValue) 
		AND [Allow_Page_Locks]=0

UPDATE IndexFragmentationMessage 
SET Exec_Alter_Index= 'Alter Index '+IndexName+' On '+SchemaName+'.'+TableName+' Reorganize'
WHERE (AddTime>=@BeginTime AND AddTime<@EndTime)
		AND (Avg_Fragmentation_In_Percent BETWEEN @Index_ReorganizeValue AND @Index_RebuildValue) 
		AND [Allow_Page_Locks]=1


/****Avg_Fragmentation_In_Percent>30,If Lob_Date_Space_Id=1, Rebuild.If Lob_Date_Space_Id=1, Rebuild online=on ***/

UPDATE IndexFragmentationMessage 
SET Exec_Alter_Index= 'Alter Index '+IndexName+' On '+SchemaName+'.'+TableName+' Rebuild'
WHERE (AddTime>=@BeginTime AND AddTime<@EndTime)
		AND Avg_Fragmentation_In_Percent>@Index_RebuildValue 
		AND Lob_Data_Space_Id=1


UPDATE IndexFragmentationMessage 
SET Exec_Alter_Index= 'Alter Index '+IndexName+' On '+SchemaName+'.'+TableName+' Rebuild With (Online=ON)'
WHERE (AddTime>=@BeginTime AND AddTime<@EndTime)
		AND Avg_Fragmentation_In_Percent>@Index_RebuildValue 
		AND Lob_Data_Space_Id=0


------- Exec SQL -----
DECLARE @i INT
DECLARE @j INT
SELECT @i=MIN(Id),@j=MAX(Id) FROM IndexFragmentationMessage WHERE (AddTime>=@BeginTime AND AddTime<@EndTime) 
WHILE @i<=@j
BEGIN
	BEGIN TRY
		DECLARE @SQL NVARCHAR(MAX)
		SELECT @SQL=LTRIM(RTRIM(Exec_Alter_Index)) FROM IndexFragmentationMessage WHERE ID=@i
		EXEC (@SQL)
		IF @@error=0
			UPDATE IndexFragmentationMessage SET MessageList='OK' WHERE ID=@i
	END TRY
			
	BEGIN CATCH
		UPDATE IndexFragmentationMessage SET MessageList=ERROR_MESSAGE() WHERE ID=@i
	END CATCH

	SET @i=@i+1
END



GO


