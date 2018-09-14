DECLARE @scriptKey varchar(128);
SET @scriptKey = 'AddNumberOfDigitsForCFSItems'

IF(NOT EXISTS(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	DECLARE @numberOfDigitsSentToScale INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'NDS');
	DECLARE @cfsSendToScaleId INT = (SELECT AttributeID FROM dbo.Attributes WHERE AttributeCode = 'CFS');

	SELECT DISTINCT ItemLocaleAttributesExt.itemid,
					i.ScanCode, 
					len(i.ScanCode) as LengthOfScanCode, 
					LocaleID
	INTO #tmp
	FROM ItemLocaleAttributesExt
	INNER JOIN dbo.Items i on ItemLocaleAttributesExt.ItemID = i.ItemID
	WHERE region = 'TS' 
		 AND AttributeID = @cfsSendToScaleId 
		 AND  LEN(i.ScanCode) <=6  and AttributeValue = 'True'

	INSERT INTO [dbo].[ItemAttributes_Locale_TS_Ext]
						([Region]
						 ,[ItemID]
						 ,[LocaleID]
						 ,[AttributeID]
						 ,[AttributeValue]
						 ,[AddedDate]
						 ,[ModifiedDate])

	SELECT  'TS', 
			ItemID,
			LocaleID,
			@numberOfDigitsSentToScale,
			LengthOfScanCode, 
			getdate(),
			getdate()
	FROM #tmp
	WHERE NOT EXISTS( SELECT 1
					  FROM [ItemAttributes_Locale_TS_Ext] AS p
					   WHERE p.[AttributeID] = @numberOfDigitsSentToScale
							 AND p.[ItemID] = #tmp.[ItemID]
							 AND p.LocaleID = #tmp.LocaleId )

	DROP TABLE #tmp
	INSERT INTO app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO