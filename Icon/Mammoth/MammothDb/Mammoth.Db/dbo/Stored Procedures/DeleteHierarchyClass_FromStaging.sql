CREATE PROCEDURE [dbo].[DeleteHierarchyClass_FromStaging]
	@transactionId uniqueidentifier
AS
BEGIN
	DELETE hc
	FROM
		HierarchyClass hc
		JOIN stage.HierarchyClass s on hc.HierarchyClassID = s.HierarchyClassID
	WHERE
		s.TransactionId = @transactionId;
END

GO


