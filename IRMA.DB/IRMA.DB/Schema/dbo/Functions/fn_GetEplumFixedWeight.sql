CREATE FUNCTION [dbo].[fn_GetEplumFixedWeight]
(
	@irmaFixedWeight VARCHAR(25),
	@gpmSellingUom VARCHAR(5),
	@inforRetailUom VARCHAR(5),
	@inforRetailSize DECIMAL (9, 4),
	@globalPriceManagementEnabled BIT = 0
)
RETURNS VARCHAR(25)
AS
--*****************************************************************
--	This function is used to transform IRMA's values to EPlum's
--	expected values during the time that a region is on GPM
--	but not on OnePlum.
--
--	Returns the Infor Retail Size as the Fixed Weight for an item
--	if the GPM selling UOM is EA and the Infor Retail UOM is OZ.
--	Based on the transformation logic provided by the Retail Apps
--	and DDS teams. Only performs the transformation is GPM is 
--	enabled. Otherwise returns the IRMA Fixed Weight.
--
--	Note many regions require 'FL OZ' and workaround. This field 
--	can support decimals but non-bizerba scales may not. 
--	Send '10.2' as '102' or '5.0' as '50'. The decimal is always 
--	assumed if configured for decimal support.
--*****************************************************************
BEGIN
	DECLARE @inforRetailSizeVarChar VARCHAR(25) = CAST(@inforRetailSize AS VARCHAR(25))
	DECLARE @fixedWeight VARCHAR(25) = 
		CASE 
			WHEN @globalPriceManagementEnabled = 1 THEN
				CASE
					WHEN ISNULL(@gpmSellingUom, '') = 'EA' THEN
						CASE 
							WHEN @inforRetailUom = 'OZ' THEN REPLACE(SUBSTRING(@inforRetailSizeVarChar, 1, LEN(@inforRetailSizeVarChar) - 2), '.', '')
							ELSE NULL
						END
					ELSE @irmaFixedWeight
				END
			ELSE @irmaFixedWeight
		END
	RETURN @fixedWeight
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumFixedWeight] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumFixedWeight] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumFixedWeight] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumFixedWeight] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetEplumFixedWeight] TO [IRMAReportsRole]
    AS [dbo];