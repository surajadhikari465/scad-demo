
/****** Object:  StoredProcedure [dbo].[OrderInvoice_GetOrderInvoiceSACTotal]    Script Date: 08/07/2008 12:54:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderInvoice_GetOrderInvoiceSACTotal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrderInvoice_GetOrderInvoiceSACTotal]
GO
/****** Object:  StoredProcedure [dbo].[OrderInvoice_GetOrderInvoiceSACTotal]    Script Date: 08/07/2008 12:54:32 ******/

CREATE PROCEDURE [dbo].[OrderInvoice_GetOrderInvoiceSACTotal]
	@OrderHeader_Id		INT,
	@SACTotal			MONEY OUTPUT
AS
-- ****************************************************************************************************************
-- Procedure: OrderInvoice_GetOrderInvoiceSACTotal
--    Author: Dave Stacey
--      Date: 2011/10/08
--
-- Description: This query populates the SAC Charges total box on the Control Group Invoice Data form.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/27	KM		3744	Added update history template; usage review;
-- ****************************************************************************************************************
BEGIN

	-- Get ID for non-allocated SACType
	DECLARE @SACType_Id AS INT

	SELECT
		@SACType_Id = SACType_Id 
	FROM
		dbo.einvoicing_sactypes (NOLOCK)
	WHERE
		SACType = 'Not Allocated'   
 
	SELECT 
		@SACTotal = SUM(OIC.Value)
	FROM 
		dbo.OrderInvoiceCharges OIC (NOLOCK)
	WHERE 
		OrderHeader_Id = @OrderHeader_Id
		AND SACType_Id = @SACType_Id

	SELECT @SACTotal = ISNULL(@SACTotal, 0)

END
GO