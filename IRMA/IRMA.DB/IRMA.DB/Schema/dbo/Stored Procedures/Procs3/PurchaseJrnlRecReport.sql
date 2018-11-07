CREATE PROCEDURE dbo.PurchaseJrnlRecReport

@Store_No int,
@SubTeam_No int,
@BeginDate varchar(20),
@EndDate varchar(20),
@OrderBy int
AS

--if @orderBy 0 then sort by InvoiceDate
--if @orderBy 1 then sort by PO No
--if @orderBy 2 then sort by Receive Date
--if @orderBy 3 then sort by Vendor

-- Convert the dates to smalldatetime for Efficiency
-- The parameters are varchar because Crystal does not cooperate otherwise.
DECLARE @BeginDateDt SMALLDATETIME, @EndDateDt SMALLDATETIME
SELECT @BeginDateDt = CONVERT(SMALLDATETIME, @BeginDate), @EndDateDt = CONVERT(SMALLDATETIME, @EndDate)

select @OrderBy = isnull(@orderBy,0)
if @OrderBy = 0
  begin
     SELECT OrderHeader.OrderHeader_ID AS PO_No,
             (SELECT 
		max(DateReceived)
	     FROM 
		OrderItem (NOLOCK)
     	     where 
		OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID) AS Received_Date,	      

	     (Select 
		Vendor.CompanyName 
	     from 
		Vendor 
	     where 
		Vendor_ID = OrderHeader.Vendor_ID) AS VendorName, 

	     InvoiceNumber AS Invoice_No,
	     OrderHeader.InvoiceDate, 
    	     OrderInvoice.InvoiceCost AS InvoiceAmount
     FROM orderheader(NOLOCK) INNER JOIN
          Vendor(NOLOCK) ON 
          Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID INNER JOIN
          OrderInvoice(NOLOCK) ON 
          orderInvoice.Orderheader_ID = OrderHeader.OrderHeader_ID
     WHERE Vendor.Store_No = ISNULL(@Store_NO, vendor.Store_NO) AND 
          OrderHeader.Transfer_to_SubTeam = ISNULL(@SubTeam_NO, OrderHeader.Transfer_to_SubTeam) and 
          exists (select * 
		from OrderItem
		where orderheader.orderheader_id = orderItem.OrderHeader_ID and						
			OrderItem.DateReceived >= @BeginDateDt AND
			OrderItem.DateReceived < DATEADD(day, 1, @EndDate))
     ORDER BY InvoiceDate DESC
  End
else if @OrderBy = 1
  begin
     SELECT OrderHeader.OrderHeader_ID AS PO_No,
             (SELECT 
		max(DateReceived)
	     FROM 
		OrderItem (NOLOCK)
     	     where 
		OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID) AS Received_Date,	      

	     (Select 
		Vendor.CompanyName 
	     from 
		Vendor 
	     where 
		Vendor_ID = OrderHeader.Vendor_ID) AS VendorName, 

	     InvoiceNumber AS Invoice_No,
	     OrderHeader.InvoiceDate, 
    	     OrderInvoice.InvoiceCost AS InvoiceAmount
     FROM orderheader(NOLOCK) INNER JOIN
          Vendor(NOLOCK) ON 
          Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID INNER JOIN
          OrderInvoice(NOLOCK) ON 
          orderInvoice.Orderheader_ID = OrderHeader.OrderHeader_ID
     WHERE Vendor.Store_No = ISNULL(@Store_NO, vendor.Store_NO) AND 
          OrderHeader.Transfer_to_SubTeam = ISNULL(@SubTeam_NO, OrderHeader.Transfer_to_SubTeam) and 
          exists (select * 
		from OrderItem
		where orderheader.orderheader_id = orderItem.OrderHeader_ID and						
			OrderItem.DateReceived >= @BeginDateDt AND
			OrderItem.DateReceived < DATEADD(day, 1, @EndDate))
     ORDER BY OrderHeader.OrderHeader_ID DESC
  End
else if @OrderBy = 2
  begin
     SELECT OrderHeader.OrderHeader_ID AS PO_No,
             (SELECT 
		max(DateReceived)
	     FROM 
		OrderItem (NOLOCK)
     	     where 
		OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID) AS Received_Date,	      

	     (Select 
		Vendor.CompanyName 
	     from 
		Vendor 
	     where 
		Vendor_ID = OrderHeader.Vendor_ID) AS VendorName,

	     InvoiceNumber AS Invoice_No,
	     OrderHeader.InvoiceDate, 
    	     OrderInvoice.InvoiceCost AS InvoiceAmount
     FROM orderheader(NOLOCK) INNER JOIN
          Vendor(NOLOCK) ON 
          Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID INNER JOIN
          OrderInvoice(NOLOCK) ON 
          orderInvoice.Orderheader_ID = OrderHeader.OrderHeader_ID
     WHERE Vendor.Store_No = ISNULL(@Store_NO, vendor.Store_NO) AND 
          OrderHeader.Transfer_to_SubTeam = ISNULL(@SubTeam_NO, OrderHeader.Transfer_to_SubTeam) and 
          exists (select * 
		from OrderItem
		where orderheader.orderheader_id = orderItem.OrderHeader_ID and						
			OrderItem.DateReceived >= @BeginDateDt AND
			OrderItem.DateReceived < DATEADD(day, 1, @EndDate))
     ORDER BY Received_Date DESC
  End
else if @OrderBy = 3
  begin
     SELECT OrderHeader.OrderHeader_ID AS PO_No,
             (SELECT 
		max(DateReceived)
	     FROM 
		OrderItem (NOLOCK)
     	     where 
		OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID) AS Received_Date,	      

       	     (Select 
		Vendor.CompanyName 
	     from 
		Vendor 
	     where 
		Vendor_ID = OrderHeader.Vendor_ID) AS VendorName,

	     InvoiceNumber AS Invoice_No,
	     OrderHeader.InvoiceDate, 
    	     OrderInvoice.InvoiceCost AS InvoiceAmount
     FROM orderheader(NOLOCK) INNER JOIN
          Vendor(NOLOCK) ON 
          Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID INNER JOIN
          OrderInvoice(NOLOCK) ON 
          orderInvoice.Orderheader_ID = OrderHeader.OrderHeader_ID
     WHERE Vendor.Store_No = ISNULL(@Store_NO, vendor.Store_NO) AND 
          OrderHeader.Transfer_to_SubTeam = ISNULL(@SubTeam_NO, OrderHeader.Transfer_to_SubTeam) and 
          exists (select * 
		from OrderItem
		where orderheader.orderheader_id = orderItem.OrderHeader_ID and						
			OrderItem.DateReceived >= @BeginDateDt AND
			OrderItem.DateReceived < DATEADD(day, 1, @EndDate))
     ORDER BY VendorName  DESC
  End
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchaseJrnlRecReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchaseJrnlRecReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchaseJrnlRecReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PurchaseJrnlRecReport] TO [IRMAReportsRole]
    AS [dbo];

