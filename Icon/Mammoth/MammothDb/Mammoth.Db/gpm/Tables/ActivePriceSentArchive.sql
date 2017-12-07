CREATE TABLE gpm.ActivePriceSentArchive
(
	Region nvarchar(2),
	PriceID bigint,
	InsertDateUtc datetime2(7)
)

GO

CREATE CLUSTERED INDEX CIX_ActivePriceSentArchive ON gpm.ActivePriceSentArchive (Region, PriceID)
GO

GRANT SELECT, UPDATE, INSERT ON [gpm].[ActivePriceSentArchive] TO [TibcoRole]
GO