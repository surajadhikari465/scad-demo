SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderCurrency]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateOrderCurrency]
GO


CREATE PROCEDURE dbo.UpdateOrderCurrency 
	@OrderHeader_ID int,
	@CurrencyID int
AS

UPDATE OrderHeader
SET CurrencyID = @CurrencyID
FROM OrderHeader (rowlock)
WHERE OrderHeader_ID = @OrderHeader_ID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

