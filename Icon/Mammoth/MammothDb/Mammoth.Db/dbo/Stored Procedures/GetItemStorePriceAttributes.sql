CREATE PROCEDURE [dbo].[GetItemStorePriceAttributes]
	@Region AS NVARCHAR(2),
	@ItemStores AS [dbo].[ScanCodeBusinessUnitIdType] READONLY
AS

SET TRANSACTION ISOLATION LEVEL SNAPSHOT

DECLARE @sql NVARCHAR(MAX);
SET @sql = N'
SELECT ScanCode, BusinessUnitID
INTO #ItemStores
FROM @ItemStores;

SELECT
	i.ItemID        as ItemID,
	i.ScanCode      as ScanCode,
	b.HierarchyClassName as BrandName,
    p.BusinessUnitID as BusinessUnitID,
	i.Desc_Product	as ItemDescription,
	i.PackageUnit	as PackageUnit,
	i.RetailSize	as RetailSize,
	i.RetailUOM		as RetailUOM,
	i.FoodStampEligible as FoodStamp,
	st.Name			as SubTeam,
	il.Sign_Desc	as SignDescription,
    il.Authorized	as Authorized,
	p.PriceType		as PriceType,
	p.Multiple		as Multiple,
	p.Price			as Price,
	p.StartDate		as StartDate,
	p.EndDate		as EndDate,
	p.CurrencyCode	as Currency,
	p.SellableUOM	as SellableUOM,
	p.PriceTypeAttribute as PriceAttribute,
	p.PercentOff	as PercentOff
FROM 
	#ItemStores							k
	INNER JOIN dbo.Items				i	on k.ScanCode = i.ScanCode
	INNER JOIN dbo.Financial_SubTeam	st	on i.PSNumber = st.PSNumber
	INNER JOIN dbo.HierarchyClass		b	on i.BrandHCID = b.HierarchyClassID
	INNER JOIN gpm.Price_' + @Region + '               p	on i.ItemID = p.ItemID
															AND k.BusinessUnitID = p.BusinessUnitID
	INNER JOIN dbo.ItemAttributes_Locale_' + @Region + '	il	on p.ItemID = il.ItemID
										AND p.BusinessUnitID = il.BusinessUnitID';

EXEC sp_executesql @sql, N'@ItemStores [dbo].[ScanCodeBusinessUnitIdType] READONLY', @ItemStores
GO