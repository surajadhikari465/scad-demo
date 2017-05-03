
CREATE  PROCEDURE [dbo].[LoadInventoryServiceExport]
AS 
BEGIN
    SET NOCOUNT ON
    
    DECLARE @Period int, @Year int, @Date smalldatetime, @Pound int, @Unit int, @Case int
    
    SET @Date = CONVERT(smalldatetime, CONVERT(varchar(255), GETDATE(), 101))

    SELECT @Period = Period, @Year = [Year]
    FROM [Date] D (nolock)
    WHERE Date_Key = @Date
    
    SELECT @Pound = Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'LB'
    SELECT @Unit = Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'UN'
    SELECT @Case = Unit_ID FROM ItemUnit (nolock) WHERE Unit_Name = 'Case' 
    
	-- Clear the load table
	DELETE FROM InventoryServiceExportLoad

    -- Get the cost per item so we don't repeat this mess for each item identifier
    CREATE TABLE #ItemCost
        (
	    Store_No int,
	    Item_Key int,
	    Cost money PRIMARY KEY (Store_No, Item_Key)
	    )
	
	INSERT INTO #ItemCost (Store_No, Item_Key, Cost)
	SELECT Price.Store_No, Price.Item_Key, 
	    CASE WHEN ST.InventoryCountByCase = 1
	             -- Case cost for Produce only.  They count all items by the case.
	             THEN dbo.fn_CostConversion(dbo.fn_AvgCostHistory(I.Item_Key, Price.Store_No, I.SubTeam_No, @Date), CASE WHEN I.CostedByWeight = 1 THEN @Pound ELSE @Unit END, @Case, I.Package_Desc1, I.Package_Desc2, I.Package_Unit_ID)
	             ELSE dbo.fn_AvgCostHistory(I.Item_Key, Price.Store_No, I.SubTeam_No, @Date) END
    FROM
        Item I (nolock)
    INNER JOIN
        Price (nolock)
        ON (Price.Item_Key = I.Item_Key)
    INNER JOIN
        SubTeam ST (nolock)
        ON (I.SubTeam_No = ST.SubTeam_No)
    INNER JOIN
        StoreSubTeam (nolock)
        ON StoreSubTeam.Store_No = Price.Store_No AND StoreSubTeam.SubTeam_No = I.SubTeam_No
    WHERE (Shipper_Item = 0 AND Deleted_Item = 0)
        AND ST.SubTeamType_ID IN (1, 2, 3)
        AND StoreSubTeam.ICVID IS NOT NULL
    
    -- Get ingredients items that are either not in the previous select or have zero cost     
    CREATE TABLE #AddMissingIngredients (Store_No int, Item_Key int, Cost money)
        
    INSERT INTO #AddMissingIngredients (Store_No, Item_Key)
    SELECT Price.Store_No, I.Item_Key
    FROM
        Item I (nolock)
    INNER JOIN
        Price (nolock)
        ON (Price.Item_Key = I.Item_Key)
    INNER JOIN
        SubTeam ST (nolock)
        ON (I.SubTeam_No = ST.SubTeam_No)
    INNER JOIN
        StoreSubTeam (nolock)
        ON StoreSubTeam.Store_No = Price.Store_No AND StoreSubTeam.SubTeam_No = I.SubTeam_No
    WHERE (Shipper_Item = 0 AND Deleted_Item = 0)
        AND ST.SubTeamType_ID IN (1, 2, 3)
        AND StoreSubTeam.ICVID IS NOT NULL
        AND NOT EXISTS (SELECT * FROM #ItemCost IC WHERE IC.Store_No = Price.Store_No AND IC.Item_Key = I.Item_Key AND ISNULL(IC.Cost, 0) > 0)
    
   -- Set the cost to the first average cost we find for this store - when PO's for ingredients are received, average costs get created directly from the PO in the OrderItem table trigger code   
    UPDATE A
    SET Cost = (SELECT TOP 1 AvgCost
                      FROM AvgCostHistory (nolock)
                      WHERE Item_Key = A.Item_Key
                        AND Store_No = A.Store_No
                        AND Effective_Date <= @Date
                      ORDER BY Effective_Date DESC)
	FROM #AddMissingIngredients A
    
   -- Make sure we actually found a cost for these items - if not, they are no good     
    DELETE #AddMissingIngredients WHERE ISNULL(Cost, 0) = 0

    -- Get rid of lines that we will be inserting next
    DELETE IC
    FROM #ItemCost IC
    INNER JOIN #AddMissingIngredients M ON M.Store_No = IC.Store_No AND M.Item_Key = IC.Item_Key


    INSERT INTO #ItemCost (Store_No, Item_Key, Cost)
	SELECT M.Store_No, M.Item_Key, 
	    CASE WHEN ST.InventoryCountByCase = 1
	             -- Case cost for Produce only.  They count all items by the case.
	             THEN dbo.fn_CostConversion(M.Cost, CASE WHEN I.CostedByWeight = 1 THEN @Pound ELSE @Unit END, @Case, I.Package_Desc1, I.Package_Desc2, I.Package_Unit_ID)
	             ELSE M.Cost END
    FROM
        #AddMissingIngredients M
    INNER JOIN Item I (nolock) ON I.Item_Key = M.Item_Key
	INNER JOIN SubTeam ST (nolock) ON ST.SubTeam_No = I.SubTeam_No
       
     
    INSERT INTO InventoryServiceExportLoad
    SELECT 
        (SELECT RegionCode FROM Region) AS Region,
        STORE_NAME,
        S.BUSINESSUNIT_ID,
        ISNULL(StoreSubTeam.PS_SubTeam_No, I.SubTeam_No),/*SH 3/25/2015 - changed from I.SUBTEAM_NO*/
        ST.SUBTEAM_NAME,
	    Identifier,
        I.ITEM_DESCRIPTION,
	    CASE WHEN ST.InventoryCountByCase = 1
                 -- Case cost for Produce only.  They count all items by the case.
                 THEN dbo.fn_CostConversion(ISNULL(CASE WHEN Multiple > 0 THEN Price / Multiple ELSE Price END, 0), CASE WHEN I.CostedByWeight = 1 THEN @Pound ELSE @Unit END, @Case, I.Package_Desc1, I.Package_Desc2, I.Package_Unit_ID)
                 ELSE ISNULL(CASE WHEN Multiple > 0 THEN Price / Multiple ELSE Price END, 0) END,
        ISNULL(Cost, 0) As AvgCost, 
        I.ITEM_KEY, 
	    StoreSubTeam.ICVID, 
	    @Period,
	    @Year,
	    GETDATE(),
	    SIV.Vendor_ID,
	    I.Package_Desc1,
	    CASE WHEN ST.InventoryCountByCase = 1 THEN 'LB' ELSE RU.EDISysCode END -- If produce, always send retail unit as pounds to allow them to count partial cases
    FROM
        Item I                         (nolock)
		INNER JOIN Price               (nolock) ON (Price.Item_Key = I.Item_Key)
		INNER JOIN #ItemCost IC	                ON IC.Store_No = Price.Store_No AND 
												   IC.Item_Key = Price.Item_Key
		INNER JOIN SubTeam ST          (nolock) ON (I.SubTeam_No = ST.SubTeam_No)
		INNER JOIN ItemIdentifier      (nolock) ON (I.Item_Key = ItemIdentifier.Item_Key)
		INNER JOIN Store S             (nolock) ON (S.Store_No = Price.Store_No)
		INNER JOIN StoreSubTeam        (nolock) ON StoreSubTeam.Store_No = Price.Store_No AND 
												   StoreSubTeam.SubTeam_No = I.SubTeam_No
		INNER JOIN ItemUnit RU         (nolock) ON RU.Unit_ID = I.Retail_Unit_ID 
		INNER JOIN StoreFTPConfig FTP  (nolock) ON FTP.Store_No = S.Store_No AND 
												   FTP.FileWriterType = 'POS'
		LEFT JOIN  StoreItemVendor SIV (nolock) ON SIV.Store_No = Price.Store_No AND 
												   SIV.Item_Key = I.Item_Key     AND 
												   PrimaryVendor = 1			 AND 
												   DeleteDate IS NULL
    WHERE 
		(ISNULL(IC.Cost, 0) > 0) OR (Price.Price > 0) AND
		S.WFM_Store = 1								  AND
		FTP.IP_Address IS NOT NULL
   
    DROP TABLE #AddMissingIngredients
    DROP TABLE #ItemCost
  		
    SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadInventoryServiceExport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadInventoryServiceExport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadInventoryServiceExport] TO [IRMASchedJobsRole]
    AS [dbo];

