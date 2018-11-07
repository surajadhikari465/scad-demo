CREATE PROCEDURE dbo.GetPriceBatchHeader_LabelSummary
    @ItemList varchar(8000),
    @ItemListSeparator char(1),
    @PriceBatchHeaderID int
AS
BEGIN
    SET NOCOUNT ON

    SELECT
        L.LabelType_ID,
        MAX(L.LabelTypeDesc) AS LabelTypeDesc,
        COUNT(DISTINCT PBD.Item_Key) AS Item_Count,
        MAX(PBD.PriceBatchHeaderID) AS PriceBatchHeaderID
    FROM PriceBatchDetail PBD 
        INNER JOIN Item I
            ON PBD.Item_Key = I.Item_Key
        INNER JOIN LabelType L
            ON I.LabelType_ID = L.LabelType_ID    
        INNER JOIN fn_Parse_List(@ItemList, @ItemListSeparator) IL
            ON IL.Key_Value = PBD.Item_Key 
    WHERE
        PBD.PriceBatchHeaderID = @PriceBatchHeaderID
    GROUP BY L.LabelType_ID
    


    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchHeader_LabelSummary] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchHeader_LabelSummary] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchHeader_LabelSummary] TO [IRMAReportsRole]
    AS [dbo];

