IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_IsGroupInCurrentPromotion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_IsGroupInCurrentPromotion]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_IsGroupInCurrentPromotion]    Script Date: 05/31/2006 16:02:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 
 CREATE PROCEDURE dbo.EPromotions_IsGroupInCurrentPromotion
	@GroupId int,
	@OfferId int
AS
BEGIN

	SET NOCOUNT ON;
	
	select case when exists (
		select * from PromotionalOfferMembers 
		where Group_Id = @GroupId and Offer_Id= @OfferId
	) then 'True' else 'False' end as IsGroupInCurrentPromotion


END
GO