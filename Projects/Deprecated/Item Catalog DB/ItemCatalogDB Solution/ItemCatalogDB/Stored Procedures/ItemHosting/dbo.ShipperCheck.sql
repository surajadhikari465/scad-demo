IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShipperCheck]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ShipperCheck]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.ShipperCheck
	@Item_Key int
	,@IsShipper bit output
AS 

/*
	This SP simply returns a (Boolean) Shipper flag for an item via output parameter.
*/

select
	@IsShipper = i.Shipper_Item 
from 
	Item i (nolock)
where
	i.Item_Key = @Item_Key

GO
