--  Update the value for @RegionCode to the proper region code
--  This script will be run as one of the clean-up scripts to remove all the subscriptions from Icon before 
--  ReCon runs and repopulates subscriptions for all active retail-sale items.
 
  declare @RegionCode as nchar(2) = 'PN'

  delete [app].[IRMAItemSubscription]
   where [regioncode] = @RegionCode