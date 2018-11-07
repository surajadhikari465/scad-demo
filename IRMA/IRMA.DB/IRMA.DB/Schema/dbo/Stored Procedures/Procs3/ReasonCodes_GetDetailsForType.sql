-- ====================================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Gets all the reason code details defined for the region
--				for the type sent as argument
--
-- Changes:  RDE		9/5/2011		Added @IncludeDisabled functionality For InvoiceMatching screen
-- ====================================================================

CREATE PROCEDURE [dbo].[ReasonCodes_GetDetailsForType]
@ReasonCodeTypeAbbr as char(2),
@IncludeDisabled INT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @ReasonCodeTypeID as int
	
	SET @ReasonCodeTypeID = (SELECT ReasonCodeTypeID FROM ReasonCodeType 
						WHERE ReasonCodeTypeAbbr = @ReasonCodeTypeAbbr)

	SELECT	RCD.ReasonCodeDetailID, 
			LTRIM(RTRIM(RCD.ReasonCode)) As ReasonCode, 
			RCD.ReasonCodeDesc, 
			RCD.ReasonCodeExtDesc 
	FROM ReasonCodeDetail RCD
		INNER JOIN ReasonCodeMappings RCM
		ON RCM.ReasonCodeDetailID = RCD.ReasonCodeDetailID
	WHERE RCM.ReasonCodeTypeID = @ReasonCodeTypeID
			And RCM.Disabled = case when @IncludeDisabled = 0 then @IncludeDisabled else RCM.Disabled end 
    ORDER BY ReasonCode ASC
			
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetDetailsForType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetDetailsForType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetDetailsForType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReasonCodes_GetDetailsForType] TO [IRMAReportsRole]
    AS [dbo];

