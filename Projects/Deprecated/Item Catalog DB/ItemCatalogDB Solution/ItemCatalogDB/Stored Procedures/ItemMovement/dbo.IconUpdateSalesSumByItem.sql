CREATE PROCEDURE dbo.IconUpdateSalesSumByItem
(
@ItemMovement	dbo.ItemMovementType READONLY
)
AS

DECLARE @totalItemMovementCount int;
DECLARE @insertItemMovementCount int;

-- put all ItemMovement data from staging to temp table
SELECT
    a.TransDate,   
    s.Store_No,   
    ii.Item_Key,   
    i.SubTeam_No,
	a.ItemType,
	a.Quantity,
	a.Weight,
	a.ItemVoid,
	a.BasePrice,
	a.MarkDownAmount,
	pst.POSSystemType 
   INTO #allItemMovement
   FROM   
    @ItemMovement a INNER JOIN Store s WITH (NOLOCK)
    ON a.BusinessUnitId = s.BusinessUnit_ID
    INNER JOIN POSSystemTypes pst WITH (NOLOCK)
    ON s.POSSystemId = pst.POSSystemId
	INNER JOIN ItemIdentifier ii WITH (NOLOCK)
	ON a.Identifier = ii.Identifier AND ii.Deleted_Identifier = 0 AND ii.Remove_Identifier = 0
	INNER JOIN Item i WITH (NOLOCK)
	ON i.Item_Key = ii.Item_Key

CREATE NONCLUSTERED INDEX IX_Store_ItemKey_TransDate_SubTeam_allItemMovement on #allItemMovement (Store_No, Item_Key, TransDate, SubTeam_No)
	   INCLUDE (ItemType, Quantity, Weight, ItemVoid, BasePrice, MarkDownAmount, POSSystemType)

SELECT
    TransDate,   
    Store_No,   
    Item_Key,   
    SubTeam_No,  
    SUM(CASE WHEN ItemType IN (0)   
        THEN Quantity * (  
         CASE  WHEN ItemVoid = 1   
         THEN -1  
         ELSE 1 END)  
        ELSE 0 END) AS Sales_Quantity,   
    SUM(CASE WHEN ItemType IN (2,3,8)  
        THEN Quantity * -1 * (  
         CASE WHEN ItemVoid = 1   
           THEN -1  
           ELSE 1 END)  
         ELSE 0 END) AS Return_Quantity,   
    SUM (CASE WHEN ItemType IN (0)   
         THEN Weight * (  
          CASE WHEN ItemVoid = 1   
            THEN -1  
            ELSE 1 END)   
         ELSE 0 END) AS Weight,   
    SUM(CASE WHEN ItemType IN (0)   
        THEN (BasePrice * Quantity) + (BasePrice * Weight)  
        ELSE 0 END) AS Sales_Amount,  
    SUM(CASE WHEN ItemType IN (2,3,8)   
        THEN (BasePrice * -1 * Quantity) + (BasePrice * -1 * Weight)  
        ELSE 0 END) AS Return_Amount,   
    ISNULL(SUM(CASE WHEN POSSystemType <> 'NCR' THEN MarkDownAmount ELSE 0 END), 0) AS Markdown_Amount       
 INTO #allSumItemMovement
FROM   
	#allItemMovement a
GROUP BY      
	Store_No,   
	Item_Key, 
	TransDate,  
	SubTeam_No 


-- keep track of total Summarized ItemMovement count to determine if updates are needed
SET @totalItemMovementCount = @@ROWCOUNT

CREATE NONCLUSTERED INDEX IX_Store_ItemKey_TransDate_SubTeam_allSumItemMovement on #allSumItemMovement (Store_No, Item_Key, TransDate, SubTeam_No)
	   INCLUDE (Sales_Quantity, Return_Quantity, Weight, Sales_Amount, Return_Amount, Markdown_Amount)

-- put all new ItemMovement into its own temp table for the inserts
	SELECT
		TransDate,   
		Store_No,   
		Item_Key,   
		SubTeam_No,
		1 AS Price_Level,   
		Sales_Quantity,   
		Return_Quantity,   
		Weight,   
		Sales_Amount,  
		Return_Amount,   
		Markdown_Amount,   
		0 AS Promotion_Amount,   
        0 AS Store_Coupon_Amount    
	INTO #insertItemMovement
	FROM   
		#allSumItemMovement a
	WHERE NOT EXISTS
	(
		SELECT 1
		FROM Sales_SumByitem s WITH (NOLOCK)
		WHERE a.Store_No = s.Store_No
		AND a.Item_Key = s.Item_Key
		AND a.TransDate = s.Date_Key
		AND a.SubTeam_No = s.SubTeam_No
	)

SET @insertItemMovementCount = @@ROWCOUNT

CREATE NONCLUSTERED INDEX IX_Store_ItemKey_TransDate_SubTeam_insertItemMovement on #insertItemMovement (Store_No, Item_Key, TransDate, SubTeam_No)
	   INCLUDE (Price_Level, Sales_Quantity, Return_Quantity, Weight, Sales_Amount, Return_Amount, Markdown_Amount, Promotion_Amount, Store_Coupon_Amount)

BEGIN TRY
	BEGIN TRANSACTION itemMovementUpdate
	
		IF @insertItemMovementCount <> @totalItemMovementCount
			UPDATE sbi
			SET sbi.Sales_Quantity = sbi.Sales_Quantity + im.Sales_Quantity,
				sbi.Return_Quantity = sbi.Return_Quantity + im.Return_Quantity,
				sbi.Weight = sbi.Weight + im.Weight,
				sbi.Sales_Amount = sbi.Sales_Amount + im.Sales_Amount,
				sbi.Return_Amount = sbi.Return_Amount + im.Return_Amount,
				sbi.Markdown_Amount = sbi.Markdown_Amount + im.Markdown_Amount
			FROM Sales_SumByItem sbi
			JOIN #allSumItemMovement im ON sbi.Store_No = im.Store_No
										AND sbi.Item_Key = im.Item_Key
										AND sbi.Date_Key = im.TransDate
										AND sbi.SubTeam_No = im.SubTeam_No

		IF @insertItemMovementCount > 0
		INSERT INTO Sales_SumByitem (
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
		SELECT
			TransDate,
			Store_No,
			Item_Key,
			SubTeam_No,
			1,
			Sales_Quantity,
			Return_Quantity,
			Weight,
			Sales_Amount,
			Return_Amount,
			Markdown_Amount,
			0,
			0
		FROM #insertItemMovement 

	COMMIT TRANSACTION itemMovementUpdate
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION itemMovementUpdate;
	
	DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

	RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );

END CATCH

GO