
CREATE PROCEDURE [dbo].[spFindProductInfoItemRegion]
@LinkedServer nvarchar(50),
@Upc varchar(25)
AS
BEGIN
DECLARE @query varchar(1000), @openQuery varchar(1000)
Select @query = 'SELECT im.upc AS UPC, im.brand AS BRAND, br.brand_name as BRAND_NAME, im.long_description AS LONG_DESCRIPTION, im.ITEM_SIZE AS ITEM_SIZE, im.ITEM_UOM AS ITEM_UOM, h1.hier_full_name AS CATEGORY_NAME, h2.hier_full_name AS CLASS_NAME FROM vim.item_region im LEFT join vim.Brand br on UPPER(trim(im.brand)) = UPPER(trim(br.brand)) LEFT join vim.reg_hierarchy h1 on im.reg_hier_ref = h1.reg_hier_ref LEFT join vim.reg_hierarchy h2 on h1.hier_parent = h2.reg_hier_ref LEFT join vim.reg_hierarchy h3 on h2.hier_parent = h3.reg_hier_ref WHERE im.upc IN (''''' + @Upc + ''''') ORDER by im.UPC'
Select @openQuery = 'SELECT TOP 1 UPC, BRAND, BRAND_NAME from OPENQUERY(' + @LinkedServer + ', ''' + @query + ''')'
EXEC(@OpenQuery)
END
