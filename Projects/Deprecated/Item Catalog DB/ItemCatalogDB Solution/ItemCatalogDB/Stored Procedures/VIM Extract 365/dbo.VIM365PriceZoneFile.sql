IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID(N'dbo.VIM365PriceZoneFile'))
	EXEC('CREATE PROCEDURE [dbo].[VIM365PriceZoneFile] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[VIM365PriceZoneFile]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	DECLARE @365RegionCode varchar(2) = 'TS';

	SELECT
		@365RegionCode AS REGION,  
		Store.BusinessUnit_ID AS PS_BU,
		Zone.Zone_ID AS PRICEZONE, 
		SubTeam.Dept_No AS POS_DEPT,
	    Zone.Zone_Name AS PZ_DESCRIPTION
	FROM Store (NOLOCK)
	INNER JOIN Zone (NOLOCK) ON Store.Zone_ID = Zone.Zone_ID
	INNER JOIN StoreSubteam (NOLOCK) ON Store.Store_No = StoreSubteam.Store_No
	INNER JOIN SubTeam (NOLOCK) ON StoreSubteam.SubTeam_No = SubTeam.SubTeam_No
	WHERE 
		(MEGA_STORE = 1 AND WFM_STORE = 0)
		AND SubTeam.Team_No <> 990
		AND Store.Store_No in (select store_no from storeregionmapping where 
									region_code = @365RegionCode)
	union  
	select @365RegionCode AS REGION, 0 as ps_bu, 
		0 as pricezone, 0 as pos_dept, 'DEFAULT' as pz_description
END	
GO
	SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


