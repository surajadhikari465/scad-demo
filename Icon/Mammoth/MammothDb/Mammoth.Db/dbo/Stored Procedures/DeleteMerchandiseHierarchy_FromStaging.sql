CREATE PROCEDURE [dbo].[DeleteMerchandiseHierarchy_FromStaging]
	@transactionId uniqueidentifier
AS
BEGIN
	DELETE m
	FROM 
		Hierarchy_Merchandise m
		JOIN stage.HierarchyClass s on m.SubBrickHCID = s.HierarchyClassID
	WHERE
		s.TransactionId = @transactionId;
END

GO

