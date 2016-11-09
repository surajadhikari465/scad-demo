CREATE TABLE [app].[ItemMovementTransactionHistory] (
    [TransactionSequenceNumber] INT			NOT NULL,
    [BusinessUnitID]			INT         NOT NULL,
    [TransDate]					DATETIME    NOT NULL,
    [RegisterNumber]			INT         NOT NULL,
    [InsertDate]				DATETIME2 (7) DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_ItemMovementTransactionNumber] PRIMARY KEY CLUSTERED ([TransactionSequenceNumber] ASC, [BusinessUnitID] ASC, [TransDate] ASC, [RegisterNumber] ASC) WITH (FILLFACTOR = 80)
);