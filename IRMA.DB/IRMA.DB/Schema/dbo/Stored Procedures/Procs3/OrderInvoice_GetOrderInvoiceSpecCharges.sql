CREATE PROCEDURE [dbo].[OrderInvoice_GetOrderInvoiceSpecCharges]
	@OrderHeader_Id		INT
AS
-- ****************************************************************************************************************
-- Procedure: OrderInvoice_GetOrderInvoiceSpecCharges
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/27	KM		3744	Added update history template; usage review;
-- ****************************************************************************************************************
BEGIN
	SELECT
		[Type]			= es.SACType,
		GLAccount		= st.GLPurchaseAcct,
		[Description]	= ISNULL(oic.[Description], st.SubTeam_Name),
		oic.[Value],
		oic.Charge_ID,  
		IsAllowance, 
		ElementName
	FROM   
		OrderInvoiceCharges					(nolock) oic
		INNER JOIN EInvoicing_SACTypes		(nolock) es		ON	oic.SACType_ID = es.SACType_ID 
		LEFT  JOIN SubTeam					(nolock) st		ON	oic.SubTeam_No = st.SubTeam_No 
	WHERE  
		oic.OrderHeader_Id = @OrderHeader_Id
	ORDER BY
		oic.SACType_ID,
		oic.Charge_ID   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderInvoice_GetOrderInvoiceSpecCharges] TO [IRMAClientRole]
    AS [dbo];

