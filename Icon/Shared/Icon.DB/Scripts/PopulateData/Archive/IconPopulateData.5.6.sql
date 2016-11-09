/*
All pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.
*/

if ((select count(*) from app.RetentionPolicy) = 0 and (select @@SERVERNAME) = 'QA-SQLSHARED3\SQLSHARED3Q')
	begin
		
		truncate table app.RetentionPolicy

		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'AppLog', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'IRMAPush', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'MessageHistory', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'MessageQueueHierarchy', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'MessageQueueItemLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'MessageQueueLocale', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'MessageQueueProduct', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'MessageQueuePrice', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'MessageQueueProductSelectionGroup', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'R10MessageResponse', 10)
		INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'idq-icon\sqlshared3q', N'Icon', N'app', N'EventQueue', 10)
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


