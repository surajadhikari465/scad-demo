IF DATABASE_PRINCIPAL_ID('MammothRole') IS NULL
	CREATE Role [MammothRole]

/*
-- General Security Guidelines --
1) Grant access to DB roles, not specific accounts/users/AD groups.
2) Blanket grants, such as exec at schema level, are okay to service accounts (not real people/users).
3) Grants to TMs and/or User Teams should be focused, meaning to specific objects (stored procedures), 
so access is controlled and more difficult to "abuse" (start reading/using other data that wasn't originally in scope).
4) Explicit grants are needed for user-defined types.
5) DB_Owner or DDL_Admin only under special exception (approved by DBA Team).
*/

-- SELECT, UPDATE, INSERT, DELETE
GRANT EXECUTE, SELECT, UPDATE, INSERT on SCHEMA::[app]			to [MammothRole];
GRANT EXECUTE, SELECT, UPDATE, INSERT on SCHEMA::[esb]			to [MammothRole];
GRANT EXECUTE, SELECT, UPDATE, INSERT, DELETE on SCHEMA::[dbo]	to [MammothRole];
GRANT EXECUTE, SELECT, UPDATE, INSERT, DELETE, ALTER on SCHEMA::[stage] to [MammothRole];
GRANT CREATE TABLE TO [MammothRole]

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
GRANT EXECUTE on [esb].[MarkMessageQueuePriceEntriesAsInProcess]		to [MammothRole];
GRANT EXECUTE on [esb].[MarkMessageQueueItemLocaleEntriesAsInProcess]	to [MammothRole];

-- IRMA Developers
GRANT SELECT on SCHEMA::[app] to [WFM\IRMA.Developers]
GRANT SELECT on SCHEMA::[dbo] to [WFM\IRMA.Developers]
GRANT SELECT on SCHEMA::[esb] to [WFM\IRMA.Developers]

-- TIBCO Role
GRANT SELECT, UPDATE, INSERT, DELETE on SCHEMA::[esb]	to [TibcoRole];
