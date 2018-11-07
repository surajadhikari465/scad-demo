CREATE PROCEDURE dbo.GetItem_LabelSummary
    @ItemList varchar(8000),
    @ItemListSeparator char(1)
AS
BEGIN
    SET NOCOUNT ON

    SELECT
        L.LabelType_ID,
        MAX(L.LabelTypeDesc) AS LabelTypeDesc,
        COUNT(I.Item_Key) AS Item_Count 
    FROM Item I
        INNER JOIN LabelType L
            ON I.LabelType_ID = L.LabelType_ID    
        INNER JOIN fn_Parse_List(@ItemList, @ItemListSeparator) IL
            ON IL.Key_Value = I.Item_Key 
    GROUP BY L.LabelType_ID
    


    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItem_LabelSummary] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItem_LabelSummary] TO [IRMAClientRole]
    AS [dbo];

