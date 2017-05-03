SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetOrderEmail') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetOrderEmail
GO


CREATE PROCEDURE dbo.GetOrderEmail
	@OrderHeader_ID int
AS

BEGIN
	SET NOCOUNT ON

	SELECT Vendor.CompanyName, Users.EMail, TUsers.Email As TEmail
	FROM 
		OrderHeader (nolock)
		INNER JOIN
			Vendor (nolock)
			ON Vendor.Vendor_ID = OrderHeader.Vendor_ID
		INNER JOIN
			Users (nolock)
			ON Users.User_ID = OrderHeader.CreatedBy
		INNER JOIN
			Vendor VendStore (nolock)
			ON VendStore.Vendor_ID = OrderHeader.ReceiveLocation_ID
		LEFT JOIN
			StoreSubTeam (nolock)
			ON StoreSubTeam.Store_No = VendStore.Store_No
			AND StoreSubTeam.SubTeam_No = OrderHeader.Transfer_To_SubTeam
		LEFT JOIN
			UserStoreTeamTitle US (nolock)
			ON US.Store_No = VendStore.Store_No AND US.Team_No = StoreSubTeam.Team_No
		LEFT JOIN
			Users TUsers (nolock)
			ON TUsers.User_ID = US.User_ID AND TUsers.Email IS NOT NULL  AND TUsers.AccountEnabled = 1
	WHERE OrderHeader_ID = @OrderHeader_ID
	 AND TUsers.Email IS NOT NULL

	SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO