CREATE TABLE [app].[ItemMovementErrorQueue] (
    [ItemMovementErrorQueueID]  INT             IDENTITY (1, 1) NOT NULL,
    [ESBMessageID]              VARCHAR (255)   NOT NULL,
    [BusinessUnitID]            INT             NOT NULL,
    [RegisterNumber]            INT             NOT NULL,
    [TransactionSequenceNumber] INT             NOT NULL,
    [LineItemNumber]            INT             NOT NULL,
    [Identifier]                VARCHAR (13)    NOT NULL,
    [TransDate]                 DATETIME2 (7)   NOT NULL,
    [Quantity]                  INT             NULL,
    [ItemVoid]                  BIT             NULL,
    [ItemType]                  INT             NOT NULL,
    [BasePrice]                 DECIMAL (9, 2)  NOT NULL,
    [Weight]                    DECIMAL (10, 3) NULL,
    [MarkDownAmount]            DECIMAL (9, 2)  NULL,
    [InsertDate]                DATETIME2 (7)   NOT NULL,
    [InProcessBy]               VARCHAR (30)    NULL,
    [ProcessFailedDate]         DATETIME2 (7)   NULL
);


