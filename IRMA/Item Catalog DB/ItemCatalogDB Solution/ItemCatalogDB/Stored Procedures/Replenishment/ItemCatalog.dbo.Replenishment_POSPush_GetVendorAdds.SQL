/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_GetVendorAdds]    Script Date: 07/21/2006 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_POSPush_GetVendorAdds]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_POSPush_GetVendorAdds]
GO

/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_GetVendorAdds]    Script Date: 07/21/2006 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

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

