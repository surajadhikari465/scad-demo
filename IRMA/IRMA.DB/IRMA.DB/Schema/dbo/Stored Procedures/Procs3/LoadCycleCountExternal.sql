
CREATE PROCEDURE [dbo].[LoadCycleCountExternal]

	@EndScan datetime,
	@UsePSSubTeamNoForImport bit

AS

-- ****************************************************************************************************************
-- Procedure: LoadCycleCountExternal
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012/10/03	AB		?????	Removed all references to ItemCaseHistory
-- 2013/07/10	BL		13124	Modified to use the CountDateSchedule
-- 2013/08/02	KM		13367	Insert average cost or last received cost into CycleCountItems depending on which cost
--								scheme is used by the region;	
-- 2013/08/12	KM		13367	Update last received cost calculation to more closely mirror transfer cost logic;
-- 2013/08/27	KM		13367	Change the structure of @importDates so that store numbers aren't being joined to business
--								unit IDs in non-South regions;
-- 2013/09/27	KM		13367	Use a table-based approach to selecting last received cost;
-- 2013/10/13   KM      11623	Prevent inserting last received cost for duplicate item keys in OrderItem (a condition which
--								shouldn't occur, but does anyway);
-- 2013/10/15	KM		11623	Implement a better version of the duplicate item key check above; Insert a missing BEGIN/END block
--								in the last received cost logic;
-- 2013/11/12	KM		14373	Further improvements to the table-based cost structure for non-avg-cost regions;
-- ****************************************************************************************************************

BEGIN
    SET NOCOUNT ON
    
	-- Debug.
	--IF OBJECT_ID('tempdb..#costId') IS NOT NULL DROP TABLE #costId
	--IF OBJECT_ID('tempdb..#promoDate') IS NOT NULL DROP TABLE #promoDate
	--IF OBJECT_ID('tempdb..#promo') IS NOT NULL DROP TABLE #promo
	--IF OBJECT_ID('tempdb..#vendorCost') IS NOT NULL DROP TABLE #vendorCost
	--IF OBJECT_ID('tempdb..#currentCostItemKey') IS NOT NULL DROP TABLE #currentCostItemKey
	--IF OBJECT_ID('tempdb..#currentCost') IS NOT NULL DROP TABLE #currentCost
	--IF OBJECT_ID('tempdb..#lastReceivedOrder') IS NOT NULL DROP TABLE #lastReceivedOrder
	--IF OBJECT_ID('tempdb..#lastReceivedCost') IS NOT NULL DROP TABLE #lastReceivedCost

	DECLARE 
		@Week			tinyint, 
		@EndOfPeriod	bit, 
		@DateKey		date

	DECLARE @CountDates TABLE (StoreNumber int, SubteamNumber int, DateKey smalldatetime)

	-- Get the "End of Period" Date and the fiscal week based upon the current date.
	SELECT @week = [Week] FROM [Date] (nolock) WHERE Date_Key = @EndScan
    
	-- Depending on which fiscal week the count is imported during, pick the import dates to insert into the CycleCount tables.
	SELECT @DateKey =	CASE WHEN @Week = 1 
							THEN DATEADD(dd, -7, @EndScan)
							ELSE @EndScan
						END

	SELECT @EndOfPeriod = CASE WHEN @Week IN (1, 4, 5) THEN 1 ELSE 0 END

	-- According to the business rules, end scan dates need to have a timestamp of 23:59:00 so that they appear as the last record of the day in ItemHistory.
	SET @EndScan = DATEADD(minute, 59, DATEADD(hour, 23, @EndScan))


	INSERT INTO @CountDates (StoreNumber, SubteamNumber, DateKey) 
		SELECT
			s.Store_No,
			CASE WHEN @UsePSSubTeamNoForImport = 1
				THEN cds.SubTeamSID
				ELSE sst.SubTeam_No 
			END,
			CASE WHEN @Week IN (2, 3) 
				THEN @EndScan 
				ELSE DATEADD(minute, 59, DATEADD(hour, 23, cds.Date_Key)) 
			END 
		FROM 
			CountDateSchedule			(nolock)	cds
			INNER JOIN	[Date]			(nolock)	d	ON	d.[Year]	= cds.FiscalYear 
														AND	d.Period	= cds.FiscalPeriod
			INNER JOIN	Store			(nolock)	s	ON	cds.BusinessUnitID	= s.BusinessUnit_ID
			INNER JOIN	StoreSubteam	(nolock)	sst	ON	sst.Store_No		= s.Store_No 
														AND sst.PS_SubTeam_No	= cds.SubTeamSID
		WHERE 
			d.Date_Key = @DateKey
    

	BEGIN TRY
		BEGIN TRAN
			-- Insert missing Cycle Count Masters
			INSERT INTO CycleCountMaster (EndofPeriod, Store_No, SubTeam_No, EndScan) 
				SELECT 
					@EndOfPeriod, 
					MN.Store_No, 
					MN.SubTeam_No, 
					MN.DateKey
						
				FROM
					(SELECT 
						ccel.Store_No, 
						sst.SubTeam_No, /*SH 3/16/2015 - changed from ccel.SubTeam_No*/
						cd.DateKey
					FROM 
						CycleCountExternalLoad ccel
						INNER JOIN @CountDates cd					ON	cd.StoreNumber		= ccel.Store_No 
																	AND cd.SubteamNumber	= ccel.SubTeam_No 
																	AND cd.DateKey IS NOT NULL
						INNER JOIN StoreSubTeam sst /*SH 3/16/2015 - added this join*/ON sst.Store_No = ccel.Store_No
																	AND sst.PS_SubTeam_No = ccel.SubTeam_No
									
					GROUP BY 
						ccel.Store_No, 
						sst.SubTeam_No, /*SH 3/16/2015 - changed from ccel.SubTeam_No*/
						cd.DateKey) MN
	
					LEFT JOIN CycleCountMaster ccm	ON	MN.Store_No		= ccm.Store_No
													AND MN.SubTeam_No	= ccm.SubTeam_No
													AND MN.DateKey		= ccm.EndScan
													AND ccm.EndofPeriod	= @EndOfPeriod
				WHERE 
					ccm.MasterCountID IS NULL

			-- Get CycleCountMaster data
			DECLARE @CCMaster TABLE (MasterCountID int, DateKey datetime)
		
			INSERT INTO @CCMaster (MasterCountID, DateKey)
				SELECT 
					ccm.MasterCountID, 
					ISNULL(MN.DateKey, @EndScan)
				FROM 
					CycleCountMaster ccm
					INNER JOIN (SELECT 
									ccel.Store_No, 
									sst.SubTeam_No, /*SH 3/16/2015 - changed from ccel.SubTeam_No*/
									cd.DateKey
								FROM 
									CycleCountExternalLoad ccel
									INNER JOIN @CountDates cd 				ON	cd.StoreNumber		= ccel.Store_No
																			AND cd.SubteamNumber	= ccel.SubTeam_No
									INNER JOIN StoreSubTeam sst	/*SH 3/16/2015 - added this join*/			ON sst.Store_No = ccel.Store_No
																			AND sst.PS_SubTeam_No = ccel.SubTeam_No

								GROUP BY 
									ccel.Store_No, 
									sst.SubTeam_No, /*SH 3/16/2015 - changed from ccel.SubTeam_No*/
									cd.DateKey) MN		ON	MN.Store_No		= ccm.Store_No 
													AND MN.SubTeam_No	= ccm.SubTeam_No
				WHERE 
					ccm.EndScan = MN.DateKey AND ccm.EndOfPeriod = @EndOfPeriod		
        
			-- Delete ItemHistory Data
			DELETE ItemHistory 
				FROM 
					ItemHistory IH
					INNER JOIN CycleCountMaster M ON M.Store_No = IH.Store_No AND M.SubTeam_No = IH.SubTeam_No AND M.EndScan = IH.DateStamp
					INNER JOIN @CCMaster CCM	  ON CCM.MasterCountID = M.MasterCountID
				WHERE 
					IHUpdated = 1 AND Adjustment_ID = 2
           
			-- Update CycleCountMaster
			UPDATE CycleCountMaster 
				SET 
					ClosedDate = NULL,
					IHUpdated = 0,
					UpdateIH = 0, 
					SetNonCountedToZero = 0	
				FROM 
					CycleCountMaster M
					INNER JOIN @CCMaster CCM ON CCM.MasterCountID = M.MasterCountID
				WHERE 
					ClosedDate IS NOT NULL
    
		
			DELETE CycleCountHistory 
				FROM 
					CycleCountHistory D
					INNER JOIN CycleCountItems I  ON I.CycleCountItemID = D.CycleCountItemID
					INNER JOIN CycleCountHeader H ON H.CycleCountID		= I.CycleCountID
					INNER JOIN @CCMaster M		  ON M.MasterCountID	= H.MasterCountID
				WHERE 
					[External] = 1 

			DELETE CycleCountItems 
				FROM 
					CycleCountItems I
					INNER JOIN CycleCountHeader H ON H.CycleCountID = I.CycleCountID
					INNER JOIN @CCMaster M		  ON M.MasterCountID = H.MasterCountID
				WHERE 
					[External] = 1 

		
			DELETE CycleCountHeader 
				FROM 
					CycleCountHeader H
					INNER JOIN @CCMaster M ON M.MasterCountID = H.MasterCountID
				WHERE 
					[External] = 1 

			INSERT INTO CycleCountHeader (MasterCountID, StartScan, [External], ClosedDate)	SELECT 
					MasterCountID, ISNULL(M.DateKey, @EndScan), 1, ISNULL(M.DateKey, @EndScan)
				FROM 
					@CCMaster M

			DECLARE @UseAverageCost bit = dbo.fn_InstanceDataValue('UseAvgCostForCostAndMargin', NULL)

			IF @UseAverageCost = 1
				BEGIN		
					INSERT INTO CycleCountItems (CycleCountID, Item_Key, AvgCost) 
						SELECT 
							CycleCountID, 
							L.Item_Key, 
							dbo.fn_AvgCostHistory(L.Item_Key, M.Store_No, M.SubTeam_No, M.EndScan) 
						FROM 
							CycleCountHeader H
							INNER JOIN CycleCountMaster M		ON	M.MasterCountID		= H.MasterCountID
							INNER JOIN @CCMaster CCM			ON	CCM.MasterCountID	= M.MasterCountID
							INNER JOIN StoreSubTeam SST	/*SH 3/16/2015 - added this join*/ON SST.Store_No = M.Store_No
																AND SST.SubTeam_No = M.SubTeam_No
							INNER JOIN CycleCountExternalLoad L	ON	L.Store_No			= M.Store_No 
																AND L.SubTeam_No		= SST.PS_SubTeam_No/*SH 3/16/2015 - changed from M.SubTeam_No*/
						WHERE 
							[External] = 1
						GROUP BY 
							CycleCountID, L.Item_Key, M.Store_No, M.SubTeam_No, M.EndScan
				END
			ELSE
				BEGIN
					
					-- The cost preference for 
					
					CREATE TABLE #lastReceivedCost 
					(	
						ItemKey int primary key, 
						LastReceivedCost smallmoney
					)

					CREATE TABLE #lastReceivedOrder
					(
						ItemKey int primary key,
						OrderHeaderId int
					)

					CREATE TABLE #currentCost 
					(
						ItemKey int primary key, 
						CurrentCost smallmoney
					)

					CREATE TABLE #currentCostItemKey
					(
						ItemKey int primary key,
						StoreNumber int
					)

					INSERT INTO #lastReceivedOrder 
						SELECT 
							ccel.Item_Key,
							MAX(oi.OrderHeader_ID)
						FROM
							CycleCountExternalLoad	(nolock)	ccel
							JOIN OrderItem			(nolock)	oi		ON ccel.Item_Key			= oi.Item_Key
							JOIN OrderHeader		(nolock)	oh		ON oh.OrderHeader_ID		= oi.OrderHeader_ID
							JOIN Vendor				(nolock)	v		ON oh.ReceiveLocation_ID	= v.Vendor_ID
							JOIN Store				(nolock)	s		ON v.Store_no				= ccel.Store_No
						WHERE   
							oh.Return_Order = 0
							AND oh.OrderType_ID <> 3
							AND oh.ApprovedDate IS NOT NULL
							AND oi.QuantityReceived > 0
						GROUP BY 
							ccel.Item_Key
						
					------------------------------------------------------------------------
					-- Mirror the last received cost logic from AutomaticOrderItemInfo.
					------------------------------------------------------------------------
					DECLARE 
						@Pounds int	= (SELECT Unit_ID FROM ItemUnit (nolock) WHERE LOWER(UnitSysCode) = 'lbs'),
						@Units	int = (SELECT Unit_ID FROM ItemUnit (nolock) WHERE LOWER(UnitSysCode) = 'unit')
	
					INSERT INTO #lastReceivedCost 
						SELECT
							lro.ItemKey,
							LastReceivedCost =	dbo.fn_CostConversion(
													CASE
														-- For an eInvoiced order, select the line item invoice cost. 
														WHEN oh.eInvoice_ID IS NOT NULL THEN 
															oi.InvoiceExtendedCost / CASE WHEN i.CostedByWeight = 1 AND oi.Total_Weight > 0 THEN oi.Total_Weight ELSE oi.QuantityReceived END
														-- For a paper invoiced order, it will be the line item received cost.
														WHEN oh.eInvoice_ID IS NULL THEN 
															oi.ReceivedItemCost / CASE WHEN i.CostedByWeight = 1 AND oi.Total_Weight > 0 THEN oi.Total_Weight ELSE oi.QuantityReceived END
													END, 
													oi.CostUnit, CASE WHEN i.CostedByWeight = 1 THEN @Pounds ELSE @Units END, oi.Package_Desc1, oi.Package_Desc2, oi.Package_Unit_ID)
					
						FROM
							#lastReceivedOrder			lro
							INNER JOIN	OrderHeader		oh	(nolock)	ON	lro.OrderHeaderId = oh.OrderHeader_ID
							CROSS APPLY (
											SELECT TOP(1) * FROM OrderItem WHERE	lro.ItemKey				= Item_Key
																					AND lro.OrderHeaderId	= OrderHeader_ID
																					AND QuantityReceived	> 0
										) oi
							INNER JOIN	Item			i	(nolock)	ON	oi.Item_Key	= i.Item_Key
						

					INSERT INTO #currentCostItemKey (ItemKey, StoreNumber) 
						SELECT
							ik.Item_Key as ItemKey,
							ik.Store_No as StoreNumber
						FROM
							(SELECT DISTINCT
								Item_Key,
								Store_No
							FROM
								CycleCountExternalLoad
							WHERE
								Item_Key NOT IN (SELECT ItemKey FROM #lastReceivedOrder)
							) ik

					SELECT
						ccik.ItemKey,
						siv.StoreItemVendorID		 AS StoreItemVendorId,
						MAX(vch.VendorCostHistoryID) AS VendorCostHistoryId
				
					INTO #costId FROM
						#currentCostItemKey				ccik
						INNER JOIN StoreItemVendor		siv		(nolock)	ON	ccik.ItemKey = siv.Item_Key
																			AND	ccik.StoreNumber = siv.Store_No
																			AND	siv.PrimaryVendor = 1
																			AND (siv.DeleteDate IS NULL OR siv.DeleteDate >= @EndScan)

						INNER JOIN VendorCostHistory	vch		(nolock)	ON siv.StoreItemVendorID = vch.StoreItemVendorID
					WHERE
						((@EndScan >= vch.StartDate) AND (@EndScan <= vch.EndDate))
					GROUP BY
						ccik.ItemKey, siv.StoreItemVendorID
				
					CREATE CLUSTERED INDEX idx_costId_VendorCostHistoryId ON #costId (VendorCostHistoryId)
					CREATE NONCLUSTERED INDEX idx_costId_StoreItemVendorId ON #costId (StoreItemVendorId) INCLUDE (VendorCostHistoryId)

					--------------------------------------------------
					-- Get promo insert dates from VendorDealHistory.
					--------------------------------------------------
					SELECT
						cid.StoreItemVendorId	AS StoreItemVendorId,
						MAX(vdh.insertdate)		AS InsertDate
					INTO #promoDate	FROM
						#costId						cid	
						LEFT JOIN VendorDealHistory vdh (nolock) ON cid.StoreItemVendorId	= vdh.StoreItemVendorID
					WHERE
						@EndScan BETWEEN vdh.StartDate AND vdh.EndDate
					GROUP BY
						cid.StoreItemVendorId

					CREATE CLUSTERED INDEX idx_promoDate_StoreItemVendorId ON #promoDate (StoreItemVendorId)
					CREATE NONCLUSTERED INDEX ix_promoDate_InsertDate ON #promoDate (InsertDate) INCLUDE (StoreItemVendorId)

					----------------------------------------------------------
					-- Get promo amounts based on Insert Dates in #promoDates
					----------------------------------------------------------
					SELECT
						cd.StoreItemVendorID	AS StoreItemVendorID,
						cd.VendorCostHistoryID	AS VendorCostHistoryID,
						ISNULL(SUM(	CASE 
										WHEN vdt.CaseAmtType = '%' THEN (vdh.CaseAmt / 100) * ISNULL(vch.UnitCost,0)
										ELSE vdh.CaseAmt
									END),0)		AS PromoAmount
					INTO #promo FROM
						#costId							cd
						INNER JOIN VendorCostHistory	vch (nolock) ON cd.VendorCostHistoryId	= vch.VendorCostHistoryID
						LEFT JOIN #promoDate			prd (nolock) ON cd.StoreItemVendorId	= prd.StoreItemVendorId
						LEFT JOIN VendorDealHistory		vdh (nolock) ON cd.StoreItemVendorId	= vdh.StoreItemVendorId
						LEFT JOIN VendorDealType		vdt	(nolock) ON vdh.VendorDealTypeID	= vdt.VendorDealTypeID
					WHERE
						prd.InsertDate = vdh.InsertDate
						AND @EndSCan BETWEEN vdh.StartDate AND vdh.EndDate
					GROUP BY
						cd.StoreItemVendorID,
						cd.VendorCostHistoryID
	
					CREATE NONCLUSTERED INDEX idx_promo_StoreItemVendorId ON #promo (StoreItemVendorId) INCLUDE (PromoAmount)

					-------------------------------------------------------
					-- Put all needed cost fields into their own table.
					-------------------------------------------------------
					SELECT
						cid.ItemKey				AS ItemKey,
						cid.StoreItemVendorID	AS StoreItemVendorID,
						cid.VendorCostHistoryID AS VendorCostHistoryID,
						vch.UnitCost			AS UnitCost,
						vch.Package_Desc1		AS Package_Desc1,
						ISNULL(vch.UnitCost,0) - ISNULL(pr.PromoAmount,0) + ISNULL(vch.UnitFreight,0)	AS NetCost
					INTO #vendorCost FROM
						#costId						cid
						LEFT JOIN VendorCostHistory vch (nolock) ON cid.VendorCostHistoryId		= vch.VendorCostHistoryID
						LEFT JOIN #promo			pr	(nolock) ON cid.StoreItemVendorId		= pr.StoreItemVendorId
				
					------------------------------------------------------------------------------
					-- Insert the cost of the items which did not have a last received cost.
					------------------------------------------------------------------------------
					INSERT INTO #currentCost 
						SELECT 
							ItemKey = vc.ItemKey,
							CurrentCost = vc.NetCost
						FROM
							#vendorCost vc	

					------------------------------------------------------------------------------
					-- The cost preference for non-avg-cost regions is 1) last received cost, 2) current primary vendor cost, 3) $0.
					------------------------------------------------------------------------------
					INSERT INTO CycleCountItems (CycleCountID, Item_Key, AvgCost) 
						SELECT 
							CycleCountID, 
							L.Item_Key, 
							ISNULL(lrc.LastReceivedCost, ISNULL(cc.CurrentCost, 0))
						FROM 
							CycleCountHeader					H
							INNER JOIN	CycleCountMaster		M	ON M.MasterCountID		= H.MasterCountID
							INNER JOIN	@CCMaster				CCM	ON CCM.MasterCountID	= M.MasterCountID
							INNER JOIN	StoreSubTeam/*SH 3/16/2015 - added this join*/SST ON SST.Store_No = M.Store_No
																	AND SST.SubTeam_No = M.SubTeam_No
							INNER JOIN	CycleCountExternalLoad	L	ON L.Store_No		= M.Store_No 
																	AND L.SubTeam_No	= SST.PS_SubTeam_No/*SH 3/16/2015 - changed from M.SubTeam_No*/
							LEFT JOIN	#lastReceivedCost		lrc	ON L.Item_Key = lrc.ItemKey
							LEFT JOIN	#currentCost			cc	ON L.Item_Key = cc.ItemKey
						WHERE 
							[External] = 1
						GROUP BY 
							CycleCountID, L.Item_Key, L.Store_No, M.Store_No, M.SubTeam_No, M.EndScan, lrc.LastReceivedCost, cc.CurrentCost

					END

			INSERT INTO CycleCountHistory (CycleCountItemID, ScanDateTime, [Count], Weight, PackSize, IsCaseCnt) 
				SELECT 
					cci.CycleCountItemID, 
					ISNULL(CCM.DateKey, @EndScan),
					CASE WHEN i.CostedByWeight = 1
						THEN 0 
						ELSE L.Weight + L.Quantity  					
					END, 
					CASE WHEN i.CostedByWeight = 1
						THEN L.Weight + L.Quantity 
						ELSE 0 
					END, 
					CASE WHEN L.IsCaseCnt = 1 
						THEN L.PackSize
						ELSE dbo.FN_GetExePack(i.Package_Desc1, i.Package_Desc2, i.CostedByWeight) 
					END, 
					L.IsCaseCnt
				
				FROM 
					CycleCountExternalLoad					L
					INNER JOIN Item				(nolock)	i	ON	i.Item_Key			= L.Item_Key
					INNER JOIN StoreSubTeam/*SH 3/16/2015 - added this join*/SST ON SST.Store_No			= L.Store_No
																AND SST.PS_SubTeam_No		= L.SubTeam_No
					INNER JOIN CycleCountMaster				M	ON	M.Store_No			= L.Store_No 
																AND M.SubTeam_No		= SST.SubTeam_No/*SH 3/16/2015 - changed from L.SubTeam_No*/
					INNER JOIN @CCMaster					CCM	ON	CCM.MasterCountID	= M.MasterCountID
					INNER JOIN CycleCountHeader				H	ON	M.MasterCountID		= H.MasterCountID
					INNER JOIN CycleCountItems				cci	ON	cci.CycleCountID	= H.CycleCountID 
																AND cci.Item_Key		= L.Item_Key
				WHERE 
					[External] = 1 
				
			DELETE FROM CycleCountExternalLoad

		COMMIT TRAN
	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT <> 0
			ROLLBACK TRAN
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('LoadCycleCountExternal failed with error number: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadCycleCountExternal] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadCycleCountExternal] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadCycleCountExternal] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadCycleCountExternal] TO [IRMAReportsRole]
    AS [dbo];

