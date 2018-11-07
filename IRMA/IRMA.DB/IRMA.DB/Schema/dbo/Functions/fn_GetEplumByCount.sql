CREATE FUNCTION [dbo].[fn_GetEplumByCount]
(
	@irmaByCount INT,
	@gpmSellingUom VARCHAR(5),
	@inforRetailUom VARCHAR(5),
	@globalPriceManagementEnabled BIT = 0
)
RETURNS INT
AS
--*****************************************************************
--	This function is used to transform IRMA's values to EPlum's
--	expected values during the time that a region is on GPM
--	but not on OnePlum.
--
--	Returns 1 when the GPM is enabled and the GPM Selling UOM is
--	EA and the Infor Retail UOM is not OZ. Otherwise returns
--	the IRMA By Count.
--*****************************************************************
BEGIN
	DECLARE @byCount INT = 
		CASE 
			WHEN @globalPriceManagementEnabled = 1 THEN
				CASE
					WHEN ISNULL(@gpmSellingUom, '') = 'EA' THEN
						CASE 
							WHEN @inforRetailUom = 'OZ' THEN NULL
							ELSE 1
						END
					ELSE @irmaByCount
				END
			ELSE @irmaByCount
		END
	RETURN @byCount
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumByCount] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumByCount] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumByCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumByCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumByCount] TO [IRMAReportsRole]
    AS [dbo];
