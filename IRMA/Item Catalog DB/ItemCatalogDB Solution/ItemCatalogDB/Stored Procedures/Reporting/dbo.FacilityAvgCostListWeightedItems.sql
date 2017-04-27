SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FacilityAvgCostListWeightedItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[FacilityAvgCostListWeightedItems]
GO

CREATE Procedure dbo.FacilityAvgCostListWeightedItems
        @Store		    INT,
        @SubTeam	INT,
        @Category	    INT,
        @Date		    SMALLDATETIME
AS

SELECT	Facility = CompanyName
		,Identifier
		,Description = Item_Description
		,SubTeam = SubTeam_Name
		,Category = Category_Name
		,AvgCost = dbo.fn_AvgCostHistory (I.Item_Key, @Store, @SubTeam, @Date)
		,AvgCostDate = ISNULL(CONVERT(VARCHAR,(SELECT TOP 1 Effective_Date
                                                                             FROM AvgCostHistory (nolock)
                                                                             WHERE Item_Key = I.Item_Key
                                                                                 AND Store_No = @Store
                                                                                 AND SubTeam_No = @SubTeam
                                                                                 AND Effective_Date <= @Date
                                                                             ORDER BY Effective_Date DESC),101),'N/A')
		,'CBW/CWR*' = CASE 
				WHEN (CostedByWeight = 1 AND CatchWeightRequired = 1) THEN 'CBW,CWR'
				WHEN (CostedByWeight = 1 AND CatchWeightRequired = 0) THEN 'CBW'
				WHEN (CostedByWeight = 0 AND CatchWeightRequired = 1) THEN 'CWR'
				WHEN (CostedByWeight = 0 AND CatchWeightRequired = 0) THEN 'N/A'
				END
FROM ItemVendor IV (NOLOCK)
INNER JOIN
	Vendor V (NOLOCK)
	ON V.Vendor_ID = IV.Vendor_ID
INNER JOIN
	Item I (NOLOCK)
	ON I.Item_Key = IV.Item_Key
INNER JOIN
	ItemIdentifier II (NOLOCK)
	ON II.Item_Key = I.Item_Key
INNER JOIN
	SubTeam ST (NOLOCK)
	ON ST.SubTeam_No = I.SubTeam_No
INNER JOIN
	ItemCategory IC (NOLOCK)
	ON IC.Category_ID = I.Category_ID
WHERE DeleteDate IS NULL
AND Default_Identifier = 1
AND Deleted_Item = 0
AND Deleted_Identifier = 0
AND I.SubTeam_No = @SubTeam
AND (I.Category_ID = @Category OR @Category IS NULL)
AND IV.Vendor_ID = (SELECT Vendor_ID FROM Vendor (NOLOCK)
					            WHERE Store_No = @Store)
ORDER BY Category, Description

GO
--GRANT EXEC ON FacilityAvgCostListWeightedItems TO IRMAClientRole, IRMAReportsRole, IrmaReportsRole
--GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

