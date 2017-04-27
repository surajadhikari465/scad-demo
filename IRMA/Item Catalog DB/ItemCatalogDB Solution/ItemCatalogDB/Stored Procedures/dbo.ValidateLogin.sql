if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ValidateLogin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[ValidateLogin]
GO

CREATE PROCEDURE dbo.ValidateLogin 
    @UserName varchar(25)
AS 
    SELECT 
		Users.*,
        (SELECT MAX(Vendor_ID) FROM Vendor WHERE Store_No = Telxon_Store_Limit) AS Vendor_Limit,
        
		-- SLIM ACCESS ATTRIBUTES
		ISNULL(SlimAccess.UserAdmin, 0) As UserAdmin,
		ISNULL(SlimAccess.ItemRequest, 0) As ItemRequest,
		ISNULL(SlimAccess.VendorRequest, 0) As VendorRequest,
		ISNULL(SlimAccess.IRMAPush, 0) As IRMAPush,
		ISNULL(SlimAccess.StoreSpecials, 0) As StoreSpecials,
		ISNULL(SlimAccess.RetailCost, 0) As RetailCost,
		ISNULL(SlimAccess.Authorizations, 0) As Authorizations,
		ISNULL(SlimAccess.WebQuery, 0) As WebQuery,
		ISNULL(SlimAccess.ScaleInfo, 0) As ScaleInfo,

		Title.Title_Desc
    FROM Users
		LEFT JOIN SlimAccess ON Users.User_ID = SlimAccess.User_ID
		LEFT JOIN Title ON Users.Title = Title.Title_ID
    WHERE UserName = @UserName
GO
