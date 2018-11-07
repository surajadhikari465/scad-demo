SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_defrag_indexes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_defrag_indexes]
GO


/****** Object:  Stored Procedure dbo.sp_defrag_indexes    Script Date: 1/22/2005 7:29:03 PM ******/
CREATE PROCEDURE dbo.sp_defrag_indexes 
@MAXFRAG DECIMAL

AS

/* T.Pullen

This stored procedure checks index fragmentation in a database and defragments
indexes whose scan densities fall below a specified threshold, @magfrag, which
is passed to the SP. This SP was initially based on a code sample in SQL Server 2000 
Books Online.

Must be run in the database to be defragmented.

*/


-- Declare variables

SET NOCOUNT ON
DECLARE @tablename VARCHAR (128)
DECLARE @execstr VARCHAR (255)
DECLARE @objectid INT
DECLARE @objectowner VARCHAR(255)
DECLARE @indexid INT
DECLARE @frag DECIMAL
DECLARE @indexname CHAR(255)
DECLARE @dbname sysname
DECLARE @tableid INT
DECLARE @tableidchar VARCHAR(255)

--check this is being run in a user database
SELECT @dbname = db_name()
IF @dbname IN ('master', 'msdb', 'model', 'tempdb')
BEGIN
PRINT 'This procedure should not be run in system databases.'
RETURN
END

--Create the load table for the showcontig results

CREATE TABLE tempdb.dbo.ICATIndexDefragList (
	[ObjectName] [char] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ObjectId] [int] NULL ,
	[IndexName] [char] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[IndexId] [int] NULL ,
	[Lvl] [int] NULL ,
	[CountPages] [int] NULL ,
	[CountRows] [int] NULL ,
	[MinRecSize] [int] NULL ,
	[MaxRecSize] [int] NULL ,
	[AvgRecSize] [int] NULL ,
	[ForRecCount] [int] NULL ,
	[Extents] [int] NULL ,
	[ExtentSwitches] [int] NULL ,
	[AvgFreeBytes] [int] NULL ,
	[AvgPageDensity] [int] NULL ,
	[ScanDensity] [decimal](18, 0) NULL ,
	[BestCount] [int] NULL ,
	[ActualCount] [int] NULL ,
	[LogicalFrag] [decimal](18, 0) NULL ,
	[ExtentFrag] [decimal](18, 0) NULL 
) ON [PRIMARY]


--begin Stage 1: checking fragmentation
-- Declare cursor
DECLARE tables CURSOR FOR
SELECT convert(varchar,so.id)
FROM sysobjects so
JOIN sysindexes si
ON so.id = si.id
WHERE so.type ='U'
AND si.indid < 2
AND si.rows > 0


-- Open the cursor
OPEN tables

-- Loop through all the tables in the database running dbcc showcontig on each one
FETCH NEXT
FROM tables
INTO @tableidchar

WHILE @@FETCH_STATUS = 0
BEGIN
-- Do the showcontig of all indexes of the table
INSERT INTO tempdb.dbo.ICATIndexDefragList 
EXEC ('DBCC SHOWCONTIG (' + @tableidchar + ') WITH FAST, TABLERESULTS, ALL_INDEXES, NO_INFOMSGS')

FETCH NEXT
FROM tables
INTO @tableidchar
END

-- Close and deallocate the cursor
CLOSE tables
DEALLOCATE tables

-- Report the ouput of showcontig for results checking
--SELECT * FROM IndexDefragList AS FRAGLIST


-- Begin Stage 2: (defrag) declare cursor for list of indexes to be defragged
DECLARE indexes CURSOR FOR
SELECT ObjectName, ObjectOwner = user_name(so.uid), ObjectId, IndexName, ScanDensity
FROM tempdb.dbo.ICATIndexDefragList f
JOIN sysobjects so ON f.ObjectId=so.id
WHERE ScanDensity <= @maxfrag
AND INDEXPROPERTY (ObjectId, IndexName, 'IndexDepth') > 0

-- Write to output start time for information purposes
--SELECT 'Started defragmenting indexes at ' + CONVERT(VARCHAR,GETDATE()) AS 'DEFRAG START'

-- Open the cursor
OPEN indexes

-- Loop through the indexes
FETCH NEXT
FROM indexes
INTO @tablename, @objectowner, @objectid, @indexname, @frag

WHILE @@FETCH_STATUS = 0
BEGIN
SET QUOTED_IDENTIFIER ON

SELECT @execstr = 'DBCC DBREINDEX (' + '''' + RTRIM(@objectowner) + '.' + RTRIM(@tablename) + '''' + 
', ' + RTRIM(@indexname) + ')' -- WITH NO_INFOMSGS'

--SELECT 'Now executing: '  AS EXECUTING
--SELECT(@execstr) AS 'DEFRAG TABLE'
EXEC (@execstr)

SET QUOTED_IDENTIFIER OFF



FETCH NEXT
FROM indexes
INTO @tablename, @objectowner, @objectid, @indexname, @frag
END

-- Close and deallocate the cursor
CLOSE indexes
DEALLOCATE indexes

-- Report on finish time for information purposes
--SELECT 'Finished defragmenting indexes at ' + CONVERT(VARCHAR,GETDATE()) AS 'DEFRAG END'

DROP TABLE tempdb.dbo.ICATIndexDefragList


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

