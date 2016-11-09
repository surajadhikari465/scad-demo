﻿CREATE TABLE [dbo].[Sales_SumByItemWkly] (
    [Date_Key]            SMALLDATETIME   NOT NULL,
    [Store_No]            INT             NOT NULL,
    [Item_Key]            INT             NOT NULL,
    [SubTeam_No]          INT             NOT NULL,
    [Price_Level]         TINYINT         NOT NULL,
    [Sales_Quantity]      INT             NULL,
    [Return_Quantity]     INT             NULL,
    [Weight]              DECIMAL (10, 3) NULL,
    [Sales_Amount]        DECIMAL (9, 2)  NULL,
    [Return_Amount]       DECIMAL (9, 2)  NULL,
    [Markdown_Amount]     DECIMAL (9, 2)  NULL,
    [Promotion_Amount]    DECIMAL (9, 2)  NULL,
    [Store_Coupon_Amount] DECIMAL (9, 2)  NULL,
    [Load_Dt]             SMALLDATETIME   NULL,
    CONSTRAINT [PK_Sales_SumByItemWkly] PRIMARY KEY CLUSTERED ([Date_Key] ASC, [Store_No] ASC, [Item_Key] ASC, [SubTeam_No] ASC, [Price_Level] ASC),
    CONSTRAINT [FK__Sales_Sum__Store__1943E849_Wkly] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_Sales_SumByItem_Date_Wkly] FOREIGN KEY ([Date_Key]) REFERENCES [dbo].[Date] ([Date_Key]),
    CONSTRAINT [FK_Sales_SumByItem_Item_Wkly] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_Sales_SumByItem_SubTeam_Wkly] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumByItemWkly] TO [IRMAReportsRole]
    AS [dbo];

