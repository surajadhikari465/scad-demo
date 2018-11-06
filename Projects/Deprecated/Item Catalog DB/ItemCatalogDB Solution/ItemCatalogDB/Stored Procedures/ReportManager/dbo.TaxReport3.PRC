SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TaxReport3]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TaxReport3]
GO


CREATE PROCEDURE dbo.TaxReport3
    @Store_No int,
    @StartDate datetime,
    @EndDate datetime
AS

    SELECT Store_No, 
           SUM(CASE WHEN Taxed = 0 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END)  AS TaxExempt,
           SUM(CASE WHEN Tax_Table = 1 AND Taxed = 1 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END) AS TT1,
           SUM(CASE WHEN Tax_Table = 2 AND Taxed = 1 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END) AS TT2,
           SUM(CASE WHEN Tax_Table = 3 AND Taxed = 1 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END) AS TT3,
           SUM(CASE WHEN (Taxed = 0 or Tax_Table = 0 or Tax_Table > 2) THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END) AS NoTax 
    FROM Sales_Fact (NOLOCK) 
    WHERE Time_Key >= @StartDate AND Time_Key < @EndDate + 1 
    AND Store_No = @Store_No  
    GROUP By Store_No
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

