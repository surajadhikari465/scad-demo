IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'JDASync_UpdateMappedValues')
	BEGIN
		DROP  Procedure  dbo.JDASync_UpdateMappedValues
	END

GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE Procedure dbo.JDASync_UpdateMappedValues
AS

	-- first update each of the four hierarchy values
	UPDATE dbo.JDA_ItemSync
		SET JDA_Dept =
		(
			SELECT JDA_Dept
			FROM dbo.JDA_HierarchyMapping (NOLOCK)
			WHERE Subteam_No = IRMA_Subteam_No
			AND ProdHierarchyLevel3_ID = IRMA_ProdHierarchyLevel3_ID
			AND ProdHierarchyLevel4_ID = IRMA_ProdHierarchyLevel4_ID
		)
	WHERE SyncState <= 1

	UPDATE dbo.JDA_ItemSync
		SET JDA_SubDept =
		(
			SELECT JDA_SubDept
			FROM dbo.JDA_HierarchyMapping (NOLOCK)
			WHERE Subteam_No = IRMA_Subteam_No
			AND ProdHierarchyLevel3_ID = IRMA_ProdHierarchyLevel3_ID
			AND ProdHierarchyLevel4_ID = IRMA_ProdHierarchyLevel4_ID
		)
	WHERE SyncState <= 1

	UPDATE dbo.JDA_ItemSync
		SET JDA_Class =
		(
			SELECT JDA_Class
			FROM dbo.JDA_HierarchyMapping (NOLOCK)
			WHERE Subteam_No = IRMA_Subteam_No
			AND ProdHierarchyLevel3_ID = IRMA_ProdHierarchyLevel3_ID
			AND ProdHierarchyLevel4_ID = IRMA_ProdHierarchyLevel4_ID
		)
	WHERE SyncState <= 1

	UPDATE dbo.JDA_ItemSync
		SET JDA_SubClass =
		(
			SELECT JDA_SubClass
			FROM dbo.JDA_HierarchyMapping (NOLOCK)
			WHERE Subteam_No = IRMA_Subteam_No
			AND ProdHierarchyLevel3_ID = IRMA_ProdHierarchyLevel3_ID
			AND ProdHierarchyLevel4_ID = IRMA_ProdHierarchyLevel4_ID
		)
	WHERE SyncState <= 1

	-- now update the three unit values
	
	UPDATE dbo.JDA_ItemSync
		SET Package_Unit_ID =
		(
			SELECT JDA_ID
			FROM dbo.JDA_ItemUnitMapping (NOLOCK)
			WHERE Unit_ID = IRMA_Package_Unit_ID
		)
	WHERE SyncState <= 1

	UPDATE dbo.JDA_ItemSync
		SET Retail_Unit_ID =
		(
			SELECT JDA_ID
			FROM dbo.JDA_ItemUnitMapping (NOLOCK)
			WHERE Unit_ID = IRMA_Retail_Unit_ID
		)
	WHERE SyncState <= 1

	UPDATE dbo.JDA_ItemSync
		SET Vendor_Unit_ID =
		(
			SELECT JDA_ID
			FROM dbo.JDA_ItemUnitMapping (NOLOCK)
			WHERE Unit_ID = IRMA_Vendor_Unit_ID
		)
	WHERE SyncState <= 1
	
	-- update the brand value
	UPDATE dbo.JDA_ItemSync
		SET JDA_Brand_ID =
		(
			SELECT JDA_ID
			FROM dbo.JDA_ItemBrandMapping (NOLOCK)
			WHERE Brand_ID = IRMA_Brand_ID
		)
	WHERE SyncState <= 1
	
	-- update the price change type value
	UPDATE dbo.JDA_PriceSync
		SET JDA_PricePriority =
		(
			SELECT JDA_Priority
			FROM dbo.JDA_PriceChgTypeMapping (NOLOCK)
			WHERE PriceChgTypeID = IRMA_PriceChgType_ID
		)
	WHERE SyncState <= 1
	
GO 