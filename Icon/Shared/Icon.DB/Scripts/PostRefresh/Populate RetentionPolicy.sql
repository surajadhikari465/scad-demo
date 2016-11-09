declare @Env varchar(4)

-- dev, test, qa, prd
set @Env = ''

if (@Env = 'dev')
	begin
		USE [iCONDev]

		truncate table app.RetentionPolicy

		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'AppLog', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'PerformanceLog', 5)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'IRMAPush', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageHistory', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageQueueHierarchy', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageQueueItemLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageQueueItemLocaleArchive', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageQueueLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageQueueProduct', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageQueuePrice', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageQueuePriceArchive', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageQueueProductSelectionGroup', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'MessageQueueNutrition', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'R10MessageResponse', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'EventQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'IconDev', N'app', N'ItemMovementErrorQueue', 10)
	end

if (@Env = 'test')
	begin
		USE [Icon]
		
		truncate table app.RetentionPolicy
		
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'AppLog', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'PerformanceLog', 5)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'IRMAPush', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageHistory', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageQueueHierarchy', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageQueueItemLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageQueueItemLocaleArchive', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageQueueLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageQueueProduct', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageQueuePrice', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageQueuePriceArchive', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageQueueProductSelectionGroup', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'MessageQueueNutrition', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'R10MessageResponse', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'EventQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'cewd1815\SQLSHARED2012D', N'Icon', N'app', N'ItemMovementErrorQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-FL', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-FL', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MA', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MA', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MW', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-MW', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-NA', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-NA', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-RM', N'ItemCatalog_Test', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDD-RM', N'ItemCatalog_Test', N'dbo', N'IconItemChangeQueue', 10)
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
		
	end

if (@Env = 'qa')
	begin
		
		USE [Icon]
		
		truncate table app.RetentionPolicy

		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'AppLog', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'PerformanceLog', 5)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'IRMAPush', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageHistory', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageQueueHierarchy', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageQueueItemLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageQueueItemLocaleArchive', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageQueueLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageQueueProduct', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageQueuePrice', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageQueuePriceArchive', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageQueueProductSelectionGroup', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'MessageQueueNutrition', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'R10MessageResponse', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'EventQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'QA-SQLSHARED3\SQLSHARED3Q', N'Icon', N'app', N'ItemMovementErrorQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-FL\FLQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-FL\FLQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-MA\MAQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-MA\MAQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-MW\MWQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-MW\MWQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-NA\NAQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-NA\NAQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-RM\RMQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-RM\RMQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-SO\SOQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-SO\SOQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-NC\NCQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-NC\NCQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-NE\NEQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-NE\NEQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-PN\PNQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-PN\PNQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-SP\SPQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-SP\SPQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-SW\SWQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-SW\SWQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-UK\UKQ', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDQ-UK\UKQ', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)

	end

if (@Env = 'prd')
	begin
		
		USE [Icon]
		
		truncate table app.RetentionPolicy

		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'AppLog', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'PerformanceLog', 5)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'IRMAPush', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageHistory', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueHierarchy', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueItemLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueItemLocaleArchive', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueProduct', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueuePrice', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueuePriceArchive', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueProductSelectionGroup', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueNutrition', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'R10MessageResponse', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'EventQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'ItemMovementErrorQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-FL\FLP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-FL\FLP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MA\MAP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MA\MAP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MW\MWP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MW\MWP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NA\NAP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NA\NAP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-RM\RMP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-RM\RMP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SO\SOP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SO\SOP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NC\NCP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NC\NCP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NE\NEP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NE\NEP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-PN\PNP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-PN\PNP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SP\SPP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SP\SPP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SW\SWP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SW\SWP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-UK\UKP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-UK\UKP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)

	end
