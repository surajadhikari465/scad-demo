CREATE PROCEDURE dbo.Administration_POSPush_GetFileWriterTypes(
	@IsConfigWriter bit
)
 AS 

BEGIN
	-- Queries the FileWriterType table to retrieve all of the available FileWriterType entries.
	-- If @IsConfigWriter = 1 then data is limited to those wehre IsConfigWriter = 1
	SELECT FileWriterType  
	FROM FileWriterType 
	WHERE ((@IsConfigWriter = 1 AND IsConfigWriter = 1) OR (@IsConfigWriter = 0)) 
	ORDER BY FileWriterType
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetFileWriterTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetFileWriterTypes] TO [IRMAClientRole]
    AS [dbo];

