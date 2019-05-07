--Used by Mammoth/WebSupport
CREATE PROCEDURE amz.GetInStockEventsByType
  @eventCode NVARCHAR(25),
  @dateFrom DATE,
  @dateTo DATE
AS
BEGIN
  SET NOCOUNT ON;
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
  
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
    SELECT A.ItemHistoryID ID, (Convert(varchar, A.DateStamp, 10) + ' ' + Convert(varchar, A.DateStamp, 8)) [Date],
      A.Item_Key, A.Store_No Store, A.Weight, A.AdjustmentReason Reason
      FROM ItemHistory A
      JOIN Vendor B ON B.Store_no = A.Store_No
      WHERE Adjustment_ID = 1 AND Cast(DateStamp AS DATE) BETWEEN @dateFrom AND @dateTo
        AND B.Vendor_ID IN(SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('AmazonInStockEnabledStoreVendorId', 'IRMA CLIENT'), '|'))
      ORDER BY ItemHistoryID;
  
     RETURN;
  END
  
  
  IF(@eventCode = @po_cre) --Purchase Order Creation
  BEGIN
    SELECT OrderHeader_ID ID, (Convert(varchar, IsNull(SentDate, OrderDate), 10) + ' ' + Convert(varchar, IsNull(SentDate, OrderDate), 8)) [Date],
      OrderHeaderDesc Description
    FROM OrderHeader
    WHERE [Sent] = 1 AND OrderType_ID <> 3
        AND Cast(IsNull(SentDate, OrderDate) AS DATE) BETWEEN @dateFrom AND @dateTo
        AND ReceiveLocation_ID IN(SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('AmazonInStockEnabledStoreVendorId', 'IRMA CLIENT'), '|'));
  
    RETURN;
  END
  
  
  IF(@eventCode = @po_mod) --Purchase Order Modification
  BEGIN
    SELECT A.OrderHeader_ID ID, (Convert(varchar, IsNull(IsNull(A.OriginalCloseDate, A.CloseDate), B.InsertDate), 10) + ' ' +
      Convert(varchar, IsNull(IsNull(A.OriginalCloseDate, A.CloseDate), B.InsertDate), 8)) [Date],
      A.OrderHeaderDesc Description
      FROM OrderHeader A
      LEFT JOIN infor.OrderExpectedDateChangeQueue B ON B.OrderHeader_ID = A.OrderHeader_ID
      WHERE A.Sent = 1 AND A.OrderType_ID <> 3
        AND Cast(IsNull(IsNull(A.OriginalCloseDate, A.CloseDate), B.InsertDate) AS DATE) BETWEEN @dateFrom AND @dateTo
        AND A.ReceiveLocation_ID IN(SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('AmazonInStockEnabledStoreVendorId', 'IRMA CLIENT'), '|'))
      ORDER BY A.OrderHeader_ID;
  
    RETURN;
  END
  
  
  IF(@eventCode = @tsf_cre) --Transfer Order Creation
  BEGIN
    SELECT OrderHeader_ID ID, (Convert(varchar, IsNull(SentDate, OrderDate), 10) + ' ' + Convert(varchar, IsNull(SentDate, OrderDate), 8)) [Date], OrderHeaderDesc Description
      FROM OrderHeader 
      WHERE [Sent] = 1 AND OrderType_ID = 3
        AND Cast(IsNull(SentDate, OrderDate) AS DATE) BETWEEN @dateFrom AND @dateTo
        AND ReceiveLocation_ID IN(SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('AmazonInStockEnabledStoreVendorId', 'IRMA CLIENT'), '|'));
  
    RETURN;
  END
  
  
  IF(@eventCode = @rcpt_cre) --Order Receipt Creation
  BEGIN
    SELECT A.OrderHeader_ID ID, Max((Convert(varchar, B.DateReceived, 10) + ' ' + Convert(varchar, B.DateReceived, 8))) [Date],
      A.OrderHeaderDesc Description, Count(*) [Order Items]
      FROM OrderHeader A
      JOIN OrderItem B ON B.OrderHeader_ID = A.OrderHeader_ID
      WHERE A.Sent = 1 AND IsNull(B.QuantityReceived, 0) > 0 
        AND Cast(B.DateReceived AS DATE) BETWEEN @dateFrom AND @dateTo
        AND A.ReceiveLocation_ID IN(SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('AmazonInStockEnabledStoreVendorId', 'IRMA CLIENT'), '|'))
      GROUP BY A.OrderHeader_ID, Cast(B.DateReceived AS DATE), A.OrderHeaderDesc
      ORDER BY A.OrderHeader_ID DESC;
  
    RETURN;
  END
  
  
  IF(@eventCode = @po_line_del) --Purchase Order Line Item Deletion
  BEGIN
    SELECT A.DeletedOrderItem_ID ID, (Convert(varchar, A.InsertDate, 10) + ' ' + Convert(varchar, A.InsertDate, 8)) [Date],
      A.Item_Key, B.Item_Description Item
    FROM amz.DeletedOrderItem A
    JOIN Item B ON B.Item_Key = A.Item_Key
    WHERE A.OrderType_ID <> 3
      AND Cast(A.InsertDate AS DATE) BETWEEN @dateFrom AND @dateTo;
  
    RETURN;
  END
  
  
  IF(@eventCode = @tsf_line_del) --Transfer Line Item Deletion
  BEGIN
    SELECT A.DeletedOrderItem_ID ID, (Convert(varchar, A.InsertDate, 10) + ' ' + Convert(varchar, A.InsertDate, 8)) [Date],
      A.Item_Key, B.Item_Description Item
    FROM amz.DeletedOrderItem A
    JOIN Item B ON B.Item_Key = A.Item_Key
    WHERE A.OrderType_ID = 3
      AND Cast(A.InsertDate AS DATE) BETWEEN @dateFrom AND @dateTo;
  
    RETURN;
  END
  
  
  IF(@eventCode = @po_del) --Purchase Order Deletion
  BEGIN
    SELECT A.DeletedOrder_ID ID, (Convert(varchar, A.DeleteDate, 10) + ' ' + Convert(varchar, A.DeleteDate, 8)) [Date], Left(A.OrderHeaderDesc, 255) [Description]
    FROM dbo.DeletedOrder A
    WHERE A.Sent = 1 AND A.OrderType_ID <> 3
      AND Cast(A.DeleteDate AS DATE) BETWEEN @dateFrom AND @dateTo
      AND A.ReceiveLocation_ID IN(SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('AmazonInStockEnabledStoreVendorId', 'IRMA CLIENT'), '|'));
  
    RETURN;
  END
  
  
  IF(@eventCode = @tsf_del) --Transfer Order Deletion
  BEGIN
    SELECT DeletedOrder_ID ID, (Convert(varchar, DeleteDate, 10) + ' ' + Convert(varchar, DeleteDate, 8)) [Date], Left(OrderHeaderDesc, 255) [Description]
    FROM dbo.DeletedOrder
    WHERE [Sent] = 1 AND OrderType_ID = 3
      AND Cast(DeleteDate AS DATE) BETWEEN @dateFrom AND @dateTo
      AND ReceiveLocation_ID IN(SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('AmazonInStockEnabledStoreVendorId', 'IRMA CLIENT'), '|'));
  
    RETURN;
  END
  
  SET @msg = 'Unsupported event code (' + @eventCode + ').';
  RAISERROR(@msg, 16, 1);
END
GO

GRANT EXECUTE ON OBJECT::amz.GetInStockEventsByType to MammothRole
GO