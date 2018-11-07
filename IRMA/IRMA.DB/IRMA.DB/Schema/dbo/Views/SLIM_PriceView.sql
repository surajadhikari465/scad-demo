CREATE VIEW [dbo].[SLIM_PriceView] 
AS
	-- **************************************************************************
	-- Procedure: SLIM_PriceView()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from both SLIM and EIM.
	--
	-- Modification History:
	-- Date        Init  TFS   Comment
	-- 08/27/2010  BBB   13358 added localitem column to output for EIM SLIM search
	-- 03/08/2011  VA    1557  added itemsurcharge column to output for EIM SLIM search
	-- **************************************************************************
	SELECT     
		ItemRequest_ID AS item_key, 
		User_Store AS store_no, 
		Price AS posprice, 
		Price, 
		CAST(PriceMultiple AS tinyint) AS multiple, 
		MSRPPrice, 
		CAST(MSRPMultiple AS tinyint) AS msrpmultiple, 
		POSLinkCode, 
		CAST(AgeCode AS int) AS agecode, 
		LineDiscount AS ibm_discount, 
		POSTare, 
		Restricted AS restricted_hours, 
		VisualVerify, MixMatch, 
		EmpDiscount AS discountable, 
		CAST(NULL AS bit) AS compflag, 
		CAST(NULL AS bit) AS grillprint, 
		CAST(NULL AS int) AS linkeditem, 
		CAST(NULL AS bit) AS notauthorizedforsale, 
		CAST(NULL AS bit) AS srcitizendiscount, 
		CAST(NULL AS smallmoney) AS possale_price, 
		CAST(1 AS tinyint) AS pricechgtypeid, 
		CAST(NULL AS smalldatetime) AS sale_end_date, 
		CAST(NULL AS tinyint) AS sale_multiple, 
		CAST(NULL AS smalldatetime) AS sale_start_date,
		Age_Restrict as age_restrict,
		CAST(NULL as bit) as localitem,
        CAST(NULL AS int) AS itemsurcharge
	FROM         
		ItemRequest (NOLOCK)
	WHERE     
		ItemStatus_ID = 2
GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_PriceView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_PriceView] TO [IRMAReportsRole]
    AS [dbo];

