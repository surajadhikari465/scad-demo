SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TaxReport2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TaxReport2]
GO


CREATE PROCEDURE dbo.TaxReport2
@Store_No int,
@StartDate datetime,
@EndDate datetime
AS

SELECT Store_No, SUM(Payment_Amount) AS SalesReceipts 
FROM Payment_SumByRegister (NOLOCK) 
WHERE Date_Key >= @StartDate AND Date_Key < @EndDate + 1 AND Store_No = @Store_No 
GROUP BY Store_No


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

