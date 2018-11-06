if exists (select * from dbo.sysobjects where id = object_id(N'dbo.HARTGetInventoryStoreOpsExport') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.HARTGetInventoryStoreOpsExport
GO

CREATE PROCEDURE [dbo].[HARTGetInventoryStoreOpsExport]

	@EndScan	datetime
AS
-- **************************************************************************
-- Procedure: HARTGetInventoryStoreOpsExport()
--    Author: Billy Blackerby
--      Date: 04.24.12
--
-- Description:
-- This procedure is called from the HART Import job to generate an output file to StoreOps
-- by SSRS procedures.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 04.24.12		BBB   	5753	Created
-- 05.08.12		BBB		5753	numerous tweaks to the query based upon SO reqs
-- 07.06.12		BBB		6902	changed Store_No output to BusinessUnit_ID for StoreOps
-- 2012-10-11	KM		8388	Allow execution during weeks 4 & 5 in addition to week 1
--2014-01-23 SH/DL 14682 Modified to work for both Thursday and Sunday inventories
-- **************************************************************************
BEGIN
    SET NOCOUNT ON
--**************************************************************************
--Only execute if HART Import run in Week 1, 4 or 5 of Period
--**************************************************************************
IF (SELECT Date_Key FROM Date 
		WHERE Date_Key = CONVERT(varchar(10), GETDATE(), 1) AND (Week = 1 OR Week = 4 OR Week = 5)) IS NOT NULL
	BEGIN
	--**************************************************************************
	--Declare internal variables
	--**************************************************************************
	DECLARE @week tinyint, @EOP smalldatetime, @EndofPeriod bit, @FiscalYear int, @FiscalPeriod int

	--**************************************************************************
	--Populate internal variables
	--**************************************************************************
	SELECT @week = Week
	FROM [Date] D (nolock) 
	WHERE Date_Key = CONVERT(date, GETDATE())


	SELECT @EOP = dbo.fn_PeriodBeginDate(GETDATE())

	-- DL - this is original section but back with a step added right below - the calculated EOP tells us which Fiscal Period we are doing for this run
	IF @week NOT IN (1,2)
		SET @EOP = DATEADD(day, 28, @EOP)

	SET @EOP = DATEADD(minute, -1, @EOP)
	
	-- DL - Added get the Fiscal Year and Period based on the calculated EOP to filter the counts below
	SELECT @FiscalYear = D.Year, @FiscalPeriod = D.Period
	FROM [Date] D (nolock) 
	WHERE Date_Key = CONVERT(date, @EOP)
	
	

	--**************************************************************************
	--Output SQL
	--**************************************************************************
	SELECT
		[FP]		=	i.Period,
		[FY]		=	i.Year,
		[BU]		=	i.BusinessUnit_ID,
		[subteam]	=	i.PS_SubTeam_No,/*SH 3/16/2015 - changed from i.SubTeam_No*/
		[AvgCost]	=	CONVERT(money, SUM(i.AvgCost))
	FROM
		(
		SELECT
			d.Period,
			d.Year,
			s.BusinessUnit_ID,
			SST.PS_SubTeam_No,/*SH 3/16/2015 - changed from ccm.SubTeam_No*/
			AvgCost = SUM(ISNULL(cci.AvgCost, 0) * 
						CASE 
							WHEN cchi.Weight >  0 THEN 
								ISNULL(cchi.Weight, 0)
							ELSE 
								ISNULL(cchi.Count,0)
						END)
		  FROM 
				CycleCountMaster        (nolock) ccm
				JOIN CycleCountHeader   (nolock) cch    ON	ccm.MasterCountID						= cch.MasterCountID
				JOIN CycleCountItems    (nolock) cci    ON	cch.CycleCountID						= cci.CycleCountID
				JOIN CycleCountHistory  (nolock) cchi   ON	cci.CycleCountItemID					= cchi.CycleCountItemID
				JOIN Date               (nolock) d      ON	CONVERT(date, ccm.EndScan)				= d.Date_Key
				JOIN Store				(nolock) s		ON	ccm.Store_No							= s.Store_No
/*SH 3/16/2015 - added this join*/JOIN StoreSubTeam SST (nolock) on SST.Store_No = ccm.Store_No and SST.SubTeam_No = ccm.SubTeam_No
				-- DL - Added to handle if and when the BU/Subteam is counting 
				JOIN CountDateSchedule CDS (nolock) ON CDS.BusinessUnitID = S.BusinessUnit_ID AND CDS.SubTeamSID = sst.PS_SubTeam_No/*SH 3/16/2015 changed from ccm.SubTeam_No*/ AND CDS.FiscalYear = @FiscalYear AND CDS.FiscalPeriod = @FiscalPeriod
		WHERE 
			cch.[External]	= 1
			-- DL - Added to filter the counts included to make sure they're for the end of this fiscal period that we determined above
			AND d.Period = @FiscalPeriod
			AND d.Year = @FiscalYear
			AND d.Week IN (4, 5)  -- This is key - we don't have to worry about Thursday or Sunday with this method
		GROUP BY
			d.Period,
			d.Year,
			s.BusinessUnit_ID,
			sst.PS_SubTeam_No/*SH 3/16/2015 - changed from ccm.SubTeam_No*/
		
		UNION
		
			SELECT
			d.Period,
			d.Year,
			s.BusinessUnit_ID,
			sst.PS_SubTeam_No,/*SH 3/16/2015 changed from ccm.SubTeam_No*/
			AvgCost = SUM(CONVERT(money, ISNULL(isl.EFF_PRICE_EXTENDED, 0)) * ISNULL(sst.CostFactor, 1))
		  FROM 
				CycleCountMaster				(nolock) ccm
				JOIN CycleCountHeader			(nolock) cch    ON	ccm.MasterCountID						= cch.MasterCountID
				JOIN Date						(nolock) d      ON	CONVERT(varchar(10), ccm.EndScan, 1)	= d.Date_Key
				JOIN Store						(nolock) s		ON	ccm.Store_No							= s.Store_No
				JOIN StoreSubTeam				(nolock) sst	ON	ccm.Store_No							= sst.Store_No
																AND ccm.SubTeam_No							= sst.SubTeam_No
				JOIN InventoryServiceImportLoad	(nolock) isl	ON	s.BusinessUnit_ID						= CONVERT(int, isl.PS_BU)
				  /*SH 3/16/2015 - changed from ccm.SubTeam_No*/AND	sst.PS_SubTeam_No						= CONVERT(int, isl.PS_PROD_SUBTEAM)
																AND	d.Year									= isl.Year 
																AND d.Period								= isl.Period
																AND ISNULL(isl.SKU, '0')					= '0'
				-- DL - Repeated from above 
				JOIN CountDateSchedule CDS (nolock) ON CDS.BusinessUnitID = S.BusinessUnit_ID AND CDS.SubTeamSID = sst.PS_SubTeam_No/*SH 3/16/2015 - changed from ccm.SubTeam_No*/ AND CDS.FiscalYear = @FiscalYear AND CDS.FiscalPeriod = @FiscalPeriod
		WHERE 
			cch.[External]	= 1
			-- DL - Repeated from above
			AND d.Period = @FiscalPeriod
			AND d.Year = @FiscalYear
			AND d.Week IN (4, 5)
		GROUP BY
			d.Period,
			d.Year,
			s.BusinessUnit_ID,
			sst.PS_SubTeam_No/*SH 3/16/2015 changed from ccm.SubTeam_No*/
		) AS i
	GROUP BY
		i.Period,
		i.Year,
		i.BusinessUnit_ID,
		i.PS_SubTeam_No/*SH 3/16/2015 - changed from i.SubTeam_No*/
			
	END

	SET NOCOUNT OFF
END
GO


