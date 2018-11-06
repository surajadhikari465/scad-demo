IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdatePromoPreOrder]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[UpdatePromoPreOrder]
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE PROCEDURE dbo.[UpdatePromoPreOrder]
@strQty int,
@strID  int,
@ppoID int
AS
update 
PromoPreOrders 
set 
OrderQty = @strQty 
where 
Item_Key = @strID and 
PromoPreOrderID = @ppoID;

GO


