CREATE PROCEDURE dbo.ValidateCopyPOItems
	@OrderHeader_ID int,
	@DaysToKeep		int,
	@CopyToStoreNo	int,
	@IsDeleted		bit

AS 

-- **************************************************************************
-- Procedure: ValidateCopyPOItems()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called Orders.vb to copy OrderItem rows to a new OrderHeader entry
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 01/07/2013   BS      8755    Updated Item.Discontinue_Item with SIV.DiscontinueItem.
-- 01/30/2013	BBB   	9942	code formatting; added conditional check for i.Deleted_Item
-- 03/14/2013	DN		8870	Added validation logic for deleted PO 
-- 2013-04-23	KM		12062	Use the updated discontinue logic for deleted orders as well as non-deleted ones;
-- **************************************************************************

BEGIN
	
	--**************************************************************************
	--Historical table clean-up
	--**************************************************************************
	DELETE FROM InvalidCopyPOItems WHERE InsertDate < DATEADD(day,@DaysToKeep,GETDATE())

	--**************************************************************************
	--Declare internal variables
	--**************************************************************************
	DECLARE @Copy_To_PO int
	DECLARE @InvalidCopyPOItems_ID int
	DECLARE @IsTransfer bit

	--**************************************************************************
	--Populate internal variables
	--**************************************************************************
	IF @IsDeleted <> 1
	BEGIN	
	SELECT @IsTransfer = (SELECT CASE WHEN OrderType_ID = 3 THEN 1 ELSE 0 END FROM OrderHeader WHERE OrderHeader_ID = @OrderHeader_ID)

	SELECT @Copy_To_PO = IDENT_CURRENT('OrderHeader') + IDENT_INCR('OrderHeader')
	SELECT @InvalidCopyPOItems_ID = (ISNULL(MAX(InvalidCopyPOItems_ID),0) + 1) FROM InvalidCopyPOItems
	
	--**************************************************************************
	--Send back the Old and New PO IDs
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT @InvalidCopyPOItems_ID, NULL, NULL, NULL, NULL, 'Passing back the Old and New PO ID' AS Reason, NULL As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate		

	--**************************************************************************
	--Check if item is still associated to the vendor on the PO for the store (for non transfers only)
	--**************************************************************************
	IF @IsTransfer = 0
		BEGIN
			INSERT INTO InvalidCopyPOItems
				SELECT 
					@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is no longer associated to vendor ' + RTRIM(V.CompanyName) + ' for ' + RTRIM(S.Store_Name) AS Reason, 1 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
				FROM
					OrderHeader			(nolock) OH
					JOIN OrderItem		(nolock) OI	ON OI.OrderHeader_ID = OH.OrderHeader_ID
					JOIN ItemIdentifier (nolock) II ON II.Item_Key = OI.Item_Key
					JOIN Item			(nolock) I	ON I.Item_Key = II.Item_Key
					JOIN ItemBrand		(nolock) IB ON IB.Brand_ID = I.Brand_ID
					JOIN Vendor			(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
					JOIN Vendor			(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
					JOIN Store			(nolock) S	ON S.Store_No = RecLoc.Store_No
				WHERE 
					II.Default_Identifier = 1 
					AND OH.OrderHeader_ID = @OrderHeader_ID 
					AND NOT EXISTS(SELECT * FROM StoreItemVendor SIV WHERE SIV.Item_Key = OI.Item_Key AND SIV.Vendor_ID = OH.Vendor_ID 
																		 AND SIV.DeleteDate IS NULL AND SIV.Store_No = RecLoc.Store_No)	
		END

	--**************************************************************************
	--Check if item is still authorized for the store
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT 
			@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is no longer authorized for ' + RTRIM(S.Store_Name) AS Reason, 2 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
		FROM 
			OrderHeader					(nolock) OH
			JOIN OrderItem				(nolock) OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
			JOIN ItemIdentifier			(nolock) II ON II.Item_Key = OI.Item_Key
			JOIN Item					(nolock) I	ON I.Item_Key = II.Item_Key
			JOIN ItemBrand				(nolock) IB ON IB.Brand_ID = I.Brand_ID
			JOIN Vendor					(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
			JOIN Vendor					(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
			JOIN Store					(nolock) S	ON S.Store_No = RecLoc.Store_No
			JOIN StoreItem				(nolock) SI ON SI.Item_Key = OI.Item_Key
		WHERE 
			II.Default_Identifier = 1 
			AND OH.OrderHeader_ID = @OrderHeader_ID 
			AND SI.Store_No = RecLoc.Store_No 
			AND Authorized = 0 
			AND EXISTS(SELECT * FROM StoreItemVendor SIV WHERE SIV.Item_Key = OI.Item_Key AND SIV.Vendor_ID = OH.Vendor_ID 
																 AND SIV.DeleteDate IS NULL AND SIV.Store_No = RecLoc.Store_No)

	--**************************************************************************
	--Check if item is Discontinued
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT 
			@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is marked as Discontinued and cannot be added to a purchase order' AS Reason, 3 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
		FROM 
			OrderHeader					(nolock) OH
			JOIN OrderItem				(nolock) OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
			JOIN ItemIdentifier			(nolock) II ON II.Item_Key = OI.Item_Key
			JOIN Item					(nolock) I	ON I.Item_Key = II.Item_Key
			JOIN ItemBrand				(nolock) IB ON IB.Brand_ID = I.Brand_ID
			JOIN Vendor					(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
			JOIN Vendor					(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
			JOIN Store					(nolock) S	ON S.Store_No = RecLoc.Store_No
            JOIN StoreItemVendor        (nolock) SIV ON OI.Item_Key = SIV.Item_Key
                                                     AND V.Vendor_ID = SIV.Vendor_ID
                                                     ANd SIV.Store_No = @CopyToStoreNo
		WHERE 
			II.Default_Identifier = 1 
			AND OH.OrderHeader_ID = @OrderHeader_ID 
			AND SIV.DiscontinueItem = 1 
			AND OH.OrderType_ID = 1
		
	--**************************************************************************
	--Check if item is Not Available
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT 
			@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is marked as Not Available and cannot be added to a distribution order' AS Reason, 4 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
		FROM
			OrderHeader					(nolock) OH
			JOIN OrderItem				(nolock) OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
			JOIN ItemIdentifier			(nolock) II ON II.Item_Key = OI.Item_Key
			JOIN Item					(nolock) I	ON I.Item_Key = II.Item_Key
			JOIN ItemBrand				(nolock) IB ON IB.Brand_ID = I.Brand_ID
			JOIN Vendor					(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
			JOIN Vendor					(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
			JOIN Store					(nolock) S	ON S.Store_No = RecLoc.Store_No
		WHERE 
			II.Default_Identifier = 1 
			AND OH.OrderHeader_ID = @OrderHeader_ID 
			AND I.Not_Available = 1 
			AND OH.OrderType_ID = 2

	--**************************************************************************
	--Check if item is Deleted
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT 
			@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is marked as Deleted and cannot be added to an order' AS Reason, 5 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
		FROM 
			OrderHeader					(nolock) OH
			JOIN OrderItem				(nolock) OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
			JOIN ItemIdentifier			(nolock) II ON II.Item_Key = OI.Item_Key
			JOIN Item					(nolock) I	ON I.Item_Key = II.Item_Key
			JOIN ItemBrand				(nolock) IB ON IB.Brand_ID = I.Brand_ID
			JOIN Vendor					(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
			JOIN Vendor					(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
			JOIN Store					(nolock) S	ON S.Store_No = RecLoc.Store_No
		WHERE 
			II.Default_Identifier = 1 
			AND OH.OrderHeader_ID = @OrderHeader_ID 
			AND I.Deleted_Item = 1
	END
	ELSE
	BEGIN
	SELECT @IsTransfer = (SELECT CASE WHEN OrderType_ID = 3 THEN 1 ELSE 0 END FROM DeletedOrder WHERE OrderHeader_ID = @OrderHeader_ID)

	SELECT @Copy_To_PO = IDENT_CURRENT('OrderHeader') + IDENT_INCR('OrderHeader')
	SELECT @InvalidCopyPOItems_ID = (ISNULL(MAX(InvalidCopyPOItems_ID),0) + 1) FROM InvalidCopyPOItems
	
	--**************************************************************************
	--Send back the old and new PO IDs
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT @InvalidCopyPOItems_ID, NULL, NULL, NULL, NULL, 'Passing back the Old and New PO ID' AS Reason, NULL As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate		

	--**************************************************************************
	--Check if item is still associated to the vendor on the PO for the store (for non transfers only)
	--**************************************************************************
	IF @IsTransfer = 0
		BEGIN
			INSERT INTO InvalidCopyPOItems
				SELECT 
					@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is no longer associated to vendor ' + RTRIM(V.CompanyName) + ' for ' + RTRIM(S.Store_Name) AS Reason, 1 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
				FROM
					DeletedOrder			(nolock) OH
					JOIN DeletedOrderItem	(nolock) OI	ON OI.OrderHeader_ID = OH.OrderHeader_ID
					JOIN ItemIdentifier		(nolock) II ON II.Item_Key = OI.Item_Key
					JOIN Item				(nolock) I	ON I.Item_Key = II.Item_Key
					JOIN ItemBrand			(nolock) IB ON IB.Brand_ID = I.Brand_ID
					JOIN Vendor				(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
					JOIN Vendor				(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
					JOIN Store				(nolock) S	ON S.Store_No = RecLoc.Store_No
				WHERE 
					II.Default_Identifier = 1 
					AND OH.OrderHeader_ID = @OrderHeader_ID 
					AND NOT EXISTS(SELECT * FROM StoreItemVendor SIV WHERE SIV.Item_Key = OI.Item_Key AND SIV.Vendor_ID = OH.Vendor_ID 
																		 AND SIV.DeleteDate IS NULL AND SIV.Store_No = RecLoc.Store_No)	
		END

	--**************************************************************************
	--Check if item is still authorized for the store
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT 
			@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is no longer authorized for ' + RTRIM(S.Store_Name) AS Reason, 2 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
		FROM 
			DeletedOrder				(nolock) OH
			JOIN DeletedOrderItem		(nolock) OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
			JOIN ItemIdentifier			(nolock) II ON II.Item_Key = OI.Item_Key
			JOIN Item					(nolock) I	ON I.Item_Key = II.Item_Key
			JOIN ItemBrand				(nolock) IB ON IB.Brand_ID = I.Brand_ID
			JOIN Vendor					(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
			JOIN Vendor					(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
			JOIN Store					(nolock) S	ON S.Store_No = RecLoc.Store_No
			JOIN StoreItem				(nolock) SI ON SI.Item_Key = OI.Item_Key
		WHERE 
			II.Default_Identifier = 1 
			AND OH.OrderHeader_ID = @OrderHeader_ID 
			AND SI.Store_No = RecLoc.Store_No 
			AND Authorized = 0 
			AND EXISTS(SELECT * FROM StoreItemVendor SIV WHERE SIV.Item_Key = OI.Item_Key AND SIV.Vendor_ID = OH.Vendor_ID 
																 AND SIV.DeleteDate IS NULL AND SIV.Store_No = RecLoc.Store_No)

	--**************************************************************************
	--Check if item is Discontinued
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT 
			@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is marked as Discontinued and cannot be added to a purchase order' AS Reason, 3 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
		FROM 
			DeletedOrder				(nolock) OH
			JOIN DeletedOrderItem		(nolock) OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
			JOIN ItemIdentifier			(nolock) II ON II.Item_Key = OI.Item_Key
			JOIN Item					(nolock) I	ON I.Item_Key = II.Item_Key
			JOIN ItemBrand				(nolock) IB ON IB.Brand_ID = I.Brand_ID
			JOIN Vendor					(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
			JOIN Vendor					(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
			JOIN Store					(nolock) S	ON S.Store_No = RecLoc.Store_No
			JOIN StoreItemVendor        (nolock) SIV ON OI.Item_Key = SIV.Item_Key
                                                     AND V.Vendor_ID = SIV.Vendor_ID
                                                     ANd SIV.Store_No = @CopyToStoreNo
		WHERE 
			II.Default_Identifier = 1 
			AND OH.OrderHeader_ID = @OrderHeader_ID 
			AND SIV.DiscontinueItem = 1 
			AND OH.OrderType_ID = 1
		
	--**************************************************************************
	--Check if item is Not Available
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT 
			@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is marked as Not Available and cannot be added to a distribution order' AS Reason, 4 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
		FROM
			DeletedOrder				(nolock) OH
			JOIN DeletedOrderItem		(nolock) OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
			JOIN ItemIdentifier			(nolock) II ON II.Item_Key = OI.Item_Key
			JOIN Item					(nolock) I	ON I.Item_Key = II.Item_Key
			JOIN ItemBrand				(nolock) IB ON IB.Brand_ID = I.Brand_ID
			JOIN Vendor					(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
			JOIN Vendor					(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
			JOIN Store					(nolock) S	ON S.Store_No = RecLoc.Store_No
		WHERE 
			II.Default_Identifier = 1 
			AND OH.OrderHeader_ID = @OrderHeader_ID 
			AND I.Not_Available = 1 
			AND OH.OrderType_ID = 2

	--**************************************************************************
	--Check if item is Deleted
	--**************************************************************************
	INSERT INTO InvalidCopyPOItems
		SELECT 
			@InvalidCopyPOItems_ID, OI.Item_Key, II.Identifier, I.Item_Description, IB.Brand_Name, 'Item is marked as Deleted and cannot be added to an order' AS Reason, 5 As ReasonType_ID, @OrderHeader_ID As Copy_From_PO, @Copy_To_PO As Copy_To_PO, GETDATE() AS InsertDate
		FROM 
			DeletedOrder				(nolock) OH
			JOIN DeletedOrderItem		(nolock) OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
			JOIN ItemIdentifier			(nolock) II ON II.Item_Key = OI.Item_Key
			JOIN Item					(nolock) I	ON I.Item_Key = II.Item_Key
			JOIN ItemBrand				(nolock) IB ON IB.Brand_ID = I.Brand_ID
			JOIN Vendor					(nolock) V	ON V.Vendor_ID = OH.Vendor_ID
			JOIN Vendor					(nolock) RecLoc ON RecLoc.Store_No = @CopyToStoreNo
			JOIN Store					(nolock) S	ON S.Store_No = RecLoc.Store_No
		WHERE 
			II.Default_Identifier = 1 
			AND OH.OrderHeader_ID = @OrderHeader_ID 
			AND I.Deleted_Item = 1
	END

		
	--**************************************************************************
	--Output
	--**************************************************************************
	SELECT *
	FROM InvalidCopyPOItems (nolock)
	WHERE InvalidCopyPOItems_ID = @InvalidCopyPOItems_ID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateCopyPOItems] TO [IRMAClientRole]
    AS [dbo];

