SET IDENTITY_INSERT [amz].[EventType] ON

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'INV_ADJ')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (1, 'INV_ADJ', 'Inventory Adjustment')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'TSF_CRE')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (2, 'TSF_CRE', 'Transfer Order Creation')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'TSF_MOD')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (3, 'TSF_MOD', 'Transfer Order Modification')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'TSF_LINE_ADD')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (4, 'TSF_LINE_ADD', 'Transfer Line Item Add')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'TSF_LINE_MOD')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (5, 'TSF_LINE_MOD', 'Transfer Line Item Modification')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'TSF_LINE_DEL')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (6, 'TSF_LINE_DEL', 'Transfer Line Item Deletion')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'TSF_DEL')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (7, 'TSF_DEL', 'Transfer Order Deletion')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'RCPT_CRE')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (8, 'RCPT_CRE', 'Order Receipt Creation')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'RCPT_MOD')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (9, 'RCPT_MOD', 'Order Receipt Modification')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'PO_CRE')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (10, 'PO_CRE', 'Purchase Order Creation')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'PO_MOD')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (11, 'PO_MOD', 'Purchase Order Modification')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'PO_LINE_ADD')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (12, 'PO_LINE_ADD', 'Purchase Order Line Item Add')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'PO_LINE_MOD')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (13, 'PO_LINE_MOD', 'Purchase Order Line Item Modification')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'PO_LINE_DEL')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (14, 'PO_LINE_DEL', 'Purchase Order Line Item Deletion')

IF NOT EXISTS (SELECT 1 FROM amz.EventType WHERE EventTypeCode = 'PO_DEL')
	INSERT INTO amz.EventType (EventTypeID, EventTypeCode, EventTypeDescription) VALUES (15, 'PO_DEL', 'Purchase Order Deletion')

SET IDENTITY_INSERT [amz].[EventType] OFF