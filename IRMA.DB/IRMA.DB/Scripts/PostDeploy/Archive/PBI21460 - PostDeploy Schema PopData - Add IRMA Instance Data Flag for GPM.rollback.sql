-- script to roll back the addition of the GlobalPriceManagement IDF in IRMA
DECLARE @flagKey VARCHAR(50)='GlobalPriceManagement';
 
IF EXISTS (
    SELECT 1 
    FROM [dbo].[InstanceDataFlagsStoreOverride] 
    WHERE FlagKey = @flagKey)
BEGIN
        DELETE 
        FROM [dbo].[InstanceDataFlagsStoreOverride] 
        WHERE [FlagKey] = @flagKey;
END;

IF EXISTS (
    SELECT 1 
    FROM [dbo].[InstanceDataFlags] 
    WHERE FlagKey = @flagKey)
BEGIN
    DELETE 
    FROM [dbo].[InstanceDataFlags] 
    WHERE [FlagKey] = @flagKey;
END;
