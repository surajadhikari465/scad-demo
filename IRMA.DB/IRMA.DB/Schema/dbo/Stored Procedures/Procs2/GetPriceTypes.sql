CREATE PROCEDURE [dbo].[GetPriceTypes] 
	@IncludeReg bit
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		PriceChgTypeDesc,
		PriceChgTypeId,
		Priority,
		On_Sale,
		MSRP_Required,
		LineDrive,
		Competitive,
		LastUpdateTimestamp
    FROM 
		PriceChgType (NOLOCK)
	WHERE @IncludeReg >= 1 - On_Sale
	-- so if IncludeReg is zero, it won't include On_Sale = 1
    ORDER BY 
		Priority asc
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceTypes] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceTypes] TO [IRMAExcelRole]
    AS [dbo];

