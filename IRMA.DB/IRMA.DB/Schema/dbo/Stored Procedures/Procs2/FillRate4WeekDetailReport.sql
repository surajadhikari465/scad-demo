CREATE PROCEDURE dbo.FillRate4WeekDetailReport
	@Facility INT,
	@Subteam_No INT,
	@Vendor_ID INT,
	@StartDate NVARCHAR(20),
	@EndDate NVARCHAR(20),
	@Store_No int
WITH RECOMPILE
AS
	--Author: Brian Robichaud
	--Date:	06/30/2009
	--Purpose:This SP is used as the one of the datasets in the FillRateReport4WeekDetail.rdl
	--Rev Date		Init	Comment
	--07/01/2009	BSR		Initial Deploy
	--07/03/2009	BSR		Optimized query to execute faster
	--EXEC FillRate4WeekDetailReport 40808, NULL, NULL, '6/2/2009', ''


	SET @EndDate = DATEADD(Day,27,@StartDate)
	DECLARE @Week1Start Datetime
	DECLARE @Week2Start Datetime
	DECLARE @Week3Start Datetime
	DECLARE @Week4Start Datetime
	DECLARE @Week1End Datetime
	DECLARE @Week2End Datetime
	DECLARE @Week3End Datetime
	DECLARE @Week4End Datetime

	SET @Week1Start = @StartDate
	SET @Week2Start = DATEADD(DAY,7,@StartDate)
	SET @Week3Start = DATEADD(DAY,14,@StartDate)
	SET @Week4Start = DATEADD(DAY,21,@StartDate)

	SET @Week1End = DATEADD(DAY,6,@StartDate)
	SET @Week1End = DATEADD(DAY,13,@StartDate)
	SET @Week1End = DATEADD(DAY,20,@StartDate)
	SET @Week1End = @EndDate

	CREATE TABLE #Detail 
		(
		Subteam_No INT,
		Vendor_ID INT,
		Identifier VARCHAR(13),
		CasesOrdered INT,
		CasesShipped INT,
		NotShipped INT,
		PackSize FLOAT,
		[Week] INT,
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
						WHEN oh.Expected_Date Between @Week1Start AND @Week1End THEN 1 
						WHEN oh.Expected_Date Between @Week2Start AND @Week2End THEN 2 
						WHEN oh.Expected_Date Between @Week3Start AND @Week3End THEN 3 
						WHEN oh.Expected_Date Between @Week4Start AND @Week4End THEN 4 
					END as [Day],
					oi.Item_Key,
					v.Store_No,
					iv.CaseDistHandlingChargeOverride,
					v.CaseDistHandlingCharge
			FROM 
					(SELECT oh2.OrderHeader_ID, oh2.Transfer_To_Subteam, oh2.Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID
					 FROM OrderHeader oh2
					 INNER JOIN OrderItem oi2 ON oh2.OrderHeader_ID = oi2.OrderHeader_ID
					 GROUP BY oh2.OrderHeader_ID,Transfer_To_Subteam, Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID) oh
					INNER JOIN OrderItem oi ON oh.OrderHeader_ID = oi.OrderHeader_ID
					INNER JOIN Subteam s ON oh.Transfer_To_Subteam = s.Subteam_No
					INNER JOIN Vendor v ON oh.Vendor_ID = v.Vendor_ID
					INNER JOIN ItemIdentifier ii ON oi.Item_Key = ii.Item_Key AND ii.Default_Identifier = 1
					INNER JOIN ItemVendor iv ON oi.Item_Key = iv.Item_Key AND iv.Vendor_ID = oh.Vendor_ID
					INNER JOIN StoreItemVendor siv on oi.Item_Key = siv.Item_Key and siv.Store_No = v.Store_No AND siv.PrimaryVendor = 1
					INNER JOIN Vendor v2 ON siv.Vendor_ID = v2.Vendor_ID
					LEFT JOIN (SELECT MVCH.Store_No, MVCH.Item_Key, [CaseSize] = VCH.Package_Desc1
							   FROM (SELECT SIV.Store_No, SIV.Item_Key,  SIV.Vendor_ID, [MaxVCHID] = (SELECT TOP 1 VCH0.VendorCostHistoryID
																									  FROM dbo.VendorCostHistory VCH0 (NOLOCK)
																									  INNER JOIN dbo.StoreItemVendor SIV0 (NOLOCK) ON SIV0.StoreItemVendorID = VCH0.StoreItemVendorID
																									  WHERE SIV0.Store_No = SIV.Store_No AND SIV0.Item_Key = SIV.Item_Key 
																											AND SIV0.Vendor_ID = SIV.Vendor_ID
																											AND GETDATE() BETWEEN VCH0.StartDate AND VCH0.EndDate
																											AND (SIV0.DeleteDate IS NULL OR GETDATE() < SIV0.DeleteDate)
																									  ORDER BY VCH0.Promotional DESC, VCH0.VendorCostHistoryID DESC)
									FROM dbo.StoreItemVendor SIV (NOLOCK)
									WHERE SIV.PrimaryVendor = 1
									GROUP BY SIV.Store_No, SIV.Item_Key, SIV.Vendor_ID) MVCH
							  LEFT JOIN dbo.VendorCostHistory VCH (NOLOCK) ON VCH.VendorCostHistoryID = MVCH.MaxVCHID
							  LEFT JOIN (SELECT StoreItemVendorID, VendorDealTypeID, [CaseAmt] = SUM(ISNULL(CaseAmt, 0))
										 FROM dbo.VendorDealHistory (NOLOCK) 
										 WHERE CostPromoCodeTypeID IN (2, 4, 5, 6) AND GETDATE() BETWEEN StartDate AND EndDate
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
						WHEN oh.Expected_Date Between @Week1Start AND @Week1End THEN 1 
						WHEN oh.Expected_Date Between @Week2Start AND @Week2End THEN 2 
						WHEN oh.Expected_Date Between @Week3Start AND @Week3End THEN 3 
						WHEN oh.Expected_Date Between @Week4Start AND @Week4End THEN 4 
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
					INNER JOIN OrderItem oi ON oh.OrderHeader_ID = oi.OrderHeader_ID
					INNER JOIN Subteam s ON oh.Transfer_To_Subteam = s.Subteam_No
					INNER JOIN Vendor v ON oh.Vendor_ID = v.Vendor_ID
					INNER JOIN ItemIdentifier ii ON oi.Item_Key = ii.Item_Key AND ii.Default_Identifier = 1
					INNER JOIN ItemVendor iv ON oi.Item_Key = iv.Item_Key AND iv.Vendor_ID = oh.Vendor_ID
					INNER JOIN StoreItemVendor siv on oi.Item_Key = siv.Item_Key and siv.Store_No = v.Store_No AND siv.PrimaryVendor = 1
					INNER JOIN Vendor v2 ON siv.Vendor_ID = v2.Vendor_ID
					INNER JOIN Vendor v3 ON v3.Vendor_ID = oh.ReceiveLocation_ID
					INNER JOIN Store st ON st.Store_No = v3.Store_No
					LEFT JOIN (SELECT MVCH.Store_No, MVCH.Item_Key, [CaseSize] = VCH.Package_Desc1
							   FROM (SELECT SIV.Store_No, SIV.Item_Key,  SIV.Vendor_ID, [MaxVCHID] = (SELECT TOP 1 VCH0.VendorCostHistoryID
																									  FROM dbo.VendorCostHistory VCH0 (NOLOCK)
																									  INNER JOIN dbo.StoreItemVendor SIV0 (NOLOCK) ON SIV0.StoreItemVendorID = VCH0.StoreItemVendorID
																									  WHERE SIV0.Store_No = SIV.Store_No AND SIV0.Item_Key = SIV.Item_Key 
																											AND SIV0.Vendor_ID = SIV.Vendor_ID
																											AND GETDATE() BETWEEN VCH0.StartDate AND VCH0.EndDate
																											AND (SIV0.DeleteDate IS NULL OR GETDATE() < SIV0.DeleteDate)
																									  ORDER BY VCH0.Promotional DESC, VCH0.VendorCostHistoryID DESC)
									FROM dbo.StoreItemVendor SIV (NOLOCK)
									WHERE SIV.PrimaryVendor = 1
									GROUP BY SIV.Store_No, SIV.Item_Key, SIV.Vendor_ID) MVCH
							  LEFT JOIN dbo.VendorCostHistory VCH (NOLOCK) ON VCH.VendorCostHistoryID = MVCH.MaxVCHID
							  LEFT JOIN (SELECT StoreItemVendorID, VendorDealTypeID, [CaseAmt] = SUM(ISNULL(CaseAmt, 0))
										 FROM dbo.VendorDealHistory (NOLOCK) 
										 WHERE CostPromoCodeTypeID IN (2, 4, 5, 6) AND GETDATE() BETWEEN StartDate AND EndDate
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
		dm.Subteam_No as Subteam_No,
		s.Subteam_Name as Subteam_Name,
		dm.Vendor_ID as Vendor_ID,
		v.CompanyName as VendorName,
		dm.Identifier as Identifier,
		i.Item_Description as Description,
		SUM(dm.CasesOrdered) as CasesOrdered,
		SUM(dm.CasesShipped) as CasesShipped,
		(ISNULL(((ISNULL(dbo.fn_AvgCostHistory(dm.Item_Key, dm.Store_No, dm.Subteam_No, GetDate()), 0)) 
			* dm.PackSize)
			+ CASE ISNULL(dm.CaseDistHandlingChargeOverride,0) WHEN 0 THEN dm.CaseDistHandlingCharge ELSE ISNULL(dm.CaseDistHandlingChargeOverride,0) END
			 * (SUM(dm.CasesOrdered) - ISNULL(SUM(dm.CasesShipped), 0)),0)) AS CostNotShipped,
		dm.PackSize, 
		w1.CasesOrdered as Week1Ordered, 
		w1.CasesShipped As Week1Shipped,
		w2.CasesOrdered as Week2Ordered, 
		w2.CasesShipped As Week2Shipped,
		w3.CasesOrdered as Week3Ordered, 
		w3.CasesShipped As Week3Shipped,
		w4.CasesOrdered as Week4Ordered, 
		w4.CasesShipped As Week4Shipped
	FROM
		#Detail dm
		INNER JOIN Vendor v ON v.Vendor_ID = dm.Vendor_ID
		INNER JOIN Item i ON i.Item_Key = dm.Item_Key
		INNER JOIN Subteam s ON s.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Week] = 1
						 GROUP BY Subteam_No, Identifier, Packsize) w1 ON w1.Identifier = dm.Identifier AND w1.PackSize = dm.PackSize AND w1.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Week] = 2
						 GROUP BY Subteam_No, Identifier, Packsize) w2 ON w2.Identifier = dm.Identifier AND w2.PackSize = dm.PackSize AND w2.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Week] = 3
						 GROUP BY Subteam_No, Identifier, Packsize) w3 ON w3.Identifier = dm.Identifier AND w3.PackSize = dm.PackSize AND w3.Subteam_No = dm.Subteam_No
		LEFT OUTER JOIN (SELECT Identifier, Packsize, Subteam_No, SUM(CasesOrdered) As CasesOrdered, SUM(CasesShipped) AS CasesShipped
						 FROM #Detail
						 WHERE [Week] = 4
						 GROUP BY Subteam_No, Identifier, Packsize) w4 ON w4.Identifier = dm.Identifier AND w4.PackSize = dm.PackSize AND w4.Subteam_No = dm.Subteam_No
	GROUP BY
		dm.Subteam_No,
		dm.Vendor_ID,
		v.CompanyName,
		dm.Identifier,
		i.Item_Description,
		dm.PackSize,
		w1.CasesOrdered,
		w1.CasesShipped,
		w2.CasesOrdered,
		w2.CasesShipped,
		w3.CasesOrdered,
		w3.CasesShipped,
		w4.CasesOrdered,
		w4.CasesShipped,
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
    ON OBJECT::[dbo].[FillRate4WeekDetailReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FillRate4WeekDetailReport] TO [IRMAReportsRole]
    AS [dbo];

