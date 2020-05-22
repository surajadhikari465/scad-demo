USE [icon]
--scripts for moving stores from (FL MET_FL->SO MET_SFL and  MA MET_OH -> MW MET_SOH)

DECLARE @localeId INT
DECLARE @parentLocaleId INT
DECLARE @localeTypeId INT = (
		SELECT localeTypeID
		FROM localeType
		WHERE localeTypeDesc = 'Metro'
		)

--set localeid for SO
SET @localeId = (
		SELECT localeid
		FROM locale
		WHERE localeName = 'South'
		)

IF NOT EXISTS (
		SELECT *
		FROM locale
		WHERE localename = 'MET_SFL'
		)
BEGIN
	SET IDENTITY_INSERT [dbo].[Locale] ON

	INSERT INTO [dbo].[Locale] (
		[localeID]
		,[ownerOrgPartyID]
		,[localeName]
		,[localeOpenDate]
		,[localeCloseDate]
		,[localeTypeID]
		,[parentLocaleID]
		)
	VALUES (
		3000
		,1
		,'MET_SFL'
		,getdate()
		,NULL
		,@localeTypeId
		,@localeId
		)

	SET IDENTITY_INSERT [dbo].[Locale] OFF
END

--Move Stores from FL to SO
DECLARE @localeIdTLH INT = (
		SELECT localeId
		FROM locale
		WHERE localename = 'Tallahassee'
		)
DECLARE @localeIdDES INT = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Destin'
		)

SET @parentLocaleId = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_SFL'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid IN (
		@localeIdTLH
		,@localeIdDES
		)

--set locale id for MW
SET @localeId = (
		SELECT localeid
		FROM locale
		WHERE localeName = 'Mid West'
		)

IF NOT EXISTS (
		SELECT *
		FROM locale
		WHERE localename = 'MET_SOH'
		)
BEGIN
	SET IDENTITY_INSERT [dbo].[Locale] ON

	INSERT INTO [dbo].[Locale] (
		[localeID]
		,[ownerOrgPartyID]
		,[localeName]
		,[localeOpenDate]
		,[localeCloseDate]
		,[localeTypeID]
		,[parentLocaleID]
		)
	VALUES (
		3002
		,1
		,'MET_SOH'
		,getdate()
		,NULL
		,@localeTypeId
		,@localeId
		)

	SET IDENTITY_INSERT [dbo].[Locale] OFF
END

--Move stores from MA to MW
DECLARE @localeIdDUB INT = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Columbus'
		)
DECLARE @localeIdEYE INT = (
		SELECT localeId
		FROM locale
		WHERE localename = 'West Lane'
		)
DECLARE @localeIdEST INT = (
		SELECT localeid
		FROM locale
		WHERE localename = 'Easton'
		)

SET @parentLocaleId = (
		SELECT TOP 1 localeId
		FROM locale
		WHERE LocaleName = 'MET_SOH'
		)

UPDATE locale
SET parentLocaleID = @parentLocaleId
WHERE localeid IN (
		@localeIdDUB
		,@localeIdEYE
		,@localeIdEST
		)