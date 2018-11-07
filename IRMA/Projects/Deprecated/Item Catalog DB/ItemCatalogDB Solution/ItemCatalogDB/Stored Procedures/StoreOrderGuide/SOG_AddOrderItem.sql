SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_AddOrderItem')
	BEGIN
		DROP Procedure [dbo].SOG_AddOrderItem
	END
GO

CREATE PROCEDURE dbo.SOG_AddOrderItem
	@CatalogOrderID	int,
	@CatalogItemID	int,
	@Quantity		int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_AddOrderItem()
--    Author: Billy Blackerby
--      Date: 3/25/2009
--
-- Description:
-- Utilized by StoreOrderGuide to insert an OrderItem
--
-- Modification History:
-- Date			Init	Comment
-- 03/25/2009	BBB		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
    --**************************************************************************
	--Retrieve Item SubTeam
	--**************************************************************************

    
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	INSERT INTO 
		CatalogOrderItem
	(
		CatalogOrderID,
		CatalogItemID,
		SubTeamID,
		Quantity
	)
	(	
	SELECT
		@CatalogOrderID,
		@CatalogItemID,
		SubTeam_No,
		@Quantity
	FROM
		Item					(nolock) i
		INNER JOIN CatalogItem	(nolock) ci ON i.Item_Key = ci.ItemKey
	WHERE
		CatalogItemID = @CatalogItemID
	)

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 