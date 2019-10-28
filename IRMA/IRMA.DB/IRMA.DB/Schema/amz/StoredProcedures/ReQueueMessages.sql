--Used by Mammoth/WebSupport
CREATE PROCEDURE amz.ReQueueMessages
  @eventCode NVARCHAR(25),
  @IDs dbo.IntType READONLY
AS
BEGIN
  SET NOCOUNT ON
  
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
         
  IF(Not Exists(select 1 from @IDs)) RETURN;
  
   IF(object_id('tempdb..#IDs') is not null) DROP TABLE #IDs;
  SELECT DISTINCT [Key] AS ID INTO #IDs FROM @IDs;
  
  IF(@eventCode = @inv_adj) --Inventory Adjustment
  BEGIN
    INSERT INTO amz.InventoryQueue(EventTypeCode, MessageType, KeyID)
      SELECT @eventCode, 'InventoryAdjustment', ID AS ItemHistoryID
      FROM #IDs;
  
    SELECT @@ROWCOUNT;
    RETURN;
  END
  
  
  IF(@eventCode = @po_cre)
  BEGIN
    INSERT INTO amz.OrderQueue(EventTypeCode, MessageType, KeyID)
      SELECT @eventCode, 'PurchaseOrder', ID AS OrderHeader_ID
      FROM #IDs;
  
    SELECT @@ROWCOUNT;
    RETURN;
  END


  IF(@eventCode = @po_mod)
  BEGIN
    INSERT INTO amz.OrderQueue(EventTypeCode, MessageType, KeyID)
      SELECT @eventCode, 'PurchaseOrderModification', ID AS OrderHeader_ID
      FROM #IDs;
  
    SELECT @@ROWCOUNT;
    RETURN;
  END
  
  
  IF(@eventCode = @tsf_cre)
  BEGIN
    INSERT INTO amz.TransferQueue(EventTypeCode, MessageType, KeyID)
      SELECT @eventCode, 'TransferOrder', ID AS OrderHeader_ID
      FROM #IDs;
  
    SELECT @@ROWCOUNT;
    RETURN;
  END
  
  
  IF(@eventCode = @rcpt_cre) --Order Receipt Creation
  BEGIN
    INSERT INTO amz.ReceiptQueue(EventTypeCode, MessageType, KeyID)
      SELECT @eventCode, 'ReceiptMessage', ID AS OrderHeader_ID
      FROM #IDs 
  
    SELECT @@ROWCOUNT;
    RETURN;
  END
  
  
  IF(@eventCode = @po_line_del) --Purchase Order Line Item Deletion
  BEGIN
    INSERT INTO amz.OrderQueue (EventTypeCode, MessageType, KeyID, SecondaryKeyID)
  		SELECT @eventCode,'PurchaseLineDelete', A.OrderHeader_ID, A.OrderItem_ID
  		FROM amz.DeletedOrderItem A
  		INNER JOIN #IDs B ON B.ID = A.DeletedOrderItem_ID
      WHERE A.OrderType_ID <> 3;
  
    SELECT @@ROWCOUNT;
    RETURN;
  END
  
  
  IF(@eventCode = @tsf_line_del) ----Transfer Line Item Deletion
  BEGIN
    INSERT INTO amz.TransferQueue(EventTypeCode, MessageType, KeyID, SecondaryKeyID)
  		SELECT @eventCode, 'TransferLineDelete', A.OrderHeader_ID, A.OrderItem_ID
  		FROM amz.DeletedOrderItem A
  		INNER JOIN #IDs B ON B.ID = A.DeletedOrderItem_ID
  		WHERE A.OrderType_ID = 3
      ORDER BY A.DeletedOrderItem_ID;
  
    SELECT @@ROWCOUNT;
    RETURN;
  END
  
  
  IF(@eventCode = @po_del) --Purchase Order Deletion
  BEGIN
    INSERT INTO amz.OrderQueue(EventTypeCode, MessageType, KeyID)
      SELECT @eventCode, 'PurchaseOrderDelete', ID AS OrderHeader_ID
      FROM #IDs;
  
    SELECT @@ROWCOUNT;
    RETURN;
  END
  
  
  IF(@eventCode = @tsf_del) --Transfer Order Deletion
  BEGIN
    INSERT INTO amz.TransferQueue(EventTypeCode, MessageType, KeyID)
      SELECT @eventCode, 'TransferOrderDelete', ID AS OrderHeader_ID
      FROM #IDs;
  
    SELECT @@ROWCOUNT;
    RETURN;
  END

  SET @msg = 'Unsupported event code (' + @eventCode + ').';
  RAISERROR(@msg, 16, 1);
  RETURN
END
GO
GRANT EXECUTE
    ON OBJECT::[amz].[ReQueueMessages] TO [MammothRole]
    AS [dbo];
