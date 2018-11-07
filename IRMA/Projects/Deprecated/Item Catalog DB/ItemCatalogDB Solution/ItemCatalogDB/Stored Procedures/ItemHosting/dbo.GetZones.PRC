SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetZones]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetZones]
GO

CREATE PROCEDURE [dbo].[GetZones]

AS 

	SET NOCOUNT ON

	BEGIN
	
	SELECT DISTINCT 
		Zone.Zone_id, 
		Zone_Name
	FROM dbo.Zone (NOLOCK) INNER JOIN Store (NOLOCK) ON Zone.Zone_Id = Store.Zone_Id
	WHERE 
		dbo.fn_GetCustomerType(Store.Store_No, Internal, BusinessUnit_ID) = 3				-- Regional

	UNION ALL											

	SELECT DISTINCT																			-- Include new zones
		Zone_ID, 
		Zone_Name
	FROM dbo.Zone (NOLOCK)
	WHERE
		Zone_Name NOT IN
		( 
			SELECT DISTINCT Zone_Name
			FROM Zone (NOLOCK) INNER JOIN Store (NOLOCK) ON Zone.Zone_Id = Store.Zone_Id
		)
	ORDER BY Zone_Name, Zone.Zone_Id
	END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

