CREATE TABLE [dbo].[WOODRUFF_UNFI] (
    [UPC#]   NVARCHAR (255) NULL,
    [DESC]   NVARCHAR (255) NULL,
    [F3]     FLOAT (53)     NULL,
    [F4]     NVARCHAR (255) NULL,
    [F5]     NVARCHAR (255) NULL,
    [F6]     NVARCHAR (255) NULL,
    [VENDOR] NVARCHAR (255) NULL,
    [F8]     NVARCHAR (255) NULL,
    [QTY]    FLOAT (53)     NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[WOODRUFF_UNFI] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[WOODRUFF_UNFI] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[WOODRUFF_UNFI] TO [IRMAReportsRole]
    AS [dbo];

