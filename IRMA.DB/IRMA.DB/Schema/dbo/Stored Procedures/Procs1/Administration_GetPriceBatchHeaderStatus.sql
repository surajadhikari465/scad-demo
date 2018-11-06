CREATE PROCEDURE [dbo].[Administration_GetPriceBatchHeaderStatus]
    @PriceBatchHeaderID int
AS
BEGIN
    SET NOCOUNT ON
		SELECT PriceBatchStatusID 
		FROM dbo.PriceBatchHeader
		WHERE PriceBatchHeaderID = @PriceBatchHeaderID
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetPriceBatchHeaderStatus] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetPriceBatchHeaderStatus] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_GetPriceBatchHeaderStatus] TO [IRMAClientRole]
    AS [dbo];

