-- =============================================
-- Author:		Alex Babichev
-- Create date: 8/30/2012
-- Description:	daily Process that Moves data from ItemHistoryQueue to ItemHistory
-- Update History

--TFS #		Date		Who			Comments

-- =============================================
/*

*/

CREATE PROCEDURE [dbo].[Moveotherdatatohist]
AS
  BEGIN
      SET nocount ON

      DECLARE @maxItemHistoryId INT

      SELECT @maxItemHistoryId = Max(itemhistoryid)
      FROM   itemhistoryqueue

      BEGIN try
          BEGIN TRAN

          INSERT INTO itemhistory
                      ([store_no],
                       [item_key],
                       [datestamp],
                       [quantity],
                       [weight],
                       [cost],
                       [extcost],
                       [retail],
                       [adjustment_id],
                       [adjustmentreason],
                       [createdby],
                       [subteam_no],
                       [insert_date],
                       [orderitem_id],
                       [inventoryadjustmentcode_id],
                       [correctionrecordflag])
          SELECT [store_no],
                 [item_key],
                 [datestamp],
                 [quantity],
                 [weight],
                 [cost],
                 [extcost],
                 [retail],
                 [adjustment_id],
                 [adjustmentreason],
                 [createdby],
                 [subteam_no],
                 [insert_date],
                 [orderitem_id],
                 [inventoryadjustmentcode_id],
                 [correctionrecordflag]
          FROM   itemhistoryqueue
          WHERE  itemhistoryid <= @maxItemHistoryId

          DELETE itemhistoryqueue
          WHERE  itemhistoryid <= @maxItemHistoryId

          COMMIT
      END try

      BEGIN catch
          ROLLBACK
      END catch
  END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Moveotherdatatohist] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Moveotherdatatohist] TO [IRMASchedJobsRole]
    AS [dbo];

