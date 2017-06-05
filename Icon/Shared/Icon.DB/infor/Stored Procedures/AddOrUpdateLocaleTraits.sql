CREATE PROCEDURE infor.AddOrUpdateLocaleTraits
	@traits infor.LocaleTraitAddOrUpdateType READONLY
	
AS
BEGIN
	DECLARE @businessUnitIdTraitId INT = (
			SELECT traitID
			FROM Trait
			WHERE traitCode = 'BU'
			)
	IF EXISTS (SELECT TOP 1 1
			   FROM @traits
			  )
	BEGIN

		SELECT tr.TraitId,
			   tr.TraitValue,
			   tr.UomId,
			   BusinessUnitId,
			   lt.localeID
		INTO #tmp
		FROM @traits tr
		INNER JOIN LocaleTrait lt
		ON lt.traitValue = businessunitid AND lt.traitID = @businessUnitIdTraitId 

		UPDATE lt
		SET traitValue = tmp.TraitValue
		FROM #tmp tmp
		JOIN LocaleTrait lt ON lt.localeID = tmp.localeID
		AND lt.traitID = tmp.TraitId
				
		INSERT INTO LocaleTrait 
				(
				localeID
				,traitID
				,traitValue
				,uomID
				)
		SELECT LocaleId
			   ,TraitId
			   ,TraitValue
			    ,NULL
		FROM  #tmp
		WHERE TraitId != @businessUnitIdTraitId
		AND NOT EXISTS 
		   ( SELECT la.localeID, la.traitID, la.traitValue, NULL
		     FROM LocaleTrait la
		     WHERE la.LocaleId = #tmp.LocaleId
		     AND la.TraitId = #tmp.TraitId
		   )
	END
END
