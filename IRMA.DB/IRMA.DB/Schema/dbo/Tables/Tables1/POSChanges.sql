CREATE TABLE [dbo].[POSChanges] (
    [DateStamp]   DATETIME CONSTRAINT [DF__POSChange__DateS__47FED732] DEFAULT (getdate()) NOT NULL,
    [Store_No]    INT      NOT NULL,
    [Sales_Date]  DATETIME NOT NULL,
    [Aggregated]  BIT      CONSTRAINT [DF__POSChange__Aggre__48F2FB6B] DEFAULT ((0)) NOT NULL,
    [GL_InQueue]  BIT      CONSTRAINT [DF_POSChanges_GL_InQueue] DEFAULT ((0)) NOT NULL,
    [GL_Pushed]   BIT      CONSTRAINT [DF__POSChange__GL_Pu__49E71FA4] DEFAULT ((0)) NOT NULL,
    [Modified_By] INT      NULL,
    CONSTRAINT [FK_POSChanges_Users] FOREIGN KEY ([Modified_By]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
CREATE NONCLUSTERED INDEX [idxPOSChanges]
    ON [dbo].[POSChanges]([DateStamp] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxAggregated]
    ON [dbo].[POSChanges]([Aggregated] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxGL_Pushed]
    ON [dbo].[POSChanges]([GL_Pushed] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxGL_InQueue]
    ON [dbo].[POSChanges]([GL_InQueue] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxSalesDate]
    ON [dbo].[POSChanges]([Sales_Date] ASC, [Store_No] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSChanges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSChanges] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSChanges] TO [IRMAReportsRole]
    AS [dbo];

