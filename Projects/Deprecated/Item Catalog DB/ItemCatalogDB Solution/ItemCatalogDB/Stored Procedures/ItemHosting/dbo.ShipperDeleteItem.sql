IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShipperDeleteItem]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ShipperDeleteItem]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.ShipperDeleteItem
	@Shipper_Key int, 
	@Item_Key int 
AS 

/*
	This SP removes a row from the Shipper table, which represents removing an item from a Shipper.
*/

delete
from
	Shipper
where
	Shipper_Key = @Shipper_Key
	and Item_Key = @Item_Key

GO
