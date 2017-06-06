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
		INTO #tmpTrait
		FROM @traits tr
		INNER JOIN LocaleTrait lt
		ON lt.traitValue = businessunitid AND lt.traitID = @businessUnitIdTraitId 

		UPDATE lt
		SET traitValue = tmpTrait.TraitValue
		FROM #tmpTrait tmpTrait
		JOIN LocaleTrait lt ON lt.localeID = tmpTrait.localeID
		AND lt.traitID = tmpTrait.TraitId
				
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
		FROM  #tmpTrait tmpTrait
		WHERE TraitId != @businessUnitIdTraitId
		AND NOT EXISTS 
		   ( SELECT la.localeID, la.traitID, la.traitValue, NULL
		     FROM LocaleTrait la
		     WHERE la.LocaleId = tmpTrait.LocaleId
		     AND la.TraitId = tmpTrait.TraitId
		   )
	END
END
