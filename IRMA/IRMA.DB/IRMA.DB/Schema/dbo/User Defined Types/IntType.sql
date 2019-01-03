CREATE TYPE [dbo].[IntType] AS TABLE (
    [Key] INT NOT NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IntType] TO [IRMAClientRole];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IntType] TO [IRSUser];

GO
GRANT EXECUTE
    ON TYPE::[dbo].[IntType] TO [MammothRole];
