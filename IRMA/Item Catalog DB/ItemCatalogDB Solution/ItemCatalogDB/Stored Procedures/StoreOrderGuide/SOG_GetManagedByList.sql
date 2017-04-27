SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetManagedByList')
	BEGIN
		DROP Procedure [dbo].SOG_GetManagedByList
	END
GO

CREATE PROCEDURE dbo.SOG_GetManagedByList

WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetManagedByList()
--    Author: Billy Blackerby
--      Date: 3/19/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of item managers
--
-- Modification History:
-- Date			Init	Comment
-- 03/19/2009	BBB		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	 SELECT
		[ManagedByID]	= im.Manager_ID,
		[ManagedBy]		= im.Value
	FROM 
		ItemManager (nolock) im
	WHERE
		im.Value IN 
					(
					SELECT
						ca.AdminValue
					FROM
						CatalogAdmin (nolock) ca
					WHERE
						ca.AdminKey = 'ItemManager'
					)

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO