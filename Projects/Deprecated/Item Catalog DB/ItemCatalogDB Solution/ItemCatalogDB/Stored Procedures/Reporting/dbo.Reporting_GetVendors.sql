
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reporting_GetVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetVendors]
GO
-- =============================================
-- Author:		Hussain Hashim
-- Create date: 8/23/2007
-- Description:	To be used for parameter list for reports.  
--				Parameter is used if All needs to be included for reports (to have all Vendors selected)
-- =============================================
CREATE PROCEDURE dbo.Reporting_GetVendors
	@blnAll	AS BIT=NULL
AS
BEGIN
	
IF @blnAll = 1
BEGIN
    SELECT	' All' AS Vendor_ID,
			' All' AS CompanyName
	UNION
    SELECT CONVERT(VARCHAR, Vendor.Vendor_ID), CompanyName
    FROM Vendor (NoLock)
    WHERE PS_Vendor_Id IS NOT NULL OR WFM = 1 OR Store_No IN 
    	(SELECT Store_No FROM Store (NoLock) WHERE Mega_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1 OR WFM_Store = 1)
    ORDER BY CompanyName
END
ELSE
BEGIN    

    SELECT Vendor.Vendor_ID, CompanyName
    FROM Vendor (NoLock)
    WHERE PS_Vendor_Id IS NOT NULL OR WFM = 1 OR Store_No IN 
    	(SELECT Store_No FROM Store (NoLock) WHERE Mega_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1 OR WFM_Store = 1)
    ORDER BY CompanyName
END

END
GO
