CREATE PROCEDURE dbo.GetVendorDealTypes
AS
BEGIN
    SET NOCOUNT ON

	SELECT VendorDealTypeID, Code, Description, CaseAmtType
    FROM VendorDealType
    ORDER BY Description
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorDealTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorDealTypes] TO [IRMAClientRole]
    AS [dbo];

