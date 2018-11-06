CREATE VIEW DC_VENDOR_COST AS
	SELECT	VCH.StoreItemVendorId,
			VCH.Promotional,
			VCH.UnitCost,
			VCH.UnitFreight,
			VCH.Package_Desc1 AS CostVendorPack,
			VCH.StartDate AS CostStartDate,
			VCH.EndDate AS CostEndDate,
			VCH.FromVendor AS CostIsFromVendor,
			VCH.MSRP AS CostMSRP,
			IUC.Unit_Name AS CostUOM,
			IUF.Unit_Name AS FreightUOM,
			SIV.Store_No AS CostStoreNo,
			S.Store_Name AS CostStoreName,
			S.Distribution_Center AS IsCostStoreDC,
		    S.BusinessUnit_ID AS CostStoreBusinessUnitID,
			II.Identifier AS CostItemIdentifier,
			II.Default_Identifier AS IsIdentifierDefault,
			II.IdentifierType AS ItemIdentifierType,
			SIV.Vendor_ID AS CostVendorID,
			V.Vendor_Key AS CostVendorKey,
			V.CompanyName AS CostVendorName,
			V.Address_Line_1 AS CostVendorAddressLine1,
			V.Address_Line_2 AS CostVendorAddressLine2,
			V.City AS CostVendorCity,
			V.State AS CostVendorState,
			V.Zip_Code AS CostVendorZipCode,
			V.Phone AS CostVendorPhone,
			V.Fax AS CostVendorFax,
			V.Comment AS CostVendorComment,
			V.Customer AS CostVendorIsCustomer,
			V.InternalCustomer AS CostVendorIsInternalCustomer,
			V.ActiveVendor AS CostVendorIsActive,
			V.Order_By_Distribution AS CostVendorOrderByDistribution,
			V.Electronic_Transfer AS CostVendorElectronicTransfer,
			V.PS_Vendor_ID AS CostVendorPeopleSoftVendorID,
			V.PS_Location_Code AS CostVendorPeopleSoftLocationCode,
			V.PS_Address_Sequence AS CostVendorPeopleSoftAddressSequence,
			V.PS_Export_Vendor_ID AS CostVendorPSExportVendorID,
			V.EFT AS CostVendorIsEFT,
			V.InStoreManufacturedProducts AS CostVendorInStoreManufacturedProducts,
			V.EXEWarehouseVendSent AS CostVendorEXEWarehouseVendSent,
			V.EXEWarehouseCustSent AS CostVendorEXEWarehouseCustSent,
			SIV.PrimaryVendor AS IsCostVendorPrimaryForItem			
	FROM VendorCostHistory VCH
		 LEFT JOIN StoreItemVendor SIV ON SIV.StoreItemVendorId = VCH.StoreItemVendorId
		 LEFT JOIN ItemIdentifier II ON II.Item_Key = SIV.Item_Key
		 LEFT JOIN Store S ON S.Store_No = SIV.Store_No
		 LEFT JOIN Vendor V ON V.Vendor_ID = SIV.Vendor_ID
		 LEFT JOIN ItemUnit IUC ON IUC.Unit_ID = VCH.CostUnit_ID
		 LEFT JOIN ItemUnit IUF ON IUF.Unit_ID = VCH.FreightUnit_ID
GO
GRANT SELECT
    ON OBJECT::[dbo].[DC_VENDOR_COST] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DC_VENDOR_COST] TO [IRMADCAnalysisRole]
    AS [dbo];

