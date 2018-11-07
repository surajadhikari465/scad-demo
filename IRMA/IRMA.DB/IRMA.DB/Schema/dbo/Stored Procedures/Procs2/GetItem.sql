CREATE PROCEDURE dbo.GetItem 
	@Item_Key int,
    @Identifier varchar(255)
AS
BEGIN


-- **************************************************************************  
-- Procedure: GetItem()  
--  
-- Description:  
-- This procedure is called to return item data to the order interface  
--  
-- Modification History:  
-- Date		Init		TFS		Comment  
-- 05.21.12	RE		xxxxx	Added Descrpiption and notes per best practices.	
-- 05.21.12	RE		xxxxx	Performance fix: Modified @item_key / @identifier 
--							search logic to prevent multiple table scans. 
-- 10.11.12 HK		7419    add select PU_Unit_ID
-- 10.16.12 HK		7419    Remove select PU_Unit_ID
-- 01.04.13 BS		8755	Updated Item.Discontinue_Item with scalar function
--							due to schema change for Discontinue Item.
-- **************************************************************************  


    SET NOCOUNT ON
  
  
      IF @item_key IS NULL 
        SET @item_key = ( SELECT TOP 1
                                    Item_Key
                          FROM      ItemIdentifier
                          WHERE     Deleted_Identifier = 0
                                    AND Remove_Identifier = 0
                                    AND Identifier = @Identifier
                        )  
  
    
    
      SELECT    Item.Item_Key ,
                Identifier ,
                Item_Description ,
                POS_Description ,
                Item.SubTeam_No ,
                SubTeam_Name ,
                SubTeam_Abbreviation ,
                Package_Desc1 ,
                Package_Desc2 ,
                PU.Unit_Abbreviation AS Package_Unit_Abbr ,
                Not_Available ,
                dbo.fn_GetDiscontinueStatus(@Item_Key, NULL, NULL) As Discontinue_Item ,
                Sign_Description ,
                ISNULL(RU.Weight_Unit, 0) AS Sold_By_Weight ,
                WFM_Item ,
                HFM_Item ,
                Retail_Sale ,
                Vendor_Unit_ID ,
                VU.Unit_Abbreviation AS Vendor_Unit_Name ,
                Item.CatchweightRequired ,
                Item.CostedByWeight ,
                RU.Unit_Name AS Retail_Unit_Name
      FROM      Item AS Item ( NOLOCK )
                INNER JOIN SubTeam AS SubTeam ( NOLOCK ) ON SubTeam.SubTeam_No = Item.SubTeam_No
                INNER JOIN ItemIdentifier II ( NOLOCK ) ON II.Item_Key = Item.Item_Key
                LEFT JOIN ItemUnit PU ( NOLOCK ) ON Item.Package_Unit_ID = PU.Unit_ID
                LEFT JOIN ItemUnit RU ( NOLOCK ) ON Item.Retail_Unit_ID = RU.Unit_ID
                LEFT JOIN ItemUnit VU ( NOLOCK ) ON Item.Vendor_Unit_ID = VU.Unit_ID
      WHERE     Item.Item_Key = @item_key
                AND ii.identifier = ISNULL(@identifier, ii.identifier)
	
	/*
	5/21/2012 RE This ISNULL() call in the WHERE clause causes the identifier table to be checked for every item. 
			Resulting in ~1,000,000 reads per call.  If you resolve the @item_key you are looking for before the main query
			reads drop to ~20.
	
	WHERE Item.Item_Key = ISNULL(@Item_Key, (SELECT TOP 1 Item_Key 
				                             FROM 
					                             ItemIdentifier 
                            				 WHERE 
                            				     Deleted_Identifier = 0 
                            					 AND Remove_Identifier = 0 
                            					 AND Identifier = @Identifier))

	*/

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItem] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItem] TO [IRMAReportsRole]
    AS [dbo];

