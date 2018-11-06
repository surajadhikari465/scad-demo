IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteItemNutrifact]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[DeleteItemNutrifact]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-03-23
-- Description:	Removes an
--				item-to-nutrifact mapping.
-- Date			Modified By		TFS		Description
-- 07/31/2015	DN				16306	Set ItemNutrifact ID to NULL
-- =============================================

CREATE PROCEDURE dbo.DeleteItemNutrifact
	@ItemKey		int
AS
BEGIN
	
	set nocount on

    UPDATE ItemNutrition 
		SET NutriFactsID = NULL 
	WHERE ItemKey = @ItemKey
			
END
GO
