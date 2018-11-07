CREATE PROCEDURE dbo.GetStoresWithNoVendorForItem 
	@Item_Key int
AS
BEGIN
    SET NOCOUNT ON



	--GETS LIST OF STORES WHERE NO PRIMARY VENDOR IS ASSIGNED FOR THE GIVEN ITEM_KEY
	SELECT  Store_Name 
	FROM (
			SELECT Store_Name, ( SELECT COUNT(Item_Key) as PrimaryVendorCount from StoreItemVendor where Item_Key = @Item_Key and Store_NO = s.Store_NO and  primaryvendor =1 and deletedate is null  ) as PrimaryVendorCount
			FROM Store s
			WHERE (S.WFM_Store = 1 OR S.Mega_Store = 1)
		) AS p1 WHERE p1.PrimaryVendorCount  = 0

/*
	SELECT Store_Name
	FROM Store S
	LEFT JOIN
		StoreItemVendor SIV
		ON SIV.Store_No = S.Store_No
			AND SIV.Item_Key = @Item_Key
	WHERE (S.WFM_Store = 1 OR S.Mega_Store = 1)
		AND (PrimaryVendor IS NULL OR PrimaryVendor = 0)
		AND DeleteDate is NULL
	ORDER BY Store_Name
*/    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoresWithNoVendorForItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoresWithNoVendorForItem] TO [IRMAClientRole]
    AS [dbo];

