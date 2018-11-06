CREATE TABLE [dbo].[Sales_SumBySubDept] (
    [Date_Key]            SMALLDATETIME  NOT NULL,
    [Store_No]            INT            NOT NULL,
    [SubTeam_No]          INT            NOT NULL,
    [Sales_Quantity]      INT            NULL,
    [Return_Quantity]     INT            NULL,
    [Sales_Amount]        DECIMAL (9, 2) NULL,
    [Return_Amount]       DECIMAL (9, 2) NULL,
    [Markdown_Amount]     DECIMAL (9, 2) NULL,
    [Promotion_Amount]    DECIMAL (9, 2) NULL,
    [Store_Coupon_Amount] DECIMAL (9, 2) NULL,
    CONSTRAINT [PK_Sales_SumBySubDept] PRIMARY KEY CLUSTERED ([Date_Key] ASC, [Store_No] ASC, [SubTeam_No] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__Sales_Sum__Store__147F332C] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_Sales_SumBySubDept_Date] FOREIGN KEY ([Date_Key]) REFERENCES [dbo].[Date] ([Date_Key]),
    CONSTRAINT [FK_Sales_SumBySubDept_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumBySubDept] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumBySubDept] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales_SumBySubDept] TO [IRMAReportsRole]
    AS [dbo];

