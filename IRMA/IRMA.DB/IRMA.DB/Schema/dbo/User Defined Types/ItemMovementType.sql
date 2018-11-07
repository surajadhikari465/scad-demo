CREATE TYPE [dbo].[ItemMovementType] AS TABLE (
    [BusinessUnitID] INT             NULL,
    [Identifier]     VARCHAR (13)    NULL,
    [TransDate]      DATETIME        NULL,
    [Quantity]       INT             NULL,
    [ItemVoid]       BIT             NULL,
    [ItemType]       INT             NULL,
    [BasePrice]      DECIMAL (9, 2)  NULL,
    [Weight]         DECIMAL (10, 3) NULL,
    [MarkDownAmount] DECIMAL (9, 2)  NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[ItemMovementType] TO [IRMAClientRole];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[ItemMovementType] TO [IRSUser];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[ItemMovementType] TO [IConInterface];

