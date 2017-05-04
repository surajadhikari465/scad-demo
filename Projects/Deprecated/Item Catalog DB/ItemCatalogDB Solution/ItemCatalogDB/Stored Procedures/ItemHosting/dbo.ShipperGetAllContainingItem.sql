if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ShipperGetAllContainingItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ShipperGetAllContainingItem]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.ShipperGetAllContainingItem
	@Item_Key int 
AS 

select
	Shipper_Key
	,Item_Key
	,Quantity
from
	Shipper (nolock)
where
	Item_Key = @Item_Key

GO
