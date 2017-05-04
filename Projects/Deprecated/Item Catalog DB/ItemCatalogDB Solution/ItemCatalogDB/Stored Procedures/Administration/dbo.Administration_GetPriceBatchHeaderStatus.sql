IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_GetPriceBatchHeaderStatus]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP  Procedure  [dbo].[Administration_GetPriceBatchHeaderStatus]
GO

CREATE PROCEDURE [dbo].[Administration_GetPriceBatchHeaderStatus]
    @PriceBatchHeaderID int
AS
BEGIN
    SET NOCOUNT ON
		SELECT PriceBatchStatusID 
		FROM dbo.PriceBatchHeader
		WHERE PriceBatchHeaderID = @PriceBatchHeaderID
    SET NOCOUNT OFF
END
GO
 