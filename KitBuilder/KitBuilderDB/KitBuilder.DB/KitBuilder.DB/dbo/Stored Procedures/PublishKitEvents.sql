﻿CREATE PROCEDURE [dbo].[PublishKitEvents] @kitId INT
	,@Action NVARCHAR(20)
	,@localeListWithNoVenues NVARCHAR(max) OUTPUT
AS
BEGIN
	CREATE TABLE #IncludedVenues (
		VenueID INT
		,KitLocaleId INT
		,StoreId INT
		)

	DECLARE @RegionLocaleTypeId INT = (
			SELECT LocaleTypeId
			FROM LocaleType
			WHERE localeTypeCode = 'RG'
			)
	DECLARE @ChainLocaleTypeId INT = (
			SELECT LocaleTypeId
			FROM LocaleType
			WHERE localeTypeCode = 'CH'
			)
	DECLARE @MetroLocaleTypeId INT = (
			SELECT LocaleTypeId
			FROM LocaleType
			WHERE localeTypeCode = 'MT'
			)
	DECLARE @StoreLocaleTypeId INT = (
			SELECT LocaleTypeId
			FROM LocaleType
			WHERE localeTypeCode = 'ST'
			)
	DECLARE @VenueLocaleTypeId INT = (
			SELECT LocaleTypeId
			FROM LocaleType
			WHERE localeTypeCode = 'VE'
			)

	SELECT KitLocale.LocaleId
		,LocaleTypeId
		,KitLocaleID
		,Exclude
	INTO #tmp
	FROM KitLocale
	INNER JOIN Locale ON KitLocale.localeId = Locale.localeId
	WHERE kitid = @kitId

	INSERT INTO #IncludedVenues (
		VenueID
		,KitLocaleId
		,StoreId
		)
	SELECT Locale.localeID
		,KitLocaleID
		,StoreId
	FROM Locale
	INNER JOIN #tmp ON Locale.LocaleId = #tmp.LocaleId
	WHERE #tmp.LocaleTypeId = @VenueLocaleTypeId
		AND Hospitality = 1
		AND Exclude = 0

	INSERT INTO #IncludedVenues (
		VenueID
		,KitLocaleId
		,StoreId
		)
	SELECT Locale.localeID
		,KitLocaleID
		,StoreId
	FROM Locale
	INNER JOIN #tmp ON Locale.storeId = #tmp.LocaleId
	WHERE #tmp.LocaleTypeId = @StoreLocaleTypeId
		AND Hospitality = 1
		AND Exclude = 0
		AND Locale.localeID NOT IN (
			SELECT VenueID
			FROM #IncludedVenues
			)

	INSERT INTO #IncludedVenues (
		VenueID
		,KitLocaleId
		,StoreId
		)
	SELECT Locale.localeID
		,KitLocaleID
		,StoreId
	FROM Locale
	INNER JOIN #tmp ON Locale.MetroId = #tmp.LocaleId
	WHERE #tmp.LocaleTypeId = @MetroLocaleTypeId
		AND Hospitality = 1
		AND Exclude = 0
		AND Locale.localeID NOT IN (
			SELECT VenueID
			FROM #IncludedVenues
			)

	INSERT INTO #IncludedVenues (
		VenueID
		,KitLocaleId
		,StoreId
		)
	SELECT Locale.localeID
		,KitLocaleID
		,StoreId
	FROM Locale
	INNER JOIN #tmp ON Locale.RegionId = #tmp.LocaleId
	WHERE #tmp.LocaleTypeId = @RegionLocaleTypeId
		AND Hospitality = 1
		AND Exclude = 0
		AND Locale.localeID NOT IN (
			SELECT VenueID
			FROM #IncludedVenues
			)

	INSERT INTO #IncludedVenues (
		VenueID
		,KitLocaleId
		,StoreId
		)
	SELECT Locale.localeID
		,KitLocaleID
		,StoreId
	FROM Locale
	INNER JOIN #tmp ON Locale.ChainId = #tmp.LocaleId
	WHERE #tmp.LocaleTypeId = @ChainLocaleTypeId
		AND Hospitality = 1
		AND Exclude = 0
		AND Locale.localeID NOT IN (
			SELECT VenueID
			FROM #IncludedVenues
			)

	----Delete excluded venues
	DELETE #IncludedVenues
	WHERE VenueID IN (
			SELECT Locale.localeId
			FROM Locale
			INNER JOIN #tmp ON CASE 
					WHEN #tmp.LocaleTypeId = @ChainLocaleTypeId
						AND #tmp.LocaleId = Locale.ChainId
						THEN 1
					WHEN #tmp.LocaleTypeId = @RegionLocaleTypeId
						AND #tmp.LocaleId = Locale.RegionId
						THEN 1
					WHEN #tmp.LocaleTypeId = @MetroLocaleTypeId
						AND #tmp.LocaleId = Locale.MetroId
						THEN 1
					WHEN #tmp.LocaleTypeId = @StoreLocaleTypeId
						AND #tmp.LocaleId = Locale.StoreId
						THEN 1
					WHEN #tmp.LocaleTypeId = @VenueLocaleTypeId
						AND #tmp.LocaleId = Locale.LocaleId
						THEN 1
					END = 1
			WHERE Locale.LocaleTypeID = @VenueLocaleTypeId
				AND Hospitality = 1
				AND Exclude = 1
			)

	SET @localeListWithNoVenues = '';

	SELECT @localeListWithNoVenues = 
	IsNull((SELECT STUFF((Select ',' + locale.LocaleName 
	FROM KitLocale
	INNER JOIN locale ON KitLocale.LocaleId = locale.LocaleId
	WHERE KitId = @kitId
		AND KitLocaleId NOT IN (
			SELECT KitLocaleId
			FROM #IncludedVenues
			)
    FOR XML PATH ('')),1,1,'')as Locales),'')

 IF(@localeListWithNoVenues ='')
 BEGIN
	INSERT INTO KitQueue (
		KitId
		,StoreId
		,StoreName
		,VenueId
		,VenueName
		,kitLocaleId
		,Action
		,STATUS
		)
	SELECT @kitId
		,iv.StoreId
		,s.LocaleName
		,VenueId
		,v.LocaleName
		,KitlocaleId
		,@Action
		,'U'
	FROM #IncludedVenues iv
	INNER JOIN dbo.Locale s ON s.LocaleId = iv.StoreId
	INNER JOIN dbo.Locale v ON v.LocaleId = iv.VenueID

	UPDATE KitLocale
	SET StatusId = (
			SELECT StatusID
			FROM STATUS
			WHERE StatusCode = 'PQ'
			)
	WHERE KITid = @kitId
		AND Exclude = 0
 END
 
END