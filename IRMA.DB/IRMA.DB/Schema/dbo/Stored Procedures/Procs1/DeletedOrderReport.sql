/****************************************************************************************************/
-- 09/30/2011 td:			TFS 3084...the report wasn't returning results when a specific PO Number was 
--							provided.  i adjusted the where clause to accept null orderheader_ids
/****************************************************************************************************/

CREATE PROCEDURE [dbo].[DeletedOrderReport]
	@Store_No int,
	@OrderHeader_ID int,
	@StartDate varchar(12), 
	@EndDate varchar(12),
	@SearchBy int
AS

BEGIN
    SET nocount ON

     SELECT [orderheader_id] = do.orderheader_id,
           [deletedate] = do.deletedate,
           [deleted_by] = deletedby.FullName,
           [created_by] = createdby.FullName,
           [Store_Name] = s.store_name,
           [OrderDate] = do.OrderDate,
           [ReasonCodeDesc] = rcd.ReasonCodeDesc,
           [CompanyName] = vendor.CompanyName,
           [OrderedCost] = do.OrderedCost,
           [OrderHeaderDesc] =do.OrderHeaderDesc       
    FROM   deletedorder do
           INNER JOIN users deletedby
             ON deletedby.user_id = do.user_id
           INNER JOIN users createdby
             ON createdby.user_id = do.createdby
           INNER JOIN vendor receivelocation
             ON receivelocation.vendor_id = do.receivelocation_id
             INNER JOIN vendor 
             ON vendor.vendor_id = do.Vendor_ID
           INNER JOIN store s
             ON s.store_no = receivelocation.store_no
           LEFT OUTER JOIN	ReasonCodeDetail	(nolock)  rcd		ON	do.DeletedReason = rcd.ReasonCodeDetailID         
    WHERE  ( do.orderheader_id = @OrderHeader_ID )-- disregard other parameters
            OR ( ( @OrderHeader_ID IS NULL )
                 AND ( receivelocation.vendor_id = @Store_No )
                 AND do.deletedate >= ( CASE
                                          WHEN @searchBy = 0 THEN Isnull(@StartDate, do.deletedate)
                                          ELSE do.deletedate
                                        END )
                 AND do.deletedate <= ( CASE
                                          WHEN @SearchBy = 0 THEN Dateadd(d, 1, Isnull(@EndDate, do.deletedate))
                                          ELSE do.deletedate
                                        END )
                 AND do.orderdate >= ( CASE
                                         WHEN @searchBy = 1 THEN Isnull(@StartDate, do.orderdate)
                                         ELSE do.orderdate
                                       END )
                 AND do.orderdate <= ( CASE
                                         WHEN @searchBy = 1 THEN Dateadd(d, 1, Isnull(@EndDate, do.orderdate))
                                         ELSE do.orderdate
                                       END ) )

    SET nocount OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletedOrderReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletedOrderReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletedOrderReport] TO [IRMAReportsRole]
    AS [dbo];

