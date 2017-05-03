/****** Object:  StoredProcedure [dbo].[Replenishment_ScalePush_DeAuthorizeItem]    Script Date: 6/26/2007 16:32:49 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_ScalePush_DeAuthorizeItem]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_ScalePush_DeAuthorizeItem]
GO

CREATE PROCEDURE dbo.Replenishment_ScalePush_DeAuthorizeItem
	@StoreItemAuthorizationId int
AS 

BEGIN
	--RESET StoreItem.POSDeAuth FLAG FOR SCALE PUSH PROCESS
	UPDATE StoreItem 
	SET ScaleDeAuth = 0
	WHERE StoreItemAuthorizationId = @StoreItemAuthorizationId
END

GO


  