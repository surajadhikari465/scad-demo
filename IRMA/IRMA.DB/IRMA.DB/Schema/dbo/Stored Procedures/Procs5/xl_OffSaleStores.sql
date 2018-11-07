CREATE PROCEDURE dbo.xl_OffSaleStores
    @Store_No INT,
    @Subteam_No INT,
    @TargetDate smalldatetime
AS 
BEGIN
    SET NOCOUNT ON

    SELECT distinct Price.Store_No as StoreNo, Store.Store_Name as StoreName
    FROM price (nolock)
        INNER JOIN
            Item            
            ON item.Item_key = Price.Item_key
        inner join 
            store
            on Store.Store_no = Price.Store_no
    WHERE dbo.fn_OnSale(PriceChgTypeId) = 1 and Sale_end_date = @TargetDate
          and Item.SubTeam_No = isnull(@SubTeam_No, Item.SubTeam_No)
          and Price.Store_No = isnull(@Store_no, Price.Store_No)



    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xl_OffSaleStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xl_OffSaleStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xl_OffSaleStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[xl_OffSaleStores] TO [IRMAReportsRole]
    AS [dbo];

