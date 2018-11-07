CREATE PROCEDURE dbo.usp_ImportMultipleFiles
    @Filepath varchar(500), 
	@Pattern varchar(100), 
    @TableName varchar(128),
    @FieldTerminator varchar(2),
    @RowTerminator varchar(2),
    @Archivepath varchar(500),
    @Errorpath varchar(500)
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @query varchar(1000)
    DECLARE @max1 int
    DECLARE @count1 int
    DECLARE @filename varchar(100)

    CREATE TABLE #X (NAME varchar(200))

    SET @query ='MASTER.DBO.XP_CMDSHELL ''DIR '+@Filepath+@Pattern +' /B'''

    INSERT #X EXEC (@query)

    DELETE FROM #X WHERE (NAME IS NULL) OR (NAME = 'File Not Found')

    SELECT IDENTITY(INT,1,1) AS ID, NAME INTO #Y FROM #X 

    DROP TABLE #X

    SET @max1 = (SELECT MAX(ID) FROM #Y)
    IF ISNULL(@max1, 0) > 0
    BEGIN
        SET @COUNT1 = 1
        WHILE @count1 <= @max1
        BEGIN
            SET @filename = (SELECT NAME FROM #Y WHERE [ID] = @count1)
    
            EXEC ('BULK INSERT '+ @TableName + ' FROM '''+ @Filepath+@filename+''' 
            	   WITH ( FIELDTERMINATOR = ''' + @FieldTerminator + ''',ROWTERMINATOR = ''' + @RowTerminator + ''')')
    
            IF @Archivepath IS NOT NULL AND @Errorpath IS NOT NULL
            BEGIN
                IF @@ERROR = 0
                    SET @query = 'MASTER.DBO.XP_CMDSHELL ''MOVE /Y ' + @Filepath+@filename + ' ' + @Archivepath + ''' ,NO_OUTPUT'
                ELSE
                    SET @query = 'MASTER.DBO.XP_CMDSHELL ''MOVE /Y ' + @Filepath+@filename + ' ' + @Errorpath + ''' ,NO_OUTPUT'
    
                EXEC (@query)
            END

            SET @count1=@count1+1
        END
    END
    
    DROP TABLE #Y
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_ImportMultipleFiles] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_ImportMultipleFiles] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_ImportMultipleFiles] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[usp_ImportMultipleFiles] TO [IRMAReportsRole]
    AS [dbo];

