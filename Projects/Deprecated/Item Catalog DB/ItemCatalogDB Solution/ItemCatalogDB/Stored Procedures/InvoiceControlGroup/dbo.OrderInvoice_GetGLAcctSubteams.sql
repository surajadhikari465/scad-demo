
/****** Object:  StoredProcedure [dbo].[OrderInvoice_GetGLAcctSubteams]    Script Date: 08/07/2008 12:54:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderInvoice_GetGLAcctSubteams]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrderInvoice_GetGLAcctSubteams]

GO
/****** Object:  StoredProcedure [dbo].[OrderInvoice_GetGLAcctSubteams]    Script Date: 08/07/2008 12:54:32 ******/

CREATE PROCEDURE [dbo].[OrderInvoice_GetGLAcctSubteams]
	@OrderHeader_ID INT
AS
BEGIN

-- 20080807 - DaveStacey - This query populates the Charges dropdown on the Control Group Invoice Data form
-- This stored procedure returns the subteams which have GLAcct populated.

	SELECT ST.SubTeam_No, SubTeamGLAcct = CAST(ST.SubTeam_Name as varchar(25)) + ' ' + CAST(ST.GLPurchaseAcct as varchar(25)) 
	FROM dbo.OrderHeader OH (NOLOCK)
		INNER JOIN dbo.Vendor (NOLOCK)ON OH.Vendor_ID = Vendor.Vendor_ID   
		INNER JOIN dbo.Vendor ReceiveLocation (NOLOCK) ON ReceiveLocation.Vendor_ID = OH.ReceiveLocation_ID
		INNER JOIN dbo.Store (NOLOCK) ON Store.Store_No = ReceiveLocation.Store_no
		INNER JOIN dbo.StoreSubTeam SST (NOLOCK) ON Store.Store_No = SST.Store_No 
		JOIN dbo.Subteam st (nolock) ON ST.SubTeam_No = SST.SubTeam_No
	WHERE st.GLPurchaseAcct is not null
		and OH.orderheader_id = @OrderHeader_ID
END
GO