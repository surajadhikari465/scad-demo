CREATE PROCEDURE [dbo].[EInvoicing_GetEInvoiceDisplay_Summary]
	@OrderId int,
	@StoreNo int
AS
/*********************************************************************************************
CHANGE LOG
DEV		DATE	TASK	Description
----------------------------------------------------------------------------------------------
BSR		091709	11130	Changed all reference to DVOOrderID to OrderExternalSourceOrderID

***********************************************************************************************/

BEGIN
    SET NOCOUNT ON
    
	declare @po_num  varchar(30)
	-- If a DVO Order, use DVOOrderId instead.
	--set @po_num= (select isnull(DVOOrderId, OrderHeader_Id) from OrderHeader where OrderHeader_Id = @OrderId)
	set @po_num= (select isnull(OrderExternalSourceOrderID, OrderHeader_Id) from OrderHeader where OrderHeader_Id = @OrderId)
   
		--Get Summary Detail SAC Charges
SELECT EC.Label, 
		EI.elementvalue, 
		EC.SacCodeType, 
		EC.IsSacCode, 
		ES.SACType
FROM einvoicing_summarydata EI
		JOIN Einvoicing_Header EH
			ON EH.einvoice_id = EI.einvoice_id
		JOIN Einvoicing_config as EC 
			ON EI.ElementName = EC.ElementName
		JOIN einvoicing_sactypes as ES
			ON ES.SACType_ID = EC.SacCodeType
		Inner Join Store S 
			on S.Store_no = @StoreNo
		
WHERE elementvalue is not null and
	EH.PO_Num = @PO_Num
	AND EH.Store_num = S.BusinessUnit_Id
	AND EC.IsSacCode = 1

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_Summary] TO [IRMAClientRole]
    AS [dbo];

