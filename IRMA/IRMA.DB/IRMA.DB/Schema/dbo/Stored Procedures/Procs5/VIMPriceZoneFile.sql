
CREATE PROCEDURE dbo.VIMPriceZoneFile
AS 
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT  --(select primaryregioncode from instancedata) AS REGION,
		(select top 1 runmode from conversion_runmode) AS REGION,  
		Store.BusinessUnit_ID AS PS_BU,
		Zone.Zone_ID AS PRICEZONE, 
		--LEFT(SubTeam.SubTeam_No, 3) AS POS_DEPT,
		SubTeam.Dept_No AS POS_DEPT,
	    	Zone.Zone_Name AS PZ_DESCRIPTION
	FROM Store (NOLOCK)
	INNER JOIN Zone (NOLOCK) ON Store.Zone_ID = Zone.Zone_ID
	INNER JOIN StoreSubteam (NOLOCK) ON Store.Store_No = StoreSubteam.Store_No
	INNER JOIN SubTeam (NOLOCK) ON StoreSubteam.SubTeam_No = SubTeam.SubTeam_No
	WHERE (MEGA_STORE = 0 AND WFM_STORE = 1) --AND IP_Address <> 'NONE'))
	AND SubTeam.Team_No <> 990
	AND Store.Store_No in (select store_no from storeregionmapping where 
									region_code in (select top 1 runmode from conversion_runmode))
	union  
	select (select top 1 runmode from conversion_runmode) AS REGION, 0 as ps_bu, 
		0 as pricezone, 0 as pos_dept, 'DEFAULT' as pz_description
END	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPriceZoneFile] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPriceZoneFile] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPriceZoneFile] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPriceZoneFile] TO [IRMAReportsRole]
    AS [dbo];

