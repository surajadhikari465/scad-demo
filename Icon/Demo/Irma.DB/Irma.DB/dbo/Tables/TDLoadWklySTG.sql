CREATE TABLE [dbo].[TDLoadWklySTG] (
    [wk_end_dt]      SMALLDATETIME  NOT NULL,
    [ps_bu]          VARCHAR (7)    NULL,
    [Store_No]       INT            NULL,
    [upc]            VARCHAR (13)   NULL,
    [Item_Key]       INT            NULL,
    [SubTeam_No]     INT            NOT NULL,
    [Sales_Quantity] INT            NULL,
    [Weight]         DECIMAL (9, 3) NULL,
    [Sales_Amount]   DECIMAL (9, 2) NULL
);

