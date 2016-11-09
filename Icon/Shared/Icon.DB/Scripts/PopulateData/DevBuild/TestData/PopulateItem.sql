print N'Populating Item...'

SET IDENTITY_INSERT Item ON
GO

insert into Item(itemID, itemTypeID, productKey)
values	(	0	, 	2	, 	200001	),
		(	1	, 	2	, 	200002	),
		(	3	, 	2	, 	200003	),
		(	4	, 	6	, 	200004	),
		(	5	, 	2	, 	200005	),
		(	819	, 	1	, 	200609	),
		(	918	, 	1	, 	200696	),
		(	1085	, 	1	, 	200844	),
		(	1086	, 	1	, 	200845	),
		(	1089	, 	1	, 	200848	)
GO

SET IDENTITY_INSERT Item OFF
GO