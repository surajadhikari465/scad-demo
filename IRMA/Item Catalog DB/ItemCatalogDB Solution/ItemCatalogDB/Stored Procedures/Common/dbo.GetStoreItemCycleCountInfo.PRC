IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'GetStoreItemCycleCountInfo' 
	   AND 	  type = 'P')
    DROP PROCEDURE GetStoreItemCycleCountInfo
GO

CREATE PROCEDURE dbo.GetStoreItemCycleCountInfo 
	@Store_No int,
    @SubTeam_No int,
    @InvLocID int,
    @Item_Key int,
    @Count decimal(18,4) OUTPUT,
    @Weight decimal(18,4) OUTPUT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SET @error_no = 0

    SELECT @Count = SUM([Count]), @Weight = SUM(Weight)
    FROM CycleCountHistory D (nolock)
    INNER JOIN
        CycleCountItems I (nolock)
        ON I.CycleCountItemID = D.CycleCountItemID
    INNER JOIN
        CycleCountHeader H (nolock)
        ON H.CycleCountID = I.CycleCountID
    INNER JOIN
        CycleCountMaster M (nolock)
        ON M.MasterCountID = H.MasterCountID
    WHERE ISNULL(H.InvLocID, 0) = ISNULL(@InvLocID, 0)
        AND H.ClosedDate IS NULL
        AND M.Store_No = @Store_No
        AND M.SubTeam_No = @SubTeam_No
        AND I.Item_Key = @Item_Key

    SELECT @error_no = @@ERROR

    SELECT @Count = ISNULL(@Count, 0), @Weight = ISNULL(@Weight, 0)

    IF @error_no <> 0
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('GetStoreItemCycleCountInfo failed with @@ERROR: %d', @Severity, 1, @error_no)
    END

    SET NOCOUNT OFF
END

GO

