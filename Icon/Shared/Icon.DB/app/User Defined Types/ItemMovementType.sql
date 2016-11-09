CREATE TYPE [app].[ItemMovementType] AS TABLE (
    [ESBMessageID]		VARCHAR(255)		NULL,
    [BusinessUnitID]    INT             NULL,
    [RegisterNumber]	INT				NULL,
    [TransactionSequenceNumber]	INT		NULL,
    [LineItemNumber]	INT				NULL,
    [Identifier]        VARCHAR (13)    NULL,
    [TransDate]         DATETIME2 (7)   NULL,
    [Quantity]          INT             NULL,
    [ItemVoid]          BIT             NULL,
    [ItemType]          INT             NULL,
    [BasePrice]         DECIMAL (9, 2)  NULL,
    [Weight]            DECIMAL (10, 3) NULL,
    [MarkDownAmount]    DECIMAL (9, 2)  NULL);

