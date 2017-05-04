CREATE TYPE [dbo].[IconItemChangeQueueIdType] AS TABLE (
    [QID] INT NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IconItemChangeQueueIdType] TO [IConInterface];

