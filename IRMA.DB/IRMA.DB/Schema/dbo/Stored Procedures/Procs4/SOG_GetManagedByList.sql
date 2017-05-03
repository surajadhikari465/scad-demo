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
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_GetManagedByList] TO [IRMASLIMRole]
    AS [dbo];

