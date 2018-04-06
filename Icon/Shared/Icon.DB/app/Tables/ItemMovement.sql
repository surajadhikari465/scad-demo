CREATE TABLE [app].[ItemMovement] (
[ItemMovementID]			INT		IDENTITY(1,1)					NOT NULL,
[ESBMessageID]				VARCHAR(255)								NOT NULL,
[BusinessUnitID]			INT										NOT NULL,
[RegisterNumber]			INT										NOT NULL,
[TransactionSequenceNumber]	INT										NOT NULL,
[LineItemNumber]			INT										NOT NULL,
[Identifier]				VARCHAR(13)								NOT NULL,
[TransDate]					DATETIME2(7)							NOT NULL,
[Quantity]					INT										NULL,
[ItemVoid]					BIT										NULL,
[ItemType]					INT										NOT NULL,
[BasePrice]					DECIMAL(9,2)							NOT NULL,
[Weight]					DECIMAL(10,3)							NULL,
[MarkDownAmount]			DECIMAL(9,2)							NULL,
[InsertDate]				DATETIME2(7)	DEFAULT(SYSDATETIME())	NOT NULL,
[InProcessBy]				VARCHAR(30)								NULL,
[ProcessFailedDate]			DATETIME2(7)							NULL,
CONSTRAINT [PK_ItemMovementID] PRIMARY KEY CLUSTERED ([ItemMovementID] ASC) WITH (FILLFACTOR = 80)
)
GO

CREATE NONCLUSTERED INDEX [IX_TransactionNumber] ON app.ItemMovement (BusinessUnitID, RegisterNumber, TransDate, TransactionSequenceNumber)
GO 

CREATE NONCLUSTERED INDEX [IX_SequenceNumber] ON app.ItemMovement (BusinessUnitID, RegisterNumber, TransDate, TransactionSequenceNumber, LineItemNumber)
GO

CREATE NONCLUSTERED INDEX [IX_InProcessBy] ON app.ItemMovement (InProcessBy)
GO

CREATE NONCLUSTERED INDEX [IX_ItemMovement_ESBMessageID] ON [app].[ItemMovement] ([ESBMessageID] ASC)
	INCLUDE ([BusinessUnitID], [RegisterNumber], [TransactionSequenceNumber], [TransDate])
GO