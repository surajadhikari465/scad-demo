CREATE FUNCTION [dbo].[fn_IsRetailLocation]
(
    @StoreNo int
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @WFMStore bit	

    SELECT @WFMStore = WFM_Store FROM Store WHERE Store_No = @StoreNo

        RETURN @WFMStore
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRetailLocation] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRetailLocation] TO [IRMAClientRole]
    AS [dbo];

