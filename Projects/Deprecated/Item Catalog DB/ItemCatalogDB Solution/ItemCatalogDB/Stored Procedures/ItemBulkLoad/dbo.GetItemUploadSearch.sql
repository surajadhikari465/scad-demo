if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetItemUploadSearch') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetItemUploadSearch
GO


CREATE PROCEDURE dbo.GetItemUploadSearch
    @ItemUploadHeader_ID int,
    @ItemUploadType_ID int,
    @User_ID int,
    @UploadDatetime smalldatetime
AS

BEGIN
    SET NOCOUNT ON

	SELECT 
		ItemUploadHeader_ID, 
		ItemUploadType_ID, 
		ItemsProcessedCount, 
		ItemsLoadedCount, 
		ErrorsCount, 
		EmailToAddress, 
		IUH.[User_ID], 
		U.UserName,
		UploadDateTime
	FROM 
		ItemUploadHeader IUH 
		LEFT OUTER JOIN Users U ON IUH.[User_ID] = U.[User_ID] 
	WHERE 
		ItemUploadHeader_ID = ISNULL(@ItemUploadHeader_ID, ItemUploadHeader_ID)
		AND ItemUploadType_ID = ISNULL(@ItemUploadType_ID, ItemUploadType_ID) 
		AND IUH.[User_ID] = ISNULL(@User_ID, IUH.[User_ID])
		AND ISNULL(UploadDateTime, 0) >= ISNULL(@UploadDateTime, ISNULL(UploadDateTime, 0))

    SET NOCOUNT OFF
END

GO
