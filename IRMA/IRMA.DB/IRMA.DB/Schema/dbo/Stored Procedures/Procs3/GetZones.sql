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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetZones] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetZones] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetZones] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetZones] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetZones] TO [IRMAExcelRole]
    AS [dbo];

