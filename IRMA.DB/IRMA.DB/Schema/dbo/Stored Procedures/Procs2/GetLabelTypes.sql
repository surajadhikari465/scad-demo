CREATE PROCEDURE dbo.[GetLabelTypes] AS

BEGIN
    SET NOCOUNT ON

	SELECT [LabelType_ID], [LabelTypeDesc]
	FROM [LabelType]
	ORDER BY [LabelTypeDesc]
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLabelTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLabelTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLabelTypes] TO [IRMAExcelRole]
    AS [dbo];

