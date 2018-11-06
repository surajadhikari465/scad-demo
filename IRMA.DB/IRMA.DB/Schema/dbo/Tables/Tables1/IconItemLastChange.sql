CREATE TABLE [dbo].[IconItemLastChange] (
    [IconItemLastChangeId] INT            IDENTITY (1, 1) NOT NULL,
    [Identifier]           VARCHAR (13)   NULL,
    [Subteam_No]           INT            NULL,
    [Brand_ID]             INT            NULL,
    [Item_Description]     VARCHAR (60)   NULL,
    [POS_Description]      VARCHAR (26)   NULL,
    [Package_Desc1]        DECIMAL (9, 4) NULL,
    [Food_Stamps]          BIT            NULL,
    [ScaleTare]            DECIMAL (9, 4) NULL,
    [TaxClassID]           INT            NULL,
    [InsertDate]           DATETIME       DEFAULT (getdate()) NOT NULL,
    [AreNutriFactsChanged] BIT            NULL,
    [ClassID]              INT            NULL,
    [Package_Unit_ID]      INT            NULL,
    [Package_Desc2]        DECIMAL (9, 4) NULL,
    CONSTRAINT [PK_IconItemLastChange_IconItemLastChangeId] PRIMARY KEY CLUSTERED ([IconItemLastChangeId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [UQ_IconItemLastChange_Identifier] UNIQUE NONCLUSTERED ([Identifier] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[IconItemLastChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IconItemLastChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IconItemLastChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IconItemLastChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[IconItemLastChange] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IconItemLastChange] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IconItemLastChange] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IconItemLastChange] TO [IConInterface]
    AS [dbo];

