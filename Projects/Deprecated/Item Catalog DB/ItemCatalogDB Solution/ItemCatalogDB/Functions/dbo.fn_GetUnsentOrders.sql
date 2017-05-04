if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GetUnsentOrders]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_GetUnsentOrders]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE FUNCTION dbo.fn_GetUnsentOrders(
    @store_no int
	,@vendor_id int
)
RETURNS TABLE     
AS    
RETURN (
    select oh.orderheader_id
	from 
		orderheader oh (nolock)
	join
		vendor rl (nolock)
		on rl.vendor_id = oh.receiveLocation_id
	join
		store s (nolock)
		on s.store_no = rl.store_no
	where
		s.store_no = @store_no
		and oh.vendor_id = @vendor_id
		and sent = 0
)
GO
  