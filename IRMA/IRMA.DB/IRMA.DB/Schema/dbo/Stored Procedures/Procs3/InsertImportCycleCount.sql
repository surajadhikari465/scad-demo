CREATE PROCEDURE dbo.InsertImportCycleCount
@Store_No int,
@Start_Scan datetime,
@End_Scan datetime
AS 
BEGIN
    SET NOCOUNT ON

    INSERT INTO CycleImportHeader ( Store_No, Start_Scan, End_Scan)
    VALUES ( @Store_No, @Start_Scan, @End_Scan)

    SELECT MAX(CycleImportHeader_ID) AS CycleImportHeaderID
    FROM CycleImportHeader

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertImportCycleCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertImportCycleCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertImportCycleCount] TO [IRMAReportsRole]
    AS [dbo];

