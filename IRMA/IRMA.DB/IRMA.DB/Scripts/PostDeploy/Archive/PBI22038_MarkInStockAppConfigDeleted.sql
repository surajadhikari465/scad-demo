declare @Name varchar(150) = 'AmazonInStockEnabledStoreVendorId',  
		@KeyID int,
		@Count As int

SELECT @Count = Count(*) FROM AppConfigKey WHERE [Name] = @Name

BEGIN
	IF @Count > 0
		BEGIN
		
			SELECT @KeyID = KeyID FROM AppConfigKey WHERE [Name] = @Name -- get the current key id
			
			UPDATE AppConfigKey SET Deleted = 1 WHERE KeyID = @KeyID 
			
			UPDATE AppConfigValue SET Deleted = 1 WHERE KeyID = @KeyID
			
		END
END
