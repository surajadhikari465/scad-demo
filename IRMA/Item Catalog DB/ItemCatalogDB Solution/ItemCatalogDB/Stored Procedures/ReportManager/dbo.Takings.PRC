SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Takings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Takings]
GO


CREATE PROCEDURE dbo.Takings
@Store_No int,
@StartDate datetime
AS 

SELECT Payment_Fact.Cashier_ID, Payment.Description, SUM(Payment_Amount) AS PaymentAmount, COUNT(*) AS PaymentCount 
FROM Time (NOLOCK) INNER JOIN (Payment (NOLOCK) INNER JOIN Payment_Fact (NOLOCK) ON (Payment.Payment_Type = Payment_Fact.Payment_Type) 
     ) ON (Time.Time_Key = Payment_Fact.Time_Key) 
WHERE Store_No = @Store_No AND Date_Key = @StartDate
GROUP BY Payment_Fact.Cashier_ID, Payment.Description


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

