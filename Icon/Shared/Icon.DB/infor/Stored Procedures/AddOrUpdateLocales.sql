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
			   EwicAgency
	   INTO #tmp
	   FROM @locale

		UPDATE l
		SET LocaleName = locale.LocaleName
			,localeTypeID = lt.localeTypeID
			,ParentLocaleID = locale.ParentLocaleID
		FROM dbo.Locale l
		JOIN #tmp locale ON l.localeID = locale.LocaleID 
		JOIN dbo.LocaleType lt ON locale.LocaleTypeCode = lt.localeTypeCode

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
		FROM #tmp l
		JOIN dbo.LocaleType lt ON l.LocaleTypeCode = lt.localeTypeCode
		WHERE l.LocaleID NOT IN 
				(
				SELECT localeID
				FROM Locale
				)

		 SET IDENTITY_INSERT Locale OFF
	END
END
