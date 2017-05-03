CREATE PROCEDURE dbo.Replenishment_Tlog_House_UpdateSalesSumByItem
(
@ItemMovement	dbo.ItemMovementType READONLY
)
AS

MERGE Sales_SumByitem AS sbi
USING -- using im
(
SELECT
    a.TransDate,   
    s.Store_No,   
    ii.Item_Key,   
    i.SubTeam_No,
    1 AS Price_Level,   
    SUM(CASE WHEN a.ItemType IN (0)   
        THEN a.Quantity * (  
         CASE  WHEN a.ItemVoid = 1   
         THEN -1  
         ELSE 1 END)  
        ELSE 0 END) AS Sales_Quantity,   
    SUM(CASE WHEN a.ItemType IN (2,3,8)  
        THEN a.Quantity * -1 * (  
         CASE WHEN a.ItemVoid = 1   
           THEN -1  
           ELSE 1 END)  
         ELSE 0 END) AS Return_Quantity,   
    SUM (CASE WHEN a.ItemType IN (0)   
         THEN a.Weight * (  
          CASE WHEN a.ItemVoid = 1   
            THEN -1  
            ELSE 1 END)   
         ELSE 0 END) AS Weight,   
    SUM(CASE WHEN a.ItemType IN (0)   
        THEN a.BasePrice * a.Quantity  
        ELSE 0 END) AS Sales_Amount,  
    SUM(CASE WHEN a.ItemType IN (2,3,8)   
        THEN a.BasePrice * a.Quantity  
        ELSE 0 END) AS Return_Amount,   
    ISNULL(SUM(CASE WHEN pst.POSSystemType <> 'NCR' THEN a.MarkDownAmount ELSE 0 END), 0) AS Markdown_Amount,   
    0 AS Promotion_Amount,   
    0 AS Store_Coupon_Amount   
   FROM   
    @ItemMovement a INNER JOIN Store s (NOLOCK)
    ON a.BusinessUnitId = s.BusinessUnit_ID
    INNER JOIN POSSystemTypes pst (NOLOCK)
    ON s.POSSystemId = pst.POSSystemId
	INNER JOIN dbo.fn_GetItemIdentifiers() ii
	ON a.Identifier = ii.Identifier
	INNER JOIN Item i (NOLOCK)
	ON i.Item_Key = ii.Item_Key
   GROUP BY   
    a.TransDate,   
    s.Store_No,   
    ii.Item_Key,   
    i.SubTeam_No 
) im
ON sbi.Date_Key = im.TransDate AND
sbi.Store_No = im.Store_No AND
sbi.Item_Key = im.Item_Key AND
sbi.SubTeam_No = im.SubTeam_No
WHEN MATCHED THEN
	UPDATE
		SET sbi.Sales_Quantity = sbi.Sales_Quantity + im.Sales_Quantity,
			sbi.Return_Quantity = sbi.Return_Quantity + im.Return_Quantity,
			sbi.Weight = sbi.Weight + im.Weight,
			sbi.Sales_Amount = sbi.Sales_Amount + im.Sales_Amount,
			sbi.Return_Amount = sbi.Return_Amount + im.Return_Amount,
			sbi.Markdown_Amount = sbi.Markdown_Amount + im.Markdown_Amount
WHEN NOT MATCHED THEN 
	INSERT (
		Date_Key,
		Store_No,
		Item_Key,
		SubTeam_No,
		Price_Level,
		Sales_Quantity,
		Return_Quantity,
		Weight,
		Sales_Amount,
		Return_Amount,
		Markdown_Amount,
		Promotion_Amount,
		Store_Coupon_Amount
		)
	VALUES (
		im.TransDate,
		im.Store_No,
		im.Item_Key,
		im.SubTeam_No,
		1,
		im.Sales_Quantity,
		im.Return_Quantity,
		im.Weight,
		im.Sales_Amount,
		im.Return_Amount,
		im.Markdown_Amount,
		0,
		0);