SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MatchOrderInvoiceCosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[MatchOrderInvoiceCosts]
GO
CREATE PROCEDURE [dbo].[MatchOrderInvoiceCosts] 
	@OrderHeader_ID		int,  
	@User_ID			int,  
	@IsSuspended		bit = 0 OUTPUT 
	 
AS  

-- *********************************************************************************************************************************
-- Procedure: MatchOrderInvoiceCosts()
--    Author: n/a
--      Date: n/a

--
-- Description:
-- This procedure is called from UpdateOrderClosed as part of 3-Way Matching
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012/11/12   TTL     8282    Removed line 'SELECT @orderlinkorder' that was causing "Unable to cast object of type 'System.Int32' to type 'System.String'" errors during
--								OL import (in DataMonkey log file) when POs would come in as 'B' (billed) status rather than the normal 'N' (new) status first.
--								The error does not occur the second time a billed order is processed.
--								Also commented-out a group of PRINT (debug) statements.
-- 2011/09/23   RE      xxxx    Added support for OrderMatchingInformation to log the values used for "3way" Matching.
-- 2011/06/29   MZ      2277    Modified the code to get @AllocatedCharges. The allocated charges include allowance and charges. Only charges and positive allowance will be 
--                              retrieved for the PO in the @AllocatedCharges calculation.
-- 2011/03/10   TTL     1355    Re-added the allocated charges logic after clarifying the expected behavior.  Fixed issue where the allocated charges were being subtracted
--								a second time at the bottom of this SP where it compares ordered and invoice costs.  Combined statements that were setting @ReceivedInvoiceCost.  Misc comment cleanup.
-- 2011/02/17   TTL     1355    Removed allocated charges from being factored into PO-suspension.  Removed all @AllocatedCharges references to make update "cleaner".
-- 2010/01/13	BBB		13836	Code formatting;
-- 2009/12/28	BSR		11491	Added logic to subtract Allocated Charges from 
--								ReceivedInvoiceCost to avoid suspension
-- 2009/06/23	DS		xxxxx	set InternalVendorFlag - this prevents EXE-processed
--								orders from getting suspended when it's a WFM vendor;
--								use InternalVendorFlag to automatically approve 
--								DC > Internal Vendor orders to keep them from getting suspende
-- 2011/12/08	BBB		3744	added OrderedCost;
-- 2011/12/12	KM		3744	added logic to prevent suspension of $0 free-fill invoices; minor coding standards updates
-- 2011/12/29	KM		3744	Replaced SUM(ReceivedItemCost) with call to oh.AdjustedReceivedCost;
-- 2011/12/31	BBB		3744	0$ PO Logic Shift; removed superfluous select;
-- 2012/01/30	BBB		4575	change from LineItem to Received in NonMatch calculation for disparity in order VS receive; added in CostByWeightLogic to PAC vendor logic;
-- 2012/02/03	BBB		4662	Added count of Invoice items vs PO items logic as initial logic wasnt capturing these issues;
-- 2012/02/08	BBB		4786	Added logic so that Rec 0 and eInv 0 line items do not suspend;
-- 2012/03/22	DBS		4863	Added logic so that OL Credit orders don't suspending - avoid 3-way matching for OL orders;
-- 2012/08/13   MD	    7252    Added logic to compute the einvoice non match items comparing with order item (joining on item_key)
-- 2012/08/16	BBB		7173	Modified suspension logic; added unsuspension logic;
-- 2012/09/13	BBB		5150	Treated PAC credit suspensions; tweaked tolerance logic;
-- 2013/03/18	KM		11551	Changed the assignment of the @AllocatedCharges variable so that positive-value allowances are not added
--								in as charges;
-- 2013/03/20	BAS		8568	Added logic to unsuspend OrderItems on PAC orders that had previously suspended.
--								Also, consolidated queries to some #temp tables which were used multiple times (e.g. #nonMatchItemsList)
-- 2013/04/12	KM		11964	Incorporate BAS 8568 into the 4.7.0.1 build;
-- 2013/04/16	KM		11983	No longer reverse the sign of allocated charges on credit orders;
-- 2013/05/09	KM		12233	Subtract line item charges from the invoice total so that they do not affect the matching calculation (similar
--								to how allocated charges are handled);
-- *********************************************************************************************************************************

BEGIN
	SET NOCOUNT ON  
	-- OrderType_ID Values: 1 = Purchase, 2 = Distribution, 3 = Transfer

	--**************************************************************************
	--Declare internal variables
	--**************************************************************************      
  DECLARE 
		@Variance				money,
		@InvoiceCost			money,
		@OrderCost				money,
		@Error_No				int,
		@Tolerance				decimal(5,2), 
		@Tolerance_Amount		smallmoney,
		@InvoiceData			int, 
		@DocumentData			int, 
		@OrderType_ID			int,   
		@ReceivedInvoiceCost	decimal(18,4), 
		@ReceivedOrderedCost	decimal(18,4), 
		@ReceivedCost			decimal(18,4),  
		@unit					int,
		@pound					int, 
		@PayOrderedCost			bit, 
		@IsEInvoiceLoaded		bit, 
		@InternalVendor			bit, 
		@AllocatedCharges		money,
		@LineItemCharges		money,
		@OrderLinkOrder			int,
		@IsReturnOrder			bit


	--**************************************************************************
	--Populate internal variables
	--**************************************************************************       
	SELECT @Error_No = 0  
	SELECT @Error_No = @@ERROR

	--**************************************************************************
	--Transaction
	--**************************************************************************      
	BEGIN TRAN  	     
		
		-- Dist and Transfer orders do not 3-way Match and remain in closed state
		-- set InternalVendorFlag - this prevents EXE-processed orders from getting suspended when it's a WFM vendor.  
		IF @Error_No = 0  
		BEGIN  
			SELECT 
				@OrderType_ID		= oh.OrderType_ID, 
				@PayOrderedCost		= oh.PayByAgreedCost,  
				@IsReturnOrder		= oh.Return_Order,
				@IsEInvoiceLoaded	= CASE 
										WHEN (oh.eInvoice_Id IS NULL) THEN 
											0 
										ELSE 
											1 
									  END,
				@InternalVendor		= CASE 
										WHEN v.Customer = 0 THEN   
											CASE 
												WHEN v.InternalCustomer = 0 THEN 
													IsNull(v.WFM, 0) 
												ELSE 
													0 
											END   
										ELSE 
											0 
									  END   
			FROM 
				dbo.OrderHeader			(nolock) oh
				INNER JOIN dbo.Vendor	(nolock) v	ON oh.Vendor_ID = v.Vendor_ID
			WHERE 
				oh.OrderHeader_ID = @OrderHeader_ID   
			  
			SELECT @Error_No = @@ERROR  
		END  

		-- The order passed three way matching. Move the order to the approved state 
		IF @Error_No = 0  
		BEGIN  			
			IF @OrderType_ID = 1 AND @InternalVendor = 1 
			BEGIN  
				EXEC dbo.UpdateOrderApproved @OrderHeader_ID, @User_ID, 0, '' 
				
				SELECT @IsSuspended = 0  
	  
				SELECT @Error_No = @@ERROR  
			END  
		END  
			   
		-- Check to see if invoice or document data exists for the order.   
		IF @Error_No = 0  
		BEGIN  
			IF @OrderType_ID = 1 AND @InternalVendor = 0 
			BEGIN    
				IF @Error_No = 0  
				BEGIN  
					SELECT 
						@InvoiceData = COUNT(1) 
					FROM 
						dbo.OrderHeader   (nolock)
					WHERE 
						OrderHeader_ID = @OrderHeader_ID   
						AND InvoiceNumber IS NOT NULL  
					
					SELECT @Error_No = @@ERROR  
				END  
	  
				IF @Error_No = 0  
				BEGIN  
					SELECT 
						@DocumentData = COUNT(1) 
					FROM 
						dbo.OrderHeader   (nolock)
					WHERE 
						OrderHeader_ID = @OrderHeader_ID   
						AND VendorDoc_ID IS NOT NULL  
				
					SELECT @Error_No = @@ERROR  
				END  
		 
				-- Perform three way matching on the invoice.  Orders are only approved if the three way matching is  
				-- a success.  Otherwise, the order is suspended.  
				IF @Error_No = 0  
				BEGIN 
					-- Are invoice and document data missing?  
					IF @InvoiceData < 1 AND @DocumentData < 1  
					BEGIN 
						-- Update the matching validation code data in the OrderHeader table to record the failure.  
						UPDATE 
							OrderHeader  
						SET 
							MatchingValidationCode	= 504,  
							MatchingUser_Id			= @User_ID,  
							MatchingDate			= GETDATE()  
						WHERE
							OrderHeader_ID = @OrderHeader_ID  

						SELECT @IsSuspended = 1  
	  
						SELECT @Error_No = @@ERROR  
					END 

					ELSE  
					BEGIN 
						-- Is just document data entered?  
						IF (@Error_No = 0) AND (@InvoiceData < 1) AND (@DocumentData >= 1)  
						BEGIN
							-- Update the matching validation code data in the OrderHeader table to record the failure.  
							UPDATE 
								OrderHeader  
							SET 
								MatchingValidationCode	= 505,  
								MatchingUser_Id			= @User_ID,  
								MatchingDate			= GETDATE()  
							WHERE 
								OrderHeader_ID = @OrderHeader_ID  
							
							SELECT @IsSuspended = 1  
	  
							SELECT @Error_No = @@ERROR  
						END 

						ELSE  
						BEGIN 
							IF (@Error_No = 0)   
							BEGIN 
							
								-- Invoice data is present.  Perform three way matching.  
								SELECT 
									@InvoiceCost = ISNULL(SUM(ISNULL(InvoiceCost,0)), 0)   
								FROM 
									dbo.OrderInvoice  (nolock)
								WHERE 
									OrderHeader_ID = @OrderHeader_ID  
	  
								SELECT @Error_No = @@ERROR  
			   
								/*
									[Get any Allocated Charges involved with Invoice]
									Invoice charges should only be positive (+) amounts, however, at the time of this writing, via the invoice-doc screen,
									the IRMA UI allows one to add charge types, such as "inv_discount_amt", that are actually negative (-) amounts.
									This logic does not handle (-) amounts everywhere because it subtracts the @AllocatedCharges value, which results
									in the charge being added.																									 
								*/
								-- SAC Types: 1 - Allocated, 2 - Non-allocated, 3 - Line Item
								SELECT 
									@AllocatedCharges = ISNULL(SUM(ISNULL(Value,0)),0)
								FROM 
									dbo.OrderInvoiceCharges (NOLOCK)
								WHERE 
									OrderHeader_ID = @OrderHeader_ID
									AND SACType_ID = 1
									AND (IsAllowance IS NULL OR IsAllowance = 0) 

								SELECT 
									@LineItemCharges = ISNULL(SUM(ISNULL(Value,0)),0)
								FROM 
									dbo.OrderInvoiceCharges (NOLOCK)
								WHERE 
									OrderHeader_ID = @OrderHeader_ID
									AND SACType_ID = 3
									

								-- If a record exists for this order in OMI then update else create a new one.
								-- this is a step towards logging what values are used for matching on an order.
								-- only 1 record per order for now. maybe this should be historical in the future?
																														
								IF EXISTS (SELECT OrderHeader_Id FROM dbo.OrderMatchingInformation WHERE OrderHeader_Id = @OrderHeader_ID)
									BEGIN
										UPDATE  dbo.OrderMatchingInformation SET
											InvoiceCost			= @InvoiceCost,
											AllocatedCharges	= @AllocatedCharges
										WHERE   
											OrderHeader_Id = @OrderHeader_ID 
									END
								ELSE
									BEGIN
										INSERT INTO dbo.OrderMatchingInformation
											( 
												OrderHeader_Id,
												InvoiceCost,
												AllocatedCharges
											)
										VALUES 
											( 
												@OrderHeader_ID,
												@InvoiceCost,
												@AllocatedCharges
											)
									END
							
							    -- Set InvoiceCost
								SET @InvoiceCost = @InvoiceCost - @AllocatedCharges - @LineItemCharges

								-- Logic to get received ordered and invoice costs per P2P v1 code  
								SELECT @pound = Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'LB'  
								SELECT @unit = Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'EA'      
					
								SELECT	
										@ReceivedOrderedCost	= oh.AdjustedReceivedCost,
										@ReceivedInvoiceCost	= SUM(ISNULL(dbo.fn_ReceivedInvoiceCost(InvoiceExtendedCost, 
																			CASE WHEN CostedByWeight = 1 THEN InvoiceTotalWeight ELSE UnitsReceived END,
																			eInvoiceQuantity, CostedByWeight, QuantityUnit, OI.Package_Desc1, 
																			OI.Package_Desc2, OI.Package_Unit_ID, UnitsReceived, @unit, @pound),0)),  
										@ReceivedCost			= ISNULL(oh.AdjustedReceivedCost, 0)
								FROM 
									OrderHeader				(nolock) oh
									INNER JOIN	OrderItem	(nolock) oi ON oh.OrderHeader_ID	= oi.OrderHeader_ID
									INNER JOIN	Item		(nolock) i	ON oi.Item_Key			= i.Item_Key									
								WHERE 
									oh.OrderHeader_ID = @OrderHeader_ID  
								GROUP BY
									oh.AdjustedReceivedCost
				   
								-- Round
								SET @InvoiceCost = ROUND(ISNULL(@InvoiceCost, 0), 2)
								SET @ReceivedOrderedCost = ROUND(@ReceivedOrderedCost, 2)
								SET @ReceivedCost = ROUND(@ReceivedCost, 2)


								/*
									If there is no electronic invoice (no electronic invoice exchange), use the total from the OrderInvoice table instead.
									Task #11420 vendors without PayAgreedCost need to use InvoiceCost against the variance.
									(The above two were in separate statements, but are combined here.)
									Round the value too.
								*/
								SELECT @ReceivedInvoiceCost = CASE
									WHEN ISNULL(@ReceivedInvoiceCost, 0) = 0 OR (@PayOrderedCost = 0 AND @IsEInvoiceLoaded = 1)
									THEN @InvoiceCost 
									ELSE ROUND(@ReceivedInvoiceCost, 2)
									END

								
								-- Create temp table for NonMatchItems since it is used multiple times:
								SELECT *
								INTO #nonMatchItemsList
								FROM 
									(SELECT
										oi.OrderItem_ID,
										i.Item_Key,
										i.CatchWeightRequired,
										oi.InvoiceExtendedCost,
										oi.ReceivedItemCost,
										oi.QuantityReceived,
										oh.Return_Order
									FROM 
										OrderHeader				(nolock) oh
										JOIN EInvoicing_Item	(nolock) eii	ON	oh.eInvoice_Id		= eii.EInvoice_id
										LEFT JOIN OrderItem		(nolock) oi		ON	oh.OrderHeader_ID	= oi.OrderHeader_ID
																				AND eii.Item_Key		= oi.Item_Key
										LEFT JOIN Item			(nolock) i		ON	eii.Item_Key		= i.Item_Key
									WHERE 
										oh.OrderHeader_ID		= @OrderHeader_ID
										AND oi.ApprovedByUserId IS NULL

									UNION

									SELECT
										oi.OrderItem_ID,
										i.Item_Key,
										i.CatchWeightRequired,
										oi.InvoiceExtendedCost,
										oi.ReceivedItemCost,
										oi.QuantityReceived,
										oh.Return_Order
									FROM 
										OrderHeader					(nolock) oh
										JOIN OrderItem				(nolock) oi		ON	oh.OrderHeader_ID	= oi.OrderHeader_ID
										LEFT JOIN EInvoicing_Item	(nolock) eii	ON	oh.eInvoice_Id		= eii.EInvoice_id
																					AND eii.Item_Key		= oi.Item_Key
										LEFT JOIN Item				(nolock) i		ON	oi.Item_Key			= i.Item_Key
									WHERE 	
										oh.OrderHeader_ID		= @OrderHeader_ID
										AND oi.ApprovedByUserId IS NULL) as inner_result
								WHERE
									(
									--non-matching logic
									ROUND(ISNULL(ReceivedItemCost, 0), 2, 1) <>	CASE ISNULL(Return_Order, 0)
																					WHEN 1 THEN
																						ABS(ROUND(ISNULL(InvoiceExtendedCost, 0), 2, 1))
																					ELSE
																						ROUND(ISNULL(InvoiceExtendedCost, 0), 2, 1)
																				END
									--tolerance logic
									AND 
									(
										(ROUND(ISNULL(ABS(ReceivedItemCost), 0), 2, 1)		-	ROUND(ISNULL(ABS(InvoiceExtendedCost), 0), 2, 1)) > .01
										OR
										(ROUND(ISNULL(ABS(InvoiceExtendedCost), 0), 2, 1)	-	ROUND(ISNULL(ABS(ReceivedItemCost), 0), 2, 1)) > .01
									)
									)
								
								-- Populate flag for to determine if there are any suspended items on a pay by agreed cost order
								DECLARE @NonMatchItems AS int = 0
								IF @PayOrderedCost = 1
									BEGIN
										SET @NonMatchItems = ISNULL((SELECT COUNT(*) FROM #NonMatchItemsList), 0)
									END


								SELECT @Error_No = @@ERROR  
								/*
									Initialize the tolerance allowed for this order.  
									Tolerances are set in the InvoiceMatchingTolerance, InvoiceMatchingTolerance_StoreOverride,  
									and InvoiceMatchingTolerance_VendorOverride configuration tables.   
								*/
								IF (@Error_No = 0)  
								BEGIN  
									SELECT @Tolerance = Tolerance, @Tolerance_Amount = Tolerance_Amount from dbo.fn_GetMatchingTolerance(@OrderHeader_ID)  

									SELECT @Error_No = @@ERROR  
								END 
				  
								-- Set the variance allowed for the invoice cost by applying the tolerance to the cost.  
								IF (@Error_No = 0)  
								BEGIN  
									SELECT @Variance = ABS(@ReceivedOrderedCost) * (ISNULL(@Tolerance,0) / 100)  

									SELECT @Error_No = @@ERROR  
								END  
		   
								-- Find the lowest variance between the tolernace amount and the percentage of the received order cost
								IF (@Error_No = 0)
								BEGIN 
									SELECT @Variance = CASE 
														WHEN @Tolerance IS NOT NULL AND isNULL(@Variance, 0) <= @Tolerance_Amount THEN 
															@Variance
														WHEN @Tolerance_Amount IS NOT NULL AND isNULL(@Variance, 0) >= @Tolerance_Amount THEN 
															@Tolerance_Amount
														WHEN @Tolerance_Amount IS NULL AND @Tolerance IS NULL THEN 
															NULL
														ELSE 
															@Variance 
													   END
													   
									UPDATE  dbo.OrderMatchingInformation
									SET     ReceivedOrderedCost = @ReceivedOrderedCost
										   ,ReceivedInvoiceCost = @ReceivedInvoiceCost
										   ,ReceivedCost = @ReceivedCost
										   ,Tolerance = @Tolerance
										   ,ToleranceAmt = @Tolerance_Amount
										   ,Variance = @Variance
										   ,Info = cast(@ReceivedInvoiceCost as varchar(100)) + ' - ' + cast(@ReceivedOrderedCost as varchar(100)) + ' <= ' +  cast(@Variance as varchar(100)) 
									WHERE   OrderHeader_Id = @OrderHeader_ID 													   

									SELECT @Error_No = @@ERROR  
								END 
								
								-- debug info
								/*
								print 'Invoice Cost: ' + cast(@InvoiceCost as varchar(100))
								print '@ReceivedInvoiceCost: ' + cast(@ReceivedInvoiceCost as varchar(100))
								print '@ReceivedOrderedCost: ' + cast(@ReceivedOrderedCost as varchar(100))
								print '@ReceivedCost: ' + cast(@ReceivedInvoiceCost as varchar(100))
								print '@Tolerance: ' + cast(@Tolerance as varchar(100))
								print '@Tolerance_Amount: ' + cast(@Tolerance_Amount as varchar(100))
								print '@Variance: ' + cast(@Variance as varchar(100))
								print 'Calculation: ABS(@ReceivedInvoiceCost - @ReceivedOrderedCost) <= @Variance'
								print cast(@ReceivedInvoiceCost as varchar(100)) + ' - ' + cast(@ReceivedOrderedCost as varchar(100)) + ' <= ' +  cast(@Variance as varchar(100))
								*/
								
								--20120322 - DBS - TFS 4863 - OL Credit orders are suspending - avoid 3-way matching for OL orders
								Select @orderlinkorder = CASE WHEN ISNULL(OES.ID, 0) > 0 THEN 1 ELSE 0 END
								FROM dbo.OrderHeader oh (NOLOCK) 
								LEFT JOIN dbo.OrderExternalSource (NOLOCK) OES ON oh.OrderExternalSourceID = OES.ID
								where 
								oh.orderheader_id = @OrderHeader_ID
								AND OES.Description = 'OrderLink'	

								-- Compare invoice and ordered cost.  Allow $0 free-fill invoices to avoid suspension.
								IF (@Error_No = 0) AND ((@orderlinkorder = 1) OR
									 (@PayOrderedCost = 0 AND (ABS(@ReceivedInvoiceCost - @ReceivedOrderedCost) <= @Variance AND (@ReceivedInvoiceCost <> 0) OR (@Variance is NULL))
									OR	(@ReceivedInvoiceCost = 0 AND @ReceivedOrderedCost = 0))
									OR (@PayOrderedCost = 1 AND @NonMatchItems = 0))
								BEGIN  
									-- The order passed three way matching.  
									-- Clear any previous suspension data
									UPDATE 
										OrderHeader
									SET 
										MatchingValidationCode		= 0,  
										MatchingUser_Id				= @User_ID,  
										MatchingDate				= GETDATE(), 
										InvoiceDiscrepancy			= NULL,                                                     
										InvoiceDiscrepancySentDate	= NULL  
									WHERE
										OrderHeader_ID = @OrderHeader_ID

									UPDATE 
										OrderItem
									SET
										LineItemSuspended = NULL
									WHERE
										OrderHeader_ID = @OrderHeader_ID


									-- Move the order to the approved state so that it can be picked up by the AP Upload process.  
									EXEC dbo.UpdateOrderApproved @OrderHeader_ID, @User_ID, 0, '' -- SUCCESS MATCHING CODE  
									
									SELECT @IsSuspended = 0  

									SELECT @Error_No = @@ERROR  
								END  
								ELSE  
								BEGIN  
									-- The order did not pass three way matching.  
									-- Update the matching validation code data in the OrderHeader table to record the failure.  
									UPDATE 
										OrderHeader  
									SET 
										MatchingValidationCode	= 500,  
										MatchingUser_Id			= @User_ID,  
										MatchingDate			= GETDATE()  
									WHERE 
										OrderHeader_ID = @OrderHeader_ID  

									/*
										If this is a PayByAgreedCost Vendor suspend lineitems where the invoice doesnt match the po
									*/
									UPDATE
										OrderItem
									SET
										LineItemSuspended = 1
									FROM
										#nonMatchItemsList	nmi
										JOIN OrderItem		oi ON nmi.OrderItem_ID = oi.OrderItem_ID
									
									-- If this is the second time or more updating Pay by Agreed Cost order, unsuspend line items that now match
									-- This step is needed since OrderItems that suspended originally still have LineItemSuspended column set to 1 up to this point
									SELECT oi.OrderItem_ID
									INTO #formerlyNonMatchedItems
									FROM 
										OrderItem oi (nolock)
									WHERE
										oi.OrderHeader_ID = @OrderHeader_ID
										AND oi.LineItemSuspended = 1
										AND NOT EXISTS (SELECT * FROM #nonMatchItemsList nmi WHERE nmi.OrderItem_ID = oi.OrderItem_ID)
									
									-- Run update if there are Items to update
									IF (SELECT COUNT(*) FROM #formerlyNonMatchedItems) >  0
									BEGIN
										UPDATE OrderItem
										SET LineItemSuspended = 0
										WHERE 
											EXISTS (SELECT * FROM #formerlyNonMatchedItems fnm WHERE fnm.OrderItem_ID = OrderItem.OrderItem_ID)
									END

									-- Set IsSuspended Flag
									SELECT @IsSuspended = 1  

									SELECT @Error_No = @@ERROR  
								END  

								IF (@Error_No = 0) AND (@PayOrderedCost = 1) and (@IsEInvoiceLoaded = 1)  
								BEGIN  
									UPDATE
										OrderHeader  
									SET 
										InvoiceDiscrepancy = CASE WHEN (@ReceivedCost <> @InvoiceCost) THEN 1 ELSE 0 END,                                                     
										InvoiceDiscrepancySentDate = NULL  
									WHERE 
										OrderHeader_ID = @OrderHeader_ID  

									SELECT @Error_No = @@ERROR  
								END
							END -- no error set  
						END -- three way matching  
					END -- invoice or document data entered  
				END -- no error set  
			END -- purchase orders logic  
		END -- no error set  
	  
	IF @Error_No = 0  
	BEGIN  
		COMMIT TRAN  
		SET NOCOUNT OFF  
	END  
	
	ELSE  
	BEGIN  
		ROLLBACK TRAN  
		DECLARE @Severity smallint  
		SELECT	@Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)  
		SET NOCOUNT OFF  
		RAISERROR ('MatchOrderInvoiceCosts failed with @@ERROR: %d', @Severity, 1, @Error_No)  
	END    
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO