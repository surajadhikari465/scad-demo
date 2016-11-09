﻿
CREATE PROCEDURE [dbo].[PricingPrintSignsSearch]
    @StoreNumber int,
    @SubteamNumber int,
    @CategoryId int,
    @SignDescription varchar(60),
    @Identifiers varchar(max),
    @BrandId int
AS
BEGIN
    SET NOCOUNT ON
    
	DECLARE @BrandName varchar(255) = (SELECT Brand_Name FROM ItemBrand (nolock) WHERE Brand_ID = @BrandId)
    
	;with SearchIdentifiers as
	(
		SELECT DISTINCT Key_Value as Identifier FROM dbo.[fn_ParseStringList](@Identifiers, ',')
	)

	SELECT 
		sq.Item_Key, 
		sq.Sign_Description, 
		sq.Identifier, 
		sq.Brand_Name, 
		st.SubTeam_Name, 
		sq.SubTeam_No,
        CASE 
			WHEN dbo.fn_OnSale(sq.PriceChgTypeID) = 0 THEN Multiple 
			ELSE Sale_Multiple 
		END As Multiple, 
        CASE 
			WHEN dbo.fn_OnSale(sq.PriceChgTypeID) = 0 THEN Price 
			ELSE Sale_Price 
		END As Price,
        pct.PriceChgTypeDesc
    FROM 
		SignQueue (nolock) sq
		INNER JOIN Item (nolock) i on sq.Item_Key = i.Item_Key
		INNER JOIN SubTeam (nolock) st ON st.SubTeam_No = sq.SubTeam_No
		INNER JOIN PriceChgType pct (nolock) ON pct.PriceChgTypeID = sq.PriceChgTypeID
		LEFT JOIN ItemBrand (nolock) ib ON i.Brand_ID = ib.Brand_ID
		LEFT JOIN SearchIdentifiers si ON sq.Identifier = si.Identifier
	WHERE 
		Store_No = @StoreNumber
        AND (@SubteamNumber IS NULL OR sq.SubTeam_No = @SubteamNumber)
		AND (@SignDescription IS NULL OR sq.Sign_Description LIKE '%' + @SignDescription + '%')
        AND (@BrandName IS NULL OR ISNULL(sq.Brand_Name, ib.Brand_Name) = @BrandName)
        AND (@CategoryId IS NULL OR @CategoryId = Category_ID)
		AND (@Identifiers is null or si.Identifier is not null)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PricingPrintSignsSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PricingPrintSignsSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PricingPrintSignsSearch] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PricingPrintSignsSearch] TO [IRMAReportsRole]
    AS [dbo];

