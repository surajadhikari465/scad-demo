SET NOCOUNT ON;

SELECT 
	ISNULL(CAST(p.GpmID as nvarchar),'') as gpm_id,
	p.Region as region,
	i.ScanCode as scan_code,
	p.ItemID as item_id,
	p.BusinessUnitID as business_unit_id,
	CONVERT(nvarchar, p.StartDate, 23) as start_date,
	ISNULL(CONVERT(nvarchar, p.EndDate, 23),'') as end_date,
	p.Price as price,
	ISNULL(CAST(p.PercentOff as nvarchar), '') as percent_off,
	p.PriceType as price_type,
	ISNULL(p.PriceTypeAttribute,'') as price_type_attribute,
	ISNULL(p.SellableUOM,'') as sellable_uom,
	ISNULL(p.CurrencyCode,'') as currency_code,
	p.Multiple as multiple,
	ISNULL(CONVERT(nvarchar, p.TagExpirationDate, 120),'') as tag_expiration_date,
	CONVERT(nvarchar, P.InsertDateUtc, 120) as insert_date_utc,
	ISNULL(CONVERT(nvarchar, P.ModifiedDateUtc, 120),'') as modified_date_utc
FROM gpm.Prices p
JOIN dbo.Items i on p.ItemID = i.ItemID;