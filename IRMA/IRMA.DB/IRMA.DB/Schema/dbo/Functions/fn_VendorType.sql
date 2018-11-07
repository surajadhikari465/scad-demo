CREATE FUNCTION dbo.fn_VendorType 
	(@PS_Vendor_ID varchar(255), 
	 @WFM bit,
     @Store_No int,
     @Internal bit)
RETURNS tinyint
AS
BEGIN
    DECLARE @Result tinyint
    SELECT @Result = CASE 
						  WHEN @PS_Vendor_ID IS NOT NULL THEN 1 -- External
                          WHEN @WFM = 1 THEN 2 -- WFM
                          WHEN (@Store_No IS NOT NULL) AND (ISNULL(@Internal, 0) = 1) THEN 3 -- Regional
                          ELSE 0 -- Error
                     END
    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_VendorType] TO [IRMAReportsRole]
    AS [dbo];

