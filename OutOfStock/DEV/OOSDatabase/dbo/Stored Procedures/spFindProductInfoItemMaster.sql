
CREATE PROCEDURE [dbo].[spFindProductInfoItemMaster]
@LinkedServer nvarchar(50),
@Upc varchar(25)
AS
BEGIN
DECLARE @query varchar(1000), @openQuery varchar(1000)
Select @query = 'SELECT im.nat_upc AS UPC, im.brand AS BRAND, br.brand_name as BRAND_NAME, im.long_description AS LONG_DESCRIPTION, im.ITEM_SIZE AS ITEM_SIZE, im.ITEM_UOM AS ITEM_UOM, h2.hier_full_name AS CATEGORY_NAME, h4.hier_full_name AS CLASS_NAME FROM vim.hierarchy h1, vim.hierarchy h2, vim.hierarchy h3, vim.hierarchy h4,  vim.item_master im LEFT join vim.Brand br on UPPER(trim(im.brand)) = UPPER(trim(br.brand)) WHERE h1.HIER_LVL_ID = 10 AND h2.HIER_PARENT = h1.hierarchy_ref AND h2.HIER_LVL_ID = 20  AND h3.HIER_PARENT = h2.hierarchy_ref AND h3.HIER_LVL_ID = 30 AND h4.HIER_PARENT = h3.hierarchy_ref AND h4.HIER_LVL_ID = 40 AND im.HIERARCHY_REF = h4.HIERARCHY_REF AND im.nat_upc IN (''''' + @Upc + ''''')'
Select @openQuery = 'SELECT top 1 UPC, BRAND, BRAND_NAME from OPENQUERY(' + @LinkedServer  + ', ''' + @query + ''')'
EXEC(@OpenQuery)
END
