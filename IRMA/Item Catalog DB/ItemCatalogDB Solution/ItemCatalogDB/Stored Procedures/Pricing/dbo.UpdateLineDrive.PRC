SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdateLineDrive') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdateLineDrive
GO


CREATE PROCEDURE dbo.UpdateLineDrive
    @Family varchar(255),
    @Brand_ID int,
    @Percent decimal(10,4),
    @Start smalldatetime,
    @End smalldatetime,
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @PricingMethod_ID int,
    @Sale_Earned_Disc1 tinyint,
    @Sale_Earned_Disc2 tinyint,
    @Sale_Earned_Disc3 tinyint
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    -- NOTE: The following joins conditions and/or formula are repeated in stored procedure GetLineDrivePreUpdate and GetLineDriveConflicts

    DECLARE @UpdateCount int
    SET @UpdateCount = 0
    
    DECLARE @PriceChgTypeID int
    SELECT @PriceChgTypeID = dbo.fn_LineDriveType()

    BEGIN TRAN

    DECLARE csList CURSOR
    READ_ONLY
    FOR
        SELECT T.Item_Key, T.Store_No, PBD.StartDate, T.Price, T.Multiple, PBD.PriceBatchDetailID
        FROM 
           (SELECT Price.Item_Key, Price.Store_No, Price, Multiple, MSRPPrice, MSRPMultiple
            FROM ItemIdentifier II (nolock)
            INNER JOIN Item (nolock) ON Item.Item_Key = II.Item_Key
            INNER JOIN ItemBrand (nolock) ON ItemBrand.Brand_ID = Item.Brand_ID
            INNER JOIN Price (nolock) ON Price.Item_Key = Item.Item_Key
            INNER JOIN fn_Parse_List(@StoreList, @StoreListSeparator) Store ON Store.Key_Value = Price.Store_No
            WHERE (Identifier LIKE @Family + '%') AND (LEN(Identifier) >= 10)
                AND Deleted_Item = 0
                AND Item.Brand_ID = ISNULL(@Brand_ID, Item.Brand_ID)
                AND Price.PriceChgTypeId  = @PriceChgTypeId) T
        LEFT JOIN
            PriceBatchDetail PBD (nolock)
            ON T.Item_Key = PBD.Item_Key AND T.Store_No = PBD.Store_No
            AND PBD.LineDrive = 1
            AND PBD.StartDate >= @Start AND PBD.StartDate <= @End
        LEFT JOIN
            PriceBatchHeader PBH (nolock)
            ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
        WHERE 
            ISNULL(PriceBatchStatusID, 0) < 6
        ORDER BY T.Item_Key
    
    DECLARE @CurrItem_Key int, @NewPrice money, @Item_Key int, @Store int, @OldStartDate smalldatetime, @Price money, @Multiple tinyint, @PriceBatchDetailID int
    SET @CurrItem_Key = 0
    OPEN csList

    SELECT @error_no = @@ERROR
    
    IF @error_no = 0
    BEGIN
        FETCH NEXT FROM csList INTO @Item_Key, @Store, @OldStartDate, @Price, @Multiple, @PriceBatchDetailID
        SELECT @error_no = @@ERROR
    END

    WHILE (@error_no = 0) AND (@@fetch_status <> -1)
    BEGIN
    	IF (@@fetch_status <> -2)
    	BEGIN
            IF @CurrItem_Key <> @Item_Key
            BEGIN
                -- Unlock in case another item admin has pulled into UI to make changes - won't be able to save since unlocked
                EXEC UnlockItem @Item_Key

                SELECT @error_no = @@ERROR
            
                IF @error_no = 0
                    SET @CurrItem_Key = @Item_Key
            END

            IF @error_no = 0
                SET @NewPrice = Round(Round(@Price - (@Price * @Percent), 2) + (0.09 - (Round(@Price - (@Price * @Percent), 2) * 10 - FLOOR(Round(@Price - (@Price * @Percent), 2) * 10)) / 10), 2)
            
            IF @error_no = 0
            BEGIN
            -- need to call insert?  Or just make sure to send it the right Price Type.  yeah.  
                EXEC UpdatePriceBatchDetailPromo @Item_Key, NULL, NULL, @Store, @PriceChgTypeId, @Start, @Multiple, @Price, @Price, 0, 1,
                                                 @PricingMethod_ID, 1, @NewPrice, @NewPrice, @End, @Sale_Earned_Disc1, @Sale_Earned_Disc2, @Sale_Earned_Disc3, @PriceBatchDetailID, 1, 'IRMA Client Line Drive'
    
                SELECT @error_no = @@ERROR
            END

            IF @error_no = 0
                SET @UpdateCount = @UpdateCount + 1
    	END

        IF @error_no = 0
        BEGIN
            FETCH NEXT FROM csList INTO @Item_Key, @Store, @OldStartDate, @Price, @Multiple, @PriceBatchDetailID
            SELECT @error_no = @@ERROR
        END
    END
    
    CLOSE csList
    DEALLOCATE csList

    IF @error_no = 0
        SELECT @UpdateCount AS RecordsAffected -- Return the affected rows for display to the user for warmth and fuzziness

    SET NOCOUNT OFF

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdateLineDrive failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

