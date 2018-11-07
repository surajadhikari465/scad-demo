-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:34:34 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetRegionsWithOrdersInQueue;1 - Script Date: 11/3/2008 4:34:34 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRegionsWithOrdersInQueue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRegionsWithOrdersInQueue]
GO

CREATE PROCEDURE dbo.GetRegionsWithOrdersInQueue
AS
BEGIN
	select 
	distinct H.RegionID
	from 
	ValidationQueue Q 
	inner join POHeader H on Q.UploadSessionHistoryID = H.UploadSessionHistoryID
	where Q.ProcessingFlag = 0

END

--insert into ValidationQueue (PONumberID) select PONumberID from POHeader

GO
