CREATE PROCEDURE dbo.ImportCycleCount

-- EXEC dbo.ImportCycleCount Remove Hard-coded store_no for Import Cycle Count procedure

AS
BEGIN
    BEGIN TRAN

    DECLARE @error_no int
    SET @error_no = 0
    
    
    

	DELETE FROM CycleCountExternalLoad

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
    
--DaveStacey - 20080625 - Set Store_no Param to cross join on "Distribution_center" store(s) for region as a way to remove hard-coded store_no that was there

		INSERT INTO CycleCountExternalLoad 
			(Store_No, Item_Key, Quantity, PackSize, IsCaseCnt, SubTeam_no, BOHFileDate)
		SELECT s.Store_No,
			item.Item_Key, WI.Tot_BOH, WI.UnitShipCase, 
			--CASE WHEN Item.CostedByWeight=1 THEN 0 ELSE 1 END,
			1,
			Item.SubTeam_No,
			cast(max(wi.DateCreated) + ' ' + max(wi.TimeCreated) as datetime)
		FROM dbo.Warehouse_Inventory WI (nolock)
			INNER JOIN ItemIdentifier II (nolock) ON II.Identifier = LTRIM(WI.Product_ID)
				AND Deleted_Identifier = 0
			INNER JOIN Item (nolock) on Item.Item_Key = II.Item_Key
			CROSS JOIN dbo.Store s 
		WHERE WI.Inv_Status = 'AC'
			AND S.Distribution_Center = 1
		GROUP BY s.Store_No, item.Item_Key, WI.Tot_BOH, WI.UnitShipCase, Item.CostedByWeight, Item.SubTeam_No
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
		declare @date datetime
		set @date = GETDATE()
		-- The following parameter for LoadCycleCountExternal()
		-- is set to any non-null value for the 7010 (BOH) import, and Null for the other imports
		exec LoadCycleCountExternal @date

        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('dbo.ImportCycleCount failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
    
    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ImportCycleCount] TO PUBLIC
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ImportCycleCount] TO [IRMAClientRole]
    AS [dbo];

