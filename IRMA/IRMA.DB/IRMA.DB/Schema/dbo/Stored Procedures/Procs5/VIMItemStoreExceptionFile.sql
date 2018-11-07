/****************************************************************************************
     Procedure: VIMItemStoreException
        Author: Robin Eudy
          Date: 3/1/2007


     Description:
     This procedure builds the Item information records for all Items with an Exception SubTeam assigned to them.
         These will be exported and sent to the VIM (Virtual Item Master) database at Central.

     Modification History:
     Date        Init Comment
     03/01/2007  RE   Initial Creation.  Comment Style ripped off from Ron Savage.
     02/13/2008  FN   Changed to allow all identifiers to be includd in the results. Also included a check
					  of Instance Flag POSPush_ExcludeSKUIdentifiers - if set, SKU identifiers are excluded from 
					  teh results.
   ****************************************************************************************/
CREATE PROCEDURE dbo.VIMItemStoreExceptionFile
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

/**************************************************************************
Find out if SKU identifiers should be excluded from the upload
***************************************************************************/
DECLARE @ExcludeSKUIdentifiers bit
SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)


SELECT  Identifier as UPC,
                --(select primaryregioncode from instancedata) AS REGION,
                (select top 1 runmode from conversion_runmode) AS REGION,
                Left(Store.BusinessUnit_ID,5) AS PS_BU,
                SubTeam.Dept_No AS POS_DEPT,
                LEFT(SubTeam.SubTeam_Name,20) as DEPT_NAME,
                Left(Package_desc2,10)  as ITEM_SIZE,
                Left(Unit_Name,5)  as ITEM_UO
FROM    Item
                LEFT JOIN ItemUnit (NOLOCK) ON (Item.Package_Unit_Id = ItemUnit.Unit_Id)
                Inner Join ItemIdentifier (NOLOCK) on Item.Item_Key = ItemIdentifier.Item_key
                        -- and Default_identifier = 1 (removed because the VIM needs all identifiers sent to POS)
                Inner Join Price (NOLOCK) on Item.Item_key = Price.Item_key
                Inner Join Store (NOLOCK) on Store.Store_no = Price.Store_no
					and Store.Mega_Store = 0
                Inner Join SubTeam (NOLOCK) on SubTeam.SubTeam_No = Price.ExceptionSubTeam_No
WHERE Item.SubTeam_No <> Isnull(Price.ExceptionSubTeam_No, Item.SubTeam_No)
         AND Store.Store_No in (select store_no from storeregionmapping where
                                                                        region_code in (select top 1 runmode from conversion_runmode))
         AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND ItemIdentifier.IdentifierType <> 'S')) -- Filter SKUs from upload if not sent to POS



END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMItemStoreExceptionFile] TO [IRMASchedJobsRole]
    AS [dbo];

