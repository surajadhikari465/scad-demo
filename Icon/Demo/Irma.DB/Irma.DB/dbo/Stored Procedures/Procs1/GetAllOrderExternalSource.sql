-- =============================================
-- Author:		Brian Robichaud	
-- Create date: 09/21/2009
-- Description:	Return list of External Order Sources
-- =============================================
CREATE PROCEDURE GetAllOrderExternalSource
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ID, Description FROM OrderExternalSource
    SET NOCOUNT OFF;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllOrderExternalSource] TO [IRMAClientRole]
    AS [dbo];

