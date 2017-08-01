CREATE PROCEDURE infor.AddOrUpdateLocales 
	 @locale infor.LocaleAddOrUpdateType READONLY
AS
BEGIN
			
DECLARE @OwnerOrgPartyID INT = (
		SELECT orgPartyID
		FROM Organization
		WHERE orgDesc = 'Whole Foods'
		)
	/* Add or update Chains */
	IF EXISTS (
			SELECT TOP 1 1
			FROM @locale
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
	   INTO #tmpLocale
	   FROM @locale

		UPDATE l
		SET LocaleName = tmpLocale.LocaleName
			,localeTypeID = lt.localeTypeID
			,ParentLocaleID = tmpLocale.ParentLocaleID
		FROM dbo.Locale l
		JOIN #tmpLocale tmpLocale ON l.localeID = tmpLocale.LocaleID  AND tmpLocale.LocaleTypeCode ='CH'
		JOIN dbo.LocaleType lt ON tmpLocale.LocaleTypeCode = lt.localeTypeCode

		SET IDENTITY_INSERT Locale ON

		INSERT INTO Locale
		   (
			LocaleID
			,LocaleName
			,localeTypeID
			,ParentLocaleID
			,ownerOrgPartyID
			)
		SELECT LocaleID
			,LocaleName
			,lt.localeTypeID
			,ParentLocaleID
			,@OwnerOrgPartyID
		FROM #tmpLocale l
		JOIN dbo.LocaleType lt ON l.LocaleTypeCode = lt.localeTypeCode
		WHERE l.LocaleTypeCode = 'CH'
		AND   l.LocaleID NOT IN 
				(
				SELECT localeID
				FROM Locale
				)

		 SET IDENTITY_INSERT Locale OFF

	    UPDATE l
		SET LocaleName = tmpLocale.LocaleName
			,localeTypeID = lt.localeTypeID
			,ParentLocaleID = tmpLocale.ParentLocaleID
		FROM dbo.Locale l
		JOIN #tmpLocale tmpLocale ON l.localeID = tmpLocale.LocaleID  AND tmpLocale.LocaleTypeCode ='RG'
		JOIN dbo.LocaleType lt ON tmpLocale.LocaleTypeCode = lt.localeTypeCode

		SET IDENTITY_INSERT Locale ON

		INSERT INTO Locale
		   (
			LocaleID
			,LocaleName
			,localeTypeID
			,ParentLocaleID
			,ownerOrgPartyID
			)
		SELECT LocaleID
			,LocaleName
			,lt.localeTypeID
			,ParentLocaleID
			,@OwnerOrgPartyID
		FROM #tmpLocale l
		JOIN dbo.LocaleType lt ON l.LocaleTypeCode = lt.localeTypeCode
		WHERE l.LocaleTypeCode = 'RG'
		AND   l.LocaleID NOT IN 
				(
				SELECT localeID
				FROM Locale
				)

	   SET IDENTITY_INSERT Locale OFF

		UPDATE l
		SET LocaleName = tmpLocale.LocaleName
			,localeTypeID = lt.localeTypeID
			,ParentLocaleID = tmpLocale.ParentLocaleID
		FROM dbo.Locale l
		JOIN #tmpLocale tmpLocale ON l.localeID = tmpLocale.LocaleID  AND tmpLocale.LocaleTypeCode ='MT'
		JOIN dbo.LocaleType lt ON tmpLocale.LocaleTypeCode = lt.localeTypeCode

		SET IDENTITY_INSERT Locale ON

		INSERT INTO Locale
		   (
			LocaleID
			,LocaleName
			,localeTypeID
			,ParentLocaleID
			,ownerOrgPartyID
			)
		SELECT LocaleID
			,LocaleName
			,lt.localeTypeID
			,ParentLocaleID
			,@OwnerOrgPartyID
		FROM #tmpLocale l
		JOIN dbo.LocaleType lt ON l.LocaleTypeCode = lt.localeTypeCode
		WHERE l.LocaleTypeCode = 'MT'
		AND   l.LocaleID NOT IN 
				(
				SELECT localeID
				FROM Locale
				)

    INSERT INTO infor.LocaleSequence(LocaleId, InforMessageId, SequenceID)
	SELECT 
		tmp.LocaleID,
		tmp.InforMessageId,
		tmp.SequenceId
	FROM #tmpLocale tmp
	WHERE tmp.SequenceId IS NOT NULL
		AND tmp.LocaleID NOT IN
		(
			SELECT LocaleID FROM infor.LocaleSequence
		)

	UPDATE ls
	SET SequenceID = tmp.SequenceId,
		ModifiedDateUtc = SYSUTCDATETIME(),
		InforMessageId = tmp.InforMessageId
	FROM #tmpLocale tmp
	JOIN infor.LocaleSequence ls ON tmp.LocaleID = ls.LocaleID
	WHERE tmp.SequenceId IS NOT NULL

	END
END
