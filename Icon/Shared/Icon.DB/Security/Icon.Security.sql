/*
Icon Security Script
*/

-- Types
GRANT EXEC ON type::app.EventQueueType to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON type::app.IRMAItemSubscriptionType to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON type::app.IRMAItemType to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON type::app.EventNameType to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON type::app.EventQueueIdType to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON type::app.ItemMovementIdType to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON type::app.ItemMovementType to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON type::app.ESBMessageIdType to [IconUser],[WFM\IConInterfaceUser]
GO

-- Tables
GRANT update, insert, select, delete on app.BusinessUnitRegionMapping to [IconUser],[WFM\IConInterfaceUser] 
GO
GRANT insert, select, delete, update on app.ItemMovement to [IconUser],[WFM\IConInterfaceUser] 
GO
GRANT insert, select, delete, update on app.ItemMovementErrorQueue to [IconUser],[WFM\IConInterfaceUser] 
GO
GRANT insert, select, delete on app.ItemMovementTransactionHistory to [IconUser],[WFM\IConInterfaceUser] 
GO
GRANT SELECT, INSERT, DELETE, UPDATE ON app.MessageResponseR10 TO [IconUser],[WFM\IConInterfaceUser] 
GO

-- Stored Procedures
GRANT EXEC ON [app].[InsertIRMAItemSubscriptions] to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON [app].[InsertIRMAItems] to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON [app].[InsertEventQueueEntries] to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON [app].[UpdateEventQueueInProcess] to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON [app].[InsertItemMovement] to [IconUser],[WFM\IConInterfaceUser]
GO
GRANT EXEC ON [app].[MassUpdateItemMovement] to [IconUser],[WFM\IConInterfaceUser]
GO
grant execute on app.GetItemSubTeamModel to [WFM\IConInterfaceUser]
GO
grant execute on [app].[IconItemSubTeamExceptions]	 to iConReports 
GO
grant execute on [app].[IconItemTaxExceptions]	 to iConReports
GO
grant execute on [app].[IconIRMASignAttributeComparison] to iConReports
GO

-- Nutricon
--WFM\nutriconservicedev' account should belogn to NutritionRole
--IF (SELECT name FROM sys.server_principals WHERE name = 'WFM\NutriconServiceDev') IS NULL
--CREATE LOGIN [WFM\NutriconServiceDev] FROM WINDOWS;
--IF (SELECT name FROM sys.database_principals WHERE name = 'WFM\nutriconservicedev') IS NULL
--CREATE USER [WFM\nutriconservicedev] FROM LOGIN [WFM\nutriconservicedev];
--IF (SELECT name FROM sys.database_principals WHERE name = 'nutritionrole') IS NOT NULL
--ALTER ROLE nutritionrole ADD MEMBER [WFM\nutriconservicedev];

GRANT DELETE ON SCHEMA::nutrition TO [NutritionRole]
GRANT INSERT ON SCHEMA::nutrition TO [NutritionRole]
GRANT SELECT ON SCHEMA::nutrition TO [NutritionRole]
GRANT UPDATE ON SCHEMA::nutrition TO [NutritionRole]
GRANT EXECUTE ON SCHEMA::nutrition TO [NutritionRole]

GRANT SELECT ON app.App TO [NutritionRole]
GRANT INSERT ON app.AppLog TO [NutritionRole]
GRANT SELECT ON App.AppLog TO [NutritionRole]
GRANT SELECT ON app.EventType TO [NutritionRole]
GRANT INSERT ON app.EventQueue TO [NutritionRole]
GRANT SELECT ON app.Settings TO [NutritionRole]
GRANT SELECT ON app.RegionalSettings TO [NutritionRole]
GRANT SELECT ON app.Regions TO [NutritionRole]
GRANT SELECT ON app.IRMAItemSubscription TO [NutritionRole]
GRANT SELECT ON dbo.ItemSignAttribute TO [NutritionRole]
GRANT INSERT ON dbo.ItemSignAttribute TO [NutritionRole]
GRANT SELECT ON dbo.ScanCode TO [NutritionRole]
GRANT SELECT on dbo.ItemTrait TO [NutritionRole]
GRANT SELECT on dbo.Trait TO [NutritionRole]
GRANT SELECT on dbo.HealthyEatingRating TO [NutritionRole]
GRANT EXECUTE ON infor.GenerateItemUpdateMessages TO [NutritionRole]
GRANT SELECT ON app.MessageQueueProduct TO [NutritionRole]
GRANT INSERT ON app.MessageQueueProduct TO [NutritionRole]
GRANT UPDATE ON app.MessageQueueProduct TO [NutritionRole]
GRANT SELECT ON app.MessageQueueNutrition TO [NutritionRole]
GRANT INSERT ON app.MessageQueueNutrition TO [NutritionRole]
GRANT UPDATE ON app.MessageQueueNutrition TO [NutritionRole]

-- PDX Extract
if (SELECT DATABASE_PRINCIPAL_ID('IconPDXExtractRole')) IS NULL
	CREATE ROLE IconPDXExtractRole

GRANT SELECT ON app.IRMAItemSubscription				TO [IconPDXExtractRole];    
GRANT SELECT ON ScanCode								TO [IconPDXExtractRole];
GRANT SELECT ON ItemTrait								TO [IconPDXExtractRole];
GRANT SELECT ON Trait									TO [IconPDXExtractRole];
GRANT SELECT ON ItemHierarchyClass						TO [IconPDXExtractRole];
GRANT SELECT ON HierarchyClass							TO [IconPDXExtractRole];
GRANT SELECT ON Hierarchy								TO [IconPDXExtractRole];
GRANT SELECT ON HierarchyClassTrait						TO [IconPDXExtractRole];
GRANT SELECT ON Locale									TO [IconPDXExtractRole];
GRANT SELECT ON LocaleTrait								TO [IconPDXExtractRole];
GRANT SELECT ON LocaleType								TO [IconPDXExtractRole];
GRANT SELECT ON LocaleAddress							TO [IconPDXExtractRole];
GRANT SELECT ON PhysicalAddress							TO [IconPDXExtractRole];
GRANT SELECT ON City									TO [IconPDXExtractRole];
GRANT SELECT ON Territory								TO [IconPDXExtractRole];
GRANT SELECT ON PostalCode								TO [IconPDXExtractRole];
GRANT EXECUTE ON [app].[PDX_ItemHierarchyFile]			TO [IconPDXExtractRole];
GRANT EXECUTE ON [app].[PDX_LocationHierarchyFile]		TO [IconPDXExtractRole];
GRANT EXECUTE ON [app].[PDX_MerchHierarchyFile]			TO [IconPDXExtractRole];
