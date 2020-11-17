/* This script will generate 3 reports that give an overall or high levelview of the indexes in a particular database. The sections are as follows:1.  Lists ALL indexes and constraints along with the key details of each2.  Lists any tables with potential Redundant indexes3.  Lists any tables with potential Reverse indexes*/--  Create a table variable to hold the core index infoDECLARE @AllIndexes TABLE ( [Table ID] [int] NOT NULL, [Schema] [sysname] NOT NULL, [Table Name] [sysname] NOT NULL, [Index ID] [int] NULL, [Index Name] [nvarchar](128) NULL, [Index Type] [varchar](12) NOT NULL, [Constraint Type] [varchar](11) NOT NULL, [Object Type] [varchar](10) NOT NULL, [AllColName] [nvarchar](2078) NULL, [ColName1] [nvarchar](128) NULL, [ColName2] [nvarchar](128) NULL, [ColName3] [nvarchar](128) NULL, [ColName4] [nvarchar](128) NULL, [ColName5] [nvarchar](128) NULL, [ColName6] [nvarchar](128) NULL, [ColName7] [nvarchar](128) NULL, [ColName8] [nvarchar](128) NULL, [ColName9] [nvarchar](128) NULL, [ColName10] [nvarchar](128) NULL)--  Load up the table variable with the index information to be used in follow on queriesINSERT INTO @AllIndexes ([Table ID],[Schema],[Table Name],[Index ID],[Index Name],[Index Type],[Constraint Type],[Object Type] ,[AllColName],[ColName1],[ColName2],[ColName3],[ColName4],[ColName5],[ColName6],[ColName7],[ColName8], [ColName9],[ColName10])SELECT o.[object_id] AS [Table ID] ,u.[name] AS [Schema],o.[name] AS [Table Name], i.[index_id] AS [Index ID] , CASE i.[name] WHEN o.[name] THEN '** Same as Table Name **' ELSE i.[name] END AS [Index Name], CASE i.[type] WHEN 1 THEN 'CLUSTERED' WHEN 0 THEN 'HEAP' WHEN 2 THEN 'NONCLUSTERED' WHEN 3 THEN 'XML' ELSE 'UNKNOWN' END AS [Index Type], CASE WHEN (i.[is_primary_key]) = 1 THEN 'PRIMARY KEY' WHEN (i.[is_unique]) = 1 THEN 'UNIQUE' ELSE '' END AS [Constraint Type], CASE WHEN (i.[is_unique_constraint]) = 1 OR (i.[is_primary_key]) = 1 THEN 'CONSTRAINT' WHEN i.[type] = 0 THEN 'HEAP' WHEN i.[type] = 3 THEN 'XML INDEX' ELSE 'INDEX' END AS [Object Type], (SELECT COALESCE(c1.[name],'') FROM [sys].[columns] AS c1 INNER JOIN [sys].[index_columns] AS ic1 ON c1.[object_id] = ic1.[object_id] AND c1.[column_id] = ic1.[column_id] AND ic1.[key_ordinal] = 1 WHERE ic1.[object_id] = i.[object_id] AND ic1.[index_id] = i.[index_id]) + CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 2) IS NULL THEN '' ELSE ', '+INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id],2) END + CASE WHEN INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id], 3) IS NULL THEN '' ELSE ', '+INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],3) END + CASE WHEN INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id], 4) IS NULL THEN '' ELSE ', '+INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],4) END + CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 5) IS NULL THEN '' ELSE ', '+INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],5) END  + CASE WHEN INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id], 6) IS NULL THEN '' ELSE ', '+INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],6) END + CASE WHEN INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id], 7) IS NULL THEN '' ELSE ', '+INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id], 7) END + CASE WHEN INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],8) IS NULL THEN '' ELSE ', '+INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],8) END + CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 9) IS NULL THEN '' ELSE ', '+INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],9) END + CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 10) IS NULL THEN '' ELSE ', '+INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],10) END  AS [AllColName], (SELECT COALESCE(c1.[name],'') FROM [sys].[columns] AS c1 INNER JOIN [sys].[index_columns] AS ic1 ON c1.[object_id] = ic1.[object_id] AND c1.[column_id] = ic1.[column_id] AND ic1.[key_ordinal] = 1 WHERE ic1.[object_id] = i.[object_id] AND ic1.[index_id] = i.[index_id])   AS [ColName1], CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 2) IS NULL THEN '' ELSE INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],2) END AS [ColName2], CASE WHEN INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id], 3) IS NULL THEN '' ELSE INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],3) END AS [ColName3], CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 4) IS NULL THEN '' ELSE INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],4) END AS [ColName4], CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 5) IS NULL THEN '' ELSE INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],5) END AS [ColName5], CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 6) IS NULL THEN '' ELSE INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],6) END AS [ColName6], CASE WHEN INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id], 7) IS NULL THEN '' ELSE INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],7) END AS [ColName7], CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id],8) IS NULL THEN '' ELSE INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],8) END AS [ColName8], CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 9) IS NULL THEN '' ELSE INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],9) END AS [ColName9], CASE WHEN INDEX_COL('[' + u.[name] + '].['+ o.[name] + ']', i.[index_id], 10) IS NULL THEN '' ELSE INDEX_COL('[' + u.[name] + '].[' + o.[name] + ']', i.[index_id],10) END AS [ColName10]FROM [sys].[objects] AS o WITH (NOLOCK) LEFT OUTER JOIN [sys].[indexes] AS i WITH (NOLOCK) ON o.[object_id] = i.[object_id] JOIN [sys].[schemas] AS u WITH (NOLOCK) ON o.[schema_id] = u.[schema_id]WHERE o.[type] = 'U' --AND i.[index_id] &lt; 255 AND o.[name] NOT IN ('dtproperties') AND i.[name] NOT LIKE '_WA_Sys_%'-----------SELECT 'Listing All Indexes' AS [Comments]SELECT I.* FROM @AllIndexes AS I ORDER BY [Table Name]-----------SELECT 'Listing Possible Redundant Index keys' AS [Comments]SELECT DISTINCT I.[Table Name], I.[Index Name] ,I.[Index Type],  I.[Constraint Type], I.[AllColName] FROM @AllIndexes AS I JOIN @AllIndexes AS I2 ON I.[Table ID] = I2.[Table ID] AND I.[ColName1] = I2.[ColName1] AND I.[Index Name] <> I2.[Index Name] AND I.[Index Type] <> 'XML' ORDER BY I.[Table Name], I.[AllColName]----------SELECT 'Listing Possible Reverse Index keys' AS [Comments]SELECT DISTINCT I.[Table Name], I.[Index Name], I.[Index Type],  I.[Constraint Type], I.[AllColName] FROM @AllIndexes AS I JOIN @AllIndexes AS I2 ON I.[Table ID] = I2.[Table ID] AND I.[ColName1] = I2.[ColName2] AND I.[ColName2] = I2.[ColName1] AND I.[Index Name] <> I2.[Index Name] AND I.[Index Type] <> 'XML'


 GO

 /****  ����Ľű���ѯ���ݿ�������������� �鷳�ֶ���������ִ�����нű������ڷ���ԭ��**/
/*
���Ƿ����������ִ������Ľű���

��һ��������Ȳ�ִ�У�������Ϊ�Ǳʼ�¼������ʱ�ļ���Ҳ���� ���Ǳʼ�¼��ִ��ʱ������ִ������Ľű���

*/


----1 ִ�����нű�1

use InstinctBOCOM
go
select t1.request_session_id as 'wait_sid',t1.resource_type as '������' ,db_name(resource_database_id) as '������',t1.request_mode as 'wait������'
	,t2.wait_duration_ms as 'wait_time_ms'	
	,(select text from sys.dm_exec_requests as r  
		cross apply sys.dm_exec_sql_text(r.sql_handle) 
		where r.session_id = t1.request_session_id) as 'wait_run_batch'
	,(select substring(qt.text,r.statement_start_offset/2+1, 
			(case when r.statement_end_offset = -1 
			then datalength(qt.text) 
			else r.statement_end_offset end - r.statement_start_offset)/2+1) 
		from sys.dm_exec_requests as r
		cross apply sys.dm_exec_sql_text(r.sql_handle) as qt
		where r.session_id = t1.request_session_id) as 'wait ���е�SQL���'
	 ,t2.blocking_session_id as '����sid'
     ,(select text from sys.sysprocesses as p		
		cross apply sys.dm_exec_sql_text(p.sql_handle) 
		where p.spid = t2.blocking_session_id) as '����SQL'
	from sys.dm_tran_locks as t1 inner join sys.dm_os_waiting_tasks as t2 
								 on t1.lock_owner_address = t2.resource_address

----2  ִ�����нű�2
use InstinctBOCOM
go
select *
from sys.dm_exec_requests 
where [status]='suspended'


/* ��������1 ��2 �����ű����з��������������ļ�¼���鷳��¼�����������Ƿ���ԭ��
   ��ע��һ�� ��1�еĲ�ѯ����� Wait_sid �Ƿ� ���� ��2�еĲ�ѯ����е� Session_id
    ���1�е� �ֶ�wait_time_ms ʱ��϶� ����������¼��˲����ʧ�����ǾͲ���Ϊ�ǳ�ʱ��������������ᡣ
*/

GO
--select 546830/1000.0/60
--
--select lock_owner_address
--from sys.dm_tran_locks
--
--select resource_address 
--from sys.dm_os_waiting_tasks
--
--select * from sys.dm_exec_requests



--������Ƭ
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
DB_NAME() AS DatbaseName 
, SCHEMA_NAME(o.Schema_ID) AS SchemaName 
, OBJECT_NAME(s.[object_id]) AS TableName 
, i.name AS IndexName 
, ROUND(s.avg_fragmentation_in_percent,2) AS [Fragmentation %] 
INTO #TempFragmentation 
FROM sys.dm_db_index_physical_stats(db_id(),null, null, null, null) s 
INNER JOIN sys.indexes i ON s.[object_id] = i.[object_id] 
AND s.index_id = i.index_id 
INNER JOIN sys.objects o ON i.object_id = O.object_id 
WHERE 1=2 
EXEC sp_MSForEachDB 'USE [?]; 
INSERT INTO #TempFragmentation 
SELECT TOP 20 
DB_NAME() AS DatbaseName 
, SCHEMA_NAME(o.Schema_ID) AS SchemaName 
, OBJECT_NAME(s.[object_id]) AS TableName 
, i.name AS IndexName 
, ROUND(s.avg_fragmentation_in_percent,2) AS [Fragmentation %] 
FROM sys.dm_db_index_physical_stats(db_id(),null, null, null, null) s 
INNER JOIN sys.indexes i ON s.[object_id] = i.[object_id] 
AND s.index_id = i.index_id 
INNER JOIN sys.objects o ON i.object_id = O.object_id 
WHERE s.database_id = DB_ID() 
AND i.name IS NOT NULL 
AND OBJECTPROPERTY(s.[object_id], ''IsMsShipped'') = 0 
ORDER BY [Fragmentation %] DESC'
SELECT top 20 * FROM #TempFragmentation ORDER BY [Fragmentation %] DESC
DROP TABLE #TempFragmentation

GO
--δʹ������
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
DB_NAME() AS DatbaseName 
, SCHEMA_NAME(O.Schema_ID) AS SchemaName 
, OBJECT_NAME(I.object_id) AS TableName 
, I.name AS IndexName 
INTO #TempNeverUsedIndexes 
FROM sys.indexes I INNER JOIN sys.objects O ON I.object_id = O.object_id 
WHERE 1=2 
EXEC sp_MSForEachDB 'USE [?]; 
INSERT INTO #TempNeverUsedIndexes 
SELECT 
DB_NAME() AS DatbaseName 
, SCHEMA_NAME(O.Schema_ID) AS SchemaName 
, OBJECT_NAME(I.object_id) AS TableName 
, I.NAME AS IndexName 
FROM sys.indexes I INNER JOIN sys.objects O ON I.object_id = O.object_id 
LEFT OUTER JOIN sys.dm_db_index_usage_stats S ON S.object_id = I.object_id 
AND I.index_id = S.index_id 
AND DATABASE_ID = DB_ID() 
WHERE OBJECTPROPERTY(O.object_id,''IsMsShipped'') = 0 
AND I.name IS NOT NULL 
AND S.object_id IS NULL'
SELECT * FROM #TempNeverUsedIndexes 
ORDER BY DatbaseName, SchemaName, TableName, IndexName 
DROP TABLE #TempNeverUsedIndexes



