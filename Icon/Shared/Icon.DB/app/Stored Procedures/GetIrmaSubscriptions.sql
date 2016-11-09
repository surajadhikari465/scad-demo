CREATE PROCEDURE [app].[GetIrmaSubscriptions]
	@IrmaSubscriptionList [app].IRMAItemSubscriptionType readonly
AS
BEGIN

select 
	  iis.[IRMAItemSubscriptionID]
      ,iis.[regioncode]
      ,iis.[identifier]
      ,iis.[insertDate]
      ,iis.[deleteDate]
	from app.IRMAItemSubscription iis
	JOIN	@IrmaSubscriptionList isl on iis.identifier = isl.Identifier and iis.regioncode = isl.RegionCode and iis.deleteDate is null

END
GO
