CREATE PROCEDURE dbo.[UpdatePriceBatchDetailPromoPlanner]
		@strtDt smalldatetime,
        @endDt smalldatetime,
		@deptno int
AS

BEGIN
    SET NOCOUNT ON

    
    BEGIN
        INSERT INTO PriceBatchDetail (Item_Key, Store_No, PriceChgTypeID, StartDate, Multiple, Price, Sale_Multiple, Sale_Price, Sale_End_Date, POSSale_Price, POSPrice, PricingMethod_ID, Sale_Earned_Disc1,Sale_Earned_Disc2,Sale_Earned_Disc3,Case_Price,InsertApplication)
        SELECT i.Item_Key, p.Store_No, p.PriceChgTypeID, p.Start_Date, p.Multiple, p.Price,  p.Sale_Multiple, p.Sale_Price, p.Sale_End_Date, p.Sale_Price, p.Price, '0','0','0','99','0','Promo Planner'
		FROM PriceBatchPromo p, Item i where p.Item_Key = i.Item_Key and p.Start_Date = @strtDt and p.Sale_End_Date = @endDt and i.SubTeam_No = @deptno and
		i.item_key not in (select Item_Key from PriceBatchDetail where StartDate = @strtDt and Sale_End_Date = @endDt);
 
    -- The equivalent of an "off-sale record" will be created when the initial record is batched.  
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePriceBatchDetailPromoPlanner] TO [IRMAPromoRole]
    AS [dbo];

