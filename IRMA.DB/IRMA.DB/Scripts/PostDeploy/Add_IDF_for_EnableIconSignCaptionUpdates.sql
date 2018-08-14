-- PBI 28448 (formerly 25133) Pass the Customer Friendly Description (CFD) Value to the Existing Sign Caption Field in IRMA

-- Part 1: insert IRMA instance data flag for configuring use of CFD vs old Sign_Description/Caption
IF NOT EXISTS(
	SELECT 1 FROM dbo.InstanceDataFlags
	WHERE FlagKey = 'EnableIconSignCaptionUpdates')
BEGIN
	INSERT INTO dbo.InstanceDataFlags (FlagKey, FlagValue, CanStoreOverride)
    VALUES ('EnableIconSignCaptionUpdates', 0, 0)
END

-- Part 2: update EIM attribute to make sign caption readonly
UPDATE UTA
SET IsReadOnlyForNewItems = 1 , IsReadOnlyForExistingItems = 1
FROM dbo.UploadTypeAttribute UTA
	INNER JOIN dbo.UploadAttribute UA ON UA.UploadAttribute_ID = UTA.UploadAttribute_ID
WHERE UA.[Name] = 'Sign Caption'
	AND (UTA.IsReadOnlyForExistingItems=0 OR UTA.IsReadOnlyForExistingItems=0)