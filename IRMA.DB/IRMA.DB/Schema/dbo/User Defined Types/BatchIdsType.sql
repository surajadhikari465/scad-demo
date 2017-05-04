CREATE TYPE [dbo].[BatchIdsType] AS TABLE (
    [PriceBatchHeaderId] INT NULL,
    [BatchID]            INT NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[BatchIdsType] TO [IRMAClientRole];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[BatchIdsType] TO [IRSUser];

