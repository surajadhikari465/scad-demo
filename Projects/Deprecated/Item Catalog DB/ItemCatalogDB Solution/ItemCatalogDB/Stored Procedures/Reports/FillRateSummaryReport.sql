SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[FillRateSummaryReport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[FillRateSummaryReport]
GO

CREATE PROCEDURE dbo.FillRateSummaryReport
	@Facility int,
	@Subteam_No int,
	@Vendor_ID int,
	@StartDate nvarchar(20),
	@EndDate nvarchar(20),
	@ReportFlag int,
	@Store_No int
WITH RECOMPILE
AS
	--Author: Brian Robichaud
	--Date:	06/30/2009
	--Purpose:This SP is used as the one of the datasets in the FillRateReportSummary.rdl
	--Rev Date		Init	Comment
	--07/01/2009	BSR		Initial Deploy
	--EXEC FillRateSummaryReport 40808, NULL, NULL, '6/16/2009', '6/26/2009',3

	IF @ReportFlag = 1
		BEGIN
			--7day Dates
			SET @EndDate = DATEADD(Day,6,@StartDate)
		END
		
	IF @ReportFlag = 2
		BEGIN
			--4Week Dates
			SET @EndDate = DATEADD(Day,27,@StartDate)
		END 
		
	IF @ReportFlag = 3
		BEGIN
			--FPTD Dates
			SET @EndDate = @StartDate
			SELECT @StartDate = MIN(Date_Key) FROM [Date]
								WHERE Period = (SELECT TOP 1 Period FROM [Date]
												WHERE date_key = @StartDate)
		END

	IF @Store_No IS NULL
		BEGIN
			SELECT  
					Subteam_No,	Subteam_Name, CAST(SUM(QuantityOrdered) AS int) AS CasesOrdered, CAST(SUM(QuantityReceived) AS int) AS CasesShipped, 
					SUM(OI.QuantityOrdered - ISNULL(OI.QuantityReceived, 0)) AS [NotShipped], HandlingCharge, 
					CAST(HandlingCharge * SUM(QuantityReceived) AS decimal(12,2))
			FROM
					(SELECT oh2.OrderHeader_ID, oh2.Transfer_To_Subteam, oh2.Vendor_ID, oh2.Expected_Date
					 FROM OrderHeader oh2 INNER JOIN OrderItem oi2 ON oh2.OrderHeader_ID = oi2.OrderHeader_ID
					 GROUP BY oh2.OrderHeader_ID,Transfer_To_Subteam, Vendor_ID, oh2.Expected_Date) oh
					INNER JOIN OrderItem oi	ON oh.OrderHeader_ID = oi.OrderHeader_ID
					INNER JOIN Subteam s ON Transfer_To_Subteam = s.Subteam_No
			WHERE
					Vendor_ID = ISNULL(@Facility, Vendor_ID)
					AND Transfer_To_Subteam = ISNULL(@Subteam_No, Transfer_To_Subteam)
					AND oh.Expected_Date BETWEEN @StartDate AND @EndDate 
			GROUP BY 
					Subteam_No, Subteam_Name, HandlingCharge
			ORDER BY 
					Subteam_No, HandlingCharge 
		END
	ELSE
		BEGIN
			SELECT  
					Subteam_No, Subteam_Name, CAST(SUM(QuantityOrdered) AS INT) AS CasesOrdered, CAST(SUM(QuantityReceived) AS INT) AS CasesShipped, 
					SUM(OI.QuantityOrdered - ISNULL(OI.QuantityReceived, 0)) AS [NotShipped], HandlingCharge, 
					CAST(HandlingCharge * SUM(QuantityReceived) AS DECIMAL(12,2)), st.Store_No
			FROM 
					(SELECT oh2.OrderHeader_ID, oh2.Transfer_To_Subteam, oh2.Vendor_ID, oh2.Expected_Date, oh2.ReceiveLocation_ID
					 FROM OrderHeader oh2
					 INNER JOIN OrderItem oi2 ON oh2.OrderHeader_ID = oi2.OrderHeader_ID
					 GROUP BY oh2.OrderHeader_ID,Transfer_To_Subteam, Vendor_ID, oh2.Expected_Date, oh2.ReceiveLocation_ID) oh
					INNER JOIN OrderItem oi ON oh.OrderHEader_ID = oi.OrderHEader_ID
					INNER JOIN Subteam s ON Transfer_To_Subteam = s.Subteam_No
					INNER JOIN Vendor v on V.Vendor_ID = oh.ReceiveLocation_ID
					INNER JOIN Store st on st.Store_No = v.Store_No
			WHERE 
					oh.Vendor_ID = ISNULL(@Facility, oh.Vendor_ID) AND
					Transfer_To_Subteam = ISNULL(@Subteam_No, Transfer_To_Subteam) AND
					oh.Expected_Date BETWEEN @StartDate AND @EndDate AND
					st.Store_No = ISNULL(@Store_No,st.Store_No)
			GROUP BY 
					Subteam_No, Subteam_Name, HandlingCharge, st.Store_No
			ORDER BY 
					Subteam_No, HandlingCharge 
		END	
	
GO