CREATE PROCEDURE [app].[AddOrUpdateCertificationAgencies]
	@agencies app.CertificationAgencyImportType READONLY,
	@updateAgencyNames bit = 1
AS
	DECLARE @taskName nvarchar(32) = 'app.AddOrUpdateCertificationAgencies',
			@glutenFreeTraitId int = (select traitID from Trait where traitDesc = 'Gluten Free'),
			@kosherTraitId int = (select traitID from Trait where traitDesc = 'Kosher'),
			@nonGMOTraitId int = (select traitID from Trait where traitDesc = 'Non GMO'),
			@organicTraitId int = (select traitID from Trait where traitDesc = 'Organic'),
			@veganTraitId int = (select traitID from Trait where traitDesc = 'Vegan'),
			@agencyHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Certification Agency Management')
	DECLARE @newAgencies table (
				HierarchyClassId int,
				HierarchyClassName nvarchar(255)
			);
	DECLARE @agencyTraits table (
				AgencyId int,
				AgencyName nvarchar(255),
				TraitId int,
				TraitValue nvarchar(1)
			);
	
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Unpivoting agency traits...';
	
	INSERT INTO @agencyTraits
    --Unpivot the table.
	SELECT AgencyId, 
		   AgencyName,
		CASE TraitId
		  when 'GlutenFree'
			Then @glutenFreeTraitId
		  when 'Kosher'
			Then @kosherTraitId
		  when 'NonGMO'
			Then @nonGMOTraitId
		  when 'Organic'
			Then @organicTraitId
		  when 'Vegan'
			Then @veganTraitId
		END as TraitId, TraitValue
	FROM 
	 (SELECT AgencyId, AgencyName, GlutenFree, Kosher, NonGMO, Organic, Vegan
		FROM @agencies) p
	UNPIVOT
	 (TraitValue FOR TraitId IN 
      (GlutenFree, Kosher, NonGMO, Organic, Vegan)
	)AS unpvt;

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Inserting new certification agencies...';
			
	--Insert new agencies
	INSERT INTO HierarchyClass(hierarchyLevel, hierarchyID, hierarchyParentClassID, hierarchyClassName)
		OUTPUT	inserted.hierarchyClassID,
				inserted.hierarchyClassName
			INTO @newAgencies
	SELECT	1,
			@agencyHierarchyId,
			NULL,
			a.AgencyName
	  FROM @agencies a
	 WHERE a.AgencyId = '0'
	   AND NOT EXISTS (SELECT 1 FROM HierarchyClass hc WHERE hc.hierarchyID = @agencyHierarchyId AND hc.hierarchyClassName = a.AgencyName)
	
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Inserting traits for new agencies...';
	
	--Insert traits for new agencies
	INSERT HierarchyClassTrait(hierarchyClassID, traitID, traitValue, uomID)
	SELECT	na.HierarchyClassId,
			at.TraitId,
			at.TraitValue,
			null
	FROM @agencies a
	JOIN @newAgencies na on na.HierarchyClassName = a.AgencyName
	JOIN @agencyTraits at on at.AgencyName = a.AgencyName
	WHERE a.AgencyId = 0
	  AND at.TraitValue  = '1'

	--Update agency name for existing agencies
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Updating agency names...';

	IF @updateAgencyNames = 1
	BEGIN
		UPDATE HierarchyClass
		SET hierarchyClassName = a.AgencyName
		FROM HierarchyClass hc
		JOIN @agencies a on hc.hierarchyClassID = a.AgencyId
	END

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Inserting new traits for existing agencies...';
	
	--Insert new traits for existing agencies
	INSERT HierarchyClassTrait(hierarchyClassID, traitID, traitValue, uomID)
	SELECT	a.AgencyId,
			at.TraitId,
			at.TraitValue,
			null
	FROM @agencies a
	JOIN @agencyTraits at on at.AgencyId = a.AgencyId
	LEFT JOIN HierarchyClassTrait hct on hct.hierarchyClassId = a.AgencyId
		AND hct.traitID = at.TraitId
	WHERE at.TraitValue = '1'	
		AND a.AgencyId <> 0
		AND hct.traitID IS NULL
	
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Removing Existing Traits...';

	--Remove existing Agency Traits that are not applicable anymore
	DELETE HierarchyClassTrait 
	FROM HierarchyClassTrait hct
	JOIN @agencies a on hct.hierarchyClassID = a.AgencyId
		AND a.AgencyId <> 0
	JOIN @agencyTraits at on at.AgencyId = a.AgencyId
		AND at.TraitId = hct.traitID
		AND at.TraitValue = '0'