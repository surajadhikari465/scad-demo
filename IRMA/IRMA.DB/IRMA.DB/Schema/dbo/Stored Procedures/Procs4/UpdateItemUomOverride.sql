
CREATE PROCEDURE [dbo].[UpdateItemUomOverride]
	@Item_Key int, 
    @Store_No int, 
    @Retail_Unit_ID int,
	@Scale_ScaleUomUnit_ID int,
	@Scale_FixedWeight varchar(25),
	@Scale_ByCount int
AS 

-- **************************************************************************************************
-- Procedure: UpdateItemUomOverride
--
-- Description: This stored procedure is called by the ItemDAO.vb in the IRMA Client code
--
-- Modification History:
-- Date       	Init  	TFS   					Comment
-- 01/13/2016	MZ	  18078(12954 & 13104)		SP creation. Update ItemUomOverride table which is used
--												to store item retail and scale UOM overrides used by 365 stores.
-- **************************************************************************************************
DECLARE @ItemUomOverrideExists bit = 0,
		@EnableRetailUomOverride bit = 0,
		@EnableScaleUomOverride bit = 0
BEGIN
	select @EnableRetailUomOverride = dbo.fn_InstanceDataValue('EnableRetailUOMbyStore', @Store_No)
	select @EnableScaleUomOverride = dbo.fn_InstanceDataValue('EnableScaleUOMbyStore', @Store_No)

	IF EXISTS(SELECT * FROM ItemUomOverride WHERE Item_Key = @Item_Key AND Store_No = @Store_No)
		BEGIN
			SET @ItemUomOverrideExists = 1
		END

	IF @Retail_Unit_ID > 0 OR @Scale_ScaleUomUnit_ID > 0 
		BEGIN
			IF @ItemUomOverrideExists <> 1
				BEGIN
					INSERT INTO ItemUomOverride (Item_Key, Store_No) VALUES (@Item_Key, @Store_No)
					SET  @ItemUomOverrideExists = 1
				END

	-- Update the values on the ItemUomOverride table that maintains retail and scale UOM overrides for 365 stores
			UPDATE ItemUomOverride 
			   SET  [Scale_ScaleUomUnit_ID] = Case When @EnableScaleUomOverride = 1
												   Then @Scale_ScaleUomUnit_ID
												   Else [Scale_ScaleUomUnit_ID]
											  End,
					[Scale_FixedWeight]     = Case When @EnableScaleUomOverride = 1
												   Then @Scale_FixedWeight
												   Else [Scale_FixedWeight]
											  End,
					[Scale_ByCount]         = Case When @EnableScaleUomOverride = 1
												   Then @Scale_ByCount
												   Else [Scale_ByCount]
											  End,
					[Retail_Unit_ID]        = Case When @EnableRetailUomOverride = 1
												   Then @Retail_Unit_ID
												   Else [Retail_Unit_ID]
											  End
			 WHERE Item_Key = @Item_Key AND Store_No = @Store_No
		END
	ELSE
		BEGIN
			IF @ItemUomOverrideExists = 1
				BEGIN
					DELETE ItemUomOverride
					 WHERE Item_Key = @Item_Key AND Store_No = @Store_No
				END
		END

    IF @ItemUomOverrideExists = 1
		UPDATE StoreItem
			SET Refresh = 1
		WHERE 
			Item_Key = @Item_Key
			AND Store_No = @Store_No 
			AND Refresh = 0
END



print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [dbo.UpdateItemUomOverride.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemUomOverride] TO [IRMAClientRole]
    AS [dbo];

