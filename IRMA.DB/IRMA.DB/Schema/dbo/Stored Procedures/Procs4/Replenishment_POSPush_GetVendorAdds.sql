CREATE PROCEDURE dbo.Replenishment_POSPush_GetVendorAdds
	@AuditReport bit,
    @Store_No int
AS 

	SELECT  Store.Store_No,
			Vendor_ID, 
			UPPER(Vendor_Key) AS Vendor_Key, 
			CompanyName, 
			dbo.fn_GetTorexJulianDate(GetDate()) As PIRUS_CurrentDate
	FROM Vendor (nolock), Store (nolock)
	WHERE ((@AuditReport = 0 AND AddVendor=1) OR (@AuditReport = 1 AND Store.Store_No = @Store_No))
		AND WFM_Store = 1
	ORDER BY Store.Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetVendorAdds] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetVendorAdds] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetVendorAdds] TO [IRMASchedJobsRole]
    AS [dbo];

