-- PBI 20832  As Infor Ordering I need StoreItemVendor data excluded from RM IRMA

-- Insert IRMA instance data flag for configuring use of whether include 365 stores (Mega_Store = 1) into 
-- the daily Infor ordering process
IF NOT EXISTS(
	SELECT 1 FROM dbo.InstanceDataFlags
	WHERE FlagKey = 'Include365StoresForInforOrdering')
BEGIN
	INSERT INTO dbo.InstanceDataFlags (FlagKey, FlagValue, CanStoreOverride)
    VALUES ('Include365StoresForInforOrdering', 0, 0)
END