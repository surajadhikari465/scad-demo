IF EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'[dbo].[StoresVW]') AND OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW StoresVW
GO
CREATE VIEW [dbo].[StoresVW]
AS
	SELECT Store_Name,
		StoreAbbr,
		Store.Store_No,
		Mega_Store,
		WFM_Store,
		Zone.Zone_ID,
		Vendor.State,
		dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType, -- 3 = Regional
		POSSystemTypes.POSSystemId,
		POSSystemTypes.POSSystemType,
		Zone.Zone_Name
	FROM Store (NOLOCK) 
	INNER JOIN 
		Zone (NOLOCK) 
		ON Store.Zone_Id = Zone.Zone_Id
	LEFT JOIN 
		Vendor (nolock) 
		ON Store.Store_No = Vendor.Store_No    
	LEFT JOIN
		POSSystemTypes (nolock)
		ON POSSystemTypes.POSSystemId = Store.POSSystemId
	WHERE (Mega_Store = 1 OR WFM_Store = 1)
		AND dbo.fn_GetCustomerType(Store.Store_No, Internal, Store.BusinessUnit_ID) = 3 -- Regional    
GO