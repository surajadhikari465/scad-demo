-- =============================================
-- Author:		Min Zhao
-- Create date: 2014-10-15
-- Description:	Bulk insert IRMAItemSubscription 
--				entities.
-- =============================================
CREATE PROCEDURE [app].[InsertIRMAItemSubscriptions]
	@IRMAItemSubscriptions app.IRMAItemSubscriptionType readonly
AS
BEGIN	
	delete [app].[IRMAItemSubscription]
	  from [app].[IRMAItemSubscription] a
inner join @IRMAItemSubscriptions b
	    on a.[regioncode] = b.[regioncode]
	   and a.[identifier] = b.[identifier]

	insert into [app].[IRMAItemSubscription]
           ([regioncode]
           ,[identifier]
		   ,[insertDate]
		   ,[deleteDate]) 
	select [regioncode], [identifier], GetDate(), null  from @IRMAItemSubscriptions
END
GO
