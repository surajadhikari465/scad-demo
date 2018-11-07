CREATE PROCEDURE dbo.GetInstanceDataFlagOverrideStoreCounts
    @FlagKey varchar(50)
AS
BEGIN
    SET NOCOUNT ON

   SELECT 
        FlagKey,
        RegionalFlagValue,
        NumStoresWithOverrides,
        NumStoresTotal
    FROM dbo.IDF_OverrideStoreCountView 
    WHERE FlagKey = IsNull(@FlagKey, FlagKey)
    
    SET NOCOUNT OFF
END

GO
GRANT EXECUTE  ON OBJECT::dbo.GetInstanceDataFlagOverrideStoreCounts
    TO IRMAAdminRole AS dbo;
GO
GRANT EXECUTE ON OBJECT::dbo.GetInstanceDataFlagOverrideStoreCounts
    TO IRMASupportRole AS dbo;
GO
GRANT EXECUTE ON OBJECT::dbo.GetInstanceDataFlagOverrideStoreCounts
    TO IRMAClientRole AS dbo;
GO
GRANT EXECUTE ON OBJECT::dbo.GetInstanceDataFlagOverrideStoreCounts
    TO IRMASchedJobsRole AS dbo;