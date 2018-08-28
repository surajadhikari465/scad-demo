SET IDENTITY_INSERT dbo.OrderType ON

IF NOT EXISTS (SELECT 1 FROM dbo.OrderType WHERE OrderType_Name = 'Purchase')
	INSERT INTO dbo.OrderType (OrderType_ID, OrderType_Desc) VALUES (1, 'Purchase')

IF NOT EXISTS (SELECT 1 FROM dbo.OrderType WHERE OrderType_Name = 'Distribution')
	INSERT INTO dbo.OrderType (OrderType_ID, OrderType_Desc) VALUES (2, 'Distribution')

IF NOT EXISTS (SELECT 1 FROM dbo.OrderType WHERE OrderType_Name = 'Transfer')
	INSERT INTO dbo.OrderType (OrderType_ID, OrderType_Desc) VALUES (3, 'Transfer')

IF NOT EXISTS (SELECT 1 FROM dbo.OrderType WHERE OrderType_Name = 'Flowthru')
	INSERT INTO dbo.OrderType (OrderType_ID, OrderType_Desc) VALUES (4, 'Flowthru')

SET IDENTITY_INSERT [amz].[EventType] OFF