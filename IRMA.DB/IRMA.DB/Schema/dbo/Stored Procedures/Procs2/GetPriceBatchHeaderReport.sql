CREATE PROCEDURE dbo.GetPriceBatchHeaderReport
    @StoreList varchar(8000),
    @StoreListSeparator char(1),
    @SubTeam_No int,
    @StartDate smalldatetime,
    @EndDate smalldatetime,
    @PriceBatchStatusID tinyint
AS

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON

    DECLARE @Store TABLE (Store_No int)

    IF @StoreList IS NOT NULL
        INSERT INTO @Store
        SELECT Key_Value
        FROM dbo.fn_Parse_List(@StoreList, @StoreListSeparator) S
    ELSE
        INSERT INTO @Store
        SELECT Store_No FROM Store (nolock)

    SELECT Store_Name, SubTeam_Name, PriceBatchStatusDesc,
           CASE ISNULL(ItemChgTypeID, 0) WHEN 0 THEN 'Price'
                                         WHEN 1 THEN 'New'
                                         WHEN 2 THEN 'Item'
                                         WHEN 3 THEN 'Delete' END As ChangeType,
           isnull(PCT.PriceChgTypeDesc,'') As PriceType, 
           BatchDescription, StartDate, PrintedDate,
           (SELECT COUNT(*) FROM (SELECT D.PriceBatchHeaderID, Item_Key 
                                  FROM PriceBatchDetail D (nolock)
                                  INNER JOIN
                                      PriceBatchHeader H (nolock)
                                      ON H.PriceBatchHeaderID = D.PriceBatchHeaderID 
                                  WHERE PriceBatchStatusID < 6
                                  GROUP BY D.PriceBatchHeaderID, Item_Key) T 
            WHERE T.PriceBatchHeaderID = PBH.PriceBatchHeaderID) As TotalItems
    FROM 
        (SELECT H.PriceBatchHeaderID, PriceBatchStatusID, H.ItemChgTypeID, H.PriceChgTypeID, H.BatchDescription , H.StartDate, PrintedDate,
                D.Store_No, ISNULL(D.SubTeam_No, Item.SubTeam_No) AS SubTeam_No
         FROM PriceBatchHeader H (nolock)
         INNER JOIN
             PriceBatchDetail D (nolock)
             ON D.PriceBatchHeaderID = H.PriceBatchHeaderID
         INNER JOIN
             @Store S
             ON S.Store_No = D.Store_No
         INNER JOIN
             Item (nolock)
             ON Item.Item_Key = D.Item_Key
         GROUP BY H.PriceBatchHeaderID, PriceBatchStatusID, H.ItemChgTypeID, H.PriceChgTypeID, H.BatchDescription, H.StartDate, PrintedDate,
                  D.Store_No, ISNULL(D.SubTeam_No, Item.SubTeam_No)) PBH
    INNER JOIN
        Store (nolock)
        ON Store.Store_No = PBH.Store_No
    INNER JOIN
        SubTeam (nolock)
        ON SubTeam.SubTeam_No = PBH.SubTeam_No
    INNER JOIN
        PriceBatchStatus PBS (nolock)
        ON PBS.PriceBatchStatusID = PBH.PriceBatchStatusID
    LEFT JOIN 
		PriceChgType PCT
		ON PCT.PriceChgTypeID = PBH.PriceChgTypeID
    WHERE ((PBH.PriceBatchStatusID < 6) OR (@PriceBatchStatusID IS NOT NULL))
        AND PBH.PriceBatchStatusID = ISNULL(@PriceBatchStatusID, PBH.PriceBatchStatusID)
        AND StartDate >= ISNULL(@StartDate, StartDate)
        AND StartDate <= ISNULL(@EndDate, StartDate)
        AND PBH.SubTeam_No = ISNULL(@SubTeam_No, PBH.SubTeam_No)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchHeaderReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchHeaderReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchHeaderReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchHeaderReport] TO [IRMAReportsRole]
    AS [dbo];

