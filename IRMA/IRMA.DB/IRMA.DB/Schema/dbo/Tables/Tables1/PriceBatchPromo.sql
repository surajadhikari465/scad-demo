CREATE TABLE [dbo].[PriceBatchPromo] (
    [PriceBatchPromoID] INT            IDENTITY (1, 1) NOT NULL,
    [Item_Key]          INT            NOT NULL,
    [Store_No]          INT            NOT NULL,
    [PriceChgTypeID]    TINYINT        NULL,
    [Start_Date]        SMALLDATETIME  NULL,
    [Multiple]          TINYINT        NULL,
    [Price]             SMALLMONEY     NULL,
    [Sale_Multiple]     TINYINT        NULL,
    [Sale_Price]        SMALLMONEY     NULL,
    [Sale_Cost]         SMALLMONEY     NULL,
    [Sale_End_Date]     SMALLDATETIME  NULL,
    [Identifier]        VARCHAR (13)   NULL,
    [Dept_No]           INT            NULL,
    [Vendor_Id]         INT            NULL,
    [ProjUnits]         INT            NULL,
    [Comment1]          CHAR (50)      NULL,
    [Comment2]          CHAR (50)      NULL,
    [BillBack]          NUMERIC (6, 2) NULL
);


GO
CREATE Trigger PriceBatchPromoAdd 
ON [dbo].[PriceBatchPromo]
FOR INSERT
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

        INSERT INTO PromoPreOrders (PriceBatchPromoID,Item_Key,Identifier,Store_No,OrderQty)
        SELECT Inserted.PriceBatchPromoID, Inserted.Item_Key, Inserted.Identifier, Inserted.Store_No, 0
        FROM Inserted

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PriceBatchPromoAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchPromo] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[PriceBatchPromo] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[PriceBatchPromo] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchPromo] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[PriceBatchPromo] TO [IRMAPromoRole]
    AS [dbo];

