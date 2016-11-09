-- Only make this change for the MA region.
IF EXISTS (SELECT RegionCode FROM Region WHERE RegionCode = 'MA')
BEGIN

	-- Drop any existing backup tables for the hierarchy data
	IF EXISTS (SELECT * FROM sys.objects WHERE name = 'ItemCategory_Backup' AND type = 'U')
	BEGIN
		DROP TABLE dbo.ItemCategory_Backup 
	END

	IF EXISTS (SELECT * FROM sys.objects WHERE name = 'ProdHierarchyLevel3_Backup' AND type = 'U')
	BEGIN
		DROP TABLE dbo.ProdHierarchyLevel3_Backup 
	END

	IF EXISTS (SELECT * FROM sys.objects WHERE name = 'ProdHierarchyLevel4_Backup' AND type = 'U')
	BEGIN
		DROP TABLE dbo.ProdHierarchyLevel4_Backup 
	END
	
	IF EXISTS (SELECT * FROM sys.objects WHERE name = 'NatHier_Class_Backup' AND type = 'U')
	BEGIN
		DROP TABLE dbo.NatHier_Class_Backup 
	END

	IF EXISTS (SELECT * FROM sys.objects WHERE name = 'JDA_HierarchyMapping_Backup' AND type = 'U')
	BEGIN
		DROP TABLE dbo.JDA_HierarchyMapping_Backup 
	END
END
ELSE
BEGIN
	PRINT 'Not in the MA Region! No changes will be applied.'
END
GO