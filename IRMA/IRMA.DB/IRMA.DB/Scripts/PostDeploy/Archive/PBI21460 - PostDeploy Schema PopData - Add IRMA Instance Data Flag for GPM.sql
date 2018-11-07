DECLARE @flagKey VARCHAR(50) = 'GlobalPriceManagement';

IF NOT EXISTS ( SELECT 1 FROM [dbo].[InstanceDataFlags] WHERE FlagKey = @flagKey)
BEGIN        
    -- Global Price Manangement (GPM) is out of scope for non-US 
    --  jurisdictions. So for regions with stores in both the USA
    --  and Canada the GPM flag should be overridable per store.
    -- (The UK region is also out of scope for GPM but has no US stores
    --  so the flag value will always be 0 & no need for overrides.)

    --determine region
    DECLARE @regionCode nvarchar(2);
    SELECT TOP 1 @regionCode = RegionCode from [dbo].[Region];

    IF (@regionCode = 'MW' OR @regionCode = 'PN')
    BEGIN
        -- for an international region, allow override by store
        INSERT INTO [dbo].[InstanceDataFlags]
            (FlagKey, FlagValue, CanStoreOverride)
        SELECT @flagKey, 0, 1;

        --set the non-US stores to never use GPM even if active for the rest of the region
        INSERT INTO [dbo].[InstanceDataFlagsStoreOverride]
            (FlagKey, FlagValue, Store_No)
        SELECT @flagKey, 0, s.Store_No
        FROM [dbo].[Store] s
            INNER JOIN [dbo].[StoreJurisdiction] j ON j.[StoreJurisdictionID] = s.[StoreJurisdictionID]
        WHERE j.[StoreJurisdictionDesc] <> 'US';
    END
    ELSE
    BEGIN
        --not an international region, just need the default value
        INSERT INTO [dbo].[InstanceDataFlags]
            (FlagKey, FlagValue, CanStoreOverride)
        SELECT @flagKey, 0, 0;
    END;
END;