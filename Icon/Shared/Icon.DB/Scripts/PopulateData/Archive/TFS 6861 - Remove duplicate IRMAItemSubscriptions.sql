Declare @subscriptionToBeDeleted as Table(IRMAItemSubscriptionID int, regioncode varchar(2), identifier varchar(13), insertdate datetime2(7), deletedate datetime2(7));

WITH subscription_CTE AS
(
	select regioncode, identifier 
	from app.IRMAItemSubscription
	group by regioncode, identifier
	having count(regioncode + identifier) > 1
),
maxSubscription_CTE AS
(
	select max(IRMAItemSubscriptionID) as IRMAItemSubscriptionID
	from app.IRMAItemSubscription iis
	join subscription_CTE  s on iis.regioncode = s.regioncode and iis.identifier = s.identifier 
	group by iis.regioncode, iis.identifier
)

insert into @subscriptionToBeDeleted
select iis.IRMAItemSubscriptionID, regioncode, identifier, insertDate,deleteDate 
from app.IRMAItemSubscription iis
join maxSubscription_CTE ms on iis.IRMAItemSubscriptionID = ms.IRMAItemSubscriptionID 
order by IRMAItemSubscriptionID

print 'The following subscriptions will be deleted ...'
select * from @subscriptionToBeDeleted

delete app.IRMAItemSubscription
from app.IRMAItemSubscription iis
join @subscriptionToBeDeleted ms on iis.IRMAItemSubscriptionID = ms.IRMAItemSubscriptionID 