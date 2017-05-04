SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdatePriceBatchDetailHeader') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdatePriceBatchDetailHeader
GO


CREATE PROCEDURE dbo.UpdatePriceBatchDetailHeader
    @DetailIDList varchar(8000),
    @DetailIDListSep char(1),
    @PriceBatchHeaderID int
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

	DECLARE @SearchDate datetime
	SELECT @SearchDate = StartDate 
	FROM PriceBatchHeader
	WHERE PriceBatchHeaderId = @PriceBatchHeaderId
	
	SELECT @SearchDate = dateadd(d,FlagValue*2,@SearchDate)
	FROM InstanceDataFlags
	WHERE FlagKey = 'TwoDayBatchingBuffer'

    BEGIN TRAN

    DECLARE @List TABLE (PriceBatchDetailID int, Item_Key int, Store_No int, StartDate smalldatetime, ReAuthFlag bit)

    INSERT INTO @List (PriceBatchDetailID, Item_Key, Store_No, StartDate, ReAuthFlag)
    SELECT Key_Value, Item_Key, Store_No, StartDate, ReAuthFlag
    FROM dbo.fn_Parse_List(@DetailIDList, @DetailIDListSep) L
    INNER JOIN
        PriceBatchDetail PBD
        ON PBD.PriceBatchDetailID = L.Key_Value

    SELECT @Error_No = @@ERROR

    IF @error_no = 0
    BEGIN
        --UPDATE THE DETAIL PASSED INTO THIS STORED PROC
        UPDATE PriceBatchDetail
        SET PriceBatchHeaderID = @PriceBatchHeaderID
        FROM PriceBatchDetail PBD
        INNER JOIN 
            @List L
            ON L.PriceBatchDetailID = PBD.PriceBatchDetailID
        WHERE PBD.PriceBatchHeaderID IS NULL
    
        SELECT @Error_No = @@ERROR
    END

    -- If this is a Price Change or New Item batch, include PBD for Item Changes (Price Change batch), New Items (New Item batch),
    -- and Off Promo Cost (Price Change batch) in the batch.
    IF (@Error_No = 0) AND EXISTS (SELECT * 
                                   FROM PriceBatchHeader (nolock)
                                   WHERE PriceBatchHeaderID = @PriceBatchHeaderID AND ISNULL(ItemChgTypeID, 0) < 2)
    BEGIN
        UPDATE PriceBatchDetail
        SET PriceBatchHeaderID = @PriceBatchHeaderID
        FROM PriceBatchDetail PBD
        INNER JOIN 
            @List L
            ON L.Item_Key = PBD.Item_Key AND L.Store_No = PBD.Store_No
        WHERE PBD.PriceBatchHeaderID IS NULL
			AND	ISNULL(PBD.StartDate,ISNULL(L.StartDate, GetDate())) <= ISNULL(L.StartDate, GetDate())  -- LIMIT ITEM CHANGES TO NON-FUTURE-DATED ITEMS (OFF-COST)
            AND ((ISNULL(L.ReAuthFlag, 0) = 0 AND ISNULL(ItemChgTypeID, 0) IN (1,2,6)) OR   -- IF THIS IS NOT A RE-AUTH FOR A STORE, LIMIT TO NEW, ITEM, AND OFF-PROMO COST RECORDS
				 (ISNULL(L.ReAuthFlag, 0) = 1 AND ISNULL(ItemChgTypeID, 0) IN (0,1,2,6)))   -- UNLIKE NEW ITEMS, RE-AUTH RECORDS MAY RETURN THE ITEM RECORD INSTEAD OF THE PRICE RECORD WHEN
																						    -- CREATING A BATCH, SO THE ITEMCHGTYPEID WILL BE NULL IF THERE ARE ALSO PRICE CHANGE RECORDS
			AND PBD.Expired = 0																-- DON'T INCLUDE ENTRIES THAT ARE EXPIRED						
			AND (@SearchDate BETWEEN														-- DON'T ADD SALES THAT HAVE ALREADY ENDED TO BATCHES
					ISNULL(PBD.StartDate, @SearchDate) AND ISNULL(PBD.Sale_End_Date, ISNULL(PBD.StartDate, @SearchDate)))
			AND	(PBD.ItemChgTypeID != 2														-- DON'T INCLUDE SUPERSEDED PRICE CHANGES
				OR dbo.fn_PriceSuperseded (PBD.Item_Key, PBD.Store_No, PBD.PriceChgTypeId, PBD.StartDate, @SearchDate) = 0  
				) 				 
        SELECT @Error_No = @@ERROR
    END  

    SET NOCOUNT OFF

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdatePriceBatchDetailHeader failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


