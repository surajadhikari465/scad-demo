CREATE PROCEDURE dbo.GetPriceBatchPrintItems
    @PriceBatchHeaderID int,
    @PrintSign bit
AS

BEGIN
    SET NOCOUNT ON

    SELECT Item_Key
    FROM PriceBatchDetail PBD
    WHERE PBD.PriceBatchHeaderID = @PriceBatchHeaderID
        AND PrintSign = @PrintSign
    GROUP BY Item_Key

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchPrintItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchPrintItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchPrintItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchPrintItems] TO [IRMAReportsRole]
    AS [dbo];

