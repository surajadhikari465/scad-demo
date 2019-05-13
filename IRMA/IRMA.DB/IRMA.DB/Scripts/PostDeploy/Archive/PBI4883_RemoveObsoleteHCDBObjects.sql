IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[amz].[GetMessagesWithError]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [amz].[GetMessagesWithError]
END

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[amz].[LockInventoryMessages]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [amz].[LockInventoryMessages]
END

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[amz].[LockPurchaseOrderEvents]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [amz].[LockPurchaseOrderEvents]
END

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[amz].[LockReceiptEvents]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [amz].[LockReceiptEvents]
END

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[amz].[LockInventorySpoilage]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [amz].[LockInventorySpoilage]
END

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[amz].[SendErrorNotificationEmail]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [amz].[SendErrorNotificationEmail]
END