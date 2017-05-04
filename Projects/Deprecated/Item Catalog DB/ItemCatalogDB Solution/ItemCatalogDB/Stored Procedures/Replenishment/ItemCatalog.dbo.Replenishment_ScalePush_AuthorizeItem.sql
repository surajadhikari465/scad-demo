 /****** Object:  StoredProcedure [dbo].[Replenishment_ScalePush_AuthorizeItem]    Script Date: 6/26/2007 16:32:49 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_ScalePush_AuthorizeItem]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_ScalePush_AuthorizeItem]
GO

CREATE PROCEDURE dbo.Replenishment_ScalePush_AuthorizeItem
	@StoreItemAuthorizationId int
AS 

BEGIN
	--RESET StoreItem.ScaleAuth FLAG FOR SCALE PUSH PROCESS
	UPDATE StoreItem 
	SET ScaleAuth = 0
	WHERE StoreItemAuthorizationId = @StoreItemAuthorizationId
END

GO


 