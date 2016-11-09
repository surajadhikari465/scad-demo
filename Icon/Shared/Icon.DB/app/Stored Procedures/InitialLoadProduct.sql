
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-07-07
-- Description:	Used for initial load of the global 
--				product data.  Parameter @Path takes 
--				the UNC location which represents 
--				the data source for	the scan codes
--				that will be sent to ESB.
-- =============================================

CREATE PROCEDURE [app].[InitialLoadProduct]
	@Path nvarchar(256) = NULL,
	@NumberOfItems int = NULL,
	@Region nvarchar(2) = NULL
AS
BEGIN
	DECLARE @items app.UpdatedItemIDsType;

	IF (@Path IS NULL)
	BEGIN
		IF (@Region IS NOT NULL AND @NumberOfItems IS NULL)
			BEGIN
				PRINT '@Region IS NOT NULL.  Getting all itemIDs that ' + @Region + ' is subscribed to.';
				INSERT INTO @items 
				SELECT itemID 
				FROM ScanCode sc JOIN app.IRMAItemSubscription s on sc.ScanCode = s.identifier 
				WHERE s.regioncode = @Region;
			END
		ELSE IF (@Region IS NOT NULL AND @NumberOfItems IS NOT NULL)
			BEGIN
				PRINT '@Region IS NOT NULL along with @NumberOfItems.  Getting top ' + CAST(@NumberOfItems as nvarchar) + ' itemIDs that ' + @Region + ' is subscribed to.';
				INSERT INTO @items 
				SELECT TOP (@NumberOfItems) itemID 
				FROM ScanCode sc JOIN app.IRMAItemSubscription s on sc.ScanCode = s.identifier 
				WHERE s.regioncode = @Region;
			END
		ELSE IF (@NumberOfItems IS NOT NULL)
			BEGIN
				PRINT 'Only the @NumberOfItems variable is supplied.  Getting top ' + CAST(@NumberOfItems as nvarchar) + ' itemIDs.';
				INSERT INTO @items SELECT DISTINCT TOP (@NumberOfItems) itemID FROM ScanCode;
			END
		ELSE
			BEGIN
				PRINT 'No parameters were supplied.  Getting a full list of itemIDs in the database.';
				INSERT INTO @items SELECT DISTINCT itemID FROM ScanCode;
			END
	END
	ELSE
	BEGIN
		DECLARE @sql nvarchar(256) = 'bulk insert @items FROM ''' + @Path + ''' with(firstrow = 1, rowterminator = ''\n'')';
		EXEC (@sql);
	END

	PRINT '----------------------------------------';
	PRINT 'INSERTING into the app.MessageQueueProduct and app.MessageQueueNutrition tables...';
	EXEC [app].[GenerateItemUpdateMessages] @items;
	PRINT 'Successfully completed INSERT into the app.MessageQueueProduct and app.MessageQueueNutrition tables.';
END
GO
