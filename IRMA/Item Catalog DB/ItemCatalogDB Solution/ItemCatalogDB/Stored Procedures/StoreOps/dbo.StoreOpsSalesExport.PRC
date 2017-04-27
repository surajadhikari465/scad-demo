/****** Object:  Stored Procedure dbo.StoreOpsSalesExport    Script Date: 12/20/2005 9:47:04 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[StoreOpsSalesExport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[StoreOpsSalesExport]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




