-- Include in S05_Triggers.SQL
-- 09.07.02 V3.5 Maria Younes
-- Add POApprovalAdmin plus other missing fields in UsersHistory table to match Users table

/****** Object:  Trigger [UsersAddUpdateDelete]    Script Date: 07/02/2009 11:53:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[UsersAddUpdateDelete]'))
DROP TRIGGER [dbo].[UsersAddUpdateDelete]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [UsersAddUpdateDelete]
ON [dbo].[Users]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO UsersHistory ([User_ID], UserName, FullName, Printer, CoverPage, EMail, Pager_Email, Fax_Number,  AccountEnabled, SuperUser, 
							  PO_Accountant, Accountant, Distributor, FacilityCreditProcessor, Buyer, Coordinator, Item_Administrator, 
							  Vendor_Administrator, Lock_Administrator, Telxon_Store_Limit, Phone_Number, Title, RecvLog_Store_Limit, 
							  Warehouse,  PriceBatchProcessor, Inventory_Administrator,	BatchBuildOnly, DCAdmin, PromoAccessLevel, 
							  CostAdmin, VendorCostDiscrepancyAdmin, POApprovalAdmin)
    SELECT [User_ID], UserName, FullName, Printer, CoverPage, EMail, Pager_Email, Fax_Number,  AccountEnabled, SuperUser, 
		   PO_Accountant, Accountant, Distributor, FacilityCreditProcessor, Buyer, Coordinator, Item_Administrator, 
		   Vendor_Administrator, Lock_Administrator, Telxon_Store_Limit, Phone_Number, Title, RecvLog_Store_Limit,  
		   Warehouse, PriceBatchProcessor, Inventory_Administrator, BatchBuildOnly, DCAdmin, PromoAccessLevel, 
		   CostAdmin, VendorCostDiscrepancyAdmin, POApprovalAdmin
    FROM Inserted

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        DELETE UsersHistory
        FROM UsersHistory H
        INNER JOIN
            Deleted ON Deleted.[User_ID] = H.[User_ID]
        LEFT JOIN
            Inserted ON Deleted.[User_ID] = Inserted.[User_ID]
        WHERE Inserted.[User_ID] IS NULL

        SELECT @Error_No = @@ERROR
    END
    
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('UsersAddUpdateDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

