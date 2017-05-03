 /****** Object:  StoredProcedure [dbo].[EPromotions_GetPublishedOffersByGroup]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_GetRewardTypes]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_GetRewardTypes]
GO

/****** Object:  StoredProcedure [dbo].[EPromotions_GetRewardTypes]    Script Date: 06/22/2006 13:08:12 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO


/*
	Retreives records from RewardType.
*/
CREATE  PROCEDURE [dbo].[EPromotions_GetRewardTypes] 
AS

	SELECT [RewardType_ID], [Reward_Name]
	FROM [RewardType]
	ORDER BY Reward_Name

 GO