CREATE TYPE [dbo].[NoTagSubteamOverrideType] AS TABLE (
    [SubteamNumber]  INT NOT NULL,
    [ThresholdValue] INT NOT NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[NoTagSubteamOverrideType] TO [IRSUser];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[NoTagSubteamOverrideType] TO [IRMAClientRole];

