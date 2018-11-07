CREATE PROCEDURE dbo.Administration_POSPush_GetScaleWriterTypes
 AS 

BEGIN
	-- Queries the ScaleWriterType table to retrieve all of the available ScaleWriterType entries.
	SELECT ScaleWriterTypeKey, ScaleWriterTypeDesc  
	FROM ScaleWriterType 
	ORDER BY ScaleWriterTypeDesc
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetScaleWriterTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetScaleWriterTypes] TO [IRMAClientRole]
    AS [dbo];

