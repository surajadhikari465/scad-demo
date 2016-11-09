CREATE FUNCTION [dbo].[fn_IsDistributionCenter]
(
    @StoreNo int
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @DistributionCenter bit	

    SELECT @DistributionCenter = Distribution_Center FROM Store WHERE Store_No = @StoreNo

	IF @DistributionCenter IS NULL
		SELECT @DistributionCenter = 0

        RETURN @DistributionCenter
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsDistributionCenter] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsDistributionCenter] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsDistributionCenter] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsDistributionCenter] TO [IRMAReportsRole]
    AS [dbo];

