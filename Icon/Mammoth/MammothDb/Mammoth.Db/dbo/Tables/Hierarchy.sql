CREATE TABLE [dbo].[Hierarchy] (
    [hierarchyID]   INT            NOT NULL,
    [hierarchyName] NVARCHAR (255) NOT NULL,
    [AddedDate]     DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]  DATETIME       NULL,
    CONSTRAINT [PK_Hierarchy] PRIMARY KEY CLUSTERED ([hierarchyID] ASC) WITH (FILLFACTOR = 100)
);



