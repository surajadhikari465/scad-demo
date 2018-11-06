CREATE PROCEDURE [dbo].[ItemStatusList]  
	@Discontinue_Item bit,  
	@EXEDistributed bit,  
	@Full_Pallet_Only bit,  
	@Keep_Frozen bit,  
	@Recall_Flag bit,  
	@NoDistMarkup bit,  
	@Organic bit,  
	@Pre_Order bit,  
	@Refrigerated bit,  
	@LockAuth bit,  
	@Retail_Sale bit,  
	@Shipper_Item bit,  
	@HFM_Item bit,  
	@WFM_Item bit,  
	@Not_Available bit,  
	@Default_Identifier bit,  
	@Deleted_Identifier bit,  
	@Add_Identifier bit,  
	@Remove_Identifier bit,  
	@National_Identifier bit,  
	@Scale_Identifier bit,
	@VendorItemStatuses varchar(30) = NULL

AS  
BEGIN  
    SET NOCOUNT ON  
    
    --Temp table to filter by different Vendor Item Statuses
    DECLARE @VendorItemStatusList TABLE (StatusCode varchar(1))
    --Include all statuses by default
    INSERT INTO @VendorItemStatusList 
		SELECT StatusCode 
		FROM VendorItemStatuses 
		WHERE StatusCode IN (SELECT * 
								FROM dbo.fn_ParseStringList(@VendorItemStatuses,','))
			OR @VendorItemStatuses IS NULL
    
    
	SELECT  
		i.SubTeam_No,  
		(substring('0000000000000', 1, 13 -len(rtrim(cast(ii.Identifier as bigint)))) + rtrim(ii.Identifier)) as IdentifierLeftPad,  
		ii.Identifier,  
		i.Item_Description,  
		i.Package_Desc2,  
		iu.Unit_Name,  
		dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, NULL),  
		i.EXEDistributed,  
		i.Full_Pallet_Only,  
		i.Keep_Frozen,  
		i.Recall_Flag,  
		i.NoDistMarkup,  
		i.Organic,  
		i.Pre_Order,  
		i.Refrigerated,  
		i.LockAuth,  
		i.Retail_Sale,  
		i.Shipper_Item,  
		i.HFM_Item,  
		i.WFM_Item,  
		i.Not_Available,  
		ii.Default_Identifier,  
		ii.Deleted_Identifier,  
		ii.Add_Identifier,  
		ii.Remove_Identifier,  
		ii.National_Identifier,  
		ii.Scale_Identifier,
		vis.StatusCode AS VendorItemStatus,
		vis.StatusName AS VendorItemStatusFull 

	FROM  
		Item								i			
		INNER JOIN		ItemIdentifier		ii  	ON	i.Item_Key = ii.Item_Key  
		INNER JOIN		ItemUnit			iu		ON	i.Package_Unit_Id = iu.Unit_ID  
		INNER JOIN 		ItemVendor			iv		ON	i.Item_Key = iv.item_key
		INNER JOIN		Vendor				v		ON	v.Vendor_ID = iv.vendor_id
		LEFT JOIN		VendorItemStatuses	VIS		ON	VIS.StatusID = iv.vendoritemstatus
						
	WHERE  
		dbo.fn_GetDiscontinueStatus(i.Item_key, NULL, NULL) = ISNULL(@Discontinue_Item, dbo.fn_GetDiscontinueStatus(i.Item_key, NULL, NULL))  
		AND ISNULL(i.EXEDistributed,0) = ISNULL(@EXEDistributed, ISNULL(i.EXEDistributed, 0))  
		AND ISNULL(i.Full_Pallet_Only,0) = ISNULL(@Full_Pallet_Only, ISNULL(i.Full_Pallet_Only, 0))  
		AND ISNULL(i.Keep_Frozen,0) = ISNULL(@Keep_Frozen, ISNULL(i.Keep_Frozen, 0))   
		AND ISNULL(i.LockAuth,0) = ISNULL(@LockAuth, ISNULL(i.LockAuth, 0))   
		AND ISNULL(i.Recall_Flag,0) = ISNULL(@Recall_flag, ISNULL(i.Recall_Flag,0))   
		AND ISNULL(i.NoDistMarkup,0) = ISNULL(@NoDistMarkup, ISNULL(i.NoDistMarkup, 0))  
		AND ISNULL(i.Organic,0) = ISNULL(@Organic, ISNULL(i.Organic, 0))  
		AND ISNULL(i.Pre_Order,0) = ISNULL(@Pre_Order, ISNULL(i.Pre_Order, 0))  
		AND ISNULL(i.Refrigerated,0) = ISNULL(@Refrigerated, ISNULL(i.Refrigerated, 0))  
		AND ISNULL(i.Retail_Sale,0) = ISNULL(@Retail_Sale, ISNULL(i.Retail_Sale, 0))  
		AND ISNULL(i.Shipper_Item,0) = ISNULL(@Shipper_Item, ISNULL(i.Shipper_Item, 0))  
		AND ISNULL(i.HFM_Item,0) = ISNULL(@HFM_Item, ISNULL(i.HFM_Item, 0))  
		AND ISNULL(i.WFM_Item,0) = ISNULL(@WFM_Item, ISNULL(i.WFM_Item, 0))  
		AND ISNULL(i.Not_Available,0) = ISNULL(@Not_Available, ISNULL(i.Not_Available, 0))  
		AND ISNULL(ii.Default_Identifier,0) = ISNULL(@Default_Identifier, ISNULL(ii.Default_Identifier, 0))  
		AND ISNULL(ii.Deleted_Identifier,0) = ISNULL(@Deleted_Identifier, ISNULL(ii.Deleted_Identifier, 0))  
		AND ISNULL(ii.Add_Identifier,0) = ISNULL(@Add_Identifier, ISNULL(ii.Add_Identifier, 0))  
		AND ISNULL(ii.Remove_Identifier,0) = ISNULL(@Remove_Identifier, ISNULL(ii.Remove_Identifier, 0))  
		AND ISNULL(ii.National_Identifier,0) = ISNULL(@National_Identifier, ISNULL(ii.National_Identifier, 0))  
		AND ISNULL(ii.Scale_Identifier,0) = ISNULL(@Scale_Identifier, ISNULL(ii.Scale_Identifier, 0))  
		AND vis.StatusCode IN (SELECT StatusCode FROM @VendorItemStatusList)  
		
	ORDER BY  
		i.SubTeam_No,  
		ii.Identifier  
    SET NOCOUNT OFF  
END
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemStatusList] TO [IRMAReportsRole]
    AS [dbo];

