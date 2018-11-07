CREATE PROCEDURE dbo.LoadESTFileUpdate
    @ImportFilename varchar(500)
AS
-- **************************************************************************
-- Procedure: LoadESTFileUpdate
--    Author: Billy Blackerby
--      Date: 05.01.12
--
-- Description:
-- Called by ESTImport job to bulk import file for Price updates
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 05.01.12		BBB   	4994.2	Created
-- **************************************************************************
BEGIN
	SET NOCOUNT ON

	--**************************************************************************
	--Declare internal variables
	--**************************************************************************
    DECLARE @SQL varchar(5000)
    CREATE TABLE #tmpESTImport (
								Activity	varchar(1),
								Identifier	varchar(13),
								Store_No	varchar(10)
								) 
    
	--**************************************************************************
	--Populate internal variables
	--**************************************************************************
	SET @SQL = 'BULK INSERT #tmpESTImport FROM ''' + @ImportFilename  + ''' WITH(FIELDTERMINATOR = ''|'', ROWTERMINATOR = ''\n'')'
	EXECUTE(@SQL)

	--**************************************************************************
	--Update SQL
	--**************************************************************************
	UPDATE
		Price
	SET
		ElectronicShelfTag = CASE
								WHEN ti.Activity = 'A' THEN
									1
								ElSE
									0
							 END
	FROM
		#tmpESTImport				ti
		INNER JOIN ItemIdentifier	ii	ON	ti.Identifier	= ii.Identifier
		INNER JOIN Price			p	ON	ti.Store_No		= p.Store_No
										AND	ii.Item_Key		= p.Item_Key

	--**************************************************************************
	--Clean-up internal variables
	--**************************************************************************			
	DROP TABLE #tmpESTImport

	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadESTFileUpdate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadESTFileUpdate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadESTFileUpdate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadESTFileUpdate] TO [IRMAReportsRole]
    AS [dbo];

