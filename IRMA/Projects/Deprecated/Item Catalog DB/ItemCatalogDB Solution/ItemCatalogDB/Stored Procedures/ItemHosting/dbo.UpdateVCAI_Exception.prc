SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateVCAI_Exception]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateVCAI_Exception]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


--Exec UpdateVCAI_Exception 4, 1, null, null, null, null, 0, 350