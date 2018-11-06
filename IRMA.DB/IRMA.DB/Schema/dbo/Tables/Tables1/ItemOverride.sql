CREATE TABLE [dbo].[ItemOverride] (
    [Item_Key]                      INT            NOT NULL,
    [StoreJurisdictionID]           INT            NOT NULL,
    [Item_Description]              VARCHAR (60)   NOT NULL,
    [Sign_Description]              VARCHAR (60)   NOT NULL,
    [Package_Desc1]                 DECIMAL (9, 4) CONSTRAINT [DF_ItemOverride_Package_Desc1] DEFAULT ((0)) NOT NULL,
    [Package_Desc2]                 DECIMAL (9, 4) CONSTRAINT [DF_ItemOverride_Package_Desc2] DEFAULT ((0)) NOT NULL,
    [Package_Unit_ID]               INT            NOT NULL,
    [Retail_Unit_ID]                INT            NOT NULL,
    [Vendor_Unit_ID]                INT            NOT NULL,
    [Distribution_Unit_ID]          INT            NOT NULL,
    [POS_Description]               VARCHAR (26)   NOT NULL,
    [Food_Stamps]                   BIT            CONSTRAINT [DF_ItemOverride_Food_Stamps] DEFAULT ((0)) NOT NULL,
    [Price_Required]                BIT            CONSTRAINT [DF_ItemOverride_Price_Required] DEFAULT ((0)) NOT NULL,
    [Quantity_Required]             BIT            CONSTRAINT [DF_ItemOverride_Quantity_Required] DEFAULT ((0)) NOT NULL,
    [Manufacturing_Unit_ID]         INT            NULL,
    [QtyProhibit]                   BIT            NULL,
    [GroupList]                     INT            NULL,
    [Case_Discount]                 BIT            NULL,
    [Coupon_Multiplier]             BIT            NULL,
    [Misc_Transaction_Sale]         SMALLINT       NULL,
    [Misc_Transaction_Refund]       SMALLINT       NULL,
    [Ice_Tare]                      INT            NULL,
    [Brand_ID]                      INT            NULL,
    [Origin_ID]                     INT            NULL,
    [CountryProc_ID]                INT            NULL,
    [SustainabilityRankingRequired] BIT            CONSTRAINT [DF_ItemOverride_SustainabilityRankingRequired] DEFAULT ((0)) NULL,
    [SustainabilityRankingID]       INT            NULL,
    [LabelType_ID]                  INT            NULL,
    [CostedByWeight]                BIT            CONSTRAINT [DF_ItemOverride_CostedByWeight] DEFAULT ((0)) NOT NULL,
    [Average_Unit_Weight]           DECIMAL (9, 4) NULL,
    [Ingredient]                    BIT            CONSTRAINT [DF_ItemOverride_Ingredient] DEFAULT ((0)) NOT NULL,
    [Recall_Flag]                   BIT            CONSTRAINT [DF_ItemOverride_Recall_Flag] DEFAULT ((0)) NULL,
    [LockAuth]                      BIT            CONSTRAINT [DF_ItemOverride_LockAuth] DEFAULT ((0)) NULL,
    [Not_Available]                 BIT            CONSTRAINT [DF_ItemOverride_Not_Available] DEFAULT ((0)) NOT NULL,
    [Not_AvailableNote]             VARCHAR (255)  NULL,
    [FSA_Eligible]                  BIT            CONSTRAINT [DF_ItemOverride_FSA_Eligible] DEFAULT ((0)) NOT NULL,
    [Product_Code]                  VARCHAR (15)   NULL,
    [Unit_Price_Category]           INT            NULL,
    [LastModifiedUser_ID]           INT            NULL,
    [SignRomanceTextLong]           NVARCHAR(300)  NULL,
    [SignRomanceTextShort]          NVARCHAR(140)  NULL
    CONSTRAINT [PK_ItemOverride] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [StoreJurisdictionID] ASC),
    CONSTRAINT [FK_ItemOverride_Distribution_Unit_ID] FOREIGN KEY ([Distribution_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_ItemOverride_Item_Key] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_ItemOverride_Manufacturing_Unit_ID] FOREIGN KEY ([Manufacturing_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_ItemOverride_Package_Unit_ID] FOREIGN KEY ([Package_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_ItemOverride_Retail_Unit_ID] FOREIGN KEY ([Retail_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_ItemOverride_StoreJurisdictionID] FOREIGN KEY ([StoreJurisdictionID]) REFERENCES [dbo].[StoreJurisdiction] ([StoreJurisdictionID]),
    CONSTRAINT [FK_ItemOverride_Vendor_Unit_ID] FOREIGN KEY ([Vendor_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID])
);


GO
ALTER TABLE [dbo].[ItemOverride] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO

CREATE Trigger ItemOverrideUpdate ON [dbo].[ItemOverride] 
FOR UPDATE AS 
/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
MZ      2015-07-28  16293   Modified the trigger so that if the item update is done by Global Controller 
                            job, then no PriceBatchDetail records will be created. 
							Also, at the end of the trigger, the iConController user id will be wiped
							out from the LastModifiedUser_ID field. This is done because not all the 
							ItemOverride Updates will re-set the LastModifiedUser_ID value. An update done
							after the Global Controller job can retain the LastModifiedUser_ID value, and 
							not having a PriceBatchDetail record generated.
***********************************************************************************************/

BEGIN
    DECLARE @error_no int,
			@IconControllerUserId int = (SELECT User_ID FROM Users WHERE UserName = 'iconcontrolleruser')

    SELECT @error_no = 0

    IF @error_no = 0
    BEGIN
		-- CREATE A HISTORY RECORD TO TRACK THE CHANGE
		INSERT INTO ItemOverrideHistory (
			Item_Key, 
			StoreJurisdictionID,
			Item_Description, 
			Sign_Description, 
			Package_Desc1, 
			Package_Desc2, 
			Package_Unit_ID, 
			Retail_Unit_ID, 
			Vendor_Unit_ID, 
			Distribution_Unit_ID, 
			POS_Description,
			Food_Stamps, 
			Price_Required,
			Quantity_Required,
			Manufacturing_Unit_ID,
			QtyProhibit, 
			GroupList,
			Case_Discount, 
			Coupon_Multiplier,
			Misc_Transaction_Sale, 
			Misc_Transaction_Refund,
			Ice_Tare,
			Brand_ID,
			Origin_ID,
			CountryProc_ID,
			SustainabilityRankingRequired,
			SustainabilityRankingID,
			LabelType_ID,
			CostedByWeight,
			Average_Unit_Weight,
			Ingredient,
			Recall_Flag,
			LockAuth,
			Not_Available,
			Not_AvailableNote,
			FSA_Eligible,
			Product_Code,
			Unit_Price_Category
		) SELECT 
			Inserted.Item_Key, 			
			Inserted.StoreJurisdictionID,
			Inserted.Item_Description, 
			Inserted.Sign_Description, 
			Inserted.Package_Desc1, 
			Inserted.Package_Desc2, 
			Inserted.Package_Unit_ID, 
			Inserted.Retail_Unit_ID, 
			Inserted.Vendor_Unit_ID, 
			Inserted.Distribution_Unit_ID, 
			Inserted.POS_Description,
			Inserted.Food_Stamps, 
			Inserted.Price_Required,
			Inserted.Quantity_Required,
			Inserted.Manufacturing_Unit_ID,
			Inserted.QtyProhibit, 
			Inserted.GroupList,
			Inserted.Case_Discount, 
			Inserted.Coupon_Multiplier,
			Inserted.Misc_Transaction_Sale, 
			Inserted.Misc_Transaction_Refund,
			Inserted.Ice_Tare,
			Inserted.Brand_ID,
			Inserted.Origin_ID,
			Inserted.CountryProc_ID,
			Inserted.SustainabilityRankingRequired,
			Inserted.SustainabilityRankingID,
			Inserted.LabelType_ID,
			Inserted.CostedByWeight,
			Inserted.Average_Unit_Weight,
			Inserted.Ingredient,
			Inserted.Recall_Flag,
			Inserted.LockAuth,
			Inserted.Not_Available,
			Inserted.Not_AvailableNote,
			Inserted.FSA_Eligible,
			Inserted.Product_Code,
			Inserted.Unit_Price_Category
        FROM Inserted
        INNER JOIN
            Deleted
            ON Deleted.Item_Key = Inserted.Item_Key
        WHERE ISNULL(Inserted.StoreJurisdictionID, 0) <> ISNULL(Deleted.StoreJurisdictionID, 0)
			OR Inserted.Item_Description <> Deleted.Item_Description
            OR Inserted.Sign_Description <> Deleted.Sign_Description
            OR Inserted.Package_Desc1 <> Deleted.Package_Desc1
            OR Inserted.Package_Desc2 <> Deleted.Package_Desc2
            OR ISNULL(Inserted.Package_Unit_ID, 0) <> ISNULL(Deleted.Package_Unit_ID, 0)
            OR ISNULL(Inserted.Retail_Unit_ID, 0) <> ISNULL(Deleted.Retail_Unit_ID, 0)
            OR ISNULL(Inserted.Vendor_Unit_ID, 0) <> ISNULL(Deleted.Vendor_Unit_ID, 0)
            OR ISNULL(Inserted.Distribution_Unit_ID, 0) <> ISNULL(Deleted.Distribution_Unit_ID, 0)            
            OR Inserted.POS_Description <> Deleted.POS_Description
            OR Inserted.Food_Stamps <> Deleted.Food_Stamps
            OR Inserted.Price_Required <> Deleted.Price_Required
            OR Inserted.Quantity_Required <> Deleted.Quantity_Required
            OR ISNULL(Inserted.Manufacturing_Unit_ID, 0) <> ISNULL(Deleted.Manufacturing_Unit_ID, 0)
            OR ISNULL(Inserted.QtyProhibit, 0) <> ISNULL(Deleted.QtyProhibit, 0)
            OR ISNULL(Inserted.GroupList, 0) <> ISNULL(Deleted.GroupList, 0)
			OR ISNULL(Inserted.Case_Discount, 0) <> ISNULL(Deleted.Case_Discount, 0)
			OR ISNULL(Inserted.Coupon_Multiplier, 0) <> ISNULL(Deleted.Coupon_Multiplier, 0)
			OR ISNULL(Inserted.Misc_Transaction_Sale, 0) <> ISNULL(Deleted.Misc_Transaction_Sale, 0)
			OR ISNULL(Inserted.Misc_Transaction_Refund, 0) <> ISNULL(Deleted.Misc_Transaction_Refund, 0)
			OR ISNULL(Inserted.Ice_Tare, 0) <> ISNULL(Deleted.Ice_Tare, 0)
			OR ISNULL(Inserted.Brand_ID, 0) <> ISNULL(Deleted.Brand_ID, 0)
			OR ISNULL(Inserted.Origin_ID, 0) <> ISNULL(Deleted.Origin_ID, 0)
			OR ISNULL(Inserted.CountryProc_ID, 0) <> ISNULL(Deleted.CountryProc_ID, 0)
			OR ISNULL(Inserted.SustainabilityRankingRequired, 0) <> ISNULL(Deleted.SustainabilityRankingRequired, 0)
			OR ISNULL(Inserted.SustainabilityRankingID, 0) <> ISNULL(Deleted.SustainabilityRankingID, 0)
			OR ISNULL(Inserted.LabelType_ID, 0) <> ISNULL(Deleted.LabelType_ID, 0)
			OR Inserted.CostedByWeight <> Deleted.CostedByWeight
			OR ISNULL(Inserted.Average_Unit_Weight, 0) <> ISNULL(Deleted.Average_Unit_Weight, 0)
			OR Inserted.Ingredient <> Deleted.Ingredient
			OR ISNULL(Inserted.Recall_Flag, 0) <> ISNULL(Deleted.Recall_Flag, 0)
			OR ISNULL(Inserted.LockAuth, 0) <> ISNULL(Deleted.LockAuth, 0)
			OR Inserted.Not_Available <> Deleted.Not_Available
			OR ISNULL(Inserted.Not_AvailableNote, 0) <> ISNULL(Deleted.Not_AvailableNote, 0)
			OR Inserted.FSA_Eligible <> Deleted.FSA_Eligible
			OR ISNULL(Inserted.Product_Code, 0) <> ISNULL(Deleted.Product_Code, 0)
			OR ISNULL(Inserted.Unit_Price_Category, 0) <> ISNULL(Deleted.Unit_Price_Category, 0)
			
		SELECT @error_no = @@ERROR
    END
   
	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED FOR EACH STORE THAT
	-- IS ASSIGNED TO THE JURISDICTION THAT CHANGED
    IF @error_no = 0
    BEGIN
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Store_No, Inserted.Item_Key, 2, 'ItemOverrideUpdate Trigger'
        FROM Inserted
        INNER JOIN Deleted ON 
            Deleted.Item_Key = Inserted.Item_Key
        INNER JOIN Item ON 
			Item.Item_Key = Inserted.Item_Key 
			AND (Item.Remove_Item = 0 AND Item.Deleted_Item = 0)
        INNER JOIN Store ON
			Store.StoreJurisdictionID = Inserted.StoreJurisdictionID
            AND (Store.WFM_Store = 1 OR Store.Mega_Store = 1)
        WHERE (Inserted.Item_Description <> Deleted.Item_Description
                OR Inserted.Sign_Description <> Deleted.Sign_Description
				OR Inserted.POS_Description <> Deleted.POS_Description 
                OR Inserted.Package_Desc1 <> Deleted.Package_Desc1
                OR Inserted.Package_Desc2 <> Deleted.Package_Desc2
                OR ISNULL(Inserted.Package_Unit_ID, 0) <> ISNULL(Deleted.Package_Unit_ID, 0)
                OR ISNULL(Inserted.Retail_Unit_ID, 0) <> ISNULL(Deleted.Retail_Unit_ID, 0)
                OR Inserted.Food_Stamps <> Deleted.Food_Stamps
                OR Inserted.Price_Required <> Deleted.Price_Required
                OR Inserted.Quantity_Required <> Deleted.Quantity_Required
                OR ISNULL(Inserted.QtyProhibit, 0) <> ISNULL(Deleted.QtyProhibit, 0)
                OR ISNULL(Inserted.GroupList, 0) <> ISNULL(Deleted.GroupList, 0)
                OR ISNULL(Inserted.Case_Discount, 0) <> ISNULL(Deleted.Case_Discount, 0)
				OR ISNULL(Inserted.Coupon_Multiplier, 0) <> ISNULL(Deleted.Coupon_Multiplier, 0)
				OR ISNULL(Inserted.Misc_Transaction_Sale, 0) <> ISNULL(Deleted.Misc_Transaction_Sale, 0)
				OR ISNULL(Inserted.Misc_Transaction_Refund, 0) <> ISNULL(Deleted.Misc_Transaction_Refund, 0)
				OR ISNULL(Inserted.Ice_Tare, 0) <> ISNULL(Deleted.Ice_Tare, 0)
				OR ISNULL(Inserted.Brand_ID, 0) <> ISNULL(Deleted.Brand_ID, 0)
				OR ISNULL(Inserted.Origin_ID, 0) <> ISNULL(Deleted.Origin_ID, 0)
				OR ISNULL(Inserted.CountryProc_ID, 0) <> ISNULL(Deleted.CountryProc_ID, 0)
				OR ISNULL(Inserted.SustainabilityRankingRequired, 0) <> ISNULL(Deleted.SustainabilityRankingRequired, 0)
				OR ISNULL(Inserted.SustainabilityRankingID, 0) <> ISNULL(Deleted.SustainabilityRankingID, 0)
				OR ISNULL(Inserted.LabelType_ID, 0) <> ISNULL(Deleted.LabelType_ID, 0)
				OR Inserted.CostedByWeight <> Deleted.CostedByWeight
				OR ISNULL(Inserted.Average_Unit_Weight, 0) <> ISNULL(Deleted.Average_Unit_Weight, 0)
				OR Inserted.Ingredient <> Deleted.Ingredient
				OR ISNULL(Inserted.Recall_Flag, 0) <> ISNULL(Deleted.Recall_Flag, 0)
				OR ISNULL(Inserted.LockAuth, 0) <> ISNULL(Deleted.LockAuth, 0)
				OR Inserted.Not_Available <> Deleted.Not_Available
				OR ISNULL(Inserted.Not_AvailableNote, 0) <> ISNULL(Deleted.Not_AvailableNote, 0)
				OR Inserted.FSA_Eligible <> Deleted.FSA_Eligible
				OR ISNULL(Inserted.Product_Code, 0) <> ISNULL(Deleted.Product_Code, 0)
				OR ISNULL(Inserted.Unit_Price_Category, 0) <> ISNULL(Deleted.Unit_Price_Category, 0))							
            AND (dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key, Store.Store_No) = 0)
			AND (ISNULL(inserted.LastModifiedUser_ID, 0) <> ISNULL(@IconControllerUserId, 0))
            
        SELECT @error_no = @@ERROR
    END  
	
	IF @error_no = 0
		BEGIN
			UPDATE ItemOverride
			   SET LastModifiedUser_ID = NULL
			  FROM INSERTED
			 WHERE INSERTED.Item_Key = ItemOverride.Item_Key
			   AND INSERTED.LastModifiedUser_ID = @IconControllerUserId

			SELECT @error_no = @@ERROR
		END
		     
    IF @error_no <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('ItemOverrideUpdate Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [ItemOverrideUdpate.sql]'

GO
CREATE Trigger ItemOverrideAdd ON [dbo].[ItemOverride] 
FOR INSERT AS 
BEGIN
    DECLARE @error_no int
    SELECT @error_no = 0

    IF @error_no = 0
    BEGIN
		-- CREATE THE INITIAL HISTORY RECORD TO TRACK THE CHANGE
		INSERT INTO ItemOverrideHistory (
			Item_Key, 
			StoreJurisdictionID,
			Item_Description, 
			Sign_Description, 
			Package_Desc1, 
			Package_Desc2, 
			Package_Unit_ID, 
			Retail_Unit_ID, 
			Vendor_Unit_ID, 
			Distribution_Unit_ID, 
			POS_Description,
			Food_Stamps, 
			Price_Required,
			Quantity_Required,
			Manufacturing_Unit_ID,
			QtyProhibit, 
			GroupList,
			Case_Discount, 
			Coupon_Multiplier,
			Misc_Transaction_Sale, 
			Misc_Transaction_Refund,
			Ice_Tare,
			Brand_ID,
			Origin_ID,
			CountryProc_ID,
			SustainabilityRankingRequired,
			SustainabilityRankingID,
			LabelType_ID,
			CostedByWeight,
			Average_Unit_Weight,
			Ingredient,
			Recall_Flag,
			LockAuth,
			Not_Available,
			Not_AvailableNote,
			FSA_Eligible,
			Product_Code,
			Unit_Price_Category
		) SELECT 
			Inserted.Item_Key, 			
			Inserted.StoreJurisdictionID,
			Inserted.Item_Description, 
			Inserted.Sign_Description, 
			Inserted.Package_Desc1, 
			Inserted.Package_Desc2, 
			Inserted.Package_Unit_ID, 
			Inserted.Retail_Unit_ID, 
			Inserted.Vendor_Unit_ID, 
			Inserted.Distribution_Unit_ID, 
			Inserted.POS_Description,
			Inserted.Food_Stamps, 
			Inserted.Price_Required,
			Inserted.Quantity_Required,
			Inserted.Manufacturing_Unit_ID,
			Inserted.QtyProhibit, 
			Inserted.GroupList,
			Inserted.Case_Discount, 
			Inserted.Coupon_Multiplier,
			Inserted.Misc_Transaction_Sale, 
			Inserted.Misc_Transaction_Refund,
			Inserted.Ice_Tare,
			Inserted.Brand_ID,
			Inserted.Origin_ID,
			Inserted.CountryProc_ID,
			Inserted.SustainabilityRankingRequired,
			Inserted.SustainabilityRankingID,
			Inserted.LabelType_ID,
			Inserted.CostedByWeight,
			Inserted.Average_Unit_Weight,
			Inserted.Ingredient,
			Inserted.Recall_Flag,
			Inserted.LockAuth,
			Inserted.Not_Available,
			Inserted.Not_AvailableNote,
			Inserted.FSA_Eligible,
			Inserted.Product_Code,
			Inserted.Unit_Price_Category
        FROM Inserted
            
		SELECT @error_no = @@ERROR
    END
              
	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED FOR EACH STORE THAT
	-- IS ASSIGNED TO THE JURISDICTION THAT CHANGED
    IF @error_no = 0
    BEGIN
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Store_No, Inserted.Item_Key, 2, 'ItemOverrideAdd Trigger'
        FROM Inserted
        INNER JOIN Item ON 
			Item.Item_Key = Inserted.Item_Key 
			AND (Item.Remove_Item = 0 AND Item.Deleted_Item = 0)
        INNER JOIN Store ON
			Store.StoreJurisdictionID = Inserted.StoreJurisdictionID
            AND (Store.WFM_Store = 1 OR Store.Mega_Store = 1)
        WHERE (dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key, Store.Store_No) = 0)
            
        SELECT @error_no = @@ERROR
    END
    
    IF @error_no <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('ItemOverrideAdd Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOverride] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOverride] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOverride] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOverride] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOverride] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOverride] TO [IRMAReports]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemOverride] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemOverride] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOverride] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemOverride] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOverride] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemOverride] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemOverride] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOverride] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOverride] TO [spice_user]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemOverride] TO [iCONReportingRole]
    AS [dbo];

