SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCreditOrderList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCreditOrderList]
GO


CREATE PROCEDURE dbo.GetCreditOrderList 
@OrderHeader_ID int
AS 

SELECT ReturnOrderHeader_ID 
FROM ReturnOrderList 
WHERE OrderHeader_ID = @OrderHeader_ID AND ReturnOrderHeader_ID <> OrderHeader_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



