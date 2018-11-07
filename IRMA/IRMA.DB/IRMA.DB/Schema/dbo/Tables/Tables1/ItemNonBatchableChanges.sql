CREATE TABLE [dbo].[ItemNonBatchableChanges] (
    [ItemNonBatchableChangesID] INT          IDENTITY (1, 1) NOT NULL,
    [Item_Key]                  INT          NOT NULL,
    [POS_Description]           VARCHAR (26) NULL,
    [Food_Stamps]               BIT          NULL,
    [TaxClassID]                INT          NULL,
    [StartDate]                 DATETIME     CONSTRAINT [DF_ItemNonBatchableChanges_StartDate] DEFAULT (CONVERT([date],getdate(),0)) NOT NULL,
    [InsertDate]                DATETIME     CONSTRAINT [DF_ItemNonBatchableChanges_InsertDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ItemNonBatchableChangesID] PRIMARY KEY CLUSTERED ([ItemNonBatchableChangesID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemNonBatchableChanges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemNonBatchableChanges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemNonBatchableChanges] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemNonBatchableChanges] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemNonBatchableChanges] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemNonBatchableChanges] TO [IRMASchedJobsRole]
    AS [dbo];

