CREATE TYPE infor.ItemTraitAddOrUpdateType AS TABLE
(
	ItemId INT NOT NULL,
	TraitId INT NOT NULL,
	TraitValue NVARCHAR(255) NULL,
	LocaleId INT NOT NULL
)