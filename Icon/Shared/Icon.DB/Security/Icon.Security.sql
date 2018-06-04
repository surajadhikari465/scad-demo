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

grant execute on [app].[IconIRMASignAttributeComparison] to iConReports

GO

-- PDX Extract
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
