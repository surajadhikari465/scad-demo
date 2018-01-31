DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddDeliverySystemValues'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	DECLARE @deliverySystemValues AS TABLE
	(
		DeliverySystemCode NVARCHAR(4),
		DeliverySystemName NVARCHAR(100)
	)

	INSERT INTO @deliverySystemValues
	VALUES ('STCK', 'Stick'),
			('SPRY', 'Spray'),
			('PST', 'Paste'),
			('CRYS', 'Crystal'),
			('WPS', 'Wipes'),
			('PCKT', 'Packet'),
			('SHOT', 'Shot')

	INSERT INTO dbo.DeliverySystem(DeliverySystemCode, DeliverySystemName)
	SELECT 
		DeliverySystemCode, 
		DeliverySystemName
	FROM @deliverySystemValues
	WHERE DeliverySystemName NOT IN (SELECT DeliverySystemName
									FROM dbo.DeliverySystem)

	UPDATE dbo.DeliverySystem
	SET DeliverySystemCode = 'VSG'
	WHERE DeliverySystemName = 'Vegetarian Soft Gel'
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO