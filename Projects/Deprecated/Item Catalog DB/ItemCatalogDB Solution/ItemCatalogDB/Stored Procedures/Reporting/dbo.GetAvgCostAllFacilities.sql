SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvgCostAllFacilities]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAvgCostAllFacilities]
GO

-- =================================================================================
-- Author:		Sekhara
-- Create date: 11/30/2007
-- To display all Facilities in a drop down box for all DailyItemAverageCost Change report.
-- =================================================================================
CREATE PROCEDURE [dbo].[GetAvgCostAllFacilities]

AS
BEGIN

SET NOCOUNT ON

-- Fetching all Facilities to show in the ListBox.
SELECT distinct Store.Store_Name, store.Store_No 
FROM Store
inner join Vendor
on store.store_no = vendor.store_no
where Vendor.InternalCustomer > 0
and Store.Internal > 0
and (Store.Manufacturer > 0 or Store.Distribution_Center > 0)
order by Store.Store_Name 
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 