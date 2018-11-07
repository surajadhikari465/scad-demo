/****** Object:  Stored Procedure dbo.StoreOpsSalesExport    Script Date: 12/20/2005 9:47:04 AM ******/
CREATE PROCEDURE dbo.StoreOpsSalesExport

AS

SELECT dbo.Store.BusinessUnit_ID, dbo.Sales_SumByItem.SubTeam_No, dbo.Sales_SumByItem.Date_Key, 
       SUM(dbo.Sales_SumByItem.Sales_Amount + dbo.Sales_SumByItem.Return_Amount + dbo.Sales_SumByItem.Markdown_Amount + dbo.Sales_SumByItem.Promotion_Amount) AS ActualSales     
FROM dbo.Sales_SumByItem (nolock)
INNER JOIN
    dbo.SalesExportQueue (nolock)
    ON dbo.Sales_SumByItem.Store_No = dbo.SalesExportQueue.Store_no 
       AND dbo.Sales_SumByItem.Date_Key = dbo.SalesExportQueue.Date_Key 
INNER JOIN
    dbo.Store (nolock)
    ON dbo.Sales_SumByItem.Store_No = dbo.Store.Store_No 
INNER JOIN
    dbo.SubTeam (nolock)
    ON dbo.SubTeam.SubTeam_No = dbo.Sales_SumByItem.SubTeam_No
WHERE dbo.SubTeam.Retail = 1
GROUP BY dbo.Store.BusinessUnit_ID, 
         dbo.Sales_SumByItem.SubTeam_No, 
         dbo.Sales_SumByItem.Date_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOpsSalesExport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOpsSalesExport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOpsSalesExport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOpsSalesExport] TO [IRMAReportsRole]
    AS [dbo];

