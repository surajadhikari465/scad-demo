--==========================================================================
-- Date:	2014-10-09
-- Author:	Benjamin Sims
-- Purpose: TFS 4725
--			We need to remove any ItemTrait rows for Retail Size and UOM
--			where the localeName is not Whole Foods.  We need to populate
--			Retail Size and Retail UOM ItemTrait rows for where the locale
--			name is Whole Foods. This will populate the traitValue with NULL
--==========================================================================
DECLARE @wfmLocale INT = (
		SELECT localeID
		FROM Locale l
		WHERE l.localeName = 'Whole Foods'
		);
DECLARE @retailSizeTrait INT = (
		SELECT traitID
		FROM Trait t
		WHERE t.traitDesc = 'Retail Size'
		);
DECLARE @retailUomTrait INT = (
		SELECT traitID
		FROM Trait t
		WHERE t.traitDesc = 'Retail UOM'
		);

BEGIN TRY
	BEGIN TRAN

	-- Delete all existing Item Trait rows where the localeID is not the Whole Foods locale
	DELETE it
	FROM ItemTrait it
	WHERE it.localeID > @wfmLocale
		AND (
			it.traitID IN (
				@retailSizeTrait,
				@retailUomTrait
				)
			);

	-- Insert Retail Size Item Trait rows
	WITH norum (itemid)
	AS (
		SELECT i.itemid
		FROM item i
		LEFT JOIN itemtrait it ON i.itemID = it.itemID
			AND it.traitid = @retailUomTrait
		WHERE it.itemID IS NULL
		)
	INSERT INTO itemtrait (
		traitID,
		itemID,
		traitValue,
		localeID
		)
	SELECT @retailUomTrait,
		n.itemid,
		NULL,
		@wfmLocale
	FROM norum n;

	WITH norsz (itemid)
	AS (
		SELECT i.itemid
		FROM item i
		LEFT JOIN itemtrait it ON i.itemID = it.itemID
			AND it.traitid = @retailSizeTrait
		WHERE it.itemID IS NULL
		)
	INSERT INTO itemtrait (
		traitID,
		itemID,
		traitValue,
		localeID
		)
	SELECT @retailSizeTrait,
		n.itemid,
		NULL,
		@wfmLocale
	FROM norsz n;

	COMMIT TRAN
END TRY

BEGIN CATCH
	ROLLBACK TRAN THROW;
END CATCH
