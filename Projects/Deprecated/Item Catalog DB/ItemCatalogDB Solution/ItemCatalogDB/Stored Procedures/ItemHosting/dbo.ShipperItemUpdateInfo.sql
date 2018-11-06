IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShipperItemUpdateInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ShipperItemUpdateInfo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.ShipperItemUpdateInfo
	@Shipper_Key int
	,@Item_Key int
	,@Qty int
AS

/*
	This SP sets the unit qty for an item in a Shipper.
*/

UPDATE
	Shipper
SET
	Quantity = @Qty
WHERE
	Shipper_Key = @Shipper_Key
	and Item_Key = @Item_Key

GO
