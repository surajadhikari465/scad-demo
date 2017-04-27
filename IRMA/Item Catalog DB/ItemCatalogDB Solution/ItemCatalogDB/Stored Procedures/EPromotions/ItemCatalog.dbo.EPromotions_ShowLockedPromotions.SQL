   /****** Object:  StoredProcedure [dbo].[EPromotions_ShowLockedPromotions]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_ShowLockedPromotions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_ShowLockedPromotions]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_ShowLockedPromotions]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE  dbo.EPromotions_ShowLockedPromotions
@OfferId int
as
BEGIN
-- return a list of users that are currently editing a specific group.
	SELECT IsEdited, FullName  
	FROM PromotionalOffer po LEFT JOIN Users u on u.User_Id = po.IsEdited 
	WHERE Offer_Id = @OfferId AND u.FullName IS NOT NULL

END
GO