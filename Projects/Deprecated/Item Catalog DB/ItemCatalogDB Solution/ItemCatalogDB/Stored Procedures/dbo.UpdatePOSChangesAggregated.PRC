SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdatePOSChangesAggregated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdatePOSChangesAggregated]
GO


CREATE PROCEDURE dbo.UpdatePOSChangesAggregated
@Store_No int,
@Sales_Date datetime
AS

UPDATE POSChanges
SET Aggregated = 1 
WHERE Aggregated = 0 AND Store_No = @Store_No AND Sales_Date = @Sales_Date

DECLARE @tmpDate smalldatetime
SELECT @tmpDate = convert(smalldatetime,convert(char(10),@sales_date,101))
UPDATE Store
SET LastSalesUpdateDate = @tmpDate
WHERE Store_No = @Store_No

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

