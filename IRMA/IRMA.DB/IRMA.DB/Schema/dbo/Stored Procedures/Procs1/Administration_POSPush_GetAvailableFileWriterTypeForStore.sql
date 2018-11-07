CREATE PROCEDURE dbo.Administration_POSPush_GetAvailableFileWriterTypeForStore(
	@Store_No int,
	@IsConfigWriter bit
)
 AS 

BEGIN
	-- Queries the StoreFTPConfig table to retrieve all of the FileWriterType entries that have not yet been
	-- configured for that store.
	-- If @IsConfigWriter = 1 then data is limited to those where IsConfigWriter = 1
	SELECT FileWriterType  
	FROM FileWriterType
	WHERE FileWriterType NOT IN (SELECT FileWriterType FROM StoreFTPConfig WHERE Store_No = @Store_No)
		AND ((@IsConfigWriter = 1 AND IsConfigWriter = 1) OR (@IsConfigWriter = 0)) 
	ORDER BY FileWriterType
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetAvailableFileWriterTypeForStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetAvailableFileWriterTypeForStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetAvailableFileWriterTypeForStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetAvailableFileWriterTypeForStore] TO [IRMAReportsRole]
    AS [dbo];

