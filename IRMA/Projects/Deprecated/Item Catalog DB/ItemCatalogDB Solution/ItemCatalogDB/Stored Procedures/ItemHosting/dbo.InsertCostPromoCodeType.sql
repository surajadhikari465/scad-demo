 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCostPromoCodeType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCostPromoCodeType]
GO

CREATE PROCEDURE [dbo].[InsertCostPromoCodeType]
    @CostPromoCode int,
    @CostPromoDesc varchar(50)
    
AS 
BEGIN
    SET NOCOUNT ON
    
    INSERT INTO CostPromoCodeType (CostPromoCode, CostPromoDesc)
    VALUES (@CostPromoCode, @CostPromoDesc)
    
    SET NOCOUNT OFF
END