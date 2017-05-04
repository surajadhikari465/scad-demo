CREATE TYPE [infor].[NewItemEventType] AS TABLE (
    [QueueId] INT NULL);


GO
GRANT EXECUTE
    ON TYPE::[infor].[NewItemEventType] TO [IConInterface];

