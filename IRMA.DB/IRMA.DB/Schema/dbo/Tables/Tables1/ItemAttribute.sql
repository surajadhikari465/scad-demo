CREATE TABLE [dbo].[ItemAttribute] (
    [ItemAttribute_ID]    INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Item_Key]            INT          NULL,
    [Check_Box_1]         BIT          DEFAULT ((0)) NULL,
    [Check_Box_2]         BIT          DEFAULT ((0)) NULL,
    [Check_Box_3]         BIT          DEFAULT ((0)) NULL,
    [Check_Box_4]         BIT          DEFAULT ((0)) NULL,
    [Check_Box_5]         BIT          DEFAULT ((0)) NULL,
    [Check_Box_6]         BIT          DEFAULT ((0)) NULL,
    [Check_Box_7]         BIT          DEFAULT ((0)) NULL,
    [Check_Box_8]         BIT          DEFAULT ((0)) NULL,
    [Check_Box_9]         BIT          DEFAULT ((0)) NULL,
    [Check_Box_10]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_11]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_12]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_13]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_14]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_15]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_16]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_17]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_18]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_19]        BIT          DEFAULT ((0)) NULL,
    [Check_Box_20]        BIT          DEFAULT ((0)) NULL,
    [Text_1]              VARCHAR (50) NULL,
    [Text_2]              VARCHAR (50) NULL,
    [Text_3]              VARCHAR (50) NULL,
    [Text_4]              VARCHAR (50) NULL,
    [Text_5]              VARCHAR (50) NULL,
    [Text_6]              VARCHAR (50) NULL,
    [Text_7]              VARCHAR (50) NULL,
    [Text_8]              VARCHAR (50) NULL,
    [Text_9]              VARCHAR (50) NULL,
    [Text_10]             VARCHAR (50) NULL,
    [Date_Time_1]         DATETIME     NULL,
    [Date_Time_2]         DATETIME     NULL,
    [Date_Time_3]         DATETIME     NULL,
    [Date_Time_4]         DATETIME     NULL,
    [Date_Time_5]         DATETIME     NULL,
    [Date_Time_6]         DATETIME     NULL,
    [Date_Time_7]         DATETIME     NULL,
    [Date_Time_8]         DATETIME     NULL,
    [Date_Time_9]         DATETIME     NULL,
    [Date_Time_10]        DATETIME     NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    CONSTRAINT [PK_ItemAttribute] PRIMARY KEY CLUSTERED ([ItemAttribute_ID] ASC),
    CONSTRAINT [FK_ItemAttribute_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
ALTER TABLE [dbo].[ItemAttribute] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ItemAttribute]
    ON [dbo].[ItemAttribute]([Item_Key] ASC);


GO
CREATE TRIGGER [dbo].[ItemAttributeAddUpdate] ON [dbo].[ItemAttribute] 
FOR INSERT,UPDATE
AS
 BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    update ItemAttribute 
		Set LastUpdateTimestamp = GetDate()
	from Inserted i 
	where ItemAttribute.ItemAttribute_Id = i.ItemAttribute_Id

    SELECT @Error_No = @@ERROR
  
 

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemAttributeAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMAReports]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAttribute] TO [BizTalk]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemAttribute] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemAttribute] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAttribute] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[ItemAttribute] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemAttribute] TO [IRMAPDXExtractRole]
    AS [dbo];

