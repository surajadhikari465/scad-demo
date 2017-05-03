SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FiscalMonthAllStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[FiscalMonthAllStores]
GO


CREATE PROCEDURE dbo.FiscalMonthAllStores 
@Store_No int,
@Zone_Id int,
@Period tinyint,
@Year smallint
AS

SET NOCOUNT ON

SELECT SSBI.Date_Key, Store.Store_No,
       SUM(ISNULL(Sales_Amount,0)) + SUM(ISNULL(Return_Amount,0)) 
       + SUM(ISNULL(Markdown_Amount,0)) + SUM(ISNULL(Promotion_Amount,0)) AS TotalPrice
FROM Store (NOLOCK) 
    INNER JOIN 
        Sales_SumByItem SSBI (NOLOCK) 
        ON Store.Store_No = SSBI.Store_No
    INNER JOIN
        Item (NOLOCK)
        ON Item.Item_Key = SSBI.Item_Key
    INNER JOIN 
        Date (NOLOCK) 
        ON SSBI.Date_Key =  Date.Date_Key     	 
WHERE Period = @Period AND Year = @Year AND 
	ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND 
	ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id AND
    Item.Sales_Account IS NULL
GROUP BY SSBI.Date_Key, Store.Store_No                          
ORDER BY SSBI.Date_key Asc
    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

