CREATE PROCEDURE [app].[DeleteIrmaSubscriptions]
	@IrmaSubscriptionList [app].IRMAItemSubscriptionType readonly
AS
BEGIN

	update app.IRMAItemSubscription
	set deleteDate = SYSDATETIME()
	from app.IRMAItemSubscription iis
	JOIN	@IrmaSubscriptionList isl on iis.identifier = isl.Identifier and iis.regioncode = isl.RegionCode

END
GO