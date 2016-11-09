CREATE FUNCTION dbo.fn_PriceBatchDetailCount 
	(@PriceBatchHeaderID int)
RETURNS int
AS
BEGIN
    DECLARE @Cnt int

    SELECT @Cnt = COUNT(*)
    FROM (SELECT Item_Key FROM PriceBatchDetail WHERE PriceBatchHeaderID = @PriceBatchHeaderID GROUP BY Item_Key) T

    RETURN @Cnt
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceBatchDetailCount] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceBatchDetailCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceBatchDetailCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_PriceBatchDetailCount] TO [IRMAReportsRole]
    AS [dbo];

