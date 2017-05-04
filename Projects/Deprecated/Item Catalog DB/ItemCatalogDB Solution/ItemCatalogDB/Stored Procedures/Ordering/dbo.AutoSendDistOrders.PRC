SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].AutoSendDistOrders') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].AutoSendDistOrders
GO


CREATE PROCEDURE dbo.AutoSendDistOrders

AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @ZoneNonTransfer TABLE (Zone_ID int, SubTeam_No int, Supplier_Store_No int)
    DECLARE @ZoneTransfer TABLE (Zone_ID int, SubTeam_No int, Supplier_Store_No int)
    DECLARE @Orders TABLE(OrderHeader_ID int PRIMARY KEY)

    BEGIN TRAN

    INSERT INTO @ZoneNonTransfer
    SELECT Zone_ID, ZoneSubTeam.SubTeam_No, Supplier_Store_No 
    FROM 
        ZoneSubTeam (nolock)
        INNER JOIN 
            SubTeam (nolock)
            ON SubTeam.SubTeam_No = ZoneSubTeam.SubTeam_No AND Team_No <> 990
    WHERE OrderStart IS NOT NULL
          AND GETDATE() >= DATEADD(day, 1, OrderEnd) -- Has not been done today
          AND GETDATE() >= DATEADD(day, DATEDIFF(day, OrderEnd, GETDATE()), OrderEnd) -- It is the right time of day

    SELECT @Error_No = @@ERROR 

    IF @Error_No = 0
    BEGIN
        INSERT INTO @ZoneTransfer
        SELECT Zone_ID, ZoneSubTeam.SubTeam_No, Supplier_Store_No 
        FROM 
            ZoneSubTeam (nolock)
            INNER JOIN 
                SubTeam (nolock) 
                ON SubTeam.SubTeam_No = ZoneSubTeam.SubTeam_No AND Team_No <> 990
        WHERE OrderStart IS NOT NULL
              AND GETDATE() >= DATEADD(day, 1, OrderEndTransfers) -- Has not been done today
              AND GETDATE() >= DATEADD(day, DATEDIFF(day, OrderEndTransfers, GETDATE()), OrderEndTransfers) -- It is the right time of day

        SELECT @Error_No = @@ERROR
    END


    IF @Error_No = 0
    BEGIN    
        INSERT INTO @Orders
        SELECT OrderHeader_ID
        FROM 
            OrderHeader (nolock)
            INNER JOIN 
                Vendor (nolock)
                ON Vendor.Vendor_ID = OrderHeader.Vendor_ID
            INNER JOIN
                @ZoneNonTransfer ZST
                ON ZST.Supplier_Store_No = Vendor.Store_No
            INNER JOIN 
                Vendor RL (nolock)
                ON RL.Vendor_ID = OrderHeader.ReceiveLocation_ID
            INNER JOIN 
                Store (nolock)
                ON Store.Store_No = RL.Store_No AND Store.Zone_ID = ZST.Zone_ID
        WHERE Transfer_SubTeam = ZST.SubTeam_No
        AND Transfer_To_SubTeam IS NULL
        AND Sent = 0 AND CloseDate IS NULL AND DATEDIFF(day, GETDATE(), Expected_Date) = 1
        AND NOT EXISTS (SELECT * FROM OrderItem INNER JOIN Item (nolock) ON Item.Item_Key = OrderItem.Item_Key WHERE OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID AND ((DateReceived IS NOT NULL) OR EXEDistributed = 0))
    
        
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO @Orders
        SELECT OrderHeader_ID
        FROM 
            OrderHeader (nolock)
            INNER JOIN 
                Vendor (nolock)
                ON Vendor.Vendor_ID = OrderHeader.Vendor_ID
            INNER JOIN
                @ZoneTransfer ZST
                ON ZST.Supplier_Store_No = Vendor.Store_No
            INNER JOIN 
                Vendor RL (nolock)
                ON RL.Vendor_ID = OrderHeader.ReceiveLocation_ID
            INNER JOIN 
                Store (nolock)
                ON Store.Store_No = RL.Store_No AND Store.Zone_ID = ZST.Zone_ID
        WHERE Transfer_SubTeam = ZST.SubTeam_No
        AND Transfer_To_SubTeam IS NOT NULL
        AND Sent = 0 AND CloseDate IS NULL AND DATEDIFF(day, GETDATE(), Expected_Date) = 1
        AND NOT EXISTS (SELECT * FROM OrderItem INNER JOIN Item (nolock) ON Item.Item_Key = OrderItem.Item_Key WHERE OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID AND ((DateReceived IS NOT NULL) OR EXEDistributed = 0))

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE OrderHeader
        FROM OrderHeader
        INNER JOIN @Orders O ON O.OrderHeader_ID = OrderHeader.OrderHeader_ID
        WHERE NOT EXISTS (SELECT * FROM OrderItem WHERE OrderHeader_ID = OrderHeader.OrderHeader_ID)

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        UPDATE OrderHeader
        SET Sent = 1, SentDate = GETDATE(), User_ID = NULL
        FROM OrderHeader
        INNER JOIN @Orders O ON O.OrderHeader_ID = OrderHeader.OrderHeader_ID

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        UPDATE ZoneSubTeam
        SET OrderEnd = DATEADD(day, DATEDIFF(day, OrderEnd, GETDATE()), OrderEnd)
        FROM 
            ZoneSubTeam
            INNER JOIN
                @ZoneNonTransfer ZNT
                ON ZNT.Zone_ID = ZoneSubTeam.Zone_ID AND ZNT.SubTeam_No = ZoneSubTeam.SubTeam_No AND ZNT.Supplier_Store_No = ZoneSubTeam.Supplier_Store_No

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        UPDATE ZoneSubTeam
        SET OrderEndTransfers = DATEADD(day, DATEDIFF(day, OrderEndTransfers, GETDATE()), OrderEndTransfers)
        FROM 
            ZoneSubTeam
            INNER JOIN
                @ZoneTransfer ZT
                ON ZT.Zone_ID = ZoneSubTeam.Zone_ID AND ZT.SubTeam_No = ZoneSubTeam.SubTeam_No AND ZT.Supplier_Store_No = ZoneSubTeam.Supplier_Store_No

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        COMMIT TRAN
        SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('AutoSendDistOrders failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
    
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


