CREATE TABLE [dbo].[TempLastVendor] (
    [Item_Key]       INT           NOT NULL,
    [Store_No]       INT           NOT NULL,
    [OrderHeader_ID] INT           NOT NULL,
    [DateReceived]   SMALLDATETIME NULL,
    [BuyingCycle]    INT           NULL,
    [Transfer_Order] INT           CONSTRAINT [DF__TempLastV__Trans__3573B09E] DEFAULT ((0)) NOT NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[TempLastVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempLastVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempLastVendor] TO [IRMAReportsRole]
    AS [dbo];

