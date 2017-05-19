-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:34:14 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: GetRegionsToPush;1 - Script Date: 11/3/2008 4:34:14 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRegionsToPush]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRegionsToPush]
GO

CREATE PROCEDURE dbo.GetRegionsToPush
AS
BEGIN
	select 
	distinct RegionID
	from Regions
	--PushToIRMAQueue Q 
	--inner join POHeader H on Q.POHeaderID = H.POHeaderID
	--where Q.ProcessingFlag = 0

END

GO
