CREATE PROCEDURE dbo.GetWarehouse
    @DistributionCenter int,
    @EXEWarehouse smallint
AS
   -- **************************************************************************
   -- Procedure: GetWarehouse
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 11/06/2009  BBB	update existing SP to specifically declare table source 
   --					for BusinessUnit_ID column to prevent ambiguity between
   --					Store and Vendor table
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON

    SELECT Store.Store_No, Store_Name, Store.BusinessUnit_ID, Vendor_ID
    FROM Store
    INNER JOIN
        Vendor ON Vendor.Store_No = Store.Store_No
    WHERE --EXEWarehouse = @EXEWarehouse AND
    ISNULL(Store.BusinessUnit_ID, 0) % 100 = @DistributionCenter
    AND store.Distribution_Center = 1

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouse] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouse] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouse] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouse] TO [IRMAReportsRole]
    AS [dbo];

