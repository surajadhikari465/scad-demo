CREATE PROCEDURE dbo.UpdateVCAI_Exception
@VCAI_ExID int,
@UserNewCost money,
@UserNewPack int,
@NewStartDate smalldatetime,
@NewEndDate Smalldatetime,
@VCH_ID int,
@ExStatus int,
@UserID int
AS
BEGIN
    SET NOCOUNT ON
    
    update VendorCostHistoryExceptions
    set UserCase_Size = isnull(@UserNewPack, UserCase_Size),
        UserUnit_Price = isnull(@UserNewCost, UserUnit_Price),
        UserStart_Date = isnull(@newStartDate, UserStart_Date),
        UserEnd_Date = isnull(@newEndDate, UserEnd_Date),
        VCH_ID = isnull(@VCH_ID, VCH_ID),
        ExStatus = @ExStatus,
        User_ID = @UserID,
        LastModified = getdate()
    where VendorCostHistoryExceptions.VCAI_ExID = @VCAI_ExID        
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVCAI_Exception] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVCAI_Exception] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVCAI_Exception] TO [IRMAReportsRole]
    AS [dbo];

