   /****** Object:  StoredProcedure [dbo].[EPromotions_ShowLockedGroups]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_ShowLockedGroups]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_ShowLockedGroups]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_ShowLockedGroups]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE  dbo.EPromotions_ShowLockedGroups
@OfferId int
as

BEGIN
-- Return a list of users that are editing any group in a specific promotion.

SELECT IsEdited = ISNULL(IsEdited,0), FullName  
FROM promotionaloffermembers  pom INNER JOIN itemgroup ig ON ig.group_id = pom.group_id  LEFT JOIN users u ON  u.user_id = ig.isedited 
WHERE offer_id = 47 AND u.fullname IS NOT NULL

END


GO