CREATE TYPE infor.LocaleTraitAddOrUpdateType AS TABLE
(	TraitId INT NOT NULL,
	TraitValue NVARCHAR(255) NULL,
	UomId INT  NULL,
    BusinessUnitId INT NULL
)