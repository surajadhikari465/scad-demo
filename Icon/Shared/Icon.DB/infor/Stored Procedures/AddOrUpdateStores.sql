CREATE PROCEDURE infor.AddOrUpdateStores 
	@localeStores infor.LocaleAddOrUpdateType READONLY
AS
BEGIN
	DECLARE @OwnerOrgPartyID INT = (
			SELECT orgPartyID
			FROM Organization
			WHERE orgDesc = 'Whole Foods'
			)
		  ,@businessUnitIdTraitId INT = (
			SELECT traitID
			FROM Trait
			WHERE traitCode = 'BU'
			)
		,@LocaleId INT
		,@BusinessUnitId INT
		,@LocaleName VARCHAR(25)
		,@LocaleOpenDate DATETIME
		,@LocaleCloseDate DATETIME
		,@LocaleTypeCode NVARCHAR(3)
		,@ParentLocaleID INT
		,@EwicAgency NVARCHAR(255)

	/* Add or update Stores */
	IF EXISTS (
			SELECT TOP 1 1
			FROM @localeStores
			)
	BEGIN
		SELECT LocaleID,
			   LocaleName,
			   LocaleOpenDate,
			   LocaleCloseDate,
			   LocaleTypeCode,
			   ParentLocaleID,
			   BusinessUnitId,
			   EwicAgency,
			   SequenceId,
		       InforMessageId
	   INTO #tmpStores
	   FROM @localeStores

		UPDATE l
		SET LocaleName = tmpStores.LocaleName
			,LocaleOpenDate = tmpStores.LocaleOpenDate
			,LocaleCloseDate = tmpStores.LocaleCloseDate
			,localeTypeID = ltp.localeTypeID
			,ParentLocaleID = tmpStores.ParentLocaleID
		FROM dbo.Locale l
		JOIN dbo.LocaleTrait lt ON l.localeID = lt.localeID
			AND lt.traitID = @businessUnitIdTraitId
		JOIN #tmpStores tmpStores ON lt.traitValue = tmpStores.BusinessUnitId
		JOIN dbo.LocaleType ltp ON tmpStores.LocaleTypeCode = ltp.localeTypeCode
		
		DELETE ewic.AgencyLocale
		WHERE LocaleID IN (
				SELECT bu.LocaleID
				FROM #tmpStores tempStores
				JOIN LocaleTrait bu ON tempStores.BusinessUnitId = bu.traitValue
					AND bu.traitID = @businessUnitIdTraitId
				WHERE LTRIM(RTRIM(ISNULL(tempStores.EwicAgency, ''))) = ''
				)

		INSERT INTO ewic.AgencyLocale (
			AgencyId
			,LocaleId
			)
		SELECT a.AgencyId
			,bu.localeID
		FROM #tmpStores tempStores
		JOIN LocaleTrait bu ON tempStores.BusinessUnitId = bu.traitValue
			AND bu.traitID = @businessUnitIdTraitId
		JOIN ewic.Agency a ON tempStores.EwicAgency = a.AgencyId

		-- Add Store using cursor because Locale ID is not provided
		IF EXISTS (
				SELECT TOP 1 1
				FROM #tmpStores
				WHERE BusinessUnitId NOT IN 
						(
						SELECT traitValue
						FROM LocaleTrait lt
						WHERE lt.traitID = @businessUnitIdTraitId
						)
				  )
		BEGIN
			DECLARE locale_cursor CURSOR
			FOR
			SELECT 
				LocaleId,
				BusinessUnitId,
				LocaleName,
				LocaleOpenDate,
				LocaleCloseDate,
				LocaleTypeCode,
				ParentLocaleID,
	     		EwicAgency
			
			FROM #tmpStores
			WHERE BusinessUnitId NOT IN (
					SELECT traitValue
					FROM LocaleTrait lt
					WHERE lt.traitID = @businessUnitIdTraitId
					)
			OPEN locale_cursor

			FETCH NEXT
			FROM locale_cursor
			INTO @LocaleId
				,@BusinessUnitId
				,@LocaleName
				,@LocaleOpenDate
				,@LocaleCloseDate
				,@LocaleTypeCode
				,@ParentLocaleID
				,@EwicAgency	

			WHILE @@FETCH_STATUS = 0
			BEGIN
	
				INSERT INTO Locale (
					LocaleName
					,LocaleOpenDate
					,LocaleCloseDate
					,localeTypeID
					,ParentLocaleID
					,ownerOrgPartyID
					)
				SELECT LocaleName
					,LocaleOpenDate
					,LocaleCloseDate
					,lt.localeTypeID
					,ParentLocaleID
					,@OwnerOrgPartyID
				FROM #tmpStores l
				JOIN dbo.LocaleType lt ON l.LocaleTypeCode = lt.localeTypeCode

				SET @LocaleId = SCOPE_IDENTITY()
				
				INSERT INTO LocaleTrait
				       (
						LocaleId
						,TraitId
						,TraitValue
						,uomID
					    )
				VALUES 
						(
						 @LocaleId
						,@businessUnitIdTraitId
						,CONVERT(NVARCHAR(255), @BusinessUnitId)
						,NULL
					    )

				IF LTRIM(RTRIM(ISNULL(@EwicAgency, ''))) <> ''
				BEGIN
					INSERT INTO ewic.AgencyLocale
					   (
						AgencyId
						,LocaleId
						)
					VALUES 
					   (
						@EwicAgency
						,@LocaleId
						)
				END

				FETCH NEXT
				FROM locale_cursor
			END

			CLOSE locale_cursor

			DEALLOCATE locale_cursor
		END
	END

	
    INSERT INTO infor.LocaleSequence(LocaleId, InforMessageId, SequenceID)
	SELECT 
		bu.LocaleID,
		tmp.InforMessageId,
		tmp.SequenceId
	FROM #tmpStores tmp
	JOIN LocaleTrait bu ON tmp.BusinessUnitId = bu.traitValue
			AND bu.traitID = @businessUnitIdTraitId
	WHERE tmp.SequenceId IS NOT NULL
		AND bu.LocaleID NOT IN
		(
			SELECT LocaleID FROM infor.LocaleSequence
		)

	UPDATE ls
	SET SequenceID = tmp.SequenceId,
		ModifiedDateUtc = SYSUTCDATETIME(),
		InforMessageId = tmp.InforMessageId
	FROM #tmpStores tmp
	JOIN LocaleTrait bu ON tmp.BusinessUnitId = bu.traitValue
			AND bu.traitID = @businessUnitIdTraitId
	JOIN infor.LocaleSequence ls ON tmp.LocaleID = bu.LocaleID
	WHERE tmp.SequenceId IS NOT NULL
END
