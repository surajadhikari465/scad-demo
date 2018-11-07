CREATE PROCEDURE dbo.GetWIMPExtract_COSTDATA
AS
BEGIN
    SET NOCOUNT ON
    
		SELECT	II.Identifier,
				S.Zone_ID,
				P.Store_No,
				V.Vendor_Key,
				IV.Item_ID,
				case 				
					when dbo.fn_IsItemAuthorizedForStore(II.Item_Key, P.Store_No) = 0 Then 'N'
					when dbo.fn_IsItemAuthorizedForStore(II.Item_Key, P.Store_No) = 1 Then 'Y'
					else 'N'
				end																				AS Authorized,
				case 				
					when dbo.fn_IsItemPrimaryVendor(II.Item_Key, P.Store_No, IV.Vendor_ID) = 0 Then 'N'
					when dbo.fn_IsItemPrimaryVendor(II.Item_Key, P.Store_No, IV.Vendor_ID) = 1 Then 'Y'
					else 'N'
				end																				AS PrimaryVendor,
				ISNULL(dbo.fn_GetCurrentNetCost(II.Item_Key, P.Store_No),0)						AS NetCaseCost, 
				ISNULL(VCA.Package_Desc1,0)														AS CaseSize,
				''																				AS allcode,
				CASE WHEN dbo.fn_OnSale(P.PriceChgTypeId) = 1 THEN 
					CAST(P.Sale_Price As money) 
				ELSE 
					CAST(P.Price As money) 
				END As Price,
				P.MixMatch,
				''																				AS report,
				CASE WHEN dbo.fn_OnSale(P.PriceChgTypeId) = 1 THEN 
					P.Sale_Multiple 
				ELSE 
					P.Multiple
				END																				As Multiple
		FROM	ItemIdentifier II (NOLOCK)
				INNER JOIN Price P (NOLOCK) ON II.Item_Key = P.Item_Key
				INNER JOIN Store S (NOLOCK) ON P.Store_No = S.Store_No
				INNER JOIN ItemVendor IV (NOLOCK) ON II.Item_Key = IV.Item_Key
				INNER JOIN Vendor V (NOLOCK) ON IV.Vendor_ID = V.Vendor_ID
				LEFT JOIN dbo.fn_VendorCostAll(CAST(getDate() As smalldatetime)) VCA ON II.Item_Key = VCA.Item_Key AND P.Store_No = VCA.Store_No AND IV.Vendor_ID = VCA.Vendor_ID
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_COSTDATA] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_COSTDATA] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_COSTDATA] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_COSTDATA] TO [IRMAReportsRole]
    AS [dbo];

