 
 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetAdPlanAuditReport') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetAdPlanAuditReport
GO

CREATE PROCEDURE [dbo].[GetAdPlanAuditReport] 
	@Store			VARCHAR(500),
	@SubTeam_No		VARCHAR(500),
	@Start_Date		SMALLDATETIME,
	@End_Date		SMALLDATETIME

AS
BEGIN
	
SELECT
    II.Identifier
	, I.Item_Description
	, P.Multiple
	, P.POSPrice AS Price
	, P.Sale_Start_Date
	, P.Sale_End_Date
	, ST.SubTeam_Name
	, P.POSSale_Price AS Sale_Price
	, P.Sale_Multiple, P.Store_No
	, S.Store_Name
	, I.SubTeam_No
	, I.Item_Key
	, dbo.fn_GetCurrentNetCost(I.Item_Key, P.Store_No) AS Cost
	, CONVERT(Numeric(5, 2), dbo.fn_GetCurrentNetCost(I.Item_Key, P.Store_No) / dbo.fn_GetCurrentVendorPackage_Desc1(I.Item_Key, P.Store_No)) AS UnitCost
	, I.Package_Desc1, I.Package_Desc2
	, I.Package_Unit_ID
	, IV.Vendor_ID
	, IV.Item_ID
	, V.Vendor_Key
	, P.PriceChgTypeId
	, PCT.PriceChgTypeDesc
	, CONVERT(varchar(10), CONVERT(numeric(7, 2), I.Package_Desc2)) + ' ' + IU.Unit_Abbreviation AS Item_Size
	, dbo.fn_GetCurrentVendorPackage_Desc1(I.Item_Key, P.Store_No) AS CasePack
FROM         
	Item I 
	INNER JOIN ItemIdentifier II ON II.Item_Key = I.Item_Key AND II.Default_Identifier = 1 
	INNER JOIN Price P ON I.Item_Key = P.Item_Key 
	INNER JOIN SubTeam ST ON I.SubTeam_No = ST.SubTeam_No 
	INNER JOIN Store S ON P.Store_No = S.Store_No 
	INNER JOIN StoreItemVendor SIV ON I.Item_Key = SIV.Item_Key 
		AND P.Store_No = SIV.Store_No 
	INNER JOIN Vendor V ON SIV.Vendor_ID = V.Vendor_ID
	INNER JOIN ItemVendor IV ON I.Item_Key = IV.Item_Key and SIV.Vendor_ID = IV.Vendor_ID 
	INNER JOIN PriceChgType PCT ON P.PriceChgTypeId = PCT.PriceChgTypeID 
	INNER JOIN ItemUnit IU ON I.Package_Unit_ID = IU.Unit_ID 
WHERE
	SIV.PrimaryVendor = 1
	AND P.Sale_Start_Date >= @Start_Date 
	AND P.Sale_End_Date <= @End_Date 
	AND P.Store_No IN 
		(SELECT Param FROM dbo.fn_MVParam(@Store, ',') AS fn_MVParam)
	AND I.SubTeam_No IN	
		(SELECT Param FROM dbo.fn_MVParam(ISNULL(@SubTeam_No, I.SubTeam_No), ',') AS fn_MVParam_2)
ORDER BY S.Store_Name, ST.SubTeam_Name, I.Item_Description

END


GO