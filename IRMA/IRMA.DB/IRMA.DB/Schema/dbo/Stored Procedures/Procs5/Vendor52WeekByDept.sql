CREATE PROCEDURE dbo.Vendor52WeekByDept
WITH RECOMPILE
AS

-- exec Vendor52WeekByDept 

BEGIN
    SET NOCOUNT ON

	SELECT 
		Vendor.Vendor_Key,
		Vendor.CompanyName as VendorName,
		SubTeam.SubTeam_Name,
		MIN(Date_Key) as MinDate,
		MAX(Date_Key) as MaxDate,
		SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) as Sales
	FROM Sales_SumByItem sales (nolock)
		INNER JOIN ItemVendor (nolock)
			ON ItemVendor.Item_Key = sales.Item_Key
		INNER JOIN Vendor (nolock)
			ON Vendor.Vendor_ID = ItemVendor.Vendor_ID
		INNER JOIN SubTeam (nolock)
			ON sales.SubTeam_No = SubTeam.SubTeam_No
	WHERE Date_Key >= DATEADD(wk, -52, GETDATE()) AND Date_Key < DATEADD(day, 1, GETDATE())
	GROUP BY 
		Vendor.CompanyName, Vendor.Vendor_Key, SubTeam.SubTeam_Name
	ORDER BY 
		Vendor.CompanyName, SubTeam.SubTeam_Name

    SET NOCOUNT OFF
END
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Vendor52WeekByDept] TO [IRMAReportsRole]
    AS [dbo];

