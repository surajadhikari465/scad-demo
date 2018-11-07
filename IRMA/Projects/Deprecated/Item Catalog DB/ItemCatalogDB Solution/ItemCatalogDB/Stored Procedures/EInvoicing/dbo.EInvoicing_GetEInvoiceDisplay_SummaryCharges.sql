IF EXISTS (SELECT * FROM sysobjects WHERE NAME = 'EInvoicing_GetEInvoiceDisplay_SummaryCharges')
    DROP PROCEDURE EInvoicing_GetEInvoiceDisplay_SummaryCharges
GO
  
CREATE PROCEDURE dbo.EInvoicing_GetEInvoiceDisplay_SummaryCharges
	@EInvoiceId INT

AS

-- ****************************************************************************************************************
-- Procedure: EInvoicing_GetEInvoiceDisplay_SummaryCharges()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called from EInvoiceHTMLDisplay.vb.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-24	KM		9874	Add update history template; Multiply eid.ElementValue by ChargeOrAllowance so that credits
--								and discounts will appear as negative in the UI.  This is consistent with the logic in EInvoicing_GetEInvoice_Display_ItemCharges;
-- ****************************************************************************************************************

BEGIN
	SET NOCOUNT ON  
	
	-- using a temp table because doing sums on varchar values even with inline casts behaved strangely.

    DECLARE @WorkingTable TABLE
    (
		ChargeName VARCHAR(100),
		[VALUES] DECIMAL(18, 4)
    )
	
    INSERT INTO @WorkingTable
    (
        ChargeName,
		[VALUES]
    )
        
	SELECT
        ISNULL(ec.Label , eid.ElementName) AS ChargeName,
		CAST(eid.ElementValue AS DECIMAL(18,4)) * ChargeOrAllowance AS ElementValue
    FROM
        EInvoicing_SummaryData eid
        INNER JOIN EInvoicing_Config ec ON eid.ElementName = ec.ElementName
    WHERE
        eid.EInvoice_Id = @EInvoiceId
        AND eid.ElementValue IS NOT NULL
        AND ec.ExcludeFromCalculations = 0
        
    SELECT
        ChargeName,
		SUM([values]) AS ChargeValue
    FROM
        @WorkingTable
    GROUP BY
        ChargeName
	
    SET NOCOUNT OFF
END
GO