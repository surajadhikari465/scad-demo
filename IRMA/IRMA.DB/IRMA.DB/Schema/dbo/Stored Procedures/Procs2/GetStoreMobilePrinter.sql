CREATE PROCEDURE dbo.GetStoreMobilePrinter
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON

    SELECT MobilePrinter.MobilePrinterID, NetworkName
    FROM MobilePrinter
    INNER JOIN
        Store_MobilePrinter
        ON Store_MobilePrinter.MobilePrinterID = MobilePrinter.MobilePrinterID AND Store_No = @Store_No 

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreMobilePrinter] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreMobilePrinter] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreMobilePrinter] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreMobilePrinter] TO [IRMAReportsRole]
    AS [dbo];

