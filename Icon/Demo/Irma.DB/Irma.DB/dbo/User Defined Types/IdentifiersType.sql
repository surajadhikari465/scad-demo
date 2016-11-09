CREATE TYPE [dbo].[IdentifiersType] AS TABLE (
    [Identifier] NVARCHAR (13) NOT NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IdentifiersType] TO [MammothRole];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IdentifiersType] TO [IConInterface];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IdentifiersType] TO [IRSUser];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IdentifiersType] TO [IRMAClientRole];

