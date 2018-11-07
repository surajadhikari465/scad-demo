CREATE TYPE [dbo].[TlogReprocessRequestType] AS TABLE (
    [BusinessUnit_ID] INT           NULL,
    [TransactionDate] SMALLDATETIME NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[TlogReprocessRequestType] TO [IRMAClientRole];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[TlogReprocessRequestType] TO [IRSUser];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[TlogReprocessRequestType] TO [IConInterface];

