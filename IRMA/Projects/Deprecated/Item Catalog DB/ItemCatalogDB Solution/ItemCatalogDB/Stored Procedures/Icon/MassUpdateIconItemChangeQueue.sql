SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name = N'MassUpdateIconItemChangeQueue')
	DROP PROCEDURE MassUpdateIconItemChangeQueue
GO

CREATE PROCEDURE [dbo].[MassUpdateIconItemChangeQueue]
	-- Add the parameters for the stored procedure here
	@QIDs dbo.IconItemChangeQueueIdType READONLY, 
	@FailedDate datetime2(7) = null
AS
-- **************************************************************************
-- Procedure: MassUpdateIconItemChangeQueue
--    Author: Min Zhao
--      Date: 10/15/2014
--
-- Description:
-- This procedure deletes IconItemChangeQueue entries that have been successfully 
-- processed, and updates/marks the entries failed to be processed in the queue.
-- **************************************************************************
BEGIN
	If (@FailedDate is null)
		Begin
			delete from dbo.IconItemChangeQueue 
				   from dbo.IconItemChangeQueue a
		inner join @QIDs b
				on a.QID = b.QID 
		End
	Else
		Begin
			update dbo.IconItemChangeQueue
			   set ProcessFailedDate = @FailedDate,
				   InProcessBy = null
			  from dbo.IconItemChangeQueue a
		inner join @QIDs b
				on a.QID = b.QID 	
		End
END
GO
