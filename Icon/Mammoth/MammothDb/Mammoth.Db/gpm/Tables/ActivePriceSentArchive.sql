CREATE TABLE gpm.ActivePriceSentArchive
(
	Region nvarchar(2),
	PriceID bigint
)

GO

CREATE CLUSTERED INDEX CIX_ActivePriceSentArchive ON gpm.ActivePriceSentArchive (Region, PriceID)