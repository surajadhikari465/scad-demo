CREATE PROCEDURE infor.AddOrUpdateLocales 
	 @localeChains infor.LocaleAddOrUpdateType READONLY
	,@localeRegions infor.LocaleAddOrUpdateType READONLY
	,@localeMetros infor.LocaleAddOrUpdateType READONLY
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
			FROM @localeChains
			)
	BEGIN
		UPDATE l
		SET LocaleName = chains.LocaleName
			,localeTypeID = lt.localeTypeID
			,ParentLocaleID = chains.ParentLocaleID
		FROM dbo.Locale l
		JOIN @localeChains chains ON l.localeID = chains.LocaleID
		JOIN dbo.LocaleType lt ON chains.LocaleTypeCode = lt.localeTypeCode

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
		FROM @localeChains l
		JOIN dbo.LocaleType lt ON l.LocaleTypeCode = lt.localeTypeCode
		WHERE l.LocaleID NOT IN 
				(
				SELECT localeID
				FROM Locale
				)

		 SET IDENTITY_INSERT Locale OFF
	END

	/* Add or update Regions */
	IF EXISTS (
			   SELECT TOP 1 1
			   FROM @localeRegions	
			  )
	BEGIN
		UPDATE l
		SET LocaleName = regions.LocaleName
			,localeTypeID = lt.localeTypeID
			,ParentLocaleID = regions.ParentLocaleID
		FROM dbo.Locale l
		JOIN @localeRegions regions ON l.localeID = regions.LocaleID
		JOIN dbo.LocaleType lt ON regions.LocaleTypeCode = lt.localeTypeCode

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
		FROM @localeRegions l
		JOIN dbo.LocaleType lt ON l.LocaleTypeCode = lt.localeTypeCode
		WHERE l.LocaleID NOT IN 
				(
				SELECT localeID
				FROM Locale
				)

		SET IDENTITY_INSERT Locale OFF
	END

	/* Add or update Metros */
	IF EXISTS (
			   SELECT TOP 1 1
			   FROM @localeMetros
			   )
	BEGIN
		UPDATE l
		SET LocaleName = metros.LocaleName
			,localeTypeID = lt.localeTypeID
			,ParentLocaleID = metros.ParentLocaleID
		FROM dbo.Locale l
		JOIN @localeMetros metros ON l.localeID = metros.LocaleID
		JOIN dbo.LocaleType lt ON metros.LocaleTypeCode = lt.localeTypeCode

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
		FROM @localeMetros l
		JOIN dbo.LocaleType lt ON l.LocaleTypeCode = lt.localeTypeCode
		WHERE l.LocaleID NOT IN 
			    (
				 SELECT localeID
				 FROM Locale
				)

		SET IDENTITY_INSERT Locale OFF
	END
END
