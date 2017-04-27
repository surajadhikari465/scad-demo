SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DailyTax]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DailyTax]
GO


CREATE PROCEDURE dbo.DailyTax
    @Store_No int,
    @StartDate datetime,
    @EndDate datetime
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Date_Key,
           Store_No, 
           SUM(Tax_Table1_Amount) AS Tax1Collected, 
           SUM(Tax_Table2_Amount) AS Tax2Collected, 
           SUM(Tax_Table3_Amount) AS Tax3Collected,
           SUM(Food_Stamp_Amount) AS FS, 
           SUM(GC_Sales_Amount) AS GC
    FROM Buggy_SumByRegister (NOLOCK)
    WHERE Date_Key >= @StartDate 
    AND Date_Key < @EndDate +1 
    AND Store_No = @Store_No  
    GROUP BY Store_No, Date_Key
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
