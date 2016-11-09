CREATE PROCEDURE dbo.InsertSASIItem 
AS 

BEGIN

    DELETE FROM SASIItem WHERE Store_No IN (SELECT Store_No FROM Store WHERE SASI = 1)

    BULK INSERT SASIItem
    FROM 'E:\SASIItem.DAT'
    WITH ( FIELDTERMINATOR = '\t',
           ROWTERMINATOR = '\n' )

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertSASIItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertSASIItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertSASIItem] TO [IRMAReportsRole]
    AS [dbo];

