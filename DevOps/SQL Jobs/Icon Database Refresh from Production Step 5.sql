--Truncate App.RetentionPolicy Table 
USE iCON
GO

TRUNCATE table App.RetentionPolicy;

INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'AppLog', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'PerformanceLog', 5)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'IRMAPush', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageHistory', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageQueueHierarchy', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageQueueItemLocale', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageQueueItemLocaleArchive', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageQueueLocale', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageQueueProduct', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageQueuePrice', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageQueuePriceArchive', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageQueueProductSelectionGroup', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'MessageQueueNutrition', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'R10MessageResponse', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'EventQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'ODWD6801', N'Icon', N'app', N'ItemMovementErrorQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'rm-db01-dev', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'rm-db01-dev', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
/*
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-FL', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-FL', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MA', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MA', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MW', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MW', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-NA', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-NA', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-SO', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-SO', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-NC', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-NC', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-NE', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-NE', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-PN', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-PN', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-SP', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-SP', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-SW', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-SW', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-UK', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDT-UK', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
*/
GO