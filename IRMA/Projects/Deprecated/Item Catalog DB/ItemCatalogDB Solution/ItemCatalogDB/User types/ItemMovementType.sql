CREATE TYPE [dbo].[ItemMovementType] AS TABLE (							
[BusinessUnitID]		INT,										
[Identifier]			VARCHAR(13),								
[TransDate]				DATETIME,
[Quantity]				INT,
[ItemVoid]				BIT,
[ItemType]				INT,
[BasePrice]				DECIMAL(9,2),
[Weight]				DECIMAL(10,3),
[MarkDownAmount]		DECIMAL(9,2)
)
GO