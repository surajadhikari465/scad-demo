CREATE PROCEDURE dbo.ApplyPMPriceChange

AS

BEGIN
    DECLARE @x float
    SET @x = 1 / 0

/*
    SET NOCOUNT ON

    DECLARE @Error_No int, @CurrDate datetime
    SELECT @Error_No = 0, @CurrDate = GETDATE()

    BEGIN TRAN

    DECLARE PriceChg CURSOR
    READ_ONLY
    FOR SELECT PMPriceChangeID, Item_Key, Price, Org_Level, Level_ID
        FROM PMPriceChange
        WHERE AppliedDate IS NULL
        ORDER BY PMPriceChangeID
    
    DECLARE @PMPriceChangeID int, @Item_Key varchar(255), @Price varchar(255), @Org_Level varchar(255), @Level_ID varchar(255)
    OPEN PriceChg
    
    FETCH NEXT FROM PriceChg INTO @PMPriceChangeID, @Item_Key, @Price, @Org_Level, @Level_ID
    WHILE (@@fetch_status <> -1) AND (@Error_No = 0)
    BEGIN
    	IF (@@fetch_status <> -2)
    	BEGIN
            UPDATE Price
            SET BuyerPrice = CONVERT(smallmoney, @Price),
                Price_Change = 1
            FROM 
                Price
                INNER JOIN 
                    Store 
                    ON Store.Store_No = Price.Store_No
                INNER JOIN
                    Zone
                    ON Zone.Zone_ID = Store.Zone_ID
            WHERE 
                Item_Key = CONVERT(int, @Item_Key)
                AND Zone.Region_ID = CASE WHEN @Org_Level = 'REGION' THEN CONVERT(int, @Level_ID) ELSE Zone.Region_ID END
                AND Store.Zone_ID =  CASE WHEN @Org_Level = 'ZONE' THEN CONVERT(int, @Level_ID) ELSE Store.Zone_ID END
                AND Price.Store_No = CASE WHEN @Org_Level = 'STORE' THEN CONVERT(int, @Level_ID) ELSE Price.Store_No END

            SELECT @Error_No = @@ERROR

            IF @Error_No = 0
            BEGIN
                UPDATE PMPriceChange
                SET AppliedDate = @CurrDate
                WHERE PMPriceChangeID = @PMPriceChangeID

                SELECT @Error_No = @@ERROR
            END
    	END
    	FETCH NEXT FROM PriceChg INTO @PMPriceChangeID, @Item_Key, @Price, @Org_Level, @Level_ID
    END
    
    CLOSE PriceChg
    DEALLOCATE PriceChg

    IF @Error_No = 0
    BEGIN
        COMMIT TRAN
        SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ApplyPMPriceChange failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
*/    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ApplyPMPriceChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ApplyPMPriceChange] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ApplyPMPriceChange] TO [IRMAReportsRole]
    AS [dbo];

