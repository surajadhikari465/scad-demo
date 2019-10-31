--Used by Mammoth/WebSupport
CREATE PROCEDURE amz.GetInStockEventsByType
  @eventCode NVARCHAR(25),
  @maxRecords int = 50001,
  @storeBU INT = NULL,
  @dateFrom DATETIME,
  @dateTo DATETIME
  WITH RECOMPILE
AS
BEGIN
  SET NOCOUNT ON;
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
  
  DECLARE @CentralTimeZoneOffset int, @localeDateFrom DATETIME, @localeDateTo DATETIME
  SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

  SELECT @localeDateFrom = DATEADD(hour, @CentralTimeZoneOffset, @dateFrom) WHERE @dateFrom IS NOT NULL
  SELECT @localeDateTo = DATEADD(hour, @CentralTimeZoneOffset, @dateTo) WHERE @dateTo IS NOT NULL

  DECLARE @msg VARCHAR(100),
          @inv_adj varchar(12) = 'INV_ADJ',           --Inventory Adjustment
          @rcpt_cre VARCHAR(12) = 'RCPT_CRE',         --Order Receipt Creation
          @po_cre VARCHAR(12) = 'PO_CRE',             --Purchase Order Creation
          @po_del VARCHAR(12) = 'PO_DEL',             --Purchase Order Deletion
          @po_mod varchar(12) = 'PO_MOD',             --Purchase Order Modification
          @po_line_del VARCHAR(12) = 'PO_LINE_DEL',   --Purchase Order Line Item Deletion
          @tsf_cre VARCHAR(12) = 'TSF_CRE',           --Transfer Order Creation
          @tsf_del VARCHAR(12) = 'TSF_DEL',           --Transfer Order Deletion
          @tsf_line_del varchar(12) = 'TSF_LINE_DEL'; --Transfer Line Item Deletion
         
  IF(@eventCode = @inv_adj) --Inventory Adjustment
  BEGIN
    SELECT TOP(@maxRecords) A.ItemHistoryID ID, A.DateStamp [Date],
      A.Item_Key, B.BusinessUnit_ID Store_BU, B.Store_Name, A.AdjustmentReason Reason
      FROM ItemHistory A
      JOIN Store B ON B.Store_no = A.Store_No
      WHERE Adjustment_ID = 1
        AND (@dateFrom IS NULL OR A.DateStamp >= @localeDateFrom)
		AND (@dateTo IS NULL OR A.DateStamp <= @localeDateTo)
        AND B.BusinessUnit_ID = ISNULL(@storeBU, B.BusinessUnit_ID)
      ORDER BY ItemHistoryID;
  
     RETURN;
  END
  
  
  IF(@eventCode = @po_cre) --Purchase Order Creation
  BEGIN
    SELECT TOP(@maxRecords) OrderHeader_ID ID, C.BusinessUnit_ID Store_BU, C.Store_Name, SentDate, OrderDate
    FROM OrderHeader A
    JOIN Vendor B on A.ReceiveLocation_ID = B.Vendor_ID
	JOIN Store C on B.Store_no = C.Store_No

    WHERE Sent = 1 
      AND OrderType_ID <> 3
      AND (@dateFrom IS NULL OR IsNull(SentDate, OrderDate) >= @localeDateFrom) 
      AND (@dateTo IS NULL OR IsNull(SentDate, OrderDate) <= @localeDateTo)
      AND C.BusinessUnit_ID = ISNULL(@StoreBU, C.BusinessUnit_ID)
    RETURN;
  END
  
  
  IF(@eventCode = @po_mod) --Purchase Order Modification
  BEGIN
    SELECT TOP(@maxRecords) A.OrderHeader_ID ID, IsNull(A.OriginalCloseDate, A.CloseDate) CloseDate, D.InsertDate
      FROM OrderHeader A
      JOIN Vendor B on A.ReceiveLocation_ID = B.Vendor_ID
	  JOIN Store C on B.Store_no = C.Store_No
      LEFT JOIN infor.OrderExpectedDateChangeQueue D ON D.OrderHeader_ID = A.OrderHeader_ID
      WHERE A.Sent = 1 
        AND A.OrderType_ID <> 3
        AND (@dateFrom IS NULL OR A.CloseDate >= @localeDateFrom OR A.OriginalCloseDate >= @dateFrom OR D.InsertDate >= @dateFrom)
        AND (@dateTo IS NULL OR A.CloseDate <= @localeDateTo OR A.OriginalCloseDate <= @dateTo OR D.InsertDate <= @dateTo)
        AND C.BusinessUnit_ID = ISNULL(@StoreBU, C.BusinessUnit_ID)
      ORDER BY A.OrderHeader_ID;
  
    RETURN;
  END
  
  
  IF(@eventCode = @tsf_cre) --Transfer Order Creation
  BEGIN
    SELECT TOP(@maxRecords) OrderHeader_ID ID, C.BusinessUnit_ID ToStore_BU, C.Store_Name ToStore_Name, SentDate, OrderDate
      FROM OrderHeader A
      JOIN Vendor B on A.ReceiveLocation_ID = B.Vendor_ID
	  JOIN Store C on B.Store_no = C.Store_No
      WHERE [Sent] = 1 
        AND OrderType_ID = 3
        AND (@dateFrom IS NULL OR IsNull(SentDate, OrderDate) >= @localeDateFrom) 
        AND (@dateTo IS NULL OR IsNull(SentDate, OrderDate) <= @localeDateTo)
        AND C.BusinessUnit_ID = ISNULL(@StoreBU, C.BusinessUnit_ID)

    RETURN;
  END
  
  IF(@eventCode = @rcpt_cre) --Order Receipt Creation
  BEGIN
    SELECT TOP(@maxRecords) A.OrderHeader_ID ID, C.BusinessUnit_ID Store_BU, C.Store_Name, Min(D.DateReceived) Earliest_Received_Date, Max(D.DateReceived) Latest_Received_Date, Count(*) [Order Items]
      FROM OrderHeader A
      JOIN Vendor B on A.ReceiveLocation_ID = B.Vendor_ID
	  JOIN Store C on B.Store_no = C.Store_No
      JOIN OrderItem D ON D.OrderHeader_ID = A.OrderHeader_ID
      WHERE A.Sent = 1 
        AND (@dateFrom IS NULL OR D.DateReceived >= @localeDateFrom) 
        AND (@dateTo IS NULL OR D.DateReceived <= @localeDateTo)
		AND C.BusinessUnit_ID = ISNULL(@StoreBU, C.BusinessUnit_ID)
      GROUP BY A.OrderHeader_ID, A.OrderHeaderDesc, C.BusinessUnit_ID, C.Store_Name
      ORDER BY A.OrderHeader_ID DESC;
  
    RETURN;
  END
  
  
  IF(@eventCode = @po_line_del) --Purchase Order Line Item Deletion
  BEGIN
    SELECT TOP(@maxRecords) B.OrderHeader_ID ID, A.DeletedOrderItem_ID LineItem_ID, C.BusinessUnit_ID Store_BU, C.Store_Name, A.InsertDate [Delete_Date], A.Item_Key
    FROM amz.DeletedOrderItem A
    JOIN OrderHeader B on A.OrderHeader_ID = B.OrderHeader_ID
    JOIN Vendor V on B.ReceiveLocation_ID = V.Vendor_ID
	JOIN Store C on V.Store_no = C.Store_No
    WHERE A.OrderType_ID <> 3
      AND (@dateFrom IS NULL OR A.InsertDate >= @dateFrom) 
      AND (@dateTo IS NULL OR A.InsertDate <= @dateTo)
      AND C.BusinessUnit_ID = ISNULL(@StoreBU, C.BusinessUnit_ID)
    RETURN;
  END
  
  
  IF(@eventCode = @tsf_line_del) --Transfer Line Item Deletion
  BEGIN
    SELECT TOP(@maxRecords) B.OrderHeader_ID ID, A.DeletedOrderItem_ID LineItem_ID, C.BusinessUnit_ID Store_BU, C.Store_Name, A.InsertDate [Delete_Date], A.Item_Key
    FROM amz.DeletedOrderItem A
    JOIN OrderHeader B on A.OrderHeader_ID = B.OrderHeader_ID
    JOIN Vendor V on B.ReceiveLocation_ID = V.Vendor_ID
	JOIN Store C on V.Store_no = C.Store_No
    WHERE A.OrderType_ID = 3
      AND (@dateFrom IS NULL OR A.InsertDate >= @dateFrom) 
      AND (@dateTo IS NULL OR A.InsertDate <= @dateTo)
      AND C.BusinessUnit_ID = ISNULL(@StoreBU, C.BusinessUnit_ID)
    RETURN;
  END
  
  
  IF(@eventCode = @po_del) --Purchase Order Deletion
  BEGIN
    SELECT TOP(@maxRecords) A.DeletedOrder_ID ID,  A.DeleteDate
    FROM dbo.DeletedOrder A
	JOIN Vendor V on A.ReceiveLocation_ID = V.Vendor_ID
	JOIN Store C on V.Store_no = C.Store_No
    WHERE A.Sent = 1 
      AND A.OrderType_ID <> 3
      AND (@dateFrom IS NULL OR A.DeleteDate >= @dateFrom) 
      AND (@dateTo IS NULL OR A.DeleteDate <= @dateTo)
      AND C.BusinessUnit_ID = ISNULL(@StoreBU, C.BusinessUnit_ID)
    RETURN;
  END
  
  
  IF(@eventCode = @tsf_del) --Transfer Order Deletion
  BEGIN
    SELECT TOP(@maxRecords) DeletedOrder_ID ID, DeleteDate
    FROM dbo.DeletedOrder A
	JOIN Vendor V on A.ReceiveLocation_ID = V.Vendor_ID
	JOIN Store C on V.Store_no = C.Store_No
    WHERE [Sent] = 1 
      AND OrderType_ID = 3
      AND (@dateFrom IS NULL OR DeleteDate >= @dateFrom) 
      AND (@dateTo IS NULL OR DeleteDate <= @dateTo)
      AND C.BusinessUnit_ID = ISNULL(@StoreBU, C.BusinessUnit_ID)
    RETURN;
  END
  
  SET @msg = 'Unsupported event code (' + @eventCode + ').';
  RAISERROR(@msg, 16, 1);
END
GO
GRANT EXECUTE
    ON OBJECT::[amz].[GetInStockEventsByType] TO [MammothRole]
    AS [dbo];
