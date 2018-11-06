CREATE PROCEDURE dbo.FlagHasNoStoreOverrides
    @FlagKey varchar(50)
AS
BEGIN
    DECLARE @FlagHasNoStoreOverrides BIT

    SET NOCOUNT ON

    IF EXISTS (
        SELECT 1
        FROM InstanceDataFlags IDF
        WHERE IDF.FlagKey = @FlagKey 
            AND NOT EXISTS (
                SELECT 1 
                FROM InstanceDataFlagsStoreOverride 
                WHERE FlagKey = @FlagKey AND FlagValue <> IDF.FlagValue
            )
    )

        SET @FlagHasNoStoreOverrides = 1
    ELSE
        SET @FlagHasNoStoreOverrides = 0

    SET NOCOUNT OFF

    SELECT @FlagHasNoStoreOverrides AS FlagHasNoStoreOverrides
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FlagHasNoStoreOverrides] TO [IRMAAdminRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FlagHasNoStoreOverrides] TO [IRMASupportRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FlagHasNoStoreOverrides] TO [IRMAClientRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FlagHasNoStoreOverrides] TO [IRMASchedJobsRole]
    AS [dbo];