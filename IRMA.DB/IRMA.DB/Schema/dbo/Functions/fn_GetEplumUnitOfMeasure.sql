CREATE FUNCTION [dbo].[fn_GetEplumUnitOfMeasure]
(
	@irmaUnitOfMeasure VARCHAR(5),
	@gpmSellingUom VARCHAR(5),
	@inforRetailUom VARCHAR(5),
	@globalPriceManagementEnabled bit = 0
)
RETURNS VARCHAR(5)
AS
--*****************************************************************
--	This function is used to transform IRMA's values to EPlum's
--	expected values during the time that a region is on GPM
--	but not on OnePlum.
--
--	Returns 'FW' when the GPM is enabled and the GPM Selling UOM is
--	'EA' and the Infor Retail UOM is 'OZ'. If the Infor Retail UOM
--	is not 'OZ' then it returns 'BC'. If the GPM Selling UOM is 'LB'
--	'KG' then it returns 'LB' or 'KG' respectively. Otherwise it
--	returns the IRMA UOM.
--*****************************************************************
BEGIN
	DECLARE @unitOfMeasure VARCHAR(5) = 
		CASE 
			WHEN @globalPriceManagementEnabled = 1 THEN
				CASE
					WHEN ISNULL(@gpmSellingUom, '') = 'EA' THEN
						CASE 
							WHEN @inforRetailUom = 'OZ' THEN 'FW'
							ELSE 'BC'
						END
					WHEN ISNULL(@gpmSellingUom, '') = 'LB' THEN 'LB'
					WHEN ISNULL(@gpmSellingUom, '') = 'KG' THEN 'KG'
					ELSE @irmaUnitOfMeasure
				END
			ELSE @irmaUnitOfMeasure
		END
	RETURN @unitOfMeasure
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumUnitOfMeasure] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumUnitOfMeasure] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumUnitOfMeasure] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumUnitOfMeasure] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumUnitOfMeasure] TO [IRMAReportsRole]
    AS [dbo];
