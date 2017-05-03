CREATE TYPE [dbo].[IconPosPublishEventIdType] AS TABLE (
    [IconPosPushPublishId] INT NOT NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IconPosPublishEventIdType] TO [IConInterface];

