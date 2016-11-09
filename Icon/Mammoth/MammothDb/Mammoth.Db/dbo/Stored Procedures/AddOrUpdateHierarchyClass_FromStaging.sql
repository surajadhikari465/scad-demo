
CREATE PROCEDURE [dbo].[AddOrUpdateHierarchyClass_FromStaging]
	@transactionId uniqueidentifier
AS
BEGIN
	-- =========================================
	-- Locale Variables
	-- =========================================
	DECLARE @date DATETIME = getdate();

	-- =========================================
	-- Insert / Update based on HierarchyClassID
	-- =========================================
	MERGE
		dbo.HierarchyClass with (updlock, rowlock) hc
	USING
		(
			SELECT 
				stg.HierarchyClassID,
				stg.HierarchyID,
				stg.HierarchyClassName
			FROM stage.HierarchyClass stg
			WHERE stg.TransactionId = @transactionId
		) s
	ON
		hc.HierarchyClassID = s.HierarchyClassID
	WHEN MATCHED THEN
		UPDATE
		SET
			hc.HierarchyClassName	= s.HierarchyClassName,
			hc.ModifiedDate			= @date
	WHEN NOT MATCHED THEN
		INSERT (HierarchyClassID, HierarchyID, HierarchyClassName, AddedDate)
		VALUES (s.HierarchyClassID, s.HierarchyID, s.HierarchyClassName, @date);
END

GO

