IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'UpdateOrderRefreshCosts')
BEGIN
	DROP PROCEDURE dbo.UpdateOrderRefreshCosts
END
GO

CREATE Procedure [dbo].[UpdateOrderRefreshCosts]
	@OrderHeader_ID		int,
	@RefreshSource		varchar(30) = null,
	@User_ID			int			= null,
	@IsSuspended		bit			= 0 OUTPUT 

AS

-- **************************************************************************
-- Procedure: UpdateOrderRefreshCosts()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple locations within IRMA client to update
-- ordered costs as needed.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 09/14/2010	BBB   			13444	Applied logic to ReturnOrder costs to use either
--										oi.Cost or oi.UnitCost based upon IU.IsPackageUnit
-- 01/18/2011	Tom Lux   		759		Added variable that pulls lead-time days for the v.
--										Updated fn_VendorCostAll() call to add lead-time to cost date passed in (no update for order type 2).
-- 01/24/2011	Tom Lux   		759		Added update of OrderHeader.POCostDate to end of procedure.
-- 02/18/2011	Tom Lux   		1390	Added logic to chose oh.Expected_Date as cost date (@CostDate var) for POET orders.
-- 04/05/2011	Dave Stacey		1720	Added criteria for Costed by Weight items so they don't refresh cost a second time.
-- 08/15/2011	DBS				1826	Changed the einvoice import routine, following that by dividing invcost by quantity 
--										for the second cost recalc if needed
-- 09/28/2011   MD	            3021    Rollback the update done in 4.2.5 for 1826, that is breaking ReceivedItemCost Calculations 
-- 12/12/2011   MZ              3748    Removed the call UpdateVendorACKCostHistoryQueue stored procedure. The SP becomes obsolete in 4.4.      
-- 12/21/2011	BBB				3744	coding standards; added OrderedCost, AdjustedReceivedCost, OriginalReceivedCost aggregations to OH;
-- 12/29/2011	BBB				3744	InvoiceTotalCost aggregation added;
-- 01/04/2011	BJL				3751	Use @VendorCost if there is an order level or item level Percent Discount. Update NetVendorItemDiscount.
-- 01.09.2012	BBB				4200	Corrected issue when UORC called from OrderItems on every form close and thusly @OrderItems being null;
-- 01.13.2012	BBB				4242	Modified update of ReceivedItemCost with InvoiceCost value to stand apart;
-- 02.06.2012	BBB				4720	update OrderInvoice regardless of close status to alleviate issue with eInvoicing closing orders before UORC was called;
-- 02.08.2012	BBB				4720	removed redundant call to CalcOICosts based upon feedback from MD;
-- 10/10/2012	BAS				8133	Added a varchar(30) parameter so that we can track which form or stored procedure calls UORC. 
--										This parameter updates the OrderHeader table based on the new table created called UpdateOrderRefreshSource.
-- 2013-04-03	KM				5689	No longer reverse the sign for non-allocated charges on credit orders;
-- 2013-07-24	KM				13083	Change the definition of the cursor which populates @OrderItem to use oi.CostUnit instead of i.Cost_Unit_Id. i.Cost_Unit_Id is 
--										frequently null and does not seem to be the proper field for determining the cost unit; During the processing of the cursor, only
--										update cost unit from fn_VendorCostAll() if a valid cost is found;
-- 2013-07-29	KM				12709	Separate the logic for transfers and credits during the processing of the cursor to prevent the @Cost variable from incorrectly being
--										assigned the unit cost rather than case cost value for credits.
-- **************************************************************************

BEGIN          
	SET NOCOUNT ON          
	
	BEGIN TRY                       
		BEGIN TRANSACTION       
 	  
			PRINT '## UpdateOrderRefreshCosts: ' + cast(@OrderHeader_ID as varchar(100))

			DECLARE		
				@CostDate					SMALLDATETIME,        
				@Store_No					INT,        
				@Vendor_ID					INT,        
				@OrderQuantityDiscount		DECIMAL(18,4),        
				@OrderDiscountType			INT,        
				@PayOrderedCost				BIT,        
				@CCCost						MONEY,        
				@HandlingCharge				MONEY,       
				@OrderItem_ID				INT,         
				@Item_Key					INT,         
				@Cost						MONEY,         
				@Freight					MONEY,         
				@AdjustedCost				MONEY,   
				@OrigReceivedItemCost		MONEY,
				@QuantityOrdered			DECIMAL(18,4),        
				@QuantityUnit				INT,        
				@QuantityReceived			DECIMAL(18,4),        
				@Total_Weight				DECIMAL(18,4),        
				@CostUnit					INT,        
				@VendorCostUnit				INT,        
				@ItemQuantityDiscount		DECIMAL(18,4),        
				@ItemDiscountType			INT,        
				@FreightUnit				INT,         
				@MarkupPercent				DECIMAL(18,4),        
				@Package_Desc1				DECIMAL(9,4),        
				@Package_Desc2				DECIMAL(9,4),        
				@Package_Unit_ID			INT,        
				@IsCostedByWeight			BIT,        
				@INVCost					MONEY,        
				@LandedCost					MONEY,   
				@NetDiscount				MONEY,       
				@MarkupCost					MONEY,        
				@LineItemCost				MONEY,        
				@LineItemFreight			MONEY,        
				@ReceivedItemCost			MONEY,        
				@ReceivedItemFreight		MONEY,        
				@Freight3Party				MONEY,  
				@UnitCost					MONEY,        
				@UnitExtCost				MONEY,          
				@OrderType_ID				INT,        
				@Return_Order				BIT,        
				@VendorCostHistoryID		INT,        
				@Variance					DECIMAL(38,28),         
				@InvoiceCost				MONEY,         
				@ReceivedInvoiceCost		DECIMAL(38,28),        
				@ReceivedOrderedCost		DECIMAL(38,28),        
				@ReceivedCost				DECIMAL(38,28),         
				@IsApproved					BIT,         
				@unit						INT,         
				@pound						INT,        
				@INVQuantity				DECIMAL(18,4),              
				@VendorCost					MONEY,        
				@UnitID						INT,         
				@PoundID					INT,        
				@VendStore_No				INT,        
				@Transfer_SubTeam			INT,        
				@InvoiceFreight				MONEY,  
				@CatchWeightRequired		INT,       
				@AvgCost					MONEY,  
				@CloseDate					DATETIME,  
				@ApprovedDate				SMALLDATETIME,  
				@EInvoiceId					INT,  
				@EXEWarehouse				SMALLINT,
				@IsOrderLinkImport			bit,
				@VendorLeadTimeDays			INT
			
			SELECT	@UnitID		= Unit_ID	FROM ItemUnit (nolock) WHERE UnitSysCode = 'unit'        
			SELECT	@PoundID	= Unit_ID	FROM ItemUnit (nolock) WHERE UnitSysCode = 'lbs'        
	   
			SELECT	
				@EXEWarehouse = s.EXEWarehouse  
			FROM
				OrderHeader				(nolock)	oh  
				INNER JOIN Vendor		(nolock)	v	ON	oh.Vendor_ID	= v.Vendor_ID  
				INNER JOIN Store		(nolock)	s	ON	v.Store_No		= s.Store_No  
			WHERE	
				oh.OrderHeader_ID = @OrderHeader_ID  
				
			/*  
			############################################################################################################################################################  
			-- @CostDate is used for the VendorCostHistory lookup below - if the PO is not to an external vendor, use the SENT date if available or current date otherwise,
			-- except for POET orders, in which case use Expected Date.
			-- This is for Distribution PO's (OrderType_ID = 2).   
			-- OrderType_ID = 1				: Purchase   
			-- OrderExternalSourceID = 5	: POET 
			-- OrderExternalSourceID = 3	: OrderLink
			############################################################################################################################################################  
			*/  
  
			SELECT	
				@Vendor_ID				=	oh.vendor_id,         
				@OrderQuantityDiscount	=	oh.QuantityDiscount,         
				@OrderDiscountType		=	oh.DiscountType,         
				@CostDate				=	CASE
												WHEN oh.OrderType_ID = 1 THEN	CASE 
																					WHEN oh.OrderExternalSourceID = 5 THEN oh.Expected_Date
																					ELSE oh.SentDate
																				END 
												ELSE CloseDate 
											END,      
				@Store_No				=	rl.store_no,         
				@OrderType_ID			=	oh.OrderType_ID,    
				@Return_Order			=	oh.Return_Order,                
				@PayOrderedCost			=	oh.PayByAgreedCost,     
				@VendStore_No			=	v.Store_No,        
				@Transfer_SubTeam		=	oh.Transfer_Subteam,        
				@InvoiceFreight			=	ISNULL(oi.InvoiceFreight, 0),  
				@CloseDate				=	CloseDate,  
				@ApprovedDate			=	ApprovedDate,  
				@EInvoiceId				=	eInvoice_Id,
				@IsOrderLinkImport		=	CASE
												WHEN OrderExternalSourceID = 3 THEN 1  
												ELSE 0
											END		
			FROM	
				OrderHeader				(nolock)	oh
				INNER JOIN Vendor		(nolock)	rl	ON	oh.ReceiveLocation_ID   = rl.Vendor_ID 
				INNER JOIN Vendor		(nolock)	v	ON	oh.Vendor_ID			= v.Vendor_ID
				LEFT  JOIN OrderInvoice (nolock)	oi	ON	oh.OrderHeader_ID       = oi.OrderHeader_ID 
	
			WHERE	
				oh.OrderHeader_ID = @OrderHeader_ID        
		
			SELECT	@CostDate = ISNULL(@CostDate, GETDATE())        
		
			-- Get cost lead-time (number of days to add to cost date) for v.  If vendor doesn't use lead-times, value will be zero.
			SELECT	@VendorLeadTimeDays = dbo.fn_GetLeadTimeDays(@Vendor_ID)
							
			DECLARE	@OrderItem	TABLE 
			(	
				OrderItem_ID			INT,  
				Cost					MONEY,  
				UnitCost				MONEY,  
				UnitExtCost				MONEY,  
				CostUnit				INT,  
				Freight					MONEY,  
				FreightUnit				INT,  
				LineItemCost			MONEY,  
				LineItemFreight			MONEY,  
				ReceivedItemCost		MONEY, 
				OrigReceivedItemCost	MONEY,
				ReceivedItemFreight		MONEY,  
				LandedCost				MONEY,  
				MarkupCost				MONEY,  
				Freight3Party			MONEY,  
				NetVendorItemDiscount	MONEY,
				VendorCostHistoryID		INT,
				CatchWeightRequired		BIT,
				QuantityReceived		INT,
				PRIMARY KEY (OrderItem_ID)
			)  
  
			DECLARE	IL CURSOR READ_ONLY	FOR SELECT	
				OrderItem_ID,         
				oi.Item_Key,         
				Cost,         
				Freight,         
				AdjustedCost,
				OrigReceivedItemCost,  
				QuantityOrdered,        
				QuantityUnit,        
				QuantityReceived,        
				Total_Weight,                        
				oi.CostUnit,
				oi.QuantityDiscount,        
				oi.DiscountType,        
				FreightUnit,        
				MarkupPercent,        
				oi.Package_Desc1,        
				oi.Package_Desc2,        
				oi.Package_Unit_ID,        
				CostedByWeight,        
				InvoiceCost,           
				eInvoiceQuantity,      
				HandlingCharge,  
				CatchWeightrequired, 
				Freight3Party             
			FROM	
				OrderItem				(nolock)	oi
				INNER JOIN Item			(nolock)	i	ON	oi.Item_Key        = i.Item_Key
				INNER JOIN OrderHeader	(nolock)	oh	ON	oi.OrderHeader_Id  = oh.Orderheader_Id 
			WHERE
				oi.OrderHeader_ID = @OrderHeader_ID 
				AND oh.ApprovedDate IS	NULL
		
			OPEN IL FETCH NEXT FROM IL INTO	
				@OrderItem_ID,         
				@Item_Key,         
				@Cost,         
				@Freight,         
				@AdjustedCost,
				@OrigReceivedItemCost,        
				@QuantityOrdered,        
				@QuantityUnit,        
				@QuantityReceived,        
				@Total_Weight,        
				@CostUnit,        
				@ItemQuantityDiscount,        
				@ItemDiscountType,        
				@FreightUnit,         
				@MarkupPercent,        
				@Package_Desc1,        
				@Package_Desc2,        
				@Package_Unit_ID,        
				@IsCostedByWeight,        
				@INVCost,          
				@INVQuantity,      
				@HandlingCharge,  
				@CatchWeightRequired,  
				@Freight3Party      
																	
			WHILE (@@fetch_status <> -1)        
				BEGIN        
					IF (@@fetch_status <> -2)        
						BEGIN  
					
							-- Refresh cost if external or distribution order               
							IF (@OrderType_Id = 2)   
								BEGIN  
									IF (@EXEWarehouse = 1)  
										BEGIN  
									
											-- avgcost  
											SELECT	@Cost = dbo.fn_AvgCostHistory(@Item_Key, @VendStore_No, @Transfer_SubTeam, @CostDate)  
											SELECT	@Cost = dbo.fn_CostConversion(@Cost,	CASE
																								WHEN @IsCostedByWeight = 1 THEN @PoundID 
																								ELSE @UnitID 
																							END, 
													@CostUnit, 
													@Package_Desc1, 
													@Package_Desc2, 
													@Package_Unit_ID)
										END  
									ELSE  
										BEGIN  
									
											-- vendor cost  
											SELECT 
												@VendorCost				= NULL, 
												@VendorCostHistoryID	= NULL,
												@VendorCostUnit			= NULL    
									
											--We may want to use fn_vendorcost function here but currently that function does not account for promo costs  
											SELECT	
												@VendorCost				= UnitCost,  
												@VendorCostHistoryID	= VendorCostHistoryID,  
												@VendorCostUnit			= CostUnit_ID,
												@NetDiscount			= NetDiscount  
											FROM
												dbo.fn_VendorCostAll(@CostDate) vc  
											WHERE	
												vc.Item_Key		= @Item_Key		AND 
												vc.Vendor_ID	= @Vendor_ID	AND 
												vc.Store_no		= @Store_No  
										
											-- Only update @Cost and @CostUnit if a valid record was returned from fn_VendorCostAll() - otherwise, undesirable nulls may find
											-- their way into the OrderItem table when the oi update happens later in this procedure.
											IF @VendorCost IS NOT NULL  
												BEGIN   
													SET @Cost		= @VendorCost - @NetDiscount
													SET @CostUnit	= @VendorCostUnit  
												END  
										END  
								END  
							ELSE  
								BEGIN  
									IF (@OrderType_ID IN (1,2) AND @IsOrderLinkImport = 0)
										BEGIN  
											-- We only need to go through the trouble of checking VendorCostHistory if the order is not a credit.
											IF @Return_Order = 0
												BEGIN	
																			
													-- vendor cost  
													SELECT	
														@VendorCost				= NULL,
														@VendorCostHistoryID	= NULL,
														@VendorCostUnit			= NULL  
									
													--We may want to use fn_vendorcost function here but currently that function does not account for promo costs  
													SELECT	
														@VendorCost				= UnitCost ,  
														@VendorCostHistoryID	= VendorCostHistoryID ,  
														@VendorCostUnit			= CostUnit_ID,
														@NetDiscount			= NetDiscount  
													FROM
														dbo.fn_VendorCostAll(@CostDate + @VendorLeadTimeDays) vc  
													WHERE
														vc.Item_Key		= @Item_Key		AND 
														vc.Vendor_ID	= @Vendor_ID	AND 
														vc.Store_no		= @Store_No  
		   
													-- Only update @Cost and @CostUnit if a valid record was returned from fn_VendorCostAll() - otherwise, undesirable nulls may find
													-- their way into the OrderItem table when the oi update happens later in this procedure.
													IF @VendorCost IS NOT NULL  
														BEGIN   
															SET @Cost		= @VendorCost - @NetDiscount 
															SET @CostUnit	= @VendorCostUnit 
														END  
												END
										
										END								
									ELSE IF (@OrderType_ID = 3)
										BEGIN
											DECLARE @IsPackageUnit	bit

											SET @CostUnit		= @QuantityUnit
											SET @IsPackageUnit	= (SELECT IsPackageUnit FROM ItemUnit WHERE Unit_ID = @CostUnit)

											IF @IsPackageUnit = 1 
												SELECT 
													@Cost =	UnitCost * Package_Desc1
												FROM 
													OrderItem 
												WHERE 
													OrderItem_ID = @OrderItem_ID
											ELSE
												SELECT @Cost = UnitCost FROM OrderItem WHERE OrderItem_ID = @OrderItem_ID
										END   
								END  
			   
			   				-- Use the adjusted cost above all else. Otherwise, if a percent disocunt is applied
							-- then use the Vendor's Cost (Reg Cost) for the Item.
							SELECT @CCCost = CASE WHEN ISNULL(@AdjustedCost, 0) > 0 THEN @AdjustedCost
														ELSE
															CASE WHEN (@OrderDiscountType = 2 OR @ItemDiscountType = 2)
																	THEN ISNULL(@VendorCost, ISNULL(@Cost,0))
																	ELSE ISNULL(@Cost, 0)
														END
												END
							PRINT '# CalculateOrderItemCosts: ItemKey: ' + cast(@Item_Key as varchar(100)) 
				
							EXEC CalculateOrderItemCosts
								@QuantityOrdered,  
								@QuantityUnit,  
								@QuantityReceived,  
								@Total_Weight,  
								@CCCost,
								@CostUnit,  
								@OrderQuantityDiscount,  
								@OrderDiscountType,  
								@ItemQuantityDiscount,  
								@ItemDiscountType,  
								@Freight,  
								@FreightUnit,  
								@MarkupPercent,  
								@Package_Desc1,  
								@Package_Desc2,  
								@Package_Unit_ID,  
								@IsCostedByWeight,
								@CatchWeightRequired,  
								@LandedCost				OUTPUT,  
								@MarkupCost				OUTPUT,  
								@LineItemCost			OUTPUT,  
								@LineItemFreight		OUTPUT,  
								@ReceivedItemCost		OUTPUT,  
								@ReceivedItemFreight	OUTPUT,  
								@UnitCost				OUTPUT,  
								@UnitExtCost			OUTPUT,  
								@HandlingCharge,  
								@Freight3Party  
					   
							INSERT INTO @OrderItem 
							(	
								OrderItem_ID, 
								Cost, 
								UnitCost, 
								UnitExtCost, 
								CostUnit, 
								Freight, 
								FreightUnit, 
								LineItemCost, 
								LineItemFreight, 
								ReceivedItemCost, 
								ReceivedItemFreight, 
								LandedCost, 
								MarkupCost, 
								VendorCostHistoryID, 
								CatchWeightRequired,
								OrigReceivedItemCost,
								NetVendorItemDiscount,
								QuantityReceived	
							)  

							VALUES 
							(	
								@OrderItem_ID, 
								ISNULL(@Cost, 0), 
								@UnitCost, 
								@UnitExtCost, 
								@CostUnit, 
								@Freight, 
								@FreightUnit, 
								@LineItemCost, 
								@LineItemFreight, 
								@ReceivedItemCost, 
								CASE
									WHEN @InvoiceFreight = 0 THEN @ReceivedItemFreight 
									ELSE NULL 
								END, 
								@LandedCost, 
								@MarkupCost, 
								@VendorCostHistoryID, 
								@CatchWeightRequired,
								@OrigReceivedItemCost,
								ISNULL(@NetDiscount, 0),
								@QuantityReceived
							)   
						END  
			
					FETCH NEXT FROM IL INTO	
						@OrderItem_ID,         
						@Item_Key,         
						@Cost,         
						@Freight,         
						@AdjustedCost,    
						@OrigReceivedItemCost,
						@QuantityOrdered,        
						@QuantityUnit,        
						@QuantityReceived,        
						@Total_Weight,        
						@CostUnit,        
						@ItemQuantityDiscount,        
						@ItemDiscountType,        
						@FreightUnit,         
						@MarkupPercent,        
						@Package_Desc1,        
						@Package_Desc2,        
						@Package_Unit_ID,        
						@IsCostedByWeight,        
						@INVCost,           
						@INVQuantity,      
						@HandlingCharge,  
						@CatchWeightRequired,  
						@Freight3Party                                     
				END        
		
			CLOSE IL        
			DEALLOCATE IL            
							
			-- Update Ordered Cost  
			UPDATE OI SET		
				Cost					= ISNULL(OIT.Cost, 0),  
				CostUnit				= OIT.CostUnit,  
				Freight					= OIT.Freight,  
				FreightUnit				= OIT.FreightUnit,  
				LandedCost				= OIT.LandedCost,
				MarkupCost				= OIT.MarkupCost,  
				LineItemCost			= OIT.LineItemCost,  
				LineItemFreight			= OIT.LineItemFreight,  
				ReceivedItemCost		= OIT.ReceivedItemCost,  
				ReceivedItemFreight		= ISNULL(OIT.ReceivedItemFreight, oi.ReceivedItemFreight),  
				UnitCost				= OIT.UnitCost,  
				UnitExtCost				= OIT.UnitExtCost,  
				VendorCostHistoryID		= OIT.VendorCostHistoryID,
				NetVendorItemDiscount	= OIT.NetVendorItemDiscount
			FROM
				OrderItem				(nolock)	oi
				INNER JOIN @OrderItem				OIT	ON	OIT.OrderItem_ID = oi.OrderItem_ID  
				  
			-- Update OrderHeader Costs
			EXEC UpdateOrderHeaderCosts @OrderHeader_ID

			-- Update OrderInvoice Costs
			UPDATE oi SET
				InvoiceTotalCost = ISNULL(InvoiceCost, 0) + ISNULL(InvoiceFreight, 0) + ISNULL((
																	SELECT 
																		SUM(oic.Value) --* (CASE WHEN oh.Return_Order = 1 THEN -1 ELSE 1 END))
																	FROM
																		OrderInvoiceCharges			oic 
																		JOIN OrderHeader			oh	ON	oh.OrderHeader_ID	= @OrderHeader_ID
																	WHERE
																		oic.OrderHeader_ID = @OrderHeader_ID
																		AND oic.SacType_ID = (SELECT SACType_Id FROM dbo.einvoicing_sactypes (nolock) WHERE SACType = 'Not Allocated')
																	), 0)
			FROM
				OrderInvoice				oi
				INNER JOIN	OrderHeader		oh ON oi.OrderHeader_ID = oh.OrderHeader_ID
			WHERE
				oi.Orderheader_ID = @OrderHeader_ID			 
		
			-- Update new POCostDate field, added for vendor lead-time update.
			IF @OrderType_ID = 1 AND @Return_Order = 0 AND @IsOrderLinkImport = 0
				BEGIN			
					UPDATE OrderHeader SET
						POCostDate = @CostDate + @VendorLeadTimeDays
					WHERE
						OrderHeader_ID = @OrderHeader_ID
				END 

			-- Update the OrderRefreshCostSource_ID Column Based on @RefreshSource
			If @RefreshSource IS NOT NULL
				BEGIN
					UPDATE OrderHeader SET
						OrderRefreshCostSource_ID = orcs.OrderRefreshCostSource_ID
					FROM
						OrderRefreshCostSource orcs
					WHERE
						OrderHeader_ID			= @OrderHeader_ID AND
						orcs.RefreshSourceName	= @RefreshSource
				END
		  
		COMMIT TRANSACTION      
	END TRY          
	BEGIN CATCH  
		IF @@TRANCOUNT > 0 ROLLBACK TRAN                 
		
		DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)  
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()  
		RAISERROR ('UpdateOrderRefreshCosts failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)  
	END CATCH     
		
	SET NOCOUNT OFF        
END
GO