
CREATE PROCEDURE dbo.Replenishment_POSPush_GetPOSFileWriterClass

AS

-- **************************************************************************
-- Procedure: Replenishment_POSPush_PopulateIconPOSPushPublish
--    Author: Denis Ng
--      Date: 06/12/2014
--
-- Description:
-- This procedure will return a list of stores with their corresponding POS
-- Writer (POSFileWriterCode)
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 05/19/2014	DN   			15056	Created
-- **************************************************************************

	BEGIN
		SELECT SPC.Store_No, POS.POSFileWriterCode
		FROM StorePOSConfig SPC (NOLOCK) INNER JOIN POSWriter POS (NOLOCK)
		ON SPC.POSFileWriterKey = POS.POSFileWriterKey

	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPOSFileWriterClass] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetPOSFileWriterClass] TO [IRSUser]
    AS [dbo];

