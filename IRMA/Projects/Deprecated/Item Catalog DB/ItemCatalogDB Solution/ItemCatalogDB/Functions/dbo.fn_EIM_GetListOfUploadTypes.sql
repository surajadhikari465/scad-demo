IF EXISTS (SELECT * 
	   FROM   sysobjects 
	   WHERE  name = N'fn_EIM_GetListOfUploadTypes')
	DROP FUNCTION fn_EIM_GetListOfUploadTypes
GO

-- Return a comma concatonated string of one letter abbreviations
-- of the UploadTypes for the provided session ID.
CREATE FUNCTION dbo.fn_EIM_GetListOfUploadTypes 
	(@UploadSession_ID int)
RETURNS varchar(50)
AS
BEGIN

	DECLARE @UploadTypeCodeDelimetedString As varchar(50)
		            
	SELECT @UploadTypeCodeDelimetedString =
			COALESCE(@UploadTypeCodeDelimetedString + ', ', '') + SUBSTRING(UploadType_Code, 1, 1)
		FROM UploadSessionUploadType (NOLOCK)
		WHERE UploadSession_ID = @UploadSession_ID
		ORDER BY UploadType_Code
		
	RETURN @UploadTypeCodeDelimetedString
	
END
GO