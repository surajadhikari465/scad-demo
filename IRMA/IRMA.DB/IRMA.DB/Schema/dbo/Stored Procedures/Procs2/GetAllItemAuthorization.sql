CREATE PROCEDURE [dbo].[GetAllItemAuthorization]
	-- Add the parameters for the stored procedure here
	@PriceChgTypeID		INT,
	@Sale_Start_Date	SMALLDATETIME,
	@Sale_End_Date		SMALLDATETIME,
	@SubTeam_No			INT
AS
   -- **************************************************************************
   -- Procedure: GetAllItemAuthorization()
   --    Author: Hussain Hashim
   --      Date: 8/30/2007
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures. Get Item Authorization infomation based on Price Change
   -- Type and Date range
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 08/11/2009  BBB	Updated data treatment of Item Size to allow for decimals;
   --					added calls to ItemOverride to both queries for jurisdiction
   -- **************************************************************************
BEGIN

declare @ReportSubteams table (Subteam_No int)
if @Subteam_No is null 
	begin
		insert into @ReportSubteams (Subteam_No)
		select Subteam_No from Subteam
	end
else
	begin 
		insert into @ReportSubteams (Subteam_No)
		select Param
		FROM dbo.fn_MVParam(@Subteam_No, ',')
	end

SELECT
	PBD.Item_Key
	, isnull(dbo.ItemVendor.Vendor_ID, 0) as Vendor_ID
	, CONVERT(VARCHAR(20), dbo.ItemVendor.Item_ID) AS Item_ID
	, PBD.Store_No
	, dbo.Store.Store_Name
	, isnull(dbo.Vendor.Vendor_Key, 'none') as Vendor_Key
	, (CASE WHEN dbo.StoreItemVendor.PrimaryVendor = 'True' THEN 'Yes' ELSE 'No' END) AS PrimaryVendor
	, isnull(dbo.Vendor.CompanyName, 'none') as CompanyName
	, ISNULL(iov.Item_Description,dbo.Item.Item_Description) As Item_Description
	, dbo.SubTeam.SubTeam_Name
	, dbo.Zone.Zone_ID
	, dbo.Zone.Zone_Name
	, (CASE WHEN dbo.StoreItem.Authorized = 'True' THEN 'Yes' ELSE 'No' END) AS Authorized
	, dbo.PriceChgType.PriceChgTypeID
	, dbo.PriceChgType.PriceChgTypeDesc
	, PBD.StartDate AS Sale_Start_Date
	, PBD.Sale_End_Date
	, dbo.ItemIdentifier.Identifier AS upcno
	, CONVERT(VARCHAR(10), CONVERT(decimal(10,2), ISNULL(iov.Package_Desc2,dbo.Item.Package_Desc2))) + ' ' + ISNULL(iu2.Unit_Abbreviation, dbo.ItemUnit.Unit_Abbreviation) AS Item_Size
	, ISNULL(iu2.Unit_Abbreviation, dbo.ItemUnit.Unit_Abbreviation) As Unit_Abbreviation
	, ISNULL(iov.Package_Desc1,dbo.Item.Package_Desc1) AS Pack
	, 'Pending' AS PriceStatus
FROM 
	dbo.PriceBatchDetail AS PBD 
	INNER JOIN dbo.Item ON PBD.Item_Key = dbo.Item.Item_Key 
	INNER JOIN dbo.ItemIdentifier ON dbo.Item.Item_Key = dbo.ItemIdentifier.Item_Key
	INNER JOIN dbo.Store ON PBD.Store_No = dbo.Store.Store_No
	LEFT OUTER JOIN dbo.StoreItem ON PBD.Item_Key = dbo.StoreItem.Item_Key
		AND PBD.Store_No = dbo.StoreItem.Store_No
	LEFT OUTER JOIN dbo.StoreItemVendor ON PBD.Item_Key = dbo.StoreItemVendor.Item_Key 
		AND PBD.Store_No = dbo.StoreItemVendor.Store_No 
	LEFT OUTER JOIN dbo.ItemVendor ON PBD.Item_Key = dbo.ItemVendor.Item_Key 
		AND dbo.StoreItemVendor.Vendor_ID = dbo.ItemVendor.Vendor_ID
	LEFT OUTER JOIN dbo.Vendor ON dbo.ItemVendor.Vendor_ID = dbo.Vendor.Vendor_ID 
	INNER JOIN dbo.SubTeam ON dbo.Item.SubTeam_No = dbo.SubTeam.SubTeam_No 
	INNER JOIN dbo.Zone ON dbo.Store.Zone_ID = dbo.Zone.Zone_ID 
	INNER JOIN dbo.PriceChgType ON PBD.PriceChgTypeID = dbo.PriceChgType.PriceChgTypeID 
	INNER JOIN dbo.ItemUnit ON dbo.Item.Package_Unit_ID = dbo.ItemUnit.Unit_ID 
	LEFT OUTER JOIN dbo.PriceBatchHeader ON PBD.PriceBatchHeaderID = dbo.PriceBatchHeader.PriceBatchHeaderID 
	LEFT JOIN dbo.ItemOverride iov (nolock)
					 on pbd.Item_Key = iov.Item_Key AND iov.StoreJurisdictionID = Store.StoreJurisdictionID
	LEFT JOIN dbo.ItemUnit iu2 ON iov.Package_Unit_ID = iu2.Unit_ID
WHERE 
	(PBD.PriceChgTypeID = @PriceChgTypeID) 
	AND (PBD.StartDate >= @Sale_Start_Date) 
	AND (isnull(PBD.Sale_End_Date,'6/6/2079') <= isnull(@Sale_End_Date,'6/6/2079'))
	AND (Item.SubTeam_No IN
		(select Subteam_No from @ReportSubteams))
	AND (PBD.Expired <> 1) 
	AND ((dbo.PriceBatchHeader.PriceBatchStatusID <> 6) or dbo.PriceBatchHeader.PriceBatchStatusID is null)
	and ItemVendor.deletedate is null -- Dont show deleted items.

UNION
SELECT     
	Item_1.Item_Key
	, isnull(ItemVendor_1.Vendor_ID, 0) as Vendor_ID
	, isnull(CONVERT(VARCHAR(20), ItemVendor_1.Item_ID),'none') AS Item_ID
	, Store_1.Store_No
	, Store_1.Store_Name
	, isnull(Vendor_1.Vendor_Key, 'none') as Vendor_Key
	, (CASE WHEN StoreItemVendor_1.PrimaryVendor = 'True' THEN 'Yes' ELSE 'No' END) AS PrimaryVendor	
	, isnull(Vendor_1.CompanyName,'none') as CompanyName
	, ISNULL(iov.Item_Description,Item_1.Item_Description) As Item_Description
	, SubTeam_1.SubTeam_Name
	, Zone_1.Zone_ID
	, Zone_1.Zone_Name
	, (CASE WHEN StoreItem_1.Authorized = 'True' THEN 'Yes' ELSE 'No' END) AS Authorized
	, dbo.Price.PriceChgTypeId
	, PriceChgType_1.PriceChgTypeDesc
	, dbo.Price.Sale_Start_Date
	, dbo.Price.Sale_End_Date
	, ItemIdentifier_1.Identifier AS upcno
	, CONVERT(VARCHAR(10), CONVERT(decimal(10,2), ISNULL(iov.Package_Desc2,Item_1.Package_Desc2))) + ' ' + ISNULL(iu2.Unit_Abbreviation, ItemUnit_1.Unit_Abbreviation) AS Item_Size
	, ISNULL(iu2.Unit_Abbreviation, ItemUnit_1.Unit_Abbreviation) As Unit_Abbreviation
	, ISNULL(iov.Package_Desc1,Item_1.Package_Desc1) AS Pack
	, 'Current' AS PriceStatus
FROM
	dbo.Price
	INNER JOIN dbo.Item AS Item_1 ON Item_1.Item_Key = dbo.Price.Item_Key  
	INNER JOIN dbo.ItemIdentifier AS ItemIdentifier_1 ON Item_1.Item_Key = ItemIdentifier_1.Item_Key
	INNER JOIN dbo.Store AS Store_1 ON Store_1.Store_No = dbo.Price.Store_No
	LEFT OUTER JOIN dbo.StoreItem as StoreItem_1 ON dbo.Price.Item_Key = StoreItem_1.Item_Key
		AND Store_1.Store_No = StoreItem_1.Store_No
	LEFT OUTER JOIN dbo.StoreItemVendor AS StoreItemVendor_1 ON dbo.Price.Item_Key = StoreItemVendor_1.Item_Key 
		AND dbo.Price.Store_No = StoreItemVendor_1.Store_No
	LEFT OUTER JOIN dbo.ItemVendor AS ItemVendor_1 ON StoreItemVendor_1.Item_Key = ItemVendor_1.Item_Key
		AND StoreItemVendor_1.Vendor_ID = ItemVendor_1.Vendor_ID
	LEFT OUTER JOIN dbo.Vendor AS Vendor_1 ON ItemVendor_1.Vendor_ID = Vendor_1.Vendor_ID 
	INNER JOIN dbo.Zone AS Zone_1 ON Store_1.Zone_ID = Zone_1.Zone_ID 
	INNER JOIN dbo.PriceChgType AS PriceChgType_1 ON dbo.Price.PriceChgTypeId = PriceChgType_1.PriceChgTypeID 
	INNER JOIN dbo.ItemUnit AS ItemUnit_1 ON Item_1.Package_Unit_ID = ItemUnit_1.Unit_ID 
	INNER JOIN dbo.SubTeam AS SubTeam_1 ON Item_1.SubTeam_No = SubTeam_1.SubTeam_No 
	LEFT JOIN dbo.ItemOverride iov (nolock)
					 on Item_1.Item_Key = iov.Item_Key AND iov.StoreJurisdictionID = Store_1.StoreJurisdictionID
	LEFT JOIN dbo.ItemUnit iu2 ON iov.Package_Unit_ID = iu2.Unit_ID
WHERE     
	(dbo.Price.PriceChgTypeId = @PriceChgTypeID) 
	AND (dbo.Price.Sale_Start_Date >= @Sale_Start_Date) 
	AND (isnull(dbo.Price.Sale_End_Date,'6/6/2079') <= isnull(@Sale_End_Date,'6/6/2079')) 
	AND (Item_1.SubTeam_No IN
		(select Subteam_No from @ReportSubteams))
	and ItemVendor_1.deletedate is null -- Dont show deleted items.


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllItemAuthorization] TO [IRMAReportsRole]
    AS [dbo];

