IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'PriceBatchPromoAdd')
	BEGIN
		DROP  Trigger PriceBatchPromoAdd
	END
GO
CREATE Trigger PriceBatchPromoAdd 
ON PriceBatchPromo
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
