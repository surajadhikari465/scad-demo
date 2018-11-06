CREATE TABLE [dbo].[ItemShelfLife] (
    [ShelfLife_ID]   INT          IDENTITY (1, 1) NOT NULL,
    [ShelfLife_Name] VARCHAR (25) NOT NULL,
    [User_ID]        INT          NULL,
    CONSTRAINT [PK_ItemShelfLife_ShelfLife_ID] PRIMARY KEY CLUSTERED ([ShelfLife_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemShelfLife_1__13] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxItemShelfLifeName]
    ON [dbo].[ItemShelfLife]([ShelfLife_Name] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemShelfLifeUserID]
    ON [dbo].[ItemShelfLife]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemShelfLife] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemShelfLife] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemShelfLife] TO [IRMAReportsRole]
    AS [dbo];

