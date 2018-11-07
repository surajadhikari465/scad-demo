CREATE PROCEDURE dbo.FillRate7DayDetailReport
	@Facility int,
	@Subteam_No int,
	@Vendor_ID int,
	@StartDate nvarchar(20),
	@EndDate nvarchar(20),
	@Store_No int
WITH RECOMPILE
AS
	--Author: Brian Robichaud
	--Date:	06/30/2009
	--Purpose:This SP is used as the one of the datasets in the FillRateReport7DayDetail.rdl
	--Rev Date		Init	Comment
	--07/01/2009	BSR		Initial Deploy
	--07/03/2009	BSR		Optimized query to execute faster
	--EXEC FillRate7DayDetailReport 40808, NULL, NULL, '6/17/2009', ''

	IF @EndDate IS NULL
		SET @EndDate = DATEADD(DAY,6,@StartDate)

	CREATE TABLE #Detail 
		(
		Subteam_No INT,
		Vendor_ID INT,
		Identifier VARCHAR(13),
		CasesOrdered INT,
		CasesShipped INT,
		NotShipped INT,
		PackSize FLOAT,
		[Day] INT,
		Item_Key INT,
		Store_No INT,
		CaseDistHandlingChargeOverride BIT,
		CaseDistHandlingCharge MONEY,
		)

	IF @Store_No IS NULL
		BEGIN
			INSERT INTO #Detail	
				SELECT  
						s.Subteam_No,
						siv.Vendor_ID,
						ii.Identifier as Identifier,
						QuantityOrdered as CasesOrdered, 
						QuantityReceived as CasesShipped, 
						0,--OI.QuantityOrdered - ISNULL(OI.QuantityReceived, 0) AS [NotShipped], 
						SIVC.CaseSize,
						CASE 
							WHEN oh.Expected_Date = DATEADD(Day, -7, @EndDate) THEN 1 
							WHEN oh.Expected_Date = DATEADD(Day, -6, @EndDate) THEN 2 
							WHEN oh.Expected_Date = DATEADD(Day, -5, @EndDate) THEN 3 
							WHEN oh.Expected_Date = DATEADD(Day, -4, @EndDate) THEN 4 
							WHEN oh.Expected_Date = DATEADD(Day, -3, @EndDate) THEN 5 
							WHEN oh.Expected_Date = DATEADD(Day, -2, @EndDate) THEN 6 
							WHEN oh.Expected_Date = DATEADD(Day, -1, @EndDate) THEN 7 
						END as [Day],
						oi.Item_Key,
						v.Store_No,
						iv.CaseDistHandlingChargeOverride,
						v.CaseDistHandlingCharge
				FROM 
						(SELECT oh2.OrderHeader_ID, oh2.Transfer_To_Subteam, oh2.Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID
						 FROM OrderHEader oh2
						 INNER JOIN ORderItem oi2 ON oh2.OrderHEader_ID = oi2.OrderHEader_ID
						 GROUP BY oh2.OrderHeader_ID,Transfer_To_Subteam, Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID) oh
						INNER JOIN OrderItem oi	ON oh.OrderHEader_ID = oi.OrderHEader_ID
						INNER JOIN Subteam s ON oh.Transfer_To_Subteam = s.Subteam_No
						INNER JOIN Vendor v ON oh.Vendor_ID = v.Vendor_ID
						INNER JOIN ItemIdentifier ii ON oi.Item_Key = ii.Item_Key AND ii.Default_Identifier = 1
						INNER JOIN ItemVendor iv ON oi.Item_Key = iv.Item_Key AND iv.Vendor_ID = oh.Vendor_ID
						INNER JOIN StoreItemVendor siv on oi.Item_Key = siv.Item_Key and siv.Store_No = v.Store_No AND siv.PrimaryVendor = 1
						INNER JOIN Vendor v2 ON siv.Vendor_ID = v2.Vendor_ID
						LEFT JOIN (SELECT MVCH.Store_No, MVCH.Item_Key,	[CaseSize] = VCH.Package_Desc1
			  					   FROM (SELECT SIV.Store_No, SIV.Item_Key,  SIV.Vendor_ID, [MaxVCHID] = 
																										(SELECT TOP 1 VCH0.VendorCostHistoryID
																										 FROM dbo.VendorCostHistory VCH0 (NOLOCK)
																										 INNER JOIN dbo.StoreItemVendor SIV0 (NOLOCK) ON SIV0.StoreItemVendorID = VCH0.StoreItemVendorID
																										 WHERE SIV0.Store_No = SIV.Store_No 
																										 AND SIV0.Item_Key = SIV.Item_Key AND SIV0.Vendor_ID = SIV.Vendor_ID
																										 AND Getdate() BETWEEN VCH0.StartDate AND VCH0.EndDate
																										 AND (SIV0.DeleteDate IS NULL OR Getdate() < SIV0.DeleteDate)
																										 ORDER BY VCH0.Promotional DESC, VCH0.VendorCostHistoryID DESC)
										 FROM dbo.StoreItemVendor SIV (NOLOCK)
										 WHERE SIV.PrimaryVendor = 1
										 GROUP BY SIV.Store_No, SIV.Item_Key, SIV.Vendor_ID) MVCH
						LEFT JOIN dbo.VendorCostHistory VCH (NOLOCK) ON VCH.VendorCostHistoryID = MVCH.MaxVCHID
						LEFT JOIN (SELECT StoreItemVendorID, VendorDealTypeID, [CaseAmt] = SUM(ISNULL(CaseAmt, 0))
								   FROM dbo.VendorDealHistory (NOLOCK) 
								   WHERE CostPromoCodeTypeID IN (2, 4, 5, 6) AND Getdate() BETWEEN StartDate AND EndDate
								   GROUP BY StoreItemVendorID, VendorDealTypeID) VDH ON VDH.StoreItemVendorID = VCH.StoreItemVendorID
							) SIVC ON SIVC.Store_No = v.Store_No AND SIVC.Item_Key = oi.Item_Key
				WHERE 
						oh.Vendor_ID = ISNULL(@Facility, oh.Vendor_ID) AND
						oh.Transfer_To_Subteam = ISNULL(@Subteam_No, oh.Transfer_To_Subteam) AND
						oh.Expected_Date BETWEEN @StartDate AND @EndDate AND
						siv.Vendor_ID = ISNULL(@Vendor_ID, siv.Vendor_ID)
		END
	ELSE
		BEGIN
			INSERT INTO #Detail	
				SELECT  
						s.Subteam_No,
						siv.Vendor_ID,
						ii.Identifier as Identifier,
						QuantityOrdered as CasesOrdered, 
						QuantityReceived as CasesShipped, 
						0,--OI.QuantityOrdered - ISNULL(OI.QuantityReceived, 0) AS [NotShipped], 
						SIVC.CaseSize,
						CASE 
							WHEN oh.Expected_Date = DATEADD(Day, -7, @EndDate) THEN 1 
							WHEN oh.Expected_Date = DATEADD(Day, -6, @EndDate) THEN 2 
							WHEN oh.Expected_Date = DATEADD(Day, -5, @EndDate) THEN 3 
							WHEN oh.Expected_Date = DATEADD(Day, -4, @EndDate) THEN 4 
							WHEN oh.Expected_Date = DATEADD(Day, -3, @EndDate) THEN 5 
							WHEN oh.Expected_Date = DATEADD(Day, -2, @EndDate) THEN 6 
							WHEN oh.Expected_Date = DATEADD(Day, -1, @EndDate) THEN 7 
						END as [Day],
						oi.Item_Key,
						v.Store_No,
						iv.CaseDistHandlingChargeOverride,
						v.CaseDistHandlingCharge
				FROM 
						(SELECT oh2.OrderHeader_ID, oh2.Transfer_To_Subteam, oh2.Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID, oh2.ReceiveLocation_ID
						 FROM OrderHeader oh2
						 INNER JOIN OrderItem oi2 ON oh2.OrderHeader_ID = oi2.OrderHeader_ID
						 GROUP BY oh2.OrderHeader_ID,Transfer_To_Subteam, Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID, oh2.ReceiveLocation_ID) oh
						INNER JOIN OrderItem oi	ON oh.OrderHEader_ID = oi.OrderHEader_ID
						INNER JOIN Subteam s ON oh.Transfer_To_Subteam = s.Subteam_No
						INNER JOIN Vendor v ON oh.Vendor_ID = v.Vendor_ID
						INNER JOIN ItemIdentifier ii ON oi.Item_Key = ii.Item_Key AND ii.Default_Identifier = 1
						INNER JOIN ItemVendor iv ON oi.Item_Key = iv.Item_Key AND iv.Vendor_ID = oh.Vendor_ID
						INNER JOIN StoreItemVendor siv on oi.Item_Key = siv.Item_Key and siv.Store_No = v.Store_No AND siv.PrimaryVendor = 1
						INNER JOIN Vendor v2 ON siv.Vendor_ID = v2.Vendor_ID
						INNER JOIN Vendor v3 ON v3.Vendor_ID = oh.ReceiveLocation_ID
						INNER JOIN Store st ON st.Store_No = v3.Store_No
						LEFT JOIN (SELECT MVCH.Store_No, MVCH.Item_Key,	[CaseSize] = VCH.Package_Desc1
			  					   FROM (SELECT SIV.Store_No, SIV.Item_Key,  SIV.Vendor_ID, [MaxVCHID] = 
																										(SELECT TOP 1 VCH0.VendorCostHistoryID
																										 FROM dbo.VendorCostHistory VCH0 (NOLOCK)
																										 INNER JOIN dbo.StoreItemVendor SIV0 (NOLOCK) ON SIV0.StoreItemVendorID = VCH0.StoreItemVendorID
																										 WHERE SIV0.Store_No = SIV.Store_No 
																										 AND SIV0.Item_Key = SIV.Item_Key AND SIV0.Vendor_ID = SIV.Vendor_ID
																										 AND Getdate() BETWEEN VCH0.StartDate AND VCH0.EndDate
																										 AND (SIV0.DeleteDate IS NULL OR Getdate() < SIV0.DeleteDate)
																										 ORDER BY VCH0.Promotional DESC, VCH0.VendorCostHistoryID DESC)
										 FROM dbo.StoreItemVendor SIV (NOLOCK)
										 WHERE SIV.PrimaryVendor = 1
										 GROUP BY SIV.Store_No, SIV.Item_Key, SIV.Vendor_ID) MVCH
						LEFT JOIN dbo.VendorCostHistory VCH (NOLOCK) ON VCH.VendorCostHistoryID = MVCH.MaxVCHID
						LEFT JOIN (SELECT StoreItemVendorID, VendorDealTypeID, [CaseAmt] = SUM(ISNULL(CaseAmt, 0))
								   FROM dbo.VendorDealHistory (NOLOCK) 
								   WHERE CostPromoCodeTypeID IN (2, 4, 5, 6) AND Getdate() BETWEEN StartDate AND EndDate
								   GROUP BY StoreItemVendorID, VendorDealTypeID) VDH ON VDH.StoreItemVendorID = VCH.StoreItemVendorID
							) SIVC ON SIVC.Store_No = v.Store_No AND SIVC.Item_Key = oi.Item_Key
				WHERE 
						oh.Vendor_ID = ISNULL(@Facility, oh.Vendor_ID) AND
						oh.Transfer_To_Subteam = ISNULL(@Subteam_No, oh.Transfer_To_Subteam) AND
						oh.Expected_Date BETWEEN @StartDate AND @EndDate AND
						siv.Vendor_ID = ISNULL(@Vendor_ID, siv.Vendor_ID) AND
						st.Store_No = ISNULL(@Store_No, st.Store_No)
		END

	SELECT
		dm.Subteam_No AS Subteam_No,
		s.Subteam_Name AS Subteam_Name,
		dm.Vendor_ID AS Vendor_ID,
		v.CompanyName AS VendorName,
		dm.Identifier AS Identifier,
		i.Item_Description AS Description,
		SUM(dm.CasesOrdered) AS CasesOrdered,
		SUM(dm.CasesShipped) AS CasesShipped,
		(ISNULL(((ISNULL(dbo.fn_AvgCostHistory(dm.Item_Key, dm.Store_No, dm.Subteam_No, GETDATE()), 0)) 
			* dm.PackSize)
			+ CASE ISNULL(dm.CaseDistHandlingChargeOverride,0) WHEN 0 THEN dm.CaseDistHandlingCharge ELSE ISNULL(dm.CaseDistHandlingChargeOverride,0) END
			 * (SUM(dm.CasesOrdered) - ISNULL(SUM(dm.CasesShipped), 0)),0)) AS CostNotShipped,
		dm.PackSize, 
		d1.CasesOrdered AS Day1Ordered, 
		d1.CasesShipped AS Day1Shipped,
		d2.CasesOrdered AS Day2Ordered, 
		d2.CasesShipped AS Day2Shipped,
		d3.CasesOrdered AS Day3Ordered, 
		d3.CasesShipped AS Day3Shipped,
		d4.CasesOrdered AS Day4Ordered, 
		d4.CasesShipped AS Day4Shipped,
		d5.CasesOrdered AS Day5Ordered, 
		d5.CasesShipped AS Day5Shipped,
		d6.CasesOrdered AS Day6Ordered, 
		d6.CasesShipped AS Day6Shipped,
		d7.CasesOrdered AS Day7Ordered, 
		d7.CasesShipped AS Day7Shipped
	FROM
		#Detail dm
		INNER JOIN Vendor v ON v.Vendor_ID = dm.Vendor_ID
		INNER JOIN Item i ON i.Item_Key = dm.Item_Key
		INNER JOIN Subteam s ON s.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Day] = 1
						 GROUP BY Subteam_No, Identifier, Packsize) d1 ON d1.Identifier = dm.Identifier AND d1.PackSize = dm.PackSize AND d1.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Day] = 2
						 GROUP BY Subteam_No, Identifier, Packsize) d2 ON d2.Identifier = dm.Identifier AND d2.PackSize = dm.PackSize AND d2.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Day] = 3
						 GROUP BY Subteam_No, Identifier, Packsize) d3 ON d3.Identifier = dm.Identifier	AND d3.PackSize = dm.PackSize AND d3.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Day] = 4
						 GROUP BY Subteam_No, Identifier, Packsize) d4 ON d4.Identifier = dm.Identifier	AND d4.PackSize = dm.PackSize AND d4.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Day] = 5
						 GROUP BY Subteam_No, Identifier, Packsize) d5 ON d5.Identifier = dm.Identifier	AND d5.PackSize = dm.PackSize AND d5.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Day] = 6
						 GROUP BY Subteam_No, Identifier, Packsize) d6 ON d6.Identifier = dm.Identifier AND d6.PackSize = dm.PackSize AND d6.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Day] = 7
						 GROUP BY Subteam_No, Identifier, Packsize) d7 ON d7.Identifier = dm.Identifier	AND d7.PackSize = dm.PackSize AND d7.Subteam_No = dm.Subteam_No
	GROUP BY
		dm.Subteam_No,
		dm.Vendor_ID,
		v.CompanyName,
		dm.Identifier,
		i.Item_Description,
		dm.PackSize,
		d1.CasesOrdered,
		d1.CasesShipped,
		d2.CasesOrdered,
		d2.CasesShipped,
		d3.CasesOrdered,
		d3.CasesShipped,
		d4.CasesOrdered,
		d4.CasesShipped,
		d5.CasesOrdered,
		d5.CasesShipped,
		d6.CasesOrdered,
		d6.CasesShipped,
		d7.CasesOrdered,
		d7.CasesShipped,
		dm.Item_Key, 
		dm.Store_No,
		dm.CaseDistHandlingChargeOverride,
		dm.CaseDistHandlingCharge,
		s.Subteam_Name
	ORDER BY 
		dm.identifier

	DROP TABLE #Detail
 

SET ANSI_NULLS ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FillRate7DayDetailReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FillRate7DayDetailReport] TO [IRMAReportsRole]
    AS [dbo];

