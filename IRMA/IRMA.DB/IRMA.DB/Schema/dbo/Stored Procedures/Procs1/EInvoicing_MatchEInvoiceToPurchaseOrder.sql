CREATE PROCEDURE [dbo].[EInvoicing_MatchEInvoiceToPurchaseOrder]
	@InvoiceId				INT,
	@UpdateIRMAInvoiceData	BIT,
	@ReturnValue			BIT OUTPUT

AS

/**********************************************************************************************************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
BSR					091709					11130					Changed all reference to oh.DVOOrderID to oh.OrderExternalSourceOrderID
Tom Lux			Aug 24, 2010			    13131					Changed matching right side of IRMA VIN to e-inv VIN to exact match.
Tom Lux			Aug 25, 2010			    13131					Added another pass for VIN matching, allowing left-padded zeros in VIN (so if IRMA has VIN '411' and vendor sends '0411' this pass will match the entries).
MYounes         Jan 13, 2011                  732                   Added validations to correctly show Catch Weight Items in Receiving List screen/ show corrected Cost calculation
MYounes         Jan 18, 2011                  735                   Added INNER JOINs to ItemIdentifier/ItemVendor tables during matching validations
Tom Lux			Feb 10, 2011			     1298					Bug can occur if po# in e-inv sent from vendor matches IRMA PO's OrderExternalSourceOrderID value and can cause e-inv ref in IRMA PO to be overwritten.
																	Added clause in query that populates @Results table, which is where the matching IRMA PO# is pulled.
																	The join to OrderHeader is now restricted to only match on POs that have not been uploaded (OH.UploadedDate IS NULL).
DBS		        Feb 15, 2011                 1254                   Added calc_net_ext_cost and cost conversion function to arrive at the cost.  See below for more comments
MYounes         Mar 11, 2011                 1581                   Bug for EInvoiced items that are not on the PO but are valid items in IRMA being identified as NOID instead of NORD 
DBS		        Feb 15, 2011                 1254                   Added logic around calc_net_ext_cost to multiply negative cost and quantity numbers by -1 so they don't kick out a cost PO difference when closing.
RDE			    Jul 01, 2011				 2464					Check that the BU from the Invoice matches the BU on the PO in IRMA 
DBS		        Jun 16, 2011                 2292                   Added logic to handle Alt Qty and UOM so they don't replace values on non-costed by weight items.
MD	            Aug 09, 2011                 2455                   Added logic for DSD Vendors (Einvoice Required Vendor), call the matching sp to run matching rules 
																	and auto approve the PO upon einvoice load if the PO was closed with document type of NONE
AM              Jun 29, 2011                 2276                   Added Logic to Get the Correct vendor for the Invoice when multiple vendors share same ps_export_vendor_id.
DBS		        Aug 10, 2011                 2140                   Remove call to Cost Conversion function when adding calc_net_ext_cost - this cost should be applied as-is and does not need converting in import 
MD              Sep 22, 2011                 3021                   Rollback the change made for 2140, this was causing saving Invoice Cost as line item total instead of unit level. Root cause of lots of production issues
																	Bugs solved due to this - 3021, 3018, 2981 and few more. We need to revisit this stored procedure to understand the fields that are used to save off invoice cost.
KM				Dec 30, 2011				 3744					Usage review; code formatting;
BBB				02.01.12					 4608					Added in additional check for OrderHeader in @Results where ExternalOrder IS NOT NULL, but PO # Match;
BBB				02.03.12					 4638					Added additional logic in @Results to use OHE in place of OH if Store=BusinessUnit for situations where externalsourceID already used
BBB				02.10.12					 4608					Added additional logic when retrieving VendorID for PO's that have an ExternalSourceID present;
HK				11.09.2012					 6247					Fix bug: 6247, duplicate UPC on Invoice causing error 
Hk				12.13.2012                   6247                   Add Einvoice_ID to narrow down the search for duplicate UPC
DF				12.12.12					 9362					Now using absolute values for OrderInvoice totals.
MZ              03.08.2013                   9264                   Remove the restriction to POs from Einvoice Required Vendors when executing SP MatchOrderInvoiceCosts, so that closed POs from other vendors can be re-evaluated 
                                                                    against the reparsed e-invoices.
FA				03/29/2013					 8325					Add code to insert eInvoice Exception items into OrderItemRefused table
KM				2013-04-13					 11974					Disable receiving refusal enhancement;
KM				2013-05-02					 12171					Fix merge error related to absolute values;
MZ              2013-06-03											Only retrieve orders created in the last 18 months to be the possible matching POs.
**********************************************************************************************************************************************************************************************************************************/

BEGIN  
	DECLARE @Results     TABLE   
	(	
		IRMA_VendorId		INT, 
		IRMA_StoreNO		INT, 
		IRMA_OrderId		INT	
	)        

	DECLARE @ValidExternalSources TABLE ( source_id INT PRIMARY KEY ) 

	-- einvoicing matching is limited to orders from DVO, POET, and IRMA
	INSERT INTO @ValidExternalSources
	SELECT id FROM dbo.OrderExternalSource oes WHERE Description IN ('DVO','POET')

	DECLARE @InvoiceItem TABLE 
	(	
		EInvoice_Id			INT,
		Line_Num			INT,
		UPC					VARCHAR(255),
		VIN					VARCHAR(255),
		Unit_Cost			MONEY,
		Qty_Shipped			DECIMAL(11,4),
		Vendor_Id			INT,
		OrderHeader_ID		INT,
		Item_Key			INT,
		NOID				BIT Default(0),
		NORD				BIT Default(0),
		OrderItem_ID		INT,
		case_uom			INT,
		calc_net_ext_cost	MONEY,
		alt_ordering_qty	DECIMAL(18,4),
		alt_ordering_uom	INT,
		eInvoiceQuantity	DECIMAL(18,4),
		eInvoiceWeight		DECIMAL(18,4),
		InvoiceQuantityUnit INT,
		InvoiceTotalWeight	DECIMAL(18,4),
		IsCostedByWeight    BIT,
		IsDuplicatedUPC     BIT Default(0)
	)	
		  
	DECLARE 
		@ErrorCode							INT,        
		@FoundError							BIT,    
		@OrderHeader_Id						INT, 
		@PayOrderedCost						BIT,
		@SubTeam_No							INT,      
		@Cost								MONEY,      
		@POCount							INT,    
		@IsReturnOrder						BIT, 
		@NOID								INT,
		@NORD								INT,
		@HasOrderBeenMatchedToAnEInvoice	INT,
		@DoesOrderHaveInvoiceInformation	INT,  
		@IRMA_PurchsingBusinessUnitId		INT,
		@CloseDate							DATETIME,
		@ApprovedDate						DATETIME,
		@EinvoiceRequired					BIT

    DECLARE @PossibleVendorMatches TABLE ( My_Vendor_ID INT )
    DECLARE @NumberOfPossibleVendors INT

    DECLARE @PossibleStoreMatches TABLE ( Store_No INT NOT NULL PRIMARY KEY, BusinessUnit_Id INT, VendorIdForStore INT ) 
    DECLARE @NumberOfPossibleStores INT

	DECLARE @OrdersMatchingAllCriteria TABLE ( orderheader_id INT NOT NULL PRIMARY KEY, Vendor_Id INT NULL, Store_no INT NULL  ) 
	DECLARE @NumberOfOrdersMatchingAllCriteria INT
  
	DECLARE @PossibleOrderMatches TABLE ( orderheader_id int NOT NULL PRIMARY KEY, Vendor_Id INT NULL, Store_no INT NULL ) 
    DECLARE @NumberOfPossibleOrderMatches INT
  
  	SET @FoundError	= 0        
	SET @ErrorCode	= 0 

    	
	-- Does the Vendor Exist in IRMA?
	INSERT INTO @PossibleVendorMatches (My_Vendor_ID)
    SELECT  v.Vendor_Id
    FROM    Vendor (NOLOCK) v
            INNER JOIN EInvoicing_Invoices (NOLOCK) ei ON v.ps_export_vendor_id = ei.psvendor_id_padded
    WHERE   ei.EInvoice_Id = @InvoiceId 
		
	SET @NumberOfPossibleVendors = @@RowCount 

	IF @NumberOfPossibleVendors = 0
	BEGIN
		-- The Vendor associated with this invoice does not exist in IRMA.
		SET @FoundError = 1        
		SET @ErrorCode = 2 
	END ELSE
        RAISERROR('Possible Vendor Matches: %d',10,1,@NumberOfPossibleVendors)   WITH NOWAIT --informational output.
		
	-- Does this Store Exist in IRMA?
	INSERT INTO @PossibleStoreMatches (Store_no, BusinessUnit_Id,VendorIdForStore)
    SELECT  s.store_no, s.businessunit_id, v.vendor_id
    FROM    STORE S
            INNER JOIN vendor v ON s.store_no = v.store_no
            INNER JOIN Einvoicing_Invoices ei ON ei.store_no = s.businessunit_id
    WHERE EInvoice_Id = @invoiceid

	SET @NumberOfPossibleStores = @@RowCount


	IF @NumberOfPossibleStores = 0
	BEGIN
		--The Store associated with this invoice does not exist in IRMA.  
		print 'ERROR'
		SET @FoundError = 1        
		SET @ErrorCode = 3 
	END  ELSE
	RAISERROR('Possible Store Matches: %d',10,1,@NumberOfPossibleStores)  WITH NOWAIT --informational output.

	-- get orders from External Sources (DVO, POET)
	INSERT INTO @PossibleOrderMatches (orderheader_id) 
	SELECT	eoi.orderheader_id
	FROM		dbo.ExternalOrderInformation eoi
			INNER JOIN OrderHeader oh on oh.OrderHeader_ID = eoi.OrderHeader_ID
			INNER JOIN einvoicing_invoices ei ON ei.po_num_clean = eoi.ExternalOrder_Id
			INNER JOIN dbo.OrderExternalSource oes ON oes.id = eoi.ExternalSource_Id
			INNER JOIN @ValidExternalSources vs ON vs.source_id = oes.ID
	WHERE	ei.EInvoice_Id = @InvoiceId
	  AND   oh.OrderDate > DATEADD(m, -18, GetDate())

	UPDATE  po
	SET     po.vendor_id = oh.vendor_id ,
			po.store_no = v.store_no
	FROM    OrderHeader oh
            INNER JOIN @possibleOrderMatches po ON po.orderheader_id = oh.orderheader_id
            INNER JOIN Vendor v ON v.vendor_id = oh.PurchaseLocation_Id
	
	-- get orders from IRMA
	INSERT INTO @PossibleOrderMatches ( orderheader_id, Vendor_Id, Store_no )
	SELECT	orderheader_id, oh.vendor_id, v.store_no 
	FROM    orderheader oh
			INNER JOIN einvoicing_invoices ei ON ei.po_num_clean = oh.OrderHeader_ID
			INNER JOIN vendor v on v.vendor_id = oh.PurchaseLocation_ID
	WHERE   ei.einvoice_id = @invoiceid
	  AND   oh.OrderDate > DATEADD(m, -18, GetDate())
	  AND   orderheader_id NOT IN ( SELECT  OrderHeader_ID
										FROM    @PossibleOrderMatches )

	-- Check to see if orders match on Invoice Number
	INSERT INTO @PossibleOrderMatches ( orderheader_id, Vendor_Id, Store_no )
	SELECT	orderheader_id, oh.vendor_id, v.store_no 
	FROM    orderheader oh
			INNER JOIN einvoicing_invoices ei ON ei.Invoice_Num = oh.InvoiceNumber
			INNER JOIN vendor v on v.vendor_id = oh.PurchaseLocation_ID
	WHERE   ei.einvoice_id = @invoiceid
			AND orderheader_id NOT IN ( SELECT  OrderHeader_ID
										FROM    @PossibleOrderMatches )


	SET @NumberOfPossibleOrderMatches = (SELECT COUNT(orderheader_id) FROM @PossibleOrderMatches)

	IF @NumberOfPossibleOrderMatches = 0
	BEGIN
		--The order number associated with this invoice does not exist in IRMA  
		SET @FoundError = 1        
		SET @ErrorCode = 4
	END ELSE
		RAISERROR('Possible Order Matches: %d',10,1, @NumberOfPossibleOrderMatches)  WITH NOWAIT --informational output.
	 
    INSERT  INTO @OrdersMatchingAllCriteria
	SELECT	pom.orderheader_id, pom.Vendor_Id, pom.Store_no
    FROM    @PossibleOrderMatches pom
				INNER JOIN @PossibleVendorMatches pvm	ON	pvm.My_Vendor_ID = pom.Vendor_Id
                INNER JOIN @PossibleStoreMatches psm	ON	psm.Store_No = pom.Store_no

	SET @NumberOfOrdersMatchingAllCriteria = @@ROWCOUNT
	        
	IF @NumberOfOrdersMatchingAllCriteria = 0
	BEGIN
		-- we cant find an order. do we have more detailed info why not.  
			
		IF NOT EXISTS (
			SELECT	1    FROM    @PossibleOrderMatches pom
			RIGHT JOIN @PossibleVendorMatches pvm	ON	pvm.My_Vendor_ID = pom.Vendor_Id
			WHERE vendor_id IS NOT NULL
		)
		BEGIN
			--- vendor on invoice exists in IRMA, but does not match any of the possible PO's
			SET @FoundError = 1        
			SET @ErrorCode = 105 -- No PO/Vendor Combo found.
		END           

		IF NOT EXISTS (
			SELECT	1
			FROM    @PossibleOrderMatches pom            
			RIGHT JOIN @PossibleStoreMatches psm	ON	psm.Store_No = pom.Store_no
			WHERE pom.store_no IS NOT NULL
		)
		BEGIN
			--- store on invoice exists in IRMA, but does not match any of the possible PO's
			SET @FoundError = 1        
			SET @ErrorCode =  106 -- The store on the eInvoice and the PO do not match.
		END              
		
	END ELSE IF   @NumberOfOrdersMatchingAllCriteria = 1
	BEGIN
		-- only one match. Lets use this one.
		SELECT @OrderHeader_Id = orderheader_id FROM @OrdersMatchingAllCriteria

	END ELSE IF @NumberOfOrdersMatchingAllCriteria > 1 
	BEGIN
		-- too many orders have been matched. we cant decide which one to use.   
		RAISERROR('Too many possible matches found: %d ',10,1, @NumberOfOrdersMatchingAllCriteria)  WITH NOWAIT --informational output.
		SET @FoundError = 1        
		SET @ErrorCode =  108
	END  


	IF @founderror = 0
	BEGIN	    
		SELECT  
			@SubTeam_No						= ISNULL(oh.transfer_to_subteam, -99),  
			@PayOrderedCost					= oh.PayByAgreedCost,  
			@HasOrderBeenMatchedToAnEInvoice	= CASE WHEN oh.eInvoice_Id IS NULL THEN 0 ELSE 1 END,    
			@IsReturnOrder						= CASE WHEN ISNULL(oh.Return_Order, 0) = 0 THEN 0 ELSE 1 END,
			@IRMA_PurchsingBusinessUnitId		= s.BusinessUnit_ID,
			@CloseDate						= oh.CloseDate,
			@ApprovedDate						= oh.ApprovedDate,
			@EinvoiceRequired					= vh.EinvoiceRequired
		FROM  
			OrderHeader			oh		(NOLOCK)	
			INNER JOIN  Vendor	v		(NOLOCK)	ON oh.PurchaseLocation_ID	= v.Vendor_ID
			INNER JOIN	Store	s		(NOLOCK)	ON v.Store_no				= s.Store_No
			INNER JOIN  Vendor	vh		(NOLOCK)	ON oh.Vendor_ID				= vh.Vendor_ID
		WHERE   
			Orderheader_Id = @OrderHeader_ID  
   
		SET @Cost = (  
			SELECT invoice_amt  
			FROM   EInvoicing_Header (NOLOCK)
			WHERE  einvoice_id = @InvoiceId  
		)       
   
		IF @SubTeam_No = -99  AND @FoundError = 0
			BEGIN  
				SET @FoundError = 1        
				SET @ErrorCode = 8 -- The subteam for this order could not be determined. Could not create Invoice records.  
			END   
 
		IF EXISTS (  
			SELECT 1  
			FROM   orderinvoice oi  (NOLOCK)
			WHERE  oi.OrderHeader_ID = @OrderHeader_Id  
		)  
			SET @DoesOrderHaveInvoiceInformation = 1  
		ELSE  
			SET @DoesOrderHaveInvoiceInformation = 0  
	
   
		IF	@DoesOrderHaveInvoiceInformation = 1  AND
			@HasOrderBeenMatchedToAnEInvoice = 0  AND @FoundError = 0
			BEGIN  
				SET @ErrorCode = 104 -- Invoice information for this PO has already been manually entered. Suspending eInvoice to prevent data loss.
				SET @FoundError = 1  
			END   
	END 
	
	IF @FoundError = 1
	BEGIN  
		UPDATE	EInvoicing_Invoices  
		SET
			ErrorCode_Id	= @ErrorCode,  
			STATUS			= 'Suspended'  
		WHERE	
			EInvoice_id = @InvoiceId  
	END        
   
	IF @FoundError = 0  
		BEGIN  
			INSERT INTO @InvoiceItem 
			(
				EInvoice_Id, 
				Line_Num, 
				UPC, 
				VIN, 
				Unit_Cost, 
				Qty_Shipped,
				Vendor_Id,
				OrderHeader_ID,
				Item_Key,
				NOID,
				NORD,
				OrderItem_ID,
				case_uom,
				calc_net_ext_cost,
				alt_ordering_qty,
				alt_ordering_uom, 
				IsCostedByWeight
			)
			SELECT 
				[EInvoice_Id]		= ei.EInvoice_Id,
				[Line_Num]			= ei.Line_Num,

				-- Bug 1581: Since UPCs/VINs are coming in from vendor sometimes padded with Zeros, UPCs/VINs are being defaulted
				-- to have these zeros in the @InvoiceItem table so there won't be a need to have separate UPDATEs to populate
				-- the Item_key so they won't suspend or be classified as invalid items.
				[UPC]				= RIGHT(REPLICATE('0', 13) + ei.UPC, 13),					
				[VIN]				= RIGHT(REPLICATE('0', 20) + ei.Vendor_Item_Num, 20), 
				[Unit_Cost]			= ei.Unit_Cost,
				-- TFS 8256:  use absolute values for Qty_Shipped
				[Qty_Shipped]		= ABS(ei.Qty_Shipped),		
				[IRMA_VendorId]		= Results.Vendor_Id,
				[IRMA_OrderId]		= Results.OrderHeader_Id,
				[Item_Key]			= NULL,
				[NOID]				= 0,
				[NORD]				= 0,
				[OrderItem_Id]		= NULL,
				[Case_UOM]			=	CASE 
											WHEN ei.case_uom = 'CS' THEN (SELECT Unit_ID FROM ItemUnit (NOLOCK) WHERE EDISysCode = 'CA')	-- force it to use 'CA' since 'CS' is not really a valid UOM					
											ELSE (SELECT Unit_ID FROM ItemUnit (NOLOCK) WHERE EDISysCode = ei.case_uom)
										END,  
				-- TFS 8256:  use absolute value for calc_net_ext_cost 			     
				[Calc_Net_Ext_Cost]	= ABS(ei.calc_net_ext_cost),					 		   	
				[Alt_Ordering_Qty]	= ei.alt_ordering_qty,
				[Alt_Ordering_UOM]	=	CASE 
											WHEN ei.alt_ordering_uom = 'CS' THEN (SELECT Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'CA')	-- force it to use 'CA' since 'CS' is not really a valid UOM					
											ELSE (SELECT Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = ei.alt_ordering_uom)
										END,
				[IsCostedByWeight]	= NULL	                   
				
			FROM	
				EInvoicing_Item (NOLOCK) ei 
				CROSS JOIN (SELECT TOP 1 * FROM @OrdersMatchingAllCriteria) AS Results
			WHERE	
				ei.EInvoice_Id = @InvoiceId


			UPDATE	OrderHeader  
			SET		eInvoice_Id		= @InvoiceId  
			WHERE	OrderHeader_Id	= @OrderHeader_id       
	   
	   
			IF @UpdateIRMAInvoiceData = 1  
				BEGIN  
					UPDATE	OrderHeader  
					SET		
						InvoiceNumber	=	(SELECT
												Invoice_Num  
											FROM	
												EInvoicing_Invoices  (NOLOCK)
											WHERE	
												EInvoice_id = @InvoiceId ),  
						Invoicedate		=	(SELECT 
												InvoiceDate  
											FROM   
												EInvoicing_Invoices  (NOLOCK)
											WHERE  
												EInvoice_id = @InvoiceId  
					)  
					WHERE	
						OrderHeader_Id = @OrderHeader_id  
				END  
	   
			IF NOT EXISTS	(
								SELECT * FROM	
									OrderInvoice  (NOLOCK)
								WHERE	
									OrderHeader_Id = @OrderHeader_Id  
							)  
				BEGIN  
					-- Cost shouldn't ever be negative
					DECLARE @AbsCost MONEY
					SELECT @AbsCost = ABS(@Cost)
					EXEC InsertOrderInvoice @OrderHeader_Id, @Subteam_No, @AbsCost
				END  
			ELSE  
				BEGIN  
					IF @UpdateIRMAInvoiceData = 1  
						BEGIN  
							UPDATE OrderInvoice SET		
								InvoiceCost = ABS(@Cost)
							WHERE	
								orderheader_id = @OrderHeader_Id  
						END  
				END      
			
			/* TFS 8256: Use absolute value if Qty_Shipped is negative on a return order  */
			IF @IsReturnOrder = 1
			BEGIN
				IF (SELECT Qty_Shipped FROM @InvoiceItem) < 0
				BEGIN
					UPDATE @InvoiceItem	SET	Qty_Shipped = ABS(Qty_Shipped)
				END
			END

			/*******************************************************************************
			Start Matching Validations
			*******************************************************************************/
			
			--check duplicated UPC exist
			UPDATE @InvoiceItem	SET 
				IsDuplicatedUPC = 1
			WHERE 
				upc IN (SELECT DISTINCT upc FROM eInvoicing_item WHERE EInvoice_Id = @InvoiceId GROUP BY upc HAVING COUNT(upc) > 1)

			-- Match e-invoice lines based on UPC
			UPDATE @InvoiceItem SET
				Item_Key = ii.Item_Key
			FROM
				@InvoiceItem eii	
				INNER JOIN	ItemIdentifier		(NOLOCK) ii		ON RIGHT(REPLICATE('0', 13) + ii.identifier, 13) = eii.UPC 
																AND	ii.Deleted_Identifier	= 0
				INNER JOIN	OrderItem			(NOLOCK) oi		ON ii.Item_Key				= oi.Item_Key			
				INNER JOIN	@OrdersMatchingAllCriteria   r		ON oi.OrderHeader_ID		= r.orderheader_id	
			WHERE	
				eii.Item_Key is NULL and IsDuplicatedUPC=0

			-- Match e-invoice lines based on VIN, with a check for unique VIN for the same vendor 
			UPDATE @InvoiceItem	SET
				Item_Key = IV.Item_Key
			FROM	
				@InvoiceItem eii 
				-- 08/24/10, Tom Lux, TFS 13131: Changed from "match right side of IRMA VIN to e-inv VIN" to exact match between VINs.
				INNER JOIN	ItemVendor		(NOLOCK) iv		ON	RIGHT(REPLICATE('0', 20) + iv.Item_Id, 20) = eii.VIN
															AND eii.Vendor_Id = iv.Vendor_Id
															AND iv.DeleteDate IS NULL
				INNER JOIN	OrderItem 		(NOLOCK) oi		ON oi.Item_Key = iv.Item_Key			-- TFS 735
				INNER JOIN	@OrdersMatchingAllCriteria r		ON oi.OrderHeader_ID = r.orderheader_id 	-- TFS 735
			WHERE	
				eii.Item_Key IS NULL
	   
			-- Bug 1581. For those EInvoiced items that are not on the PO and did not pass the first two matching validations,
			-- make sure that if the item exists in IRMA it should still get a valid Item_Key value and should not be NULL.
			-- It should be NORD instead of NOID in the next validation sections
			UPDATE @InvoiceItem	SET
				Item_Key = i.Item_Key
			FROM	
				@InvoiceItem eii
				INNER JOIN	ItemIdentifier		(NOLOCK) ii		ON	RIGHT(REPLICATE('0', 13) + ii.identifier, 13) = eii.UPC 
																AND ii.Deleted_Identifier	= 0
				INNER JOIN	Item 				(NOLOCK) i		ON ii.Item_Key				= i.Item_Key				
			WHERE 	
				eii.Item_Key IS NULL 
				AND	NOT EXISTS	(SELECT * 
								FROM OrderItem OI (NOLOCK) 
								WHERE OI.OrderHeader_ID = eii.OrderHeader_ID AND OI.Item_Key = eii.Item_Key
								)

			-- If it fails matching on UPC, it should check against VIN. If VIN is valid it should be NORD not NOID
			UPDATE @InvoiceItem	SET
				Item_Key = iv.Item_Key
			FROM	
				@InvoiceItem eii 
				INNER JOIN ItemVendor iv		(NOLOCK)	ON	RIGHT(REPLICATE('0', 20) + iv.Item_Id, 20) =  eii.VIN 
															AND eii.Vendor_Id = iv.Vendor_Id 
															AND	iv.DeleteDate IS NULL
			WHERE	
				eii.Item_Key IS NULL 
				AND	NOT EXISTS	(SELECT * 
								FROM OrderItem OI (NOLOCK) 
								WHERE OI.OrderHeader_ID = eii.OrderHeader_ID AND OI.Item_Key = eii.Item_Key
								)
			
			/*******************************************************************************
			End Matching Validations
			*******************************************************************************/
		
			-- Match e-invoice lines based on Order Lines
			UPDATE	@InvoiceItem
			SET		OrderItem_Id = OI.OrderItem_Id
			FROM	@InvoiceItem eii INNER JOIN	OrderItem oi (NOLOCK) ON	OI.OrderHeader_Id	= eii.OrderHeader_Id 
																	  AND	OI.Item_Key			= eii.Item_Key

			-- MD 7/13/2009: NORD/NOID
			UPDATE	@InvoiceItem 
			SET		NOID = CASE WHEN Item_Key IS NULL THEN 1 ELSE 0 END
			 
			UPDATE	II 
			SET		NORD = 1 
			FROM	@InvoiceItem II 
			WHERE	NOT EXISTS(	SELECT * 
								FROM
									OrderItem OI (NOLOCK) 
								WHERE 
									OI.OrderHeader_ID	= II.OrderHeader_ID 
									AND OI.Item_Key		= II.Item_Key
								) 
									AND	II.Item_Key		IS NOT NULL 

			UPDATE	EI 
			SET		
				Item_Key			= II.Item_Key,
				IsNotIdentifiable	=  NOID,
				IsNotOrdered		= NORD 
			FROM	
				einvoicing_item EI	(NOLOCK)	
				INNER JOIN @InvoiceItem II 	ON	EI.EInvoice_Id = II.EInvoice_Id 
											AND EI.line_num = II.line_num			
			

			-- Get Case Unit ID
			DECLARE @CaseUnitID AS INT		
			SELECT @CaseUnitID = (SELECT Unit_ID FROM ItemUnit (NOLOCK) WHERE EDISysCode = 'CA')
			
			-- Get Pound Unit ID
			DECLARE @LBUnitID AS INT		
			SELECT @LBUnitID = (SELECT Unit_ID FROM ItemUnit (NOLOCK) WHERE EDISysCode = 'LB')	
			
			-- Get Each Unit ID
			DECLARE @EachUnitID AS INT		
			SELECT @EachUnitID = (SELECT Unit_ID FROM ItemUnit (NOLOCK) WHERE EDISysCode = 'EA')
						
			
			/***********************************************************************************
			Bugs 1427 to 1428 are related. 
			Code is changed to reflect that if the OrderItem's Unit is not 'CA', 
			default it to the field being used if it were a 'CA' instead of 0. 
			Same thing if OrderItem's Unit is not 'LB' or 'EA'. 
			MYounes 02/27/2011 Added ISNULL checking per field used to populate the EInvoice fields
			MYounes 02/27/2011 Added NOLOCK in SELECT statements to get Unit ID from ItemUnit table
			DStacey 06/16/2011 Added Costed By Weight logic to straighten out ordered cost
			DStacey 06/22/2011 Added Update to calc_net_ext_cost so it gets divided by pack size before the fn_CostConversion call if 
								it's ordered by each but costed by case
			DStacey 07/02/2011 Added logic preventing alt_ordering_qty from being populated if item is not costed by weight 
			**************************************************************************************************************************************/

			UPDATE @InvoiceItem    
			SET
				eInvoiceQuantity =
						CASE 
							WHEN eii.alt_ordering_uom = @CaseUnitID THEN
								 CASE WHEN OI.QuantityUnit = @CaseUnitID 
								 THEN ISNULL(eii.alt_ordering_qty, 0) ELSE ISNULL(eii.Qty_Shipped, 0) END
							WHEN (eii.alt_ordering_uom = @LBUnitID) OR (eii.alt_ordering_uom = @EachUnitID)  
								THEN CASE WHEN (OI.QuantityUnit = @CaseUnitID OR i.CostedByWeight = 0) 
										  THEN ISNULL(eii.Qty_Shipped, 0) 
									 ELSE ISNULL(eii.alt_ordering_qty, 0) END			   
							ELSE ISNULL(eii.Qty_Shipped, 0)
						END,
				
				eInvoiceWeight =   
						CASE 
							WHEN i.CostedByWeight = 0 THEN 0
							WHEN i.CostedByWeight = 1 
								THEN  CASE 
										WHEN eii.alt_ordering_uom = @CaseUnitID THEN ISNULL(eii.Qty_Shipped, 0) 
										WHEN ((eii.case_uom = @LBUnitID) OR (eii.case_uom = @EachUnitID)) 
											THEN ISNULL(eii.Qty_Shipped, 0)
										WHEN ((eii.alt_ordering_uom = @LBUnitID) OR (eii.alt_ordering_uom = @EachUnitID))
											THEN  ISNULL(eii.alt_ordering_qty, 0)
										END
						END,
				
				InvoiceQuantityUnit =    
						CASE WHEN i.CostedByWeight = 0 THEN
							CASE 
								WHEN eii.alt_ordering_uom = @CaseUnitID THEN 
									CASE WHEN OI.QuantityUnit = @CaseUnitID 
										THEN eii.alt_Ordering_UOM ELSE eii.case_uom END
								WHEN (eii.alt_ordering_uom = @LBUnitID) OR (eii.alt_ordering_uom = @EachUnitID) THEN 
									CASE WHEN (OI.QuantityUnit = @CaseUnitID) 
										THEN eii.case_uom ELSE eii.alt_Ordering_UOM END
								ELSE 
									eii.case_uom
							END
						ELSE
							CASE 
								WHEN eii.alt_ordering_uom = @CaseUnitID THEN eii.case_uom
								WHEN ((eii.case_uom = @LBUnitID) OR (eii.case_uom = @EachUnitID)) 
									THEN eii.case_uom
								WHEN ((eii.alt_ordering_uom = @LBUnitID) OR (eii.alt_ordering_uom = @EachUnitID))
									THEN  eii.alt_Ordering_UOM
								END						
						END,					
				
				InvoiceTotalWeight =   
						CASE 
							WHEN i.CostedByWeight = 0 THEN 0
							WHEN i.CostedByWeight = 1 
								THEN  CASE 
										WHEN eii.alt_ordering_uom = @CaseUnitID THEN ISNULL(eii.Qty_Shipped, 0) 
										WHEN ((eii.case_uom = @LBUnitID) OR (eii.case_uom = @EachUnitID)) 
											THEN ISNULL(eii.Qty_Shipped, 0)
										WHEN ((eii.alt_ordering_uom = @LBUnitID) OR (eii.alt_ordering_uom = @EachUnitID))
											THEN  ISNULL(eii.alt_ordering_qty, 0)
										END
						END,
				
				IsCostedByWeight = i.CostedByWeight
															
			FROM	
				@InvoiceItem eii	
				INNER JOIN dbo.OrderItem oi (NOLOCK) ON eii.OrderItem_Id	= oi.OrderItem_Id    
				INNER JOIN dbo.Item i		(NOLOCK) ON oi.Item_Key			= i.item_key  
     
		        
			/* #########################################################################################################################################################        
				Dave Stacey - 20110215 - Changes to this part of the process include using the calculated columns eInvoiceQuantity and InvoiceQuantityUnit
				and adding back the CostConversion function call.  The calc_net_ext_cost is used because it includes any discounts and because it's the total 
				for the line item, it is divided by the quantity calculated above.
			########################################################################################################################################################## */         
	
		      		 
			UPDATE OrderItem    
			SET		
				eInvoiceQuantity	= eii.eInvoiceQuantity,
				eInvoiceWeight		=  eii.eInvoiceWeight,
				InvoiceQuantityUnit = ISNULL(eii.InvoiceQuantityUnit, @CaseUnitID),         --  MYounes 02/28/2011; Added ISNULL checking use caseuom as default
				InvoiceTotalWeight	=  eii.InvoiceTotalWeight,
				InvoiceCost			=	dbo.fn_CostConversion(eii.calc_net_ext_cost /(CASE WHEN eii.IsCostedByWeight = 1 
																							THEN CASE WHEN eii.eInvoiceWeight = 0 THEN 1 
																								ELSE CASE WHEN eii.eInvoiceWeight  < 0 
																										THEN eii.eInvoiceWeight * -1 
																										ELSE eii.eInvoiceWeight 
																									END 
																								END
																						ELSE
																							CASE WHEN eii.eInvoiceQuantity= 0 THEN 1 
																							ELSE CASE WHEN eii.eInvoiceQuantity  < 0 
																										THEN eii.eInvoiceQuantity * -1 
																									ELSE eii.eInvoiceQuantity 
																									END 
																							END
																						END),	eii.InvoiceQuantityUnit, CostUnit, Package_Desc1, Package_Desc2, Package_Unit_ID),							
				InvoiceExtendedCost = eii.calc_net_ext_cost 
			FROM
				@InvoiceItem eii  
			WHERE	
				OrderItem.OrderItem_Id = eii.OrderItem_Id       
					
			/* #############################################################        
				Create OrderInvoiceCharges Records        
			##############################################################*/       
	   
			IF @UpdateIRMAInvoiceData = 1  
				BEGIN  
					-- If OrderInvoiceCharges records already exist, clear them out.         
					DELETE   
					FROM	OrderInvoiceCharges  
					WHERE	OrderHeader_Id = @OrderHeader_id   
			   
			   
					-- If PO is a Return/Credit, use absolute values.  
					-- Create Invoice Level Records        
					INSERT INTO OrderInvoiceCharges  
					(  
						OrderHeader_Id,  
						SacType_Id,  
						OrderItem_Id,  
						Subteam_No,  
						[Description],  
						[Value],
						IsAllowance,
						ElementName
					)  
					SELECT	
						oh.OrderHeader_Id,  
						Config.saccodetype,  
						OrderItem_Id		=	NULL,  
						Config.subteam_no,  
						[Description]		=	CASE   
													WHEN config.subteam_no IS NULL THEN ISNULL(config.label, config.ElementName)  
													ELSE ISNULL(s.SubTeam_Name, ISNULL(config.Label, config.ElementName))  
												END,  
						-- TFS 8256:  Using absolute value if < 0, instead of blindly flipping the sign
						[value]				=	CASE
													WHEN (CAST(Summary.ElementValue AS MONEY) * ISNULL(config.ChargeOrAllowance, 1)) < 0 THEN ABS(CAST(Summary.ElementValue AS MONEY) * ISNULL(config.ChargeOrAllowance, 1))
													ELSE CAST(Summary.ElementValue AS MONEY) * ISNULL(config.ChargeOrAllowance, 1)
												END,
						IsAllowance			=	CASE
													WHEN config.ChargeOrAllowance = -1 THEN 1 
													ELSE 0 
												END,
						config.elementname
					FROM	
						EInvoicing_SummaryData Summary			(NOLOCK)	
						INNER JOIN	EInvoicing_Config	Config	(NOLOCK)	ON		Config.elementname		= Summary.elementname  
						INNER JOIN	EInvoicing_Invoices Invoices(NOLOCK)	ON		Invoices.einvoice_id	= Summary.einvoice_id  
						LEFT JOIN	Subteam				s		(NOLOCK)	ON		s.subteam_no			= Config.subteam_no  
						INNER JOIN	OrderHeader			oh		(NOLOCK)	ON		ISNULL(oh.OrderExternalSourceOrderID, oh.Orderheader_id) = Invoices.po_num_clean  
					WHERE	
						Config.IsSacCode					= 1  
						AND	Summary.ElementValue			IS NOT NULL  
						AND Invoices.EInvoice_Id			= @InvoiceId 
						AND Config.ExcludeFromCalculations	= 0
		   
		   
					-- Create Item Level Records        
					INSERT INTO OrderInvoiceCharges  
					(  
						OrderHeader_Id,  
						SacType_Id,  
						OrderItem_Id,  
						Subteam_No,  
						[Description],   
						[Value],
						IsAllowance,
						ElementName
					)  
					SELECT	ei.OrderHeader_Id,  
							Config.saccodetype,  
							ei.OrderItem_Id,  
							Config.subteam_no,  
							[Description]		=	CASE   
														WHEN config.subteam_no IS NULL THEN ISNULL(config.label, config.ElementName)  
														ELSE ISNULL(s.SubTeam_Name, ISNULL(config.Label, config.ElementName))  
													END,  
							-- TFS 8256:  Using absolute value when < 0, instead of blindly flipping the sign
							[Value]				=	CASE
														WHEN (CAST(ItemData.ElementValue AS MONEY) * ISNULL(config.ChargeOrAllowance, 1)) < 0 THEN ABS(CAST(ItemData.ElementValue AS MONEY) * ISNULL(config.ChargeOrAllowance, 1))
														ELSE CAST(ItemData.ElementValue AS MONEY) * ISNULL(config.ChargeOrAllowance, 1)
													END,
							IsAllowance			=	CASE 
														WHEN config.ChargeOrAllowance = -1 THEN 1 
														ELSE 0 
													END,
							config.elementname
					FROM	
						einvoicing_itemdata ItemData			(NOLOCK)	
						INNER JOIN	einvoicing_config	Config	(NOLOCK)	ON		Config.elementname = ItemData.elementname  
						INNER JOIN	@InvoiceItem		ei					ON		ei.line_num = ItemData.ItemId  AND ei.einvoice_id = ItemData.einvoice_id  
						LEFT JOIN	subteam				s		(NOLOCK)	ON		s.subteam_no = Config.subteam_no  
					WHERE	
						Config.issaccode			= 1  
						AND	ItemData.elementvalue	IS NOT NULL  
						AND ItemData.einvoice_id	= @InvoiceId  
				END   
	   
			--20081005 - DaveStacey - Subtract non-allocated SAC charges from the total charge of the invoice so they load properly      
			DECLARE @NonAllocSAC INT      
		   
			SELECT	@NonAllocSAC = SACType_ID  
			FROM	dbo.einvoicing_sactypes (NOLOCK)  
			WHERE	SACTYpe = 'Not Allocated'      
		   
			DECLARE @InvoiceTot SMALLMONEY      
		   
			SELECT	@InvoiceTot = SUM([Value])  
			FROM	dbo.OrderInvoiceCharges OIC(NOLOCK)  
			WHERE	OIC.orderheader_id		= @OrderHeader_id  
					AND	OIC.SACType_ID		= @NonAllocSAC 
					AND oic.[value]			IS NOT NULL
				     
			-- 20080108 - Robin Eudy - Little known fact. X - NULL = NULL. Put isnull() around @InvoiceTot below. That was a pain to debug.      
			IF @UpdateIRMAInvoiceData = 1  
				BEGIN  
					UPDATE dbo.OrderInvoice  
					SET    InvoiceCost = InvoiceCost - ISNULL(@InvoiceTot, 0)  
					FROM   
						dbo.OrderInvoice OI	(NOLOCK)  
					WHERE  
						OI.orderheader_id = @OrderHeader_id  
				END  
   
   		
		
		/* 4.8 - Refusal functionality is disabled until final requirements are delivered.

				-- TFS 8325, 03/29/2013, Faisal Ahmed - Added eInvoicing Exception Items to Refused Items list.
				EXEC AddOrderItemRefusedForEInvoiceExceptionItems @OrderHeader_ID, @InvoiceId

				-- Update the qty and cost of any currently refused items.
				UPDATE OrderItemRefused SET
					InvoiceCost		= oi.InvoiceCost,
					InvoiceQuantity = oi.eInvoiceQuantity
				FROM
					OrderItemRefused oir
					JOIN @InvoiceItem	ii ON oir.OrderItem_ID	= ii.OrderItem_ID
					JOIN OrderItem		oi ON ii.OrderItem_ID	= oi.OrderItem_ID
		*/



		--MD 6/30/2009: Added call to update order refresh costs to refresh PO costs (step to be performed on EInvoice Load)  
		--Check if its a P2P vendor here and then call the alert buyer stored procedure
		--RDE 7/7/2009: Call UORC for all EInvoices not just PayByAgreedCost.
		EXEC UpdateOrderRefreshCosts @OrderHeader_ID, 'EInvoiceMatching', NULL, 0  
	
		IF @PayOrderedCost = 1 
			BEGIN
				EXEC AlertBuyerAbout_ED_SS_OOS @InvoiceId
			END 
	END
	
	-- If no errors found, einvoice should be successful and ready to archive. 
	IF @FoundError = 0	
	BEGIN
		UPDATE	EInvoicing_Invoices
		SET		
			Archived		= 1,
			ArchivedDate	= GETDATE()
		WHERE	
			EInvoice_Id			= @InvoiceId 
			AND	ErrorCode_id	IS NULL   	

		--MD 08/09/2011: TFS 2455 DSD Vendor Enhancement, If the PO was closed with document type of NONE (i.e. closed without any invoice information)
		--and is for an Einvoice Required Vendor, then allow auto approval after the Matching of the Einvoice
		--this way receivers will not have to manually reopen and close the PO to get it approved.
		--MZ 03/08/2013: TFS 9264 Remove the restriction to POs from Einvoice Required Vendors when executing SP MatchOrderInvoiceCosts, 
		--               so that closed POs from other vendors can be re-evaluated against the reparsed e-invoices.
		IF (@CloseDate IS NOT NULL) AND (@ApprovedDate IS NULL) ---AND (@EinvoiceRequired = 1)
			BEGIN
				DECLARE @IsSuspended BIT
				EXEC MatchOrderInvoiceCosts @OrderHeader_ID, 0, @IsSuspended OUTPUT
			END
	END
 
	-- If @FoundError = TRUE return FALSE else return TRUE        
	SELECT @ReturnValue = @FoundError ^ 1 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_MatchEInvoiceToPurchaseOrder] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_MatchEInvoiceToPurchaseOrder] TO [IRMAClientRole]
    AS [dbo];

