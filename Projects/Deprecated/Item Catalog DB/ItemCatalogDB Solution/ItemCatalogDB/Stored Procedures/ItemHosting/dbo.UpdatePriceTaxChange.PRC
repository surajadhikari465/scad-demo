SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdatePriceTaxChange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdatePriceTaxChange]
GO

CREATE PROCEDURE dbo.[UpdatePriceTaxChange]
    @Item_Key int,
    @Store_No int,
    @Price smallmoney,
    @Sale_Price smallmoney
AS

UPDATE	PRICE
SET		Price = @Price, 
		Sale_Price = @Sale_Price
WHERE	Item_Key = @Item_Key	
		AND Store_No = Store_No
		
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
