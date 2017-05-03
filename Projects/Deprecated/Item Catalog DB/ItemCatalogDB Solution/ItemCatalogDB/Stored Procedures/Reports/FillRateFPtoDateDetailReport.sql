SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[FillRateFPtoDateDetailReport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[FillRateFPtoDateDetailReport]
GO

CREATE PROCEDURE dbo.FillRateFPtoDateDetailReport
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
	--Purpose:This SP is used as the one of the datasets in the FillRateReportFPtD.rdl
	--Rev Date		Init	Comment
	--07/01/2009	BSR		Initial Deploy
	--EXEC FillRateFPtoDateDetailReport 40808, NULL, NULL, '6/20/2009', ''

	DECLARE @Year NVARCHAR(4)
	DECLARE @YearDate NVARCHAR(20)
	DECLARE @FPStartDate NVARCHAR(20)


	SELECT @FPStartDate = MIN(Date_Key) FROM [Date]
						WHERE Period = (SELECT TOP 1 Period FROM [Date]
										WHERE date_key = @StartDate)

	SET @Year = DATEPART(YEAR,GETDATE())
	SET @YearDate = '1/1/' + @Year
	SET @EndDate = GETDATE()

	CREATE TABLE #RepTemp
		(
		Subteam_No INT,
		Subteam_Name NVARCHAR(100),
		Vendor_ID INT,
		VendorName NVARCHAR(50),
		Identifier NVARCHAR(13),
		Description NVARCHAR(60),
		CasesOrdered INT,
		CasesShipped INT,
		NotShipped BIGINT,
		CostNotShipped MONEY,
		YTDCasesOrdered INT DEFAULT 0,
		YTDCasesShipped INT DEFAULT 0
		)

	IF @Store_No IS NULL
		BEGIN
			INSERT INTO #RepTemp (Subteam_No, Subteam_Name, Vendor_ID, VendorName, Identifier, Description, CasesOrdered, CasesShipped, NotShipped, CostNotShipped)
				SELECT
					s.Subteam_No,
					s.Subteam_Name,
					siv.Vendor_ID,
					v2.CompanyName as VendorName,
					ii.Identifier as Identifier,
					i.Item_Description as Description,
					SUM(QuantityOrdered) as CasesOrdered, 
					ISNULL(SUM(QuantityReceived),0)as CasesShipped, 
					SUM(OI.QuantityOrdered - ISNULL(OI.QuantityReceived, 0)) AS [NotShipped], 
					(ISNULL(((ISNULL(dbo.fn_AvgCostHistory(oi.Item_Key, v.Store_No, s.Subteam_No, GetDate()), 0)) 
						* SIVC.CaseSize)
						+ CASE ISNULL(iv.CaseDistHandlingChargeOverride,0) WHEN 0 THEN v.CaseDistHandlingCharge ELSE ISNULL(iv.CaseDistHandlingChargeOverride,0) END
						* (SUM(OI.QuantityOrdered - ISNULL(OI.QuantityReceived, 0))),0)) AS CostNotShipped
				FROM
					(SELECT oh2.OrderHeader_ID, oh2.Transfer_To_Subteam, oh2.Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID
					 FROM OrderHeader oh2
					 INNER JOIN OrderItem oi2 ON oh2.OrderHeader_ID = oi2.OrderHeader_ID
					 GROUP BY oh2.OrderHeader_ID,Transfer_To_Subteam, Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID) oh
					INNER JOIN OrderItem oi ON oh.OrderHeader_ID = oi.OrderHeader_ID
					INNER JOIN Subteam s ON oh.Transfer_To_Subteam = s.Subteam_No
					INNER JOIN Vendor v ON oh.Vendor_ID = v.Vendor_ID
					INNER JOIN ItemIdentifier ii ON oi.Item_Key = ii.Item_Key AND ii.Default_Identifier = 1
					INNER JOIN Item i ON oi.Item_Key = i.Item_Key
					INNER JOIN ItemVendor iv ON oi.Item_Key = iv.Item_Key AND iv.Vendor_ID = oh.Vendor_ID
					INNER JOIN StoreItemVendor siv on oi.Item_Key = siv.Item_Key and siv.Store_No = v.Store_No AND siv.PrimaryVendor = 1
					INNER JOIN Vendor v2 ON siv.Vendor_ID = v2.Vendor_ID
					LEFT JOIN (SELECT MVCH.Store_No, MVCH.Item_Key,	[CaseSize] = VCH.Package_Desc1
							   FROM (SELECT SIV.Store_No, SIV.Item_Key,  SIV.Vendor_ID, [MaxVCHID] = (SELECT TOP 1 VCH0.VendorCostHistoryID
																									  FROM dbo.VendorCostHistory VCH0 (NOLOCK)
																									  INNER JOIN dbo.StoreItemVendor SIV0 (NOLOCK) ON SIV0.StoreItemVendorID = VCH0.StoreItemVendorID
																									  WHERE SIV0.Store_No = SIV.Store_No 
  																											AND SIV0.Item_Key = SIV.Item_Key 
																											AND SIV0.Vendor_ID = SIV.Vendor_ID
																											AND Getdate() BETWEEN VCH0.StartDate AND VCH0.EndDate
																											AND (SIV0.DeleteDate IS NULL OR Getdate() < SIV0.DeleteDate)
																									  ORDER BY VCH0.Promotional DESC, VCH0.VendorCostHistoryID DESC)
									FROM dbo.StoreItemVendor SIV (NOLOCK)
									WHERE SIV.PrimaryVendor = 1
									GROUP BY SIV.Store_No, SIV.Item_Key, SIV.Vendor_ID) MVCH
							LEFT JOIN dbo.VendorCostHistory VCH (NOLOCK) ON VCH.VendorCostHistoryID = MVCH.MaxVCHID
							LEFT JOIN (SELECT StoreItemVendorID, VendorDealTypeID, [CaseAmt] = SUM(ISNULL(CaseAmt, 0))
									   FROM dbo.VendorDealHistory (NOLOCK) 
									   WHERE CostPromoCodeTypeID IN (2, 4, 5, 6) AND GETDATE() BETWEEN StartDate AND EndDate
									   GROUP BY StoreItemVendorID, VendorDealTypeID) VDH ON VDH.StoreItemVendorID = VCH.StoreItemVendorID
							) SIVC ON SIVC.Store_No = v.Store_No AND SIVC.Item_Key = I.Item_Key
				WHERE 
					oh.Vendor_ID = ISNULL(@Facility, oh.Vendor_ID) AND
					oh.Transfer_To_Subteam = ISNULL(@Subteam_No, oh.Transfer_To_Subteam) AND
					oh.Expected_Date BETWEEN @FPStartDate AND @EndDate AND
					siv.Vendor_ID = ISNULL(@Vendor_ID, siv.Vendor_ID) 
				GROUP BY 
					s.Subteam_No, s.Subteam_Name,
					siv.Vendor_ID,
					v2.CompanyName,
					ii.Identifier,
					i.Item_Description,
					oi.Item_Key,
					v.Store_No,
					v.CaseDistHandlingCharge,
					iv.CaseDistHandlingChargeOverride,
					SIVC.CaseSize
				ORDER BY 
					s.Subteam_No
		END
	ELSE
		BEGIN
			INSERT INTO #RepTemp (Subteam_No, Subteam_Name, Vendor_ID, VendorName, Identifier, Description, CasesOrdered, CasesShipped, NotShipped, CostNotShipped)
				SELECT
					s.Subteam_No,
					s.Subteam_Name,
					siv.Vendor_ID,
					v2.CompanyName as VendorName,
					ii.Identifier as Identifier,
					i.Item_Description as Description,
					SUM(QuantityOrdered) as CasesOrdered, 
					ISNULL(SUM(QuantityReceived),0)as CasesShipped, 
					SUM(OI.QuantityOrdered - ISNULL(OI.QuantityReceived, 0)) AS [NotShipped], 
					(ISNULL(((ISNULL(dbo.fn_AvgCostHistory(oi.Item_Key, v.Store_No, s.Subteam_No, GetDate()), 0)) 
						* SIVC.CaseSize)
						+ CASE ISNULL(iv.CaseDistHandlingChargeOverride,0) WHEN 0 THEN v.CaseDistHandlingCharge ELSE ISNULL(iv.CaseDistHandlingChargeOverride,0) END
						* (SUM(OI.QuantityOrdered - ISNULL(OI.QuantityReceived, 0))),0)) AS CostNotShipped
				FROM
					(SELECT oh2.OrderHeader_ID, oh2.Transfer_To_Subteam, oh2.Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID, oh2.ReceiveLocation_ID
					 FROM OrderHeader oh2
					 INNER JOIN OrderItem oi2 ON oh2.OrderHeader_ID = oi2.OrderHeader_ID
					 GROUP BY oh2.OrderHeader_ID,Transfer_To_Subteam, Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID, oh2.ReceiveLocation_ID) oh
					INNER JOIN OrderItem oi ON oh.OrderHeader_ID = oi.OrderHeader_ID
					INNER JOIN Subteam s ON oh.Transfer_To_Subteam = s.Subteam_No
					INNER JOIN Vendor v ON oh.Vendor_ID = v.Vendor_ID
					INNER JOIN ItemIdentifier ii ON oi.Item_Key = ii.Item_Key AND ii.Default_Identifier = 1
					INNER JOIN Item i ON oi.Item_Key = i.Item_Key
					INNER JOIN ItemVendor iv ON oi.Item_Key = iv.Item_Key AND iv.Vendor_ID = oh.Vendor_ID
					INNER JOIN StoreItemVendor siv on oi.Item_Key = siv.Item_Key and siv.Store_No = v.Store_No AND siv.PrimaryVendor = 1
					INNER JOIN Vendor v2 ON siv.Vendor_ID = v2.Vendor_ID
					INNER JOIN Vendor v3 ON v3.Vendor_ID = oh.ReceiveLocation_ID
					INNER JOIN Store st ON st.Store_No = v3.Store_No
					LEFT JOIN (SELECT MVCH.Store_No, MVCH.Item_Key,	[CaseSize] = VCH.Package_Desc1
							   FROM (SELECT SIV.Store_No, SIV.Item_Key,  SIV.Vendor_ID, [MaxVCHID] = (SELECT TOP 1 VCH0.VendorCostHistoryID
																									  FROM dbo.VendorCostHistory VCH0 (NOLOCK)
																									  INNER JOIN dbo.StoreItemVendor SIV0 (NOLOCK) ON SIV0.StoreItemVendorID = VCH0.StoreItemVendorID
																									  WHERE SIV0.Store_No = SIV.Store_No 
  																											AND SIV0.Item_Key = SIV.Item_Key 
																											AND SIV0.Vendor_ID = SIV.Vendor_ID
																											AND Getdate() BETWEEN VCH0.StartDate AND VCH0.EndDate
																											AND (SIV0.DeleteDate IS NULL OR Getdate() < SIV0.DeleteDate)
																									  ORDER BY VCH0.Promotional DESC, VCH0.VendorCostHistoryID DESC)
									FROM dbo.StoreItemVendor SIV (NOLOCK)
									WHERE SIV.PrimaryVendor = 1
									GROUP BY SIV.Store_No, SIV.Item_Key, SIV.Vendor_ID) MVCH
							LEFT JOIN dbo.VendorCostHistory VCH (NOLOCK) ON VCH.VendorCostHistoryID = MVCH.MaxVCHID
							LEFT JOIN (SELECT StoreItemVendorID, VendorDealTypeID, [CaseAmt] = SUM(ISNULL(CaseAmt, 0))
									   FROM dbo.VendorDealHistory (NOLOCK) 
									   WHERE CostPromoCodeTypeID IN (2, 4, 5, 6) AND GETDATE() BETWEEN StartDate AND EndDate
									   GROUP BY StoreItemVendorID, VendorDealTypeID) VDH ON VDH.StoreItemVendorID = VCH.StoreItemVendorID
							) SIVC ON SIVC.Store_No = v.Store_No AND SIVC.Item_Key = I.Item_Key
				WHERE 
					oh.Vendor_ID = ISNULL(@Facility, oh.Vendor_ID) AND
					oh.Transfer_To_Subteam = ISNULL(@Subteam_No, oh.Transfer_To_Subteam) AND
					oh.Expected_Date BETWEEN @FPStartDate AND @EndDate AND
					siv.Vendor_ID = ISNULL(@Vendor_ID, siv.Vendor_ID) AND
					st.Store_No = ISNULL(@Store_No, st.Store_No)
				GROUP BY 
					s.Subteam_No, s.Subteam_Name,
					siv.Vendor_ID,
					v2.CompanyName,
					ii.Identifier,
					i.Item_Description,
					oi.Item_Key,
					v.Store_No,
					v.CaseDistHandlingCharge,
					iv.CaseDistHandlingChargeOverride,
					SIVC.CaseSize
				ORDER BY 
					s.Subteam_No
		END



	UPDATE rp SET YTDCasesOrdered = ytd.CasesOrdered, YTDCasesShipped = ytd.CasesShipped
	FROM 
		#RepTemp rp
		INNER JOIN (SELECT
						s.Subteam_No,
						s.Subteam_Name,
						siv.Vendor_ID,
						v2.CompanyName as VendorName,
						ii.Identifier as Identifier,
						i.Item_Description as Description,
						SUM(QuantityOrdered) as CasesOrdered, 
						ISNULL(SUM(QuantityReceived),0)as CasesShipped 
					FROM 
						(SELECT oh2.OrderHeader_ID, oh2.Transfer_To_Subteam, oh2.Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID
						 FROM OrderHeader oh2
						 INNER JOIN OrderItem oi2 ON oh2.OrderHeader_ID = oi2.OrderHeader_ID
						 GROUP BY oh2.OrderHeader_ID,Transfer_To_Subteam, Vendor_ID, oh2.Expected_Date, oh2.PurchaseLocation_ID) oh
						INNER JOIN OrderItem oi	ON oh.OrderHEader_ID = oi.OrderHEader_ID
						INNER JOIN Subteam s ON oh.Transfer_To_Subteam = s.Subteam_No
						INNER JOIN Vendor v ON oh.Vendor_ID = v.Vendor_ID
						INNER JOIN ItemIdentifier ii ON oi.Item_Key = ii.Item_Key AND ii.Default_Identifier = 1
						INNER JOIN Item i ON oi.Item_Key = i.Item_Key
						INNER JOIN ItemVendor iv ON oi.Item_Key = iv.Item_Key AND iv.Vendor_ID = oh.Vendor_ID
						INNER JOIN StoreItemVendor siv on oi.Item_Key = siv.Item_Key and siv.Store_No = v.Store_No AND siv.PrimaryVendor = 1
						INNER JOIN Vendor v2 ON siv.Vendor_ID = v2.Vendor_ID
					WHERE 
						oh.Vendor_ID = ISNULL(@Facility, oh.Vendor_ID) AND
						oh.Transfer_To_Subteam = ISNULL(@Subteam_No, oh.Transfer_To_Subteam) AND
						oh.Expected_Date BETWEEN @YearDate AND @EndDate AND
						siv.Vendor_ID = ISNULL(@Vendor_ID, siv.Vendor_ID)
					GROUP BY 
						s.Subteam_No, s.Subteam_Name,
						siv.Vendor_ID,
						v2.CompanyName,
						ii.Identifier,
						i.Item_Description,
						oi.Item_Key,
						v.Store_No,
						v.CaseDistHandlingCharge,
						iv.CaseDistHandlingChargeOverride) ytd ON rp.Identifier = ytd.Identifier AND rp.SubTeam_No = ytd.Subteam_No

	SELECT * FROM #RepTemp

	DROP TABLE #RepTemp 

GO