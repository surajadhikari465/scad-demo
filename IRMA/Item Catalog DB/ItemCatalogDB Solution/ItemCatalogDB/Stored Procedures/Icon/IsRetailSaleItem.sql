SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsRetailSaleItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[IsRetailSaleItem]
GO

CREATE PROCEDURE [dbo].[IsRetailSaleItem] 
	@ItemKey int,
	@IsRetailSaleItem bit OUTPUT
AS 
BEGIN

    SET @IsRetailSaleItem = 
		(SELECT
			i.Retail_Sale
		FROM
			Item i
		WHERE
			i.Item_Key = @ItemKey)

	SELECT @IsRetailSaleItem

END
GO