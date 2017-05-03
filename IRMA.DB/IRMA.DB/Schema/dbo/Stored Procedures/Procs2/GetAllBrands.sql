CREATE Procedure dbo.GetAllBrands
AS
    SELECT   Brand_Name, Brand_ID
    FROM     ItemBrand
    ORDER BY Brand_Name