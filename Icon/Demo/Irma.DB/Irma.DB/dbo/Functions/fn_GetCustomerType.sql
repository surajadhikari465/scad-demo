CREATE FUNCTION dbo.fn_GetCustomerType
	(@Store_No int,
     @Internal tinyint,
     @BusinessUnit_ID int
    )
RETURNS tinyint
AS
BEGIN
    DECLARE @Result tinyint

    SELECT @Result = CASE WHEN @Internal = 0 and @BusinessUnit_ID IS NULL
                              THEN 1 --External Customer
                          WHEN @Internal = 0 and @BusinessUnit_ID IS NOT NULL
                              THEN 2 --WFM Customer (Internal but not in region)
                          WHEN @Internal = 1 and @BusinessUnit_ID IS NOT NULL
                              THEN 3 --Regional (Internal and in region)
                          WHEN @Internal = 1 and @BusinessUnit_ID IS NULL
                              THEN 0 --ERR (Should never exist in store table)
                          END

    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCustomerType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCustomerType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCustomerType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCustomerType] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetCustomerType] TO [IRMASLIMRole]
    AS [dbo];

