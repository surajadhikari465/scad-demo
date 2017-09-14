CREATE TABLE gpm.ActivePriceSentArchive
(
	Region nvarchar(2),
	PriceID bigint
)

GO

CREATE CLUSTERED INDEX CIX_ActivePriceSentArchive ON gpm.ActivePriceSentArchive (Region, PriceID)
GO

GRANT INSERT ON [gpm].[ActivePriceSentArchive] TO [TibcoRole]
GO