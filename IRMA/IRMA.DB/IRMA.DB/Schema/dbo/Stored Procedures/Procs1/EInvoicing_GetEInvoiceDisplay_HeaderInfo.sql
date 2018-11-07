CREATE PROCEDURE [dbo].[EInvoicing_GetEInvoiceDisplay_HeaderInfo]
	@EinvoiceId int,
	@StoreNo	int  

AS  

/*********************************************************************************************
CHANGE LOG
DEV		DATE	TASK	Description
----------------------------------------------------------------------------------------------
BSR		091709	11130	Changed all reference to DVOOrderID to OrderExternalSourceOrderID
RDE     050710	12688	Added regex to remove alpha characters from PO_NUM mathing so it works like the rest of einvoicing.
BAS		031213	9946	Updated FROM Clause to JOIN to OrderHeader which then uses oh.Vendor_ID to join to Vendor.
						This resolved the issue related to an incorrect vendor name being displayed to the user.
KM		2013/05/17	12354	Don't return the dates as strings; Include CreatedBy in the selection set;
***********************************************************************************************/

BEGIN
	SET NOCOUNT ON   

	DECLARE @lineitem_amt		AS MONEY
	DECLARE @invoicecharges_amt AS MONEY
	
	SET @lineitem_amt = (
	SELECT CAST(SUM(eid.ElementValue * CAST(ChargeOrAllowance AS DECIMAL)) AS DECIMAL)
				   FROM   EInvoicing_ItemData eid
						  INNER JOIN EInvoicing_Config ec
							   ON  eid.ElementName = ec.ElementName
						  INNER JOIN EInvoicing_SACTypes es
							   ON  ec.SacCodeType = es.SACType_Id
				   WHERE  elementvalue IS NOT NULL
						  AND ec.IsSacCode = 1
						  AND es.SACType = 'Line Item'
						  AND eid.EInvoice_Id = @EinvoiceId
	)

	SET @lineitem_amt = CASE WHEN @lineitem_amt IS NULL THEN 0.0 ELSE @lineitem_amt END

	SET @invoicecharges_amt =  (
	           SELECT CAST(SUM(eid.elementvalue * CAST(chargeorallowance AS DECIMAL)) AS DECIMAL)
	           FROM   EInvoicing_ItemData eid
	                  INNER JOIN EInvoicing_Config ec
	                       ON  eid.ElementName = ec.ElementName
	                  INNER JOIN EInvoicing_SACTypes es
	                       ON  ec.SacCodeType = es.SACType_Id
	           WHERE  elementvalue IS NOT NULL
	                  AND ec.IsSacCode = 1
	                  AND es.SACType <> 'Line Item'
	                  AND eid.EInvoice_Id =  @EinvoiceId
	)

	SET @invoicecharges_amt = CASE WHEN @invoicecharges_amt IS NULL THEN 0.0 ELSE @invoicecharges_amt END
	
	SELECT 
		Invoice_Num,
	    eh.store AS Store,
	    eh.street1 AS S_Address1,
	    eh.city AS S_City,
	    eh.state AS S_State,
	    eh.postal AS S_ZipCode,
	    S.Phone_Number AS S_Phone,
	    V.CompanyName,
	    V.Address_Line_1 AS V_Address1,
	    V.Address_Line_2 AS V_Address2,
	    V.City AS V_City,
	    V.State AS V_State,
	    V.Zip_Code AS V_ZipCode,
	    V.Phone AS V_Phone,
	    V.Fax AS V_Fax,
	    EH.po_num AS Purchase_Order,
	    EH.cust_num AS Customer_ID,
	    S.BusinessUnit_Id AS Business_Unit,
	    EH.invoice_date AS Sent_On,
	    EH.invoice_date AS Invoice_Date,
		EH.order_date AS Order_Date,
	    (
	        SELECT SUM(ei.qty_shipped)
	        FROM   EInvoicing_Item ei
	        WHERE  ei.EInvoice_id = eh.Einvoice_id
	    ) AS itemcount,
	    eh.invoice_amt,
	    @lineitem_amt AS lineitem_amt,
	    @invoicecharges_amt AS invoicecharges_amt, 
		null as notes,
		u.FullName as Buyer
	FROM   
		einvoicing_header	EH	(nolock)
		JOIN OrderHeader	oh	(nolock) ON	eh.Einvoice_id = oh.eInvoice_Id 
	    JOIN Vendor			V	(nolock) ON oh.Vendor_ID = V.Vendor_ID
	    JOIN Store			S	(nolock) ON s.businessunit_id = eh.store_num
		JOIN Users			u	(nolock) ON oh.CreatedBy = u.User_ID
	WHERE  
		s.Store_No = @StoreNo
		And eh.einvoice_id = @EinvoiceId
	
	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_HeaderInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_HeaderInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_HeaderInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_HeaderInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_HeaderInfo] TO [IRMAReportsRole]
    AS [dbo];

