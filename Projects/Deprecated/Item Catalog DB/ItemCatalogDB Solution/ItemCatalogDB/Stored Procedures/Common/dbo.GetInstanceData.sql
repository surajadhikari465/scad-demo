/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_GetPriceBatchOffers]    Script Date: 07/13/2006 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetInstanceData]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetInstanceData]
GO

/****** Object:  StoredProcedure [dbo].[GetInstanceData]    Script Date: 07/13/2006 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.GetInstanceData 
AS
BEGIN
    SET NOCOUNT ON

     --20100215 - Dave Stacey - Add UG Culture, DateMask to facilitate UK EIM 
     
     
	SELECT PrimaryRegionName, PrimaryRegionCode, PluDigitsSentToScale, UG_Culture, UG_DateMask
    FROM dbo.InstanceData (NOLOCK)
    
    SET NOCOUNT OFF
END
GO
