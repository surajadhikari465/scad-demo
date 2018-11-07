 
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_ValidatePromotionName]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_ValidatePromotionName]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.EPromotions_ValidatePromotionName
	@PromotionName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;
	-- returns true if @PromotionName does not already exist
	-- returns false if @PromotionName is already used as a promotion name. 
	select case when exists ( select Offer_Id from PromotionalOffer where Description = @PromotionName ) then 'False' else 'True' end as IsValid
END
GO