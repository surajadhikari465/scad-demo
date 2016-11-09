
CREATE PROCEDURE [dbo].[VIM365ItemStoreExceptionFile]
AS
BEGIN

/*****************************************************************************************
     Procedure: VIMItemStoreException
        Author: Robin Eudy
          Date: 3/1/2007


     Description:
     This procedure builds the Item information records for all Items with an Exception SubTeam assigned to them.
         These will be exported and sent to the VIM (Virtual Item Master) database at Central.
		 This only applies to items associated to only 365 stores (Store.Mega_Store = 1)

   ****************************************************************************************/

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

/**************************************************************************
Find out if SKU identifiers should be excluded from the upload
***************************************************************************/
DECLARE @ExcludeSKUIdentifiers bit
SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('POSPush_ExcludeSKUIdentifiers', NULL), 0)

/**************************************************************************
365 Region Code variable
***************************************************************************/
DECLARE @365RegionCode varchar(2) = 'TS';

SELECT  
	Identifier						AS UPC,
    @365RegionCode					AS REGION,
    Left(Store.BusinessUnit_ID,5)	AS PS_BU,
    SubTeam.Dept_No					AS POS_DEPT,
    LEFT(SubTeam.SubTeam_Name,20)	AS DEPT_NAME,
    Left(Package_desc2,10)			AS ITEM_SIZE,
    Left(Unit_Name,5)				AS ITEM_UO
FROM    
	Item
    LEFT JOIN ItemUnit			(NOLOCK) on (Item.Package_Unit_Id = ItemUnit.Unit_Id)
    INNER JOIN ItemIdentifier	(NOLOCK) on Item.Item_Key = ItemIdentifier.Item_key
    INNER JOIN Price			(NOLOCK) on Item.Item_key = Price.Item_key
    INNER JOIN Store			(NOLOCK) on Store.Store_no = Price.Store_no
											AND Store.Mega_Store = 1
    INNER JOIN SubTeam			(NOLOCK) on SubTeam.SubTeam_No = Price.ExceptionSubTeam_No
WHERE 
	Item.SubTeam_No <> Isnull(Price.ExceptionSubTeam_No, Item.SubTeam_No)
	AND Store.Store_No in (select store_no from StoreRegionMapping where region_code = @365RegionCode)
	AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND ItemIdentifier.IdentifierType <> 'S'))

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIM365ItemStoreExceptionFile] TO [IRMASchedJobsRole]
    AS [dbo];

