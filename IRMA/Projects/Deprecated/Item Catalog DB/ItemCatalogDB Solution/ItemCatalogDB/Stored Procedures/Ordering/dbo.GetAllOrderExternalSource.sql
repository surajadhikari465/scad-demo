
/****** Object:  StoredProcedure [dbo].[GetAllOrderExternalSource]    Script Date: 09/21/2009 13:46:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllOrderExternalSource]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAllOrderExternalSource]
GO
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
