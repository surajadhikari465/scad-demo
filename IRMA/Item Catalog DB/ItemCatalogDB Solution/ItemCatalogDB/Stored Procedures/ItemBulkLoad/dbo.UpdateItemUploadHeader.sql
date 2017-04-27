if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdateItemUploadHeader') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdateItemUploadHeader
GO

CREATE PROCEDURE dbo.UpdateItemUploadHeader
	@ItemUploadHeader_ID as int,
	@ItemsProcessedCount as int,
	@ItemsLoadedCount as int, 
	@ErrorsCount as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE 
		ItemUploadHeader 
	SET 
		ItemsProcessedCount = @ItemsProcessedCount,
		ItemsLoadedCount = @ItemsLoadedCount,
		ErrorsCount = @ErrorsCount
	WHERE 
		ItemUploadHeader_ID = @ItemUploadHeader_ID

	SET NOCOUNT OFF;

END
GO
