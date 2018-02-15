DECLARE @CountRows INT

SET @CountRows = (SELECT COUNT(*) FROM [dbo].[ShrinkSubType])

IF @CountRows = 0
BEGIN

	DECLARE @FoodBankInventoryAdjustmentCodeID INT 
	DECLARE @SamplesInventoryAdjustmentCodeID INT 
	DECLARE @SpoilageInventoryAdjustmentCodeID INT 
	DECLARE @UserId INT

	SET @FoodBankInventoryAdjustmentCodeID = (SELECT InventoryAdjustmentCode_ID FROM InventoryAdjustmentCode WHERE Abbreviation = 'FB')
	SET @SamplesInventoryAdjustmentCodeID = (SELECT InventoryAdjustmentCode_ID FROM InventoryAdjustmentCode WHERE Abbreviation = 'SM')
	SET @SpoilageInventoryAdjustmentCodeID = (SELECT InventoryAdjustmentCode_ID FROM InventoryAdjustmentCode WHERE Abbreviation = 'SP')
	SET @UserId = (SELECT User_Id FROM dbo.Users WHERE UserName= 'System')

	INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@FoodBankInventoryAdjustmentCodeID,'Donation Out of Date',@UserId , GETDATE())

	INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@FoodBankInventoryAdjustmentCodeID,'Donation Mispick',@UserId , GETDATE())

	INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SamplesInventoryAdjustmentCodeID,'Sampling',@UserId , GETDATE())

	INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SamplesInventoryAdjustmentCodeID,'Sampling Demo',@UserId , GETDATE())

	INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SpoilageInventoryAdjustmentCodeID,'Returns',@UserId , GETDATE())

	INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SpoilageInventoryAdjustmentCodeID,'Spoilage BNR',@UserId , GETDATE())

	INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SpoilageInventoryAdjustmentCodeID,'Spoilage Conversion',@UserId , GETDATE())

	INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SpoilageInventoryAdjustmentCodeID,'Spoilage Damage',@UserId , GETDATE())

    INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SpoilageInventoryAdjustmentCodeID,'Spoilage Mispick',@UserId , GETDATE())

    INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SpoilageInventoryAdjustmentCodeID,'Spoilage Out of Date',@UserId , GETDATE())

    INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SpoilageInventoryAdjustmentCodeID,'Spoilage Quality',@UserId , GETDATE())

    INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SpoilageInventoryAdjustmentCodeID,'Recall',@UserId , GETDATE())

    INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@SpoilageInventoryAdjustmentCodeID,'Spoilage Discontinued',@UserId , GETDATE())
				   
	INSERT INTO [dbo].[ShrinkSubType] (InventoryAdjustmentCode_ID, ReasonCodeDescription, [LastUpdateUser_Id], [LastUpdateDateTime])
	                   VALUES(@FoodBankInventoryAdjustmentCodeID,'Donation Discontinued',@UserId , GETDATE())
END