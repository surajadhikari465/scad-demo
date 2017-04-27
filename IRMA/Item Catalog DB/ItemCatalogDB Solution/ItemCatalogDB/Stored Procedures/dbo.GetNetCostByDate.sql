if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetNetCostByDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetNetCostByDate]
GO

CREATE PROCEDURE dbo.GetNetCostByDate
	@Item_Key int, 
	@Store_No int,
	@Vendor_ID int, 
	@EffectiveDate datetime
AS
BEGIN
	select * from dbo.fn_getnetcost(@Item_Key, @Store_No, @Vendor_ID, @EffectiveDate)
END
GO 