IF DATABASE_PRINCIPAL_ID('MammothRole') IS NULL
	CREATE Role [MammothRole]

-- SELECT, UPDATE, INSERT, DELETE
GRANT SELECT, UPDATE, INSERT on SCHEMA::[app]			to [MammothRole];
GRANT SELECT, UPDATE, INSERT on SCHEMA::[esb]			to [MammothRole];
GRANT SELECT, UPDATE, INSERT, DELETE on SCHEMA::[dbo]	to [MammothRole];
GRANT SELECT, UPDATE, INSERT, DELETE on SCHEMA::[stage] to [MammothRole];

-- Stored Procedures
GRANT EXECUTE on [dbo].[AddOrUpdateHierarchyClass_FromStaging]			to [MammothRole];
GRANT EXECUTE on [dbo].[AddOrUpdateItemAttributesLocale_FromStaging]	to [MammothRole];
GRANT EXECUTE on [dbo].[AddOrUpdateItems_FromStaging]					to [MammothRole];
GRANT EXECUTE on [dbo].[AddOrUpdatePrices_FromStaging]					to [MammothRole];
GRANT EXECUTE on [dbo].[DeleteExpiredPrices]							to [MammothRole];
GRANT EXECUTE on [dbo].[DeleteExpiredPricesAndSales]					to [MammothRole];
GRANT EXECUTE on [dbo].[DeleteExpiredSales]								to [MammothRole];
GRANT EXECUTE on [dbo].[DeleteHierarchyClass_FromStaging]				to [MammothRole];
GRANT EXECUTE on [dbo].[DeleteMerchandiseHierarchy_FromStaging]			to [MammothRole];
GRANT EXECUTE on [esb].[LoadEsbItemLocaleMessagesByRegionOrStores]		to [MammothRole];
GRANT EXECUTE on [esb].[LoadEsbPriceMessagesByRegionOrStores]			to [MammothRole];
GRANT EXECUTE on [esb].[MarkMessageQueueEntriesAsInProcess]				to [MammothRole];

-- IRMA Developers
GRANT SELECT on SCHEMA::[app] to [WFM\IRMA.Developers]
GRANT SELECT on SCHEMA::[dbo] to [WFM\IRMA.Developers]
GRANT SELECT on SCHEMA::[esb] to [WFM\IRMA.Developers]
