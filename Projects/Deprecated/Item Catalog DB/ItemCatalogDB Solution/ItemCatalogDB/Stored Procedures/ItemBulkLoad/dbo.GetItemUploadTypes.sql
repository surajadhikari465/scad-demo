IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemUploadTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetItemUploadTypes]
GO

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

