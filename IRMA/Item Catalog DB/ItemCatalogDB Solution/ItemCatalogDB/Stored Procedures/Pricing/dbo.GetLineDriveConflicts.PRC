SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetLineDriveConflicts') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetLineDriveConflicts
GO

/*
CREATE PROCEDURE dbo.GetLineDriveConflicts
    @Family varchar(255),
    @Brand_ID int,
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @Start varchar(255),
    @End varchar(255)
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @StartDate smalldatetime, @EndDate smalldatetime

    SELECT @StartDate = CONVERT(smalldatetime, @Start), @EndDate = CONVERT(smalldatetime, @End)

    SELECT PBD.LineDrive, PriceChgTypeDesc, T.Identifier, Item_Description, Store_Name, PBD.StartDate, Sale_End_Date
    FROM 
       (SELECT Price.Item_Key, Price.Store_No, Identifier, Item_Description
        FROM ItemIdentifier II (nolock)
        INNER JOIN Item (nolock) ON Item.Item_Key = II.Item_Key
        INNER JOIN ItemBrand (nolock) ON ItemBrand.Brand_ID = Item.Brand_ID
        INNER JOIN Price (nolock) ON Price.Item_Key = Item.Item_Key
        INNER JOIN fn_Parse_List(@StoreList, @StoreListSeparator) Store ON Store.Key_Value = Price.Store_No
        WHERE (Identifier LIKE @Family + '%') AND (LEN(Identifier) >= 10)
            AND Deleted_Item = 0
            AND Item.Brand_ID = ISNULL(@Brand_ID, Item.Brand_ID)
            AND Sale_EDLP = 0) T
    INNER JOIN
        PriceBatchDetail PBD (nolock)
        ON T.Item_Key = PBD.Item_Key AND T.Store_No = PBD.Store_No
    LEFT JOIN
        PriceBatchHeader PBH (nolock)
        ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
    INNER JOIN
        PriceChgType (nolock)
        ON PBD.PriceChgTypeID = PriceChgType.PriceChgTypeID
    INNER JOIN
        Store (nolock)
        ON Store.Store_No = T.Store_No
    WHERE ISNULL(PriceBatchStatusID, 0) < 6
        AND PBD.PriceChgTypeID IN (1,2,3)
        AND ((-- Promo conflict - start date within existing
                 (PBD.StartDate <= @StartDate AND PBD.Sale_End_Date >= @StartDate)
              -- Promo conflict - end date within existing
              OR (PBD.StartDate <= @EndDate AND PBD.Sale_End_Date >= @EndDate)
              -- Promo conflict - existing engulfed
              OR (PBD.StartDate > @StartDate AND PBD.Sale_End_Date < @EndDate)
              -- Reg price chg conflict - reg price start date within input promo
              OR (PBD.PriceChgTypeID = 1 AND PBD.StartDate >= @StartDate AND PBD.StartDate <= @EndDate)
             )
             AND NOT (PBD.LineDrive = 1 AND PBD.StartDate >= @StartDate AND PBD.StartDate <= @EndDate)
            )

    SET NOCOUNT OFF
END

GO*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


