CREATE proc [dbo].[OrderLink_ImportOrder]
	(
	@OrderORCredit			varchar(10),	-- Flag value from that will be 'Order' or 'Credit'
	@OrderNumber			varchar(15),	-- Peoplesoft OrderHeader.Order_Number.  Also used for IRMA Invoice Number.
	@OrderDate				varchar(20),	-- Peoplesoft OrderDate.  Also used for IRMA Invoice Date.
	@DC_Business_Unit		varchar(10),	-- This is the DC ID # provided to the OL Query in DM (Example: 20028 is BRS in SO Region)
	@User_GL_Unit			varchar(10),	-- Ordering Store
	@GL_Unit				varchar(10),	-- Ordering Store?
	@Order_Line_No			varchar(5),		-- Original Line Number.  Could be gaps?
	@Item_Code				varchar(10),	-- The Identifier to be used to locate the Item_Key in IRMA.
	@UPC_PLU				varchar(15),	-- Not used in V1.  Could be used as secondary to Item_Code.
	@Description1			varchar(255),	-- This is the PS description to be plugged into the comments field of the Order_Item.
	@Order_Quantity			varchar(10),	-- Duh
	@Std_UOM				varchar(10),	-- Standard Unit Of Measure.  CS for case, EA for Each.
	@Price					varchar(10),	-- Cost per unit case (and each?).
	@BuyerName				varchar(20),	-- UserName of buyer.
	@ShipDate				varchar(20),	-- Ship Date.  
	@Product				varchar(4),		-- The sub-team.
	@Qty_Ship				varchar(10),	-- Count for non-Catchweight items, Total Weight for Catchweight items.
	@CatchWeight_Flag		varchar(10),	-- True if this is a catchweight item.
	@LineItemWt				varchar(10),	-- Real?=>  Count for non-Catchweight items, Total Weight for Catchweight items.
	@Pack					varchar(10),	-- Billed_Quantity/OL.Shipped_Quantity.  Not so fast.  
	@Product_Type			varchar(1),		-- Product:1, Packaging:2, Other Supplies:3
	@Order_Item_Min			varchar(10),	-- This is the lowest Order_Line_No on the order.
	@Order_Item_Max			varchar(10),	-- This is the highest Order_Line_No on the order.
	@Order_Item_Count		varchar(10),	-- This is the total line number/count on the order.
	@Inv_Amt				varchar(10),	-- Sub-total for the invoice line item.
	@Inv_Total				varchar(10),	-- Total price for the invoice.
	@FilterDate				varchar(20),	-- Date/Time from OL used to filter range of orders/credits to import
	@CreditReason			varchar(20),		-- 
	@Order_Status			varchar(1),		-- Will contain a "N" or a "B" related to New and Billed.  Credits all come as Billed.
	@OL_DCStore				varchar(10),
	@PDXOrderNumber			varchar(10)		-- Predictix Order Number
	)
AS
	-- ****************************************************************************************************************************************************
	-- Procedure: OrderLink_ImportOrder()
	--    Author: Ron Savage
	--      Date: 11/04/2007
	--
	-- Description:
	-- This procedure imports order information from OrderLink queries.
	--
	-- Modification History (Recent at Top):
	-- Date			Init	Comment
	-- -2/29/2016	MU		[PBI 13587] Added Predictix Order Number
	-- 08/26/2013   MZ      [TFS 13159] Updated the INSERT statement to the OrderImportExceptionLog table due to the table expansion to log more info for reporting
	-- 04/09/2013	Lux		[TFS 11908, v4.7.1] Updated logic for insert into ExternalOrderInformation table for new IRMA orders to include 'OrderLink' system/source in the filter
	--                      and it doesn't reference OH external source PO # at all, since we don't care what's in OH fields, we just need to insert an OL entry in EOI as long as one does not yet exist.
	-- 03/29/2013	Lux		[TFS 11714, v4.7.1] Added dup-prevention logic for insert into ExternalOrderInformation table.  Corrected update of ExternalOrderInformation
	--						table by properly joining to OrderExternalSource to ensure only the "OrderLink" order is updated.
	-- 01/08/2013	HK		[TFS 9717, v4.7.0] Added missing is null to remove deleted itemVendor record
	-- 11/09/2012	TTL		[TFS 8282, v4.7.0] Added missing isnull() checks around values going into @Log and OrderImportExceptionLog tables.
	-- 11/09/2012	TTL		[TFS 8264, v4.7.0] Added missing (NOLOCK) hints.
	-- 10/02/2012	RDE		Insert Orderlink OrderId into ExternalOrderInformation table for tracking purposes.
	-- 03/22/2012   DBS     Changed a few usages of quantity to @Order_Quantity_Dec so decimal catchweight values don't throw an error during cost calc
	-- 03/30/2011   DBS     Changed catchweight check to costed by weight check 
	-- 03/30/2011   MY      Added checking for Bug 1740 Division by 0 error 
	-- 03/23/2010	DBS		Divided cost by weight in the case of catchweight items to set Invoice cost to solve issue with resulting transfers 
	-- 03/16/2010	DBS		Divided cost by quantity to set Invoice cost to solve issue with resulting transfers 
	-- 12/23/2010	DBS		V4.0.11 - Set quantity shipped = 0 for items in IRMA order but no longer appearing in OL order.  TFS 13780
	-- 10/07/2010	RDS		V4.0.1 - Change the order of match on PS Item code to PSID = Identifier first and VIN last. No longer partial match on VIN.
	--						REMOVE IF ORDER condition when creating an invoice so invoice information is always created for credits and orders both
	--						Moved the update of order header invoice number and amount into higher IF block so it is not dependent on no exceptions existing
	-- 08/01/2010	RDS		V4 OL Modifications
	-- 07/02/2010	BS		Modified/Enhanced for V1 Orderlink imports.  Expected prep for V4 Orderlink Imports next.
	--						There are numerous changes coming for V4.  These will be detailed in the V4 Tech Spec doc and perhaps in this comments section for V4.
	-- 12/03/2009	BSR		Adding Shipping info to populate in INSERT statement 
	-- 11/18/2009	Tom Lux	Increased IRMA PO-note (OrderHeader.OrderOeaderDesc) field limit enforced herein
	--						from 255 to 2000 chars (half the available 4,000 chars).
	--						Updated NOF logic to add PS ID and 32 chars of item desc to PO note when OrderLink UPC is NULL/empty.
	--						The VIN is differentiated from other NOF entries using "VIN=" text.
	-- 09/17/2009	BSR		Added Update/Insert values for OrderExternalSourceID 
	--						and OrderExternalSourceOrderID
	-- 09/25/2009	BSR		Added params (@inv_date, @inv_num) and logic to add/update orderinvoice information
	--						Also added the new params to update OH 
	--						Also altered update to populated QuantityShipped
	--						added to update flag QtyShippedProvided
	-- 07/14/2009	BBB		Added left joins to ItemOverride table from Item in
	--						all SQL calls; added IsNull on column values that should
	--						pull from override table if value available; added call to
	--						Currency table for OrderHeader insert based upon Vendor;
	-- 08/20/2008	RS		Added " from OrderLink" to the search for an existing order number to avoid
	--						updating a DVO order with the same order number.
	-- 07/15/2008	MU		added ship date and removed @ShipDelayDays
	-- 07/14/2008	MU		using PS_Subteam_No to get Subteam_No, instead of first item's Subteam
	-- 06/10/2008	RS		Added a section to check incoming arguments for being null,
	--						to set them to '' as they were coming in before the
	--						DataMonkey 1.2.0 fix to start sending nulls.
	-- 06/02/2008	MU		replacing 0 with values for LineItemCost, UnitCost and UnitExtCost
	-- 05/29/2008   MU		Inserting 0 into NetVendorItemDiscount to allow
	--						deprecated fn to work properly
	-- 11/28/2007	RS		Added a check for UPCs that don't exist in IRMA and
	--						appended them to the Notes field in the OrderHeader.
	-- 11/27/2007	RS		Added setting SentDate to getdate() in the OrderHeader
	--						table, to enable the recieving button on the order form.
	-- 11/26/2007	RS		Fixed second Users join to use u1.username
	-- 11/23/2007	RS		Corrected the BusinessUnit linking logic and removed
	--						the first item description from the Order notes.
	-- 11/20/2007	RS		Added code to update an existing header, or insert a
	--						new order if it doesn't exist.
	-- 10/20/2007	RS		Created.
	-- ****************************************************************************************************************************************************
BEGIN		--Script Start
	--**************************************************************************
	-- Create a log table variable to store messages to be returned to the
	-- calling application (Data Monkey).
	-- Create other Variables needed to process the Order/Item.
	--**************************************************************************
	DECLARE @Log				AS table ( err bit, msg varchar(MAX) );
	DECLARE @OrderHeader_id		AS int;
	DECLARE @OrderItem_id		AS int;
 	DECLARE @Item_Key			AS int;
	DECLARE @Buy_Store			AS int;
	DECLARE @Rec_Store			AS int;
	DECLARE @SubTeam_No			AS int;
	DECLARE @SuppliesSubteam	AS int;
	
	DECLARE @OrderHeaderDescLimit  AS int; SELECT @OrderHeaderDescLimit = 4000;
	
	--**************************************************************************
	-- Check for null incoming values and set them to '' or other default value.
	--**************************************************************************
	IF (@OrderORCredit IS NULL)			SET @OrderORCredit = 'Order';
	IF (@OrderNumber IS NULL)			SET @OrderNumber = '';
	IF (@OrderDate IS NULL)				SET @OrderDate = '';
	IF (@DC_Business_Unit IS NULL)		SET @DC_Business_Unit = '';
	IF (@User_GL_Unit IS NULL)			SET @User_GL_Unit = '';
	IF (@GL_Unit IS NULL)				SET @GL_Unit = '';
	IF (@Order_Line_No IS NULL)			SET @Order_Line_No = '';
	IF (@Item_Code IS NULL)				SET @Item_Code = '';
	IF (@UPC_PLU IS NULL)				SET @UPC_PLU = '';
	IF (@Description1 IS NULL)			SET @Description1 = '';
	IF (@Order_Quantity IS NULL)		SET @Order_Quantity = '';
	IF (@Std_UOM IS NULL)				SET @Std_UOM = '';
	IF (@Price IS NULL)					SET @Price = '';
	IF (@BuyerName IS NULL)				SET @BuyerName = '';
	IF (@ShipDate IS NULL)				SET @ShipDate = '';
	IF (@Product IS NULL)				SET @Product = '';
	IF (@Qty_Ship IS NULL)				SET @Qty_Ship = '0';
	IF (@LineItemWt IS NULL)			SET @LineItemWt = '0';
	IF (@Pack IS NULL)					SET @Pack = '0';
	IF (@Product_Type IS NULL)			SET @Product_Type = '1';
	IF (@Order_Item_Min IS NULL)		SET @Order_Item_Min = '0';
	IF (@Order_Item_Max IS NULL)		SET @Order_Item_Max = '0';
	IF (@Order_Item_Count IS NULL)		SET @Order_Item_Count = '0';
	IF (@Inv_Amt IS NULL)				SET @Inv_Amt = '0';
	IF (@Inv_Total IS NULL)				SET @Inv_Total = '0';
	IF (@FilterDate IS NULL)			SET @FilterDate = '';
	IF (@CreditReason IS NULL)			SET @CreditReason = '';
	IF (@Order_Status IS NULL)			SET @Order_Status = '';
	IF (@PDXOrderNumber IS NULL)		SET @PDXOrderNumber = '0';
	
	--**************************************************************************
	-- Internalize unit values.  ???CONFIG???
	--**************************************************************************
	DECLARE @CaseUomEDISysCode varchar(10)
	SELECT @CaseUomEDISysCode = 'CA'
	DECLARE @WtUomEDISysName varchar(10)
	SELECT @WtUomEDISysName = 'LB'
	
	BEGIN
	IF ISNULL(@CatchWeight_Flag, 'FALSE') = 'TRUE'
		SET @CatchWeight_Flag = 1
	ELSE
		SET @CatchWeight_Flag = 0
	END

	--**************************************************************************
	-- Check if the DC for this order is in this regions Store table, and set 
	-- the buy / receive locations.
	-- Assign the Subteam based on the imported 'product' value
	--**************************************************************************

	DECLARE @DC_Vendor_ID AS int
				
	IF ISNULL(@OL_DCStore, 0) = 0
		BEGIN
			SELECT @DC_Vendor_ID  = V.Vendor_ID 
				FROM Vendor V (NoLock)
					JOIN Store S (NoLock) ON V.Store_No = S.Store_No
				WHERE RIGHT(S.BusinessUnit_Id, LEN(@DC_Business_Unit)) = @DC_Business_Unit 
		END
	ELSE		
		SELECT @DC_Vendor_ID = V.Vendor_ID
			FROM Vendor V (NoLock)
			WHERE V.Store_No = @OL_DCStore
	
	SELECT @Buy_Store = S.Store_No 
		FROM Store S (NoLock)
		WHERE RIGHT(S.BusinessUnit_Id, LEN(@User_GL_Unit)) = @User_GL_Unit;
	
	SELECT @Rec_Store = S.Store_No 
		FROM Store S (NoLock)
		WHERE RIGHT(S.BusinessUnit_Id, LEN(@GL_Unit)) = @GL_Unit;
	
	SELECT TOP(1) @Subteam_No = Subteam_No 
		FROM StoreSubTeam SST (nolock)
		WHERE @Buy_Store = SST.Store_No 
			AND CAST(@Product AS int) = SST.PS_Subteam_No
			
	SELECT TOP(1) @SuppliesSubteam = Subteam_No
		FROM Subteam (nolock)
		WHERE SubteamType_ID = 6

	IF ( @DC_Vendor_ID IS NULL ) INSERT INTO @Log VALUES(1, 'ERROR: Could not find the DC Vendor record for business unit: [' + ISNULL(@DC_Business_Unit,'') + ']. ');
	IF ( @Buy_Store IS NULL ) INSERT INTO @Log VALUES(1, 'ERROR: Could not find the Purchasing store for business unit: [' + ISNULL(@User_GL_Unit,'') + ']. ');
	IF ( @Rec_Store IS NULL ) INSERT INTO @Log VALUES(1, 'ERROR: Could not find the Receiving store for business unit: [' + ISNULL(@GL_Unit,'') + ']. ');
	IF ( @Subteam_No IS NULL ) INSERT INTO @Log VALUES(1, 'ERROR: Could not find the store/subteam combination for business unit: [' + ISNULL(@User_GL_Unit,'') + '], PS subteam: [' + ISNULL(@Product,'') + ']');
	
	IF ( @DC_Vendor_ID IS NOT NULL )
									
		BEGIN
			declare @PDXSourceID int
			select @PDXSourceID = ID from OrderExternalSource where Description = 'PDX'

			--**************************************************************************
			-- If this order has already been imported, update the record in case
			-- something has been corrected or changed.
			--**************************************************************************
			SELECT @OrderHeader_ID = MAX(OrderHeader_Id)
			FROM OrderHeader (nolock)
			WHERE OrderExternalSourceID = 3
				AND OrderExternalSourceOrderID = rtrim(@orderNumber)
			
			BEGIN TRY		--Order Header Processing
				--**************************************************************************
				-- If we found the item and got the POS Dept, add the OrderHeader
				--**************************************************************************
				IF  @Order_Line_No = @Order_Item_Min
					BEGIN		--Order Header Processing - First Order Item condition - THEN		--No ELSE as this should only process for first item for efficiency.
						IF ( @OrderHeader_id IS NULL)		--If there is no existing order then INSERT else UPDATE.
							BEGIN		--Order Header Processing - THEN - INSERT
								--**************************************************************************
								-- Insert the new header information
								--**************************************************************************
								INSERT INTO 
									OrderHeader (
										Vendor_ID,
										OrderHeaderDesc,
										OrderDate,
										OrderType_ID,
										ProductType_ID,
										PurchaseLocation_ID,
										ReceiveLocation_ID,
										Transfer_To_SubTeam,
										Fax_Order,
										Expected_Date,
										CreatedBy,
										Return_Order,
										Sent,
										SentDate,
										CurrencyID, 
										OrderExternalSourceID,
										OrderExternalSourceOrderID,
										SystemGenerated,
										SupplyTransferToSubTeam
									)
								SELECT TOP(1)
									V.Vendor_Id							AS Vendor_ID,
									SUBSTRING(@OrderORCredit + ': ' + @orderNumber + ' FROM OrderLink (' + @BuyerName + ')', 1, @OrderHeaderDescLimit) AS OrderHeaderDesc,
									CAST(@orderDate AS smalldatetime)	AS OrderDate,
									1									AS OrderType_ID,
									@Product_Type						AS ProductType_ID,
									vp.Vendor_Id						AS PurchaseLocation_ID,
									vr.Vendor_Id						AS ReceiveLocation_ID,
									CASE WHEN @Product_Type = 3
									 THEN @SuppliesSubteam
									 ELSE @SubTeam_No
									END									AS Transfer_To_SubTeam,
									0									AS Fax_Order,
									CAST(@shipDate AS smalldatetime)	AS Expected_Date,
									ISNULL(u1.User_Id,u.User_Id)		AS CreatedBy,
									(CASE @OrderORCredit
										WHEN 'Credit'
										THEN 1
										ELSE 0
										END)							AS Return_Order,
									1									AS Sent,
									GETDATE()							AS SentDate,
									C.CurrencyID						AS CurrencyID,
									3									AS OrderExternalSourceID,
									@orderNumber						AS OrderExternalSourceOrderID,
									1									AS SystemGenerated,
									CASE WHEN @Product_Type = 3
										 THEN @SubTeam_No
										 ELSE NULL
										END								AS SupplyTransferToSubTeam
								FROM
									Vendor V (NoLock) -- Vendor entry for the DC
									LEFT OUTER JOIN Vendor VP (NoLock)	-- Vendor table entry for the Purchasing store
										ON ( VP.Store_No = @Buy_Store )
									LEFT OUTER JOIN Vendor VR (NoLock)	-- Vendor table entry for the recieving store
										ON ( VR.Store_No = @Rec_Store )
									LEFT OUTER JOIN Users U (NoLock)
										ON ( U.UserName = 'ol_import' )
									LEFT OUTER JOIN Users U1 (NoLock)
										ON ( U1.username = @BuyerName )
									LEFT OUTER JOIN Currency C (NoLock)
										ON ( V.CurrencyID = C.CurrencyID )
								WHERE
									V.Vendor_ID = @DC_Vendor_ID;
									
								--**************************************************************************
								-- Get the Orderheader_Id we just inserted.
								--**************************************************************************
								SELECT @OrderHeader_id = Scope_Identity()
								
								IF ( @OrderHeader_Id is not null )
									BEGIN
										INSERT INTO @Log 
											VALUES (0, 'IMPORT: IRMA Order Header created - PO [' + ISNULL(CONVERT(varchar(10), @OrderHeader_id ),'') + ']');
									END

								--**************************************************************************
								-- Log the Orderlink OrderId for tracking puporses.
								--**************************************************************************
									if not exists (
										select 1
										from ExternalOrderInformation
										where
											ExternalSource_Id = 3 -- OrderExternalSource.Description = 'OrderLink' 
											and ExternalOrder_Id = @OrderHeader_Id
									)
									begin
										INSERT    INTO ExternalOrderInformation ( OrderHeader_Id, ExternalSource_Id, ExternalOrder_Id )
										SELECT		@OrderHeader_id ,
													3 , -- OrderExternalSource.Description = 'OrderLink' 
													@orderNumber
									end

								--**************************************************************************
								-- Enter the PDX Order Number
								--**************************************************************************
								if isnull(@PDXOrderNumber,0) != 0
								begin
									if not exists (
										select 1
										from ExternalOrderInformation
										where
											ExternalSource_Id = @PDXSourceID 
											and ExternalOrder_Id = @OrderHeader_Id
									)
									begin
										INSERT    INTO ExternalOrderInformation ( OrderHeader_Id, ExternalSource_Id, ExternalOrder_Id )
										SELECT		@OrderHeader_id ,
													@PDXSourceID,
													@PDXOrderNumber
									end
								end

							END		--Order Header Processing - THEN - INSERT
						ELSE
							BEGIN		--Order Header Processing - ELSE - UPDATE
								--**************************************************************************
								-- If this is the first record of the sequence, update the header info
								--**************************************************************************
								UPDATE OH SET
									OrderHeaderDesc				= SUBSTRING(@OrderORCredit + ': ' + @orderNumber + ' FROM OrderLink (' + @BuyerName + ')', 1, @OrderHeaderDescLimit),
									Expected_Date				= CAST(@shipDate AS datetime),
									QtyShippedProvided			= CASE WHEN @qty_ship IS NOT NULL
																	THEN 1 
																	ELSE 0
																	END
								FROM
									OrderHeader OH (NoLock)
									LEFT OUTER JOIN Vendor V (NoLock) ON ( V.Vendor_ID = @DC_Vendor_ID )		-- Vendor entry for the DC
									LEFT OUTER JOIN Vendor VP (NoLock) ON ( VP.Store_No = @Buy_Store )	-- Vendor table entry for the Purchasing store
									LEFT OUTER JOIN Vendor VR (NoLock) ON ( VR.Store_No = @Rec_Store )	-- Vendor table entry for the recieving store
									LEFT OUTER JOIN Users U (NoLock) ON ( U.UserName = 'ol_import' )
									LEFT OUTER JOIN Users U1 (NoLock) ON ( U1.username = @BuyerName )
									LEFT OUTER JOIN Currency C (NoLock) ON ( V.CurrencyID = C.CurrencyID )
								WHERE
									OH.OrderHeader_Id = @OrderHeader_Id;

								IF ( @OrderHeader_Id is not null )
									BEGIN
										INSERT INTO @Log
										VALUES (0, 'UPDATE: IRMA Order header updated - PO [' + ISNULL(convert(varchar(10), @OrderHeader_Id ),'') + ']');
									END

								--**************************************************************************
								-- Update External OrderId Info for Orderlink
								--**************************************************************************
								UPDATE	eoi
								SET		ExternalSource_Id = oes.Id,
										ExternalOrder_Id = @orderNumber
								FROM    ExternalOrderInformation eoi
										join OrderExternalSource oes (nolock)
											on eoi.ExternalSource_Id = oes.id
								WHERE   oes.Description = 'Orderlink' 
										AND eoi.OrderHeader_Id = @OrderHeader_Id
								--**************************************************************************
								-- Update External OrderId for PDX Order Number
								--**************************************************************************
								UPDATE	eoi
								SET		ExternalSource_Id = oes.Id,
										ExternalOrder_Id = @PDXOrderNumber
								FROM    ExternalOrderInformation eoi
										join OrderExternalSource oes (nolock)
											on eoi.ExternalSource_Id = oes.id
								WHERE   oes.Description = 'PDX' 
										AND eoi.OrderHeader_Id = @OrderHeader_Id
								


							END		--Order Header Processing - ELSE - UPDATE
					END		--Order Header Processing - First Order Item condition - THEN		--No ELSE as this should only process for first item for efficiency.
					
				--**************************************************************************
				-- Process the Order Item
				--**************************************************************************
				DECLARE @CBW_IRMA bit				--Costed By Weight from matching Item in IRMA
				DECLARE @PD1_IRMA decimal(18,4)		--Pack from OL
				DECLARE @PD1_NEW decimal(18,4)		--PD1 variable used to hold value be stored in the imported order
				DECLARE @PD2_NEW decimal(18,4)		--PD2 variable used to hold value be stored in the imported order
				DECLARE @TW_NEW decimal(18,4)		--Total_Weight and INVWeight to be stored in the imported order
				DECLARE @CreditReason_ID int
				
				IF @CreditReason <> ''
					BEGIN
						SELECT @CreditReason_ID = CR.CreditReason_ID 
							FROM dbo.CreditReasons CR (NoLock) 
								JOIN dbo.CreditReasons_ExternalMap CRXM (NoLock) ON CR.CreditReason_ID = CRXM.CreditReason_ID
							WHERE CRXM.ExternalSourceReasonName = @CreditReason
					END
				IF @CreditReason_ID = NULL AND @OrderORCredit = 'CREDIT'
					BEGIN
						INSERT INTO @Log VALUES (1, 'ERROR: Corresponding IRMA Credit reason not found for OrderLink credit reason [' + ISNULL(@CreditReason,'') + ']) for Item_Key [' + ISNULL(CONVERT(varchar,@Item_Key),'') + '] (aka OL Item Code [' + ISNULL(@Item_Code,'') + ']) on OL Order [' + ISNULL(@OrderNumber,'') + '].')
					END
				
				--**************************************************************************
				-- Get the Item_Key based on the Item Code
				--**************************************************************************
				
				--10.07.10 4.0.1 - No longer partial match on VIN, change order of match to PSID = Identifier first, VIN last
				SELECT TOP(1)
					@Item_Key = COALESCE(IV.Item_Key, IIU.Item_Key,IIP.Item_Key)--Take what you can get.  VIN, UPC, PS_ID in that order.
				FROM
					Store S (NoLock)
					JOIN Vendor V (NoLock)						ON V.Store_No = S.Store_No
					LEFT OUTER JOIN ItemVendor IV (NoLock)		ON IV.Vendor_Id = V.Vendor_Id
																	AND IV.Item_Id = @Item_Code
					LEFT OUTER JOIN ItemIdentifier IIU (NoLock) ON IIU.Deleted_Identifier = 0
																	AND IIU.Identifier = @UPC_PLU
					LEFT OUTER JOIN ItemIdentifier IIP (NoLock) ON IIP.Deleted_Identifier = 0
																	AND IIP.Identifier = @Item_Code
				WHERE
					RIGHT(S.BusinessUnit_Id, LEN(@DC_Business_Unit)) = @DC_Business_Unit
					AND (IV.DeleteDate is NULL OR GETDATE()< IV.DeleteDate) 

				--**************************************************************************
				-- Get the PD1 based on the Item_Key
				--**************************************************************************
				IF @Item_Key IS NOT NULL
					BEGIN
						-- Fill input variables
						SELECT @CBW_IRMA = I.CostedByWeight
						FROM Item I (NoLock)
							LEFT JOIN ItemOverride IOV (NoLock) 
								ON ( I.Item_Key = IOV.Item_Key 
									AND IOV.StoreJurisdictionID = (SELECT StoreJurisdictionID 
																	FROM Store S (NoLock)
																		JOIN Vendor V (NoLock) ON S.Store_No = V.Store_No 
																	WHERE V.Vendor_ID = @DC_Business_Unit))
						WHERE I.Item_Key = @Item_Key
						
						-- Set the IRMA Pack to the Inbound Pack from OL
						SELECT @PD1_IRMA = @Pack
					END
				ELSE
					BEGIN
						INSERT INTO @Log VALUES (1, 'ERROR: OrderLink Item [' + ISNULL(@Item_Code,'') + '] on OL Order [' + ISNULL(@OrderNumber,'') + '] NOF in IRMA ')
					END
					
				DECLARE @PackageUnitID int
				SELECT TOP(1)
					@PackageUnitID = Package_Unit_ID
				FROM Item (nolock)
				WHERE Item_Key = @Item_Key
				
				--Set Item PD and Weight values based on OL & IRMA values.
				--Use the following table to determine the "pack scenario" for the item
				--			INPUTS					|		OUTPUTS
				--	Pack							|
				--	Scenario	CBW-IRMA	CWI-OI	|	PD1_N		PD2_N		TW_N		Comment
				--	________	___			___		|	_____		_____		____		_______
				--	1			1			1		|	PACK_OL		1			TW_OL_P		TW_OL_P = TotalWeight_OrderLink_Passed
				--	2			1			0		|	PACK_OL		1			TW_OL_C		TW_OL_C = TotalWeight_OrderLink_Calculated
				--	3			0			0		|	PACK_OL		1			0			Not a weigted item.
				
					
				--Now set the Pack scenario for the item
				IF ( @Item_Key IS NOT NULL AND @PD1_IRMA IS NOT NULL)
					BEGIN

						-- Fill output variables
						SELECT @PD1_NEW		= @Pack
						SELECT @PD2_NEW		= 1
						IF ( @CBW_IRMA = 1 AND @CatchWeight_Flag = 1 ) 
							BEGIN
								SELECT @TW_NEW		= @LineItemWt
							END
						IF ( @CBW_IRMA = 1 AND @CatchWeight_Flag = 0 ) 
							BEGIN
								SELECT @TW_NEW		= ABS(CONVERT(int,@Qty_Ship) * CONVERT(decimal(18,4),@Pack))
							END
						IF ( @CBW_IRMA = 0 AND @CatchWeight_Flag = 0 ) 
							BEGIN
								SELECT @TW_NEW		= 0
							END
					
						SELECT
							@OrderItem_id = OI.OrderItem_Id
						FROM
							OrderHeader OH (NoLock)
							JOIN OrderItem OI (NoLock) ON OH.OrderHeader_ID = OI.OrderHeader_ID
						WHERE
							OH.OrderHeader_ID		= @OrderHeader_id
							AND OI.Item_Key			= @Item_Key
							AND OI.Package_Desc1	= @PD1_NEW
							AND OI.Package_Desc2	= @PD2_NEW
					END
					
				DECLARE @Item_Validated bit
				IF @Item_Key IS NULL --OR @PD1_IRMA IS NULL OR @PD1_NEW IS NULL OR @PD2_NEW IS NULL OR @TW_NEW IS NULL
					BEGIN
						SELECT @Item_Validated = 0
						INSERT INTO @Log VALUES (1, 'ERROR: Cannot determine the Item and/or Item/Pack Setup (Inputs: ' 
							+ ISNULL(CONVERT(varchar,@CBW_IRMA),'') + ', ' 
							+ ISNULL(CONVERT(varchar,@CatchWeight_Flag),'') + ', ' 
							+ ISNULL(CONVERT(varchar,@PD1_IRMA),'') + ', ' 
							+ ') for Item_Key = [' + ISNULL(CONVERT(varchar,@Item_Key),'') 
							+ '] (aka OL Item Code[' + ISNULL(@Item_Code,'') 
							+ ']) on OL Order [' + ISNULL(@OrderNumber,'') + '].')
					END
				ELSE
					BEGIN
						SELECT @Item_Validated = 1
					END
				
				DECLARE @LandedCost decimal(12,4),
					@MarkupCost decimal(12,4) ,
					@LineItemCost decimal(12,4) ,
					@LineItemFreight decimal(12,4) ,
					@ReceivedItemCost decimal(12,4) ,
					@ReceivedItemFreight decimal(12,4) ,
					@UnitCost decimal(12,4) ,
					@UnitExtCost decimal(12,4) 
				DECLARE @OrderItem	TABLE 
						(	LandedCost			decimal(12,4),  
							MarkupCost			decimal(12,4),
							LineItemCost		decimal(12,4),  
							LineItemFreight		decimal(12,4),  
							ReceivedItemCost	decimal(12,4),  
							ReceivedItemFreight decimal(12,4),  
							UnitCost			decimal(12,4),  
							UnitExtCost			decimal(12,4)
						) 
				
				INSERT INTO @OrderItem (
							LandedCost,  
							MarkupCost	,
							LineItemCost,  
							LineItemFreight,  
							ReceivedItemCost,  
							ReceivedItemFreight,  
							UnitCost,  
							UnitExtCost
							)
				VALUES (0,0,0,0,0,0,0,0)

				DECLARE @UOM_CWF int
				SELECT TOP(1) @UOM_CWF = CASE WHEN @CatchWeight_Flag = 1 AND @OrderORCredit = 'CREDIT'
										THEN iuw.Unit_Id
									ELSE iuc.Unit_Id
									END
					FROM ITEM (NoLock)
						JOIN ItemUnit iuc (nolock) on ( iuc.EDISysCode = @CaseUomEDISysCode )
						JOIN ItemUnit iuw (nolock) on ( iuw.EDISysCode = @WtUomEDISysName )

				DECLARE @UOM_COM int
				SELECT TOP(1) @UOM_COM = CASE @CatchWeight_Flag
									WHEN 1
									THEN iuw.Unit_Id
									ELSE iuc.Unit_Id
									END
					FROM ITEM (NoLock)
						JOIN ItemUnit iuc (nolock) on ( iuc.EDISysCode = @CaseUomEDISysCode )
						JOIN ItemUnit iuw (nolock) on ( iuw.EDISysCode = @WtUomEDISysName )

				DECLARE @Order_Quantity_Dec decimal(18,4), @Price_Dec decimal(18,4)
				SELECT @Order_Quantity_Dec = ABS(CONVERT(decimal(18,4),@Order_Quantity))
				SELECT @Price_Dec = ABS(CONVERT(money,@Price))

				EXEC CalculateOrderItemCosts 
					@Order_Quantity_Dec,    
					@UOM_CWF,    
					0,    
					@LineItemWt,    
					@Price_Dec,    
					@UOM_COM,    
					0,    
					0,    
					0,    
					0,    
					0,    
					@UOM_COM,    
					0,    
					@PD1_NEW,   
					@PD2_NEW,    
					@PackageUnitID,    
					@CBW_IRMA,
					@CatchWeight_Flag,
					@LandedCost OUTPUT,
					@MarkupCost OUTPUT ,
					@LineItemCost OUTPUT ,
					@LineItemFreight OUTPUT ,
					@ReceivedItemCost OUTPUT ,
					@ReceivedItemFreight OUTPUT ,
					@UnitCost OUTPUT ,
					@UnitExtCost OUTPUT,    
					0,
					0 
					UPDATE		@OrderItem    
					SET			LandedCost = @LandedCost,    
								MarkupCost = @MarkupCost,
								LineItemCost = @LineItemCost,
								LineItemFreight = @LineItemFreight,
								ReceivedItemCost = @ReceivedItemCost,
								ReceivedItemFreight = @ReceivedItemFreight,
								UnitCost = @UnitCost,    
								UnitExtCost = @UnitExtCost    				

				--**************************************************************************
				-- Insert or Update the Item portion of the information:
				-- Add the item if we haven't already entered it
				--**************************************************************************
				IF ( @Item_Key IS NOT NULL AND @Item_Validated = 1 )		--Order Item Processing - Did we find an Item_Key
					BEGIN		--Order Item Processing - Did we find an Item_Key - THEN	
					
						IF ( @OrderItem_Id IS NULL )		--Order Item Processing - Is the Order Item already in IRMA
							BEGIN		--Order Item Processing - Is the Order Item already in IRMA - THEN
								INSERT INTO OrderItem (
									OrderHeader_ID,
									Item_Key,
									Comments,
									QuantityUnit,
									QuantityOrdered,
									Cost,
									CostUnit,
									UnitCost,
									UnitExtCost,
									Handling,
									HandlingUnit,
									Freight,
									FreightUnit,
									QuantityDiscount,
									DiscountType,
									LandedCost,
									LineItemCost,
									LineItemFreight,
									LineItemHandling,
									Package_Desc1,
									Package_Desc2,
									Package_Unit_ID,
									Retail_Unit_ID,
									--Total_Weight,
									InvoiceTotalWeight,
									MarkupPercent,
									MarkupCost,
									Origin_ID,
									CountryProc_ID,
									CreditReason_ID,
									NetVendorItemDiscount,
									WeightShipped,
									InvoiceCost,
									QuantityShipped,
									Units_Per_Pallet
									)
								SELECT TOP(1)
									oh.OrderHeader_Id               AS OrderHeader_ID,
									@Item_Key                       AS Item_Key,
									substring(@description1,1,255)  AS Comments,
									CASE WHEN @CatchWeight_Flag = 1 AND @OrderORCredit = 'CREDIT'
											THEN iuw.Unit_Id
										ELSE iuc.Unit_Id
										END							AS QuantityUnit,
									ABS(@order_quantity)			AS QuantityOrdered,
									
									Cost =
										CASE WHEN @CatchWeight_Flag = 1 AND @OrderORCredit = 'CREDIT' THEN 
											CASE WHEN @Order_Quantity_Dec > 0 THEN ABS(@Price) / @Order_Quantity_Dec   -- Added checking for 0 Order Quantity
											ELSE ABS(@Price)
											END
										ELSE ABS(@Price)
										END,

									@UOM_COM						AS CostUnit,
									@UnitCost						AS UnitCost,
									@UnitExtCost					AS UnitExtCost,
									0                               AS Handling,
									iuc.Unit_Id                     AS HandlingUnit,
									0                               AS Freight,
									iuc.Unit_Id                     AS FreightUnit,
									0								AS QuantityDiscount,
									0                               AS DiscountType,
									@LandedCost                     AS LandedCost,
									@LineItemCost					AS LineItemCost,
									@LineItemFreight				AS LineItemFreight,
									0                               AS LineItemHandling,
									@PD1_NEW						AS Package_Desc1,
									@PD2_NEW						AS Package_Desc2,
									ISNULL(IOV.Package_Unit_ID,I.Package_Unit_ID)	AS Package_Unit_ID,
									ISNULL(IOV.Retail_Unit_ID,I.Retail_Unit_ID)		AS Retail_Unit_ID,
									--@TW_NEW							AS Total_Weight,
									@TW_NEW							AS InvoiceTotalWeight,
									0                               AS MarkupPercent,
									@MarkupCost						AS MarkupCost,
									i.Origin_ID                     AS Origin_ID,
									i.CountryProc_ID                AS CountryProc_ID,
									(CASE WHEN @OrderORCredit = 'Credit'
										THEN @CreditReason_ID
										ELSE NULL
										END)						AS CreditReason_ID,
									0								AS NetVendorItemDiscount,
									CASE WHEN @CatchWeight_Flag = 1 
										THEN @Qty_Ship 
										ELSE NULL 
										END							AS WeightShipped,
										
									-- Added Checking for Division by 0		
									InvoiceCost = 
										@inv_amt/
													CASE WHEN @CBW_IRMA = 1 
													THEN 
															CASE WHEN @TW_NEW  > 0 
															THEN @TW_NEW 
															ELSE 1 
															END
													ELSE 
														CASE WHEN (CONVERT(decimal(18,4),@Qty_Ship)) > 0 
														THEN CONVERT(decimal(18,4),@Qty_Ship) 
														ELSE 1 
														END 
													END,
									ABS(CONVERT(decimal(18,4),@Qty_Ship))	AS QuantityShipped,
									0										AS Units_Per_Pallet
								FROM
									OrderHeader OH (NoLock)
									JOIN Item I (NoLock) ON ( I.Item_Key = @Item_Key )
									JOIN ItemUnit IUC (NoLock) ON ( IUC.EDISysCode = @CaseUomEDISysCode )
									JOIN ItemUnit IUW (NoLock) ON ( IUW.EDISysCode = @WtUomEDISysName ) 
									LEFT JOIN ItemOverride IOV (NoLock) 
										ON ( I.Item_Key = IOV.Item_Key 
											AND IOV.StoreJurisdictionID = (SELECT StoreJurisdictionID 
																			FROM Store (nolock)
																				JOIN Vendor (nolock) ON Store.Store_No = Vendor.Store_No 
																			WHERE Vendor.Vendor_ID = OH.PurchaseLocation_ID))
								WHERE
									oh.OrderHeader_Id = @OrderHeader_Id;
									
								SELECT @OrderItem_Id = Scope_Identity() 
								IF ( @OrderItem_Id is not null )
									BEGIN
										INSERT INTO @Log
										VALUES (0, 'IMPORT: Inserted a new order item entry, id: [' 
											+ ISNULL(CONVERT(varchar(10), Scope_Identity() ),'') + '] for ['+ ISNULL(@Item_Code,'') +']');
									END
							END		--Order Item Processing - Is the Order Item already in IRMA: INSERT - THEN
						ELSE
							BEGIN		--Order Item Processing - Is the Order Item already in IRMA: UPDATE - ELSE
								--**************************************************************************
								-- Update the entry if we already entered it
								--**************************************************************************
								INSERT INTO @Log
								VALUES (0, 'UPDATE: Updating item entry, id: [' + ISNULL(CONVERT(varchar(10), @OrderItem_Id ),'') + '] for ['+ ISNULL(@Item_Code,'') +']');

								UPDATE OI SET
									Comments			= SUBSTRING(@description1,1,255),
									QuantityUnit		= CASE WHEN @CatchWeight_Flag = 1 AND @OrderORCredit = 'CREDIT'
																THEN iuw.Unit_Id
															ELSE iuc.Unit_Id
															END,
									QuantityOrdered		= ABS(@order_quantity),
																								
									Cost				=
														CASE WHEN @CatchWeight_Flag = 1 AND @OrderORCredit = 'CREDIT' THEN 
															CASE WHEN @Order_Quantity_Dec > 0 THEN ABS(@Price) / @Order_Quantity_Dec   -- Added checking for 0 Order Quantity
															ELSE ABS(@Price)
															END
														ELSE ABS(@Price)
														END,
										
									CostUnit			= CASE WHEN @Catchweight_Flag = 1 
															THEN iuw.Unit_Id
															ELSE iuc.Unit_Id
															END,
									UnitCost			= @UnitCost,
									UnitExtCost			= @UnitExtCost,
									HandlingUnit		= IUC.Unit_Id,
									FreightUnit			= IUC.Unit_Id,
									QuantityDiscount	= 0,
									DiscountType		= 0,
									LandedCost			= @LandedCost,    
									LineItemCost		= @LineItemCost,
									LineItemFreight		= @LineItemFreight,
									LineItemHandling	= 0,
									Package_Desc1		= @PD1_NEW,
									Package_Desc2		= @PD2_NEW,
									Package_Unit_ID		= I.Package_Unit_ID,
									--Total_Weight		= @TW_NEW,	
									InvoiceTotalWeight	= @TW_NEW,
									MarkupPercent		= 0,
									MarkupCost			= @MarkupCost,
									Retail_Unit_ID		= I.Retail_Unit_ID,
									Origin_ID			= I.Origin_ID,
									CountryProc_ID		= I.CountryProc_ID,
									CreditReason_ID		= (CASE WHEN @OrderORCredit = 'Credit'
															THEN @CreditReason_ID
															ELSE NULL
															END),
									NetVendorItemDiscount = 0,
									WeightShipped		= CASE WHEN @CatchWeight_Flag = 1 
															THEN @Qty_Ship 
															ELSE NULL 
															END ,
									
									-- Added Checking for Division by 0		
									InvoiceCost = 
										@inv_amt/
													CASE WHEN @CBW_IRMA = 1 
													THEN 
															CASE WHEN @TW_NEW  > 0 
															THEN @TW_NEW 
															ELSE 1 
															END
													ELSE 
														CASE WHEN (CONVERT(decimal(18,4),@Qty_Ship)) > 0 
														THEN CONVERT(decimal(18,4),@Qty_Ship) 
														ELSE 1 
														END 
													END,
									QuantityShipped		= ABS(CONVERT(decimal(18,4),@Qty_Ship))
								FROM
									OrderItem OI (NoLock)
									JOIN OrderHeader OH (NoLock) ON ( OI.OrderHeader_ID = OH.OrderHeader_ID )
									JOIN Item I (NoLock) ON ( I.Item_Key = @Item_Key )
									JOIN ItemUnit IUC (NoLock) ON ( IUC.EDISysCode = @CaseUomEDISysCode )
									JOIN ItemUnit IUW (NoLock) ON ( IUW.EDISysCode = @WtUomEDISysName ) 
									LEFT JOIN ItemOverride IOV (NoLock) ON ( I.Item_Key = IOV.Item_Key 
																		AND iov.StoreJurisdictionID = (SELECT StoreJurisdictionID
																										FROM Store (nolock) JOIN Vendor (nolock) ON Store.Store_No = Vendor.Store_No 
																										WHERE Vendor.Vendor_ID = oh.PurchaseLocation_ID))
								WHERE
									OI.OrderItem_Id = @OrderItem_Id
							END		--Order Item Processing - Is the Order Item already in IRMA: UPDATE - ELSE
					END		--Order Item Processing - Did we find an Item_Key - THEN
				ELSE
					BEGIN		--Order Item Processing - Did we find an Item_Key - ELSE
						INSERT INTO @Log VALUES (1, 'ERROR: Could not find UPC [' + ISNULL(@UPC_PLU,'') + '] or VIN/PS_ID [' + ISNULL(@item_code,'') + ']');
						
						--**************************************************************************
						-- Update the Order header notes with the NOF items
						--**************************************************************************
						IF (@OrderHeader_id is not null)
							BEGIN
								-- Handle empty/NULL UPC case by adding ID's and item desc to PO note.
								DECLARE @nofInfo varchar(64); 
								SELECT @NOFInfo = CASE WHEN @UPC_PLU = '' 
													THEN 'OL_Item_Code=' + @Item_Code + ' (' + SUBSTRING(@Description1, 1, 32) + ')' 
													ELSE @UPC_PLU END
								UPDATE OrderHeader 
								SET OrderHeaderDesc = SUBSTRING(OrderHeaderDesc + ' ' + @NOFInfo + ' NOF,', 1, @OrderHeaderDescLimit)
								WHERE OrderHeader_Id = @OrderHeader_Id 
							END
					END		--Order Item Processing - Did we find an Item_Key - ELSE
			
			END TRY
			BEGIN CATCH
				INSERT INTO @Log VALUES (1, 'ERROR: ' + ISNULL(ERROR_MESSAGE(),''))
			END CATCH
		END
	ELSE
		BEGIN
			INSERT INTO @Log VALUES (1, 'ERROR: Store entry for DC: [' + ISNULL(@DC_Business_Unit,'') +'] not found for this region.')
		END

	--**********************************************************************
	-- If this is the last item for an Order then evaluate entire order for validity. 
	--**********************************************************************
	IF (@Order_Line_No = @Order_Item_Max AND @Order_Status = 'B')		-- Used Order Item Max Line No in case there was a gap in the Order_Line_No's that would make a Count <> the highest Order_Line_No.
		BEGIN
		
			DECLARE @IRMAItemCountForOrder int
			SELECT @IRMAItemCountForOrder = COUNT(*) 
				FROM OrderHeader OH (NoLock)
					JOIN OrderItem OI (NoLock) ON OH.OrderHeader_ID = OI.OrderHeader_ID
				WHERE OH.OrderExternalSourceID = 3 
					AND OH.OrderExternalSourceOrderID = @OrderNumber
					
			-- Update the invoice header info.
			UPDATE OrderHeader 
			SET
				InvoiceNumber = @OrderNumber
				, InvoiceDate = @OrderDate
			WHERE OrderHeader_ID = @OrderHeader_ID
			
			-- If the Orderlink Item Count < What got inserted above then the order is incomplete and we cannot auto receive/close.
			IF CONVERT(int,@Order_Item_Count) > @IRMAItemCountForOrder
				BEGIN
					INSERT INTO @Log VALUES (1, 'ERROR: Not all Order Items could be imported for OrderHeader.OrderHeader_ID: [' 
						+ isnull(convert(varchar(10), @OrderHeader_Id), '') + '].  The order will not be received/closed.')
				END
			ELSE
				INSERT INTO @log VALUES (0, 'RECEVING: Preparing PO [' + isnull(convert(varchar(10), @OrderHeader_Id), '') + '] for Receving and Auto-Close.')
					
				BEGIN
					DECLARE @Lines TABLE 
						(
							OrderItem_ID int,
							DateReceived DateTime,
							Quantity decimal(18,4),
							[Weight] decimal(18,4),
							User_ID int
						)
					
					INSERT INTO @Lines (OrderItem_ID, DateReceived, Quantity, [Weight], User_ID)
						SELECT
								OI.OrderItem_ID, 
								GETDATE(),
								OI.QuantityShipped,
								ISNULL(OI.InvoiceTotalWeight, 0),
								0 As UserID
							FROM OrderHeader OH (NoLock)
								JOIN OrderItem OI (NoLock) ON OH.OrderHeader_ID = OI.OrderHeader_ID
							WHERE OH.OrderExternalSourceID = 3 
								AND OH.OrderExternalSourceOrderID = @OrderNumber
								
					DECLARE @RCOrderItem_ID int
					DECLARE @RCDateReceived DateTime
					DECLARE @RCQuantity decimal(18,4)
					DECLARE @RCWeight decimal(18,4)
					DECLARE @RCUser_ID int
					
					--Receive as much of order as possible.
					DECLARE ItemsReadyForReceiving CURSOR FOR 
						SELECT
							OrderItem_ID, DateReceived, Quantity, [Weight], User_ID
						FROM @Lines

					OPEN ItemsReadyForReceiving

					FETCH NEXT FROM ItemsReadyForReceiving 
					INTO @RCOrderItem_ID, @RCDateReceived, @RCQuantity, @RCWeight, @RCUser_ID

					WHILE (@@fetch_status = 0)
					BEGIN

						INSERT INTO @Log VALUES (0, 'RECEIVING: IRMA Order [' + isnull(convert(varchar(10), @OrderHeader_Id), '') 
							+ '] Order Item [' + isnull(convert(varchar(10), @RCOrderItem_ID), '') + '] received.')			

						--Added null parameter for Receiving Discrepancy Reason Code Id - TFS 2459
						EXEC ReceiveOrderItem4 @RCOrderItem_ID, @RCDateReceived, @RCQuantity, @RCWeight, null, @RCUser_ID
						
						--SET @RCOrderItem_ID = null
						--SET @RCDateReceived = null
						--SET @RCQuantity = null
						--SET @RCWeight = null
						
						--Print the parameters here for diagnostics
						PRINT (' ')
						PRINT ('@RCOrderItem_ID:  ' + CAST(@RCOrderItem_ID AS varchar(50)))
						PRINT ('@RCDateReceived:  ' + CAST(@RCDateReceived AS varchar(50)))
						PRINT ('@RCQuantity:      ' + CAST(@RCQuantity AS varchar(50)))
						PRINT ('@RCWeight:        ' + CAST(@RCWeight AS varchar(50)))
						PRINT ('@RCUser_ID:       ' + CAST(@RCUser_ID AS varchar(50)))
						PRINT (' ')
							
						FETCH NEXT FROM ItemsReadyForReceiving
						INTO @RCOrderItem_ID, @RCDateReceived, @RCQuantity, @RCWeight, @RCUser_ID

					END
					
					CLOSE ItemsReadyForReceiving
					DEALLOCATE ItemsReadyForReceiving
					
					--**********************************************************************************
					--20101223 - Dave Stacey - TFS 13780 - Set items no longer in OL order shipped = 0
					--20120328 - Robin Eudy - Added index hint and no lock to improve performance.
					--**********************************************************************************
					IF EXISTS (SELECT OI.OrderItem_ID
								FROM OrderHeader OH (NOLOCK) 
									JOIN   OrderItem OI  with (index (idxOrderItemIDHeaderID), NOLOCK) ON OH.OrderHeader_ID = OI.OrderHeader_ID
								WHERE OH.OrderExternalSourceID = 3 
									AND OH.OrderExternalSourceOrderID = @OrderNumber
									AND OI.OrderItem_ID Not In (SELECT OrderItem_ID FROM @Lines))
					BEGIN
						UPDATE OI SET 
							QuantityShipped		= 0
						FROM OrderItem OI 
							JOIN OrderHeader OH (NoLock) ON OH.OrderHeader_ID = OI.OrderHeader_ID
						WHERE OH.OrderExternalSourceID = 3 
							AND OH.OrderExternalSourceOrderID = @OrderNumber
							AND OI.OrderItem_ID Not In (SELECT OrderItem_ID FROM @Lines)	
								
						INSERT INTO @Log VALUES (0, 'Set QuantityShipped = 0 for IRMA Order items not in OL order Number [' 
							+ isnull(convert(varchar(10), @OrderNumber), '') + '] ')
					END
					
					--**********************************************************************
					--Add/Update the OrderInvoice information 
					--**********************************************************************
					DECLARE @Inv_Cost AS decimal(12,4), @Inv_Num AS int
					SELECT TOP(1) @Inv_Num = InvoiceNumber
						FROM OrderHeader (nolock)
						WHERE OrderExternalSourceID = 3
							AND OrderExternalSourceOrderID	= @OrderNumber
							
					-- 10.7.2010 4.0.1 - Invoice information required for credits too
					--IF (@OrderHeader_ID IS NOT NULL AND @OrderORCredit = 'ORDER')
					IF (@OrderHeader_ID IS NOT NULL)
						BEGIN
							INSERT INTO @Log VALUES (0, 'RECEIVING: Record Invoice.')
							SELECT @Inv_Cost = @Inv_Total
							IF NOT EXISTS(SELECT 1 FROM OrderInvoice (nolock) WHERE OrderHeader_ID = @OrderHeader_ID)
								BEGIN
									-- Insert the new line.
									INSERT INTO OrderInvoice (OrderHeader_ID, SubTeam_No, InvoiceCost, InvoiceFreight) 
										VALUES (@OrderHeader_ID
												, CASE WHEN @Product_Type = '3'
													THEN @SuppliesSubteam
													ELSE @SubTeam_No
													END
												, @Inv_Cost
												, 0)
								END
							ELSE
								BEGIN
									-- Update Totals.
									UPDATE OrderInvoice
									SET 
										SubTeam_No = CASE WHEN @Product_Type = '3'
													THEN @SuppliesSubteam 
													ELSE @SubTeam_No
													END,
										InvoiceCost = @Inv_Cost
									WHERE OrderHeader_ID = @OrderHeader_ID
								END
						END
						
					DECLARE @OLUserID int
					SELECT @OLUserID = User_ID FROM Users (nolock) WHERE UserName = 'ol_import'
					SELECT @OLUserID = ISNULL(@OLUserID, 0)
					EXEC dbo.UpdateOrderClosed @OrderHeader_id, @OLUserID
					INSERT INTO @Log VALUES (0, 'CLOSED: OrderHeader_ID [' 
						+ isnull(convert(varchar(10), @OrderHeader_id), '') + '] processed by UpdateOrderClosed')
						
			END

		END
				
	INSERT INTO dbo.OrderImportExceptionLog (OrderHeader_Id, [Timestamp], Msg, Store_No, SubTeam_No, Identifier, Item_ID, ItemDescription, PackSize, OrderUnit, Cost)
		SELECT
			case when @OrderHeader_id is null
				then 0
				else @OrderHeader_id end
			,isnull(@FilterDate, getdate())
			,case when @OrderHeader_id is null
				then '*OrderHeader_Id NULL for OL PO [' + isnull(@OrderNumber, 'null') + ']* ' + msg
				else msg end
			,@Buy_Store
			,@Subteam_No
			,@UPC_PLU
			,@Item_Code
			,@Description1
			,@Pack
			,@Std_UOM
			,@Price
		FROM @log WHERE err = 1

	SELECT msg FROM @Log

END

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [dbo.OrderLink_ImportOrder.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OrderLink_ImportOrder] TO [IRMASchedJobsRole]
    AS [dbo];

