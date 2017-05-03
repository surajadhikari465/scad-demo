SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
#################################################################################################################################################################
Change History:

11/16/2009	DN	Copied SP from OpenOrdersDetailReport as a base for creating the new report eInvoiceItemExceptionReport.

03/30/2010	BS	Modified SP to support new eInvoiceItemExceptionReport and indicate purpose of SP.

04/07/2010  BS  Modified to include Exception Description and to clean up commented functionality.

04/15/2010	BS	Corrected fields for filtering.

05/03/2010	RS	Wrapped Authorized flag in ISNULL to properly evaluate True or False

05/04/2010	RS	Basically rewrote joins, etc to match dbo.GetReceivingListForNOIDNORD.sql

01/11/2013	BAS Updated reference to Item.Discontinue_Item to SIV.DiscontinueItem to account for schema change

#################################################################################################################################################################
*/

IF EXISTS (SELECT *	FROM dbo.SysObjects
			WHERE ID = Object_Id(N'[dbo].[OpenOrdersReportNOIDNORD]')
				AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[OpenOrdersReportNOIDNORD]
GO


--DN removed the inner join on OrderItem since this sproc is for NORD items
CREATE PROCEDURE [dbo].[OpenOrdersReportNOIDNORD]
	@ReceiveLocation_ID int = null,
    @SubTeam_No int = null,
    @OrderDateStart varchar(12) = null,
    @OrderDateEnd varchar(12) = null,
    @Vendor_ID int = null --,
AS
BEGIN
    SET NOCOUNT ON
    
	SELECT 
		Vendor.CompanyName AS VendorName,
		ReceiveLocation.CompanyName AS ReceiveLocationName, 
		OrderHeader.OrderHeader_ID,
		OrderHeader.OrderDate,
		OrderHeader.SentDate,
		OrderHeader.Expected_Date,
		SubTeam_Name,
		UserName,
		OrderHeader.Return_Order,
		OrderingStore.CompanyName AS OrderingStore,
		ItemIdentifier.Identifier,
		Item.Item_Description,
		Item.Package_Desc1,
		OI.QuantityOrdered,
		IUnit.Unit_Name,
		IB.Brand_Name,
		eInvItm.calc_net_ext_cost as Cost,		
		(CASE WHEN eInvItm.IsNotOrdered = 1 AND SI.Authorized = 0 THEN 'Not Authorized: item is on file in IRMA, but is not authorized for the ordering store.' ELSE
			CASE WHEN eInvItm.IsNotOrdered = 1 AND StoreItemVendorID IS NULL THEN 'Incorrect Vendor: item is on file in IRMA, but does not have a item-vendor-store association for the vendor specified on the PO.' ELSE
				CASE WHEN eInvItm.IsNotOrdered = 1 AND SIV.DiscontinueItem = 1 THEN 'Discontinued: item is on file in IRMA, but has been marked as discontinued.'
				ELSE 'Not Ordered: The item is not on the PO sent to the vendor, but is otherwise a valid item.' END
			END
		END) as eInvoiceItemException
	FROM 
		OrderHeader									(nolock)
		INNER JOIN	SubTeam							(NOLOCK)	ON (SubTeam.SubTeam_No = OrderHeader.Transfer_To_SubTeam)
		INNER JOIN	Users							(nolock)	ON (Users.User_ID = OrderHeader.CreatedBy)
		INNER JOIN	Einvoicing_Header eInvHeader	(NOLOCK)	ON eInvHeader.Einvoice_id = OrderHeader.EInvoice_ID 
		INNER JOIN	eInvoicing_Item eInvItm			(NOLOCK)	ON eInvItm.EInvoice_ID = eInvHeader.Einvoice_id
		INNER JOIN  Item							(nolock)    ON eInvItm.Item_Key = Item.Item_Key  
		INNER JOIN  ItemIdentifier					(nolock)    ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1  
		LEFT JOIN	OrderItem OI					(nolock)	ON OrderHeader.OrderHeader_ID = OI.OrderHeader_ID AND eInvItm.Item_Key = OI.Item_Key  
		LEFT JOIN	ItemVendor IV					(nolock)	ON Item.Item_Key = IV.Item_Key AND OrderHeader.Vendor_Id = IV.Vendor_Id
		INNER JOIN 	ItemBrand IB					(nolock)	on Item.Brand_Id = IB.Brand_Id
		INNER JOIN 	ItemUnit IUnit					(nolock)	ON IUnit.Unit_ID = Item.Package_Unit_ID
		INNER JOIN	Vendor ReceiveLocation			(NOLOCK)	ON (ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID)
		INNER JOIN	Vendor							(NOLOCK)	ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)
		INNER JOIN	Vendor OrderingStore			(NOLOCK)	ON (OrderingStore.Vendor_ID = OrderHeader.ReceiveLocation_ID)
		LEFT JOIN	storeitemvendor SIV				(NOLOCK)	ON SIV.Store_No = OrderingStore.Store_No AND SIV.Vendor_ID = OrderHeader.Vendor_ID 	AND SIV.Item_Key = eInvItm.Item_Key
		LEFT JOIN 	StoreItem SI					(nolock)	ON SI.Store_No = OrderingStore.Store_No AND  SI.Item_Key = Item.Item_Key 
	WHERE
		OrderingStore.Store_No = ISNULL(@ReceiveLocation_ID, OrderingStore.Store_No)
		AND Transfer_To_SubTeam = ISNULL(@SubTeam_No, Transfer_To_SubTeam)
		AND (DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) >= ISNULL(@OrderDateStart, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)) AND DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) <= ISNULL(@OrderDateEnd, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)))   
		AND OrderHeader.Vendor_ID = ISNULL(@Vendor_ID,OrderHeader.Vendor_ID) 
		AND eInvItm.IsNotOrdered = 1
	UNION
	SELECT 
		Vendor.CompanyName AS VendorName,
		ReceiveLocation.CompanyName AS ReceiveLocationName, 
		OrderHeader.OrderHeader_ID,
		OrderHeader.OrderDate,
		OrderHeader.SentDate,
		OrderHeader.Expected_Date,
		SubTeam_Name,
		UserName,
		OrderHeader.Return_Order,
		OrderingStore.CompanyName AS OrderingStore,
		ISNULL(upc, vendor_item_num),
		descrip,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		'Not On File: item is not on file in IRMA database.' as eInvoiceItemException
	FROM OrderHeader (nolock)
		INNER JOIN SubTeam					(NOLOCK) ON (SubTeam.SubTeam_No = OrderHeader.Transfer_To_SubTeam)
		INNER JOIN Users (nolock) ON (Users.User_ID = OrderHeader.CreatedBy)
		INNER JOIN Einvoicing_Header eInvHeader (NOLOCK) ON eInvHeader.Einvoice_id = OrderHeader.EInvoice_ID 
		INNER JOIN eInvoicing_Item eInvItm (NOLOCK) ON eInvItm.EInvoice_ID = eInvHeader.Einvoice_id
		INNER JOIN Vendor ReceiveLocation	(NOLOCK) ON (ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID)
		INNER JOIN Vendor					(NOLOCK) ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)
		INNER JOIN Vendor OrderingStore		(NOLOCK) ON (OrderingStore.Vendor_ID = OrderHeader.ReceiveLocation_ID)
				WHERE OrderingStore.Store_No = ISNULL(@ReceiveLocation_ID, OrderingStore.Store_No)
					--AND Orderheader.CloseDate IS NULL 
					AND Transfer_To_SubTeam = ISNULL(@SubTeam_No, Transfer_To_SubTeam)
					AND (DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) >= ISNULL(@OrderDateStart, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)) AND DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) <= ISNULL(@OrderDateEnd, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)))   
					AND OrderHeader.Vendor_ID = ISNULL(@Vendor_ID,OrderHeader.Vendor_ID) 
		AND eInvItm.IsNotIdentifiable = 1
	ORDER BY
		SubTeam_Name,
		Vendor.CompanyName,
		OrderHeader.Return_Order,
		OrderHeader.OrderDate
			
    SET NOCOUNT OFF
END

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
