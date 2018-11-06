IF EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'[dbo].[SLIM_ItemVendorView]') AND OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW SLIM_ItemVendorView
GO

CREATE VIEW [dbo].[SLIM_ItemVendorView]
AS
-- **************************************************************************
	-- Procedure: SLIM_ItemVendorView()
	-- Author: n/a
	-- Date: n/a
	--
	-- Description:
	-- This procedure is called from both SLIM and EIM.
	--
	-- Modification History:
	-- 03/08/2011  VA    1557  added ignorecasepack, retailcasepack columns to output for EIM SLIM search
-- **************************************************************************

SELECT     
	ItemRequest_ID AS item_key, 
	CAST(VendorNumber AS int) AS vendor_id, 
	Warehouse AS item_id, 
	CAST(NULL AS datetime) AS DeleteDate,
	CaseDistHandlingChargeOverride AS casedisthandlingchargeoverride,
    CAST(NULL AS bit) AS ignorecasepack,
    CAST(NULL AS decimal(9,4)) AS retailcasepack
FROM
	ItemRequest (NOLOCK)
WHERE     
	ItemStatus_ID = 2

GO