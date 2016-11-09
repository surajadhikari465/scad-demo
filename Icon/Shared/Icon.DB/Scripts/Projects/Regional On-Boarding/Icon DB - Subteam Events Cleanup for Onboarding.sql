--  This script is to be run after ReCon is done with processing all the New Item events
--  in the on-boarding region's IRMA queue.
--  This script will delete all the Item Sub Team Update events in Icon queue generated by 
--  ReCon during its on-boarding run, because the majority of items' subteam have been aligned
--  with Icon, and processing these events won't add much value or have much effect to the region
 
  declare @RegionCode as nchar(2) = 'SO'
  declare @ECCStartDateTime as datetime = '2015-06-22 13:00:00'

  declare @SubTeamEventId as int 
  select @SubTeamEventId = EventId from app.EventType
   where EventName  = 'Item Sub Team Update' 

  delete eq
  from  [app].[EventQueue] eq
  where [ProcessFailedDate] is NULL
    and [RegionCode] = @RegionCode
	and eq.EventId  = @SubTeamEventId
	and eq.InsertDate > @ECCStartDateTime
	and eventReferenceId not in   -- During the On-boarding process, GDT continues to work in Icon. We should keep the Item Subteam events generated by GDT's work during this time
	(
		select eventReferenceId 
		  from app.eventQueue equ
		 where equ.ProcessFailedDate is null
		   and equ.EventId  = @SubTeamEventId
	  group by equ.EventReferenceId
		having count(equ.eventReferenceId) > 1
	)
