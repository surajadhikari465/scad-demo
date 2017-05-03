CREATE TABLE [dbo].[AICSErrors] (
    [ASTORE_NUMBER]      VARCHAR (255) NULL,
    [ASKU]               VARCHAR (255) NULL,
    [BUNITS]             VARCHAR (255) NULL,
    [AQUANTITY2]         VARCHAR (255) NULL,
    [BPRICE]             VARCHAR (255) NULL,
    [BEXTENDED_PRICE]    VARCHAR (255) NULL,
    [BEXTENDED_QUANTITY] VARCHAR (255) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[AICSErrors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AICSErrors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AICSErrors] TO [IRMAReportsRole]
    AS [dbo];

