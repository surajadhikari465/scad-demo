CREATE PROCEDURE dbo.EIM_SearchUploadSessions
		@UploadSession_ID int,
		@UploadTypeCode varchar(50),
		@SessionName varchar(100),
		@CreatedByUserID int,
		@CreatedDateTime dateTime,
		@SavedOnly bit,
		@UploadedOnly bit,
		@SavedAndUploaded bit	
AS

	-- Created By:	David Marine
	-- Created   :	Feb 13, 2007

	SELECT DISTINCT
		us.UploadSession_ID,
		us.IsUploaded,
		us.[Name],
		us.ItemsProcessedCount,
		us.IsNewItemSessionFlag,
		us.IsDeleteItemSessionFlag,
		us.IsFromSLIM,
		us.ItemsLoadedCount,
		us.ErrorsCount,
		us.EmailToAddress,
		CreatedByUser.FullName as CreatedByName,
		us.CreatedDateTime,
		ModifiedByUser.FullName as ModifiedByName,
		us.ModifiedDateTime,
		dbo.fn_EIM_GetListOfUploadTypes(us.UploadSession_ID) AS UploadTypeList
	FROM UploadSession us (NOLOCK)
		LEFT OUTER JOIN UploadSessionUploadType usut (NOLOCK)
			ON usut.UploadSession_ID = us.UploadSession_ID
		JOIN Users CreatedByUser (NOLOCK)
			ON CreatedByUser.User_ID = us.CreatedByUserID
		LEFT OUTER JOIN Users ModifiedByUser (NOLOCK)
			ON ModifiedByUser.User_ID = us.ModifiedByUserID
	WHERE
		(@UploadSession_ID IS NULL OR us.UploadSession_ID = @UploadSession_ID) AND
		(@UploadTypeCode IS NULL OR usut.UploadType_Code = @UploadTypeCode) AND
		(@SessionName IS NULL OR LOWER(us.[Name]) LIKE LOWER(REPLACE(LTRIM(RTRIM(@SessionName)), '''', '''''')) + '%') AND
		(@CreatedByUserID IS NULL OR us.CreatedByUserID = @CreatedByUserID) AND
		(@CreatedDateTime IS NULL OR dbo.fn_GetDateOnly(us.CreatedDateTime) = dbo.fn_GetDateOnly(@CreatedDateTime)) AND
		(@SavedAndUploaded = 1 OR @SavedOnly = 0 OR us.IsUploaded = 0) AND
		(@SavedAndUploaded = 1 OR @UploadedOnly = 0 OR us.IsUploaded = 1)



--=====================================================================
--*********      dbo.EIM_UpdateUploadAttribute                    
--=====================================================================

SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_SearchUploadSessions] TO [IRMAClientRole]
    AS [dbo];

