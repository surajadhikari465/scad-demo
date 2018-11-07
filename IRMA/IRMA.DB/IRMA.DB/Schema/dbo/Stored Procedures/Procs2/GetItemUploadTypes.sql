CREATE PROCEDURE dbo.GetItemUploadTypes
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		ItemUploadType_ID,
		Description
	FROM 
		ItemUploadType

	SET NOCOUNT OFF;

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUploadTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUploadTypes] TO [IRMAClientRole]
    AS [dbo];

