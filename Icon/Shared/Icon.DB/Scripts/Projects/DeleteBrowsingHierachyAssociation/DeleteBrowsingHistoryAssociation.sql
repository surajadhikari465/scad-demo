BEGIN TRAN

BEGIN TRY
	DECLARE @itemIds esb.MessageQueueItemIdsType
	DECLARE @browsingHierarchyId INT

	SET @browsingHierarchyId = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE HierarchyName = 'Browsing'
			)

	INSERT INTO @itemIds
	SELECT itemId
		   ,SYSUTCDATETIME()
	       ,SYSUTCDATETIME()
	FROM ItemHierarchyClass
	WHERE hierarchyClassID IN (
			SELECT hierarchyClassID
			FROM HierarchyClass
			WHERE HIERARCHYID = @browsingHierarchyId
			)

	DELETE ItemHierarchyClass
	WHERE hierarchyClassID IN (
			SELECT hierarchyClassID
			FROM HierarchyClass
			WHERE HIERARCHYID = 4
			)

	EXEC esb.AddMessageQueueItem @itemIds
	COMMIT
END TRY

BEGIN CATCH
	PRINT 'Error occurred during script'

	SELECT ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage;

	ROLLBACK
END CATCH
GO