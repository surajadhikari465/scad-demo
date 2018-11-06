IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].GetAllBrands') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure [dbo].GetAllBrands
	END
GO

CREATE Procedure dbo.GetAllBrands
AS
    SELECT   Brand_Name, Brand_ID
    FROM     ItemBrand
    ORDER BY Brand_Name

GO

