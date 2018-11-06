CREATE PROCEDURE dbo.ItemHosting_GetLabelType AS

BEGIN
    SET NOCOUNT ON

    SELECT LabelType_ID, LabelTypeDesc FROM LabelType ORDER BY LabelTypeDesc
    
    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemHosting_GetLabelType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemHosting_GetLabelType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemHosting_GetLabelType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemHosting_GetLabelType] TO [IRMAReportsRole]
    AS [dbo];

