CREATE PROCEDURE [dbo].[GetOrderID_for_ExternalSourceOrderID]
    @ExternalSourceOrder_ID int,
    @Store_no int
    
AS
   -- **************************************************************************
   -- Procedure: GetOrderID_for_ExternalSourceOrderID()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description: Lookup OrderID then call dbo.GetOrderID Given DVO ID , eInvoice ID, 
   -- has same return set as GetOrderInfo(@OrderHeader_ID int)
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 1/4/2012      RWA		3561	use to populate combobox with valid Order_ID
   -- 1/16/2012		RWA		3561	added store_No filter (amongst 361,060 duplicates only 500 link to the same store -- this improves efficiency)
   -- 1/24/2012		RWA		3561	Corrected link between Vendor table (with store name in it) to Store Table so it is like GetOrderInfo and not region specific
   -- **************************************************************************


BEGIN

    SET NOCOUNT ON
   

    SELECT top 20 OH.OrderHeader_ID, isNull(ES.[Description], 'IRMA') as [Source], V.CompanyName 
    --,OH.OrderExternalSourceOrderID,  St.CompanyName as Store, st.Vendor_ID as StoreVendor_ID , s.store_No, s.Store_Name -- for testing only --
    FROM dbo.OrderHeader OH
    LEFT JOIN dbo.OrderExternalSource ES On ES.ID = OH.OrderExternalSourceID
    LEFT JOIN dbo.Vendor V On V.Vendor_ID = OH.Vendor_ID
    LEFT JOIN dbo.Vendor St On St.Vendor_ID = OH.receiveLocation_ID
    LEFT JOIN dbo.Store S ON  S.Store_No = st.Store_No
    WHERE (OH.OrderExternalSourceOrderID = @ExternalSourceOrder_ID or OH.OrderHeader_ID = @ExternalSourceOrder_ID)
    and (S.Store_No = @Store_No or isNull(@Store_No,0) = 0 )
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderID_for_ExternalSourceOrderID] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderID_for_ExternalSourceOrderID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderID_for_ExternalSourceOrderID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderID_for_ExternalSourceOrderID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderID_for_ExternalSourceOrderID] TO [IRMAReportsRole]
    AS [dbo];

