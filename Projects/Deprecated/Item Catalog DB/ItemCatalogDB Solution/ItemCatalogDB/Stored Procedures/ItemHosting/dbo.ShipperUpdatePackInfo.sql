if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ShipperUpdatePackInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ShipperUpdatePackInfo]
GO

SET ANSI_NULLS ON 
GO
SET QUOTED_IDENTIFIER ON 
GO

CREATE PROCEDURE dbo.ShipperUpdatePackInfo
	@Item_Key int
AS 

update Item 
set
	Package_Desc1 = (
		select
			case
				when sum(Quantity) is null then 0
				else sum(Quantity) end
		from Shipper 
		where Shipper_Key = @Item_Key
	)
where
	Item_Key = @Item_Key

GO
