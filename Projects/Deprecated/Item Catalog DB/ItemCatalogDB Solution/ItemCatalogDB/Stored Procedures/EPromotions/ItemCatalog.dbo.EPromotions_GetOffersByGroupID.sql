
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.EPromotions_GetOffersByGroupID    Script Date: 6/6/2006 6:59:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EPromotions_GetOffersByGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EPromotions_GetOffersByGroupID]
GO

/*
	Retreives records from PromotionalOffers which have the specified GroupID as a member.
*/
CREATE  PROCEDURE dbo.EPromotions_GetOffersByGroupID 
	@GroupID as integer
AS

	SELECT POM.[Offer_ID]
      ,[Description]
      ,[PricingMethod_ID]
      ,[StartDate]
      ,[EndDate]
      ,[RewardType]
      ,[RewardQuantity]
      ,[RewardAmount]
      ,[RewardGroupID]
      ,[createdate]
      ,[modifieddate]
      ,POM.[User_ID]
      ,[ReferenceCode]
      ,[TaxClass_ID]
      ,[SubTeam_No]
      ,[IsEdited]
	FROM [PromotionalOfferMembers] POM
	INNER JOIN [PromotionalOffer] PO
	ON POM.Offer_ID = PO.Offer_ID
	WHERE [Group_ID] = @GroupID



GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

 