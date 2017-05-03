SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVCAI_Exceptions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVCAI_Exceptions]
GO


CREATE PROCEDURE dbo.GetVCAI_Exceptions
@SubTeam_No int,
@ExRuleID int,
@Severity int,
@Vendor_ID int
AS
BEGIN
    SET NOCOUNT ON
    


select VCAI_Exception.VCAI_ExID, VCAI_Exception.status_flag as ChangeType, VCAI_Exception.Store_no, 
       VCAI_Exception.Sku_regional as Item_Key, VCAI_Exception.UPC as Identifier, VCAI_Exception.Vendor_ID,
       CAST((VCAI_Exception.Case_Price / VCAI_Exception.Case_Size) AS money) as NewUnitCost,
       VCAI_Exception.Case_Size as NewPackSize, VCAI_Exception.UserCase_Size as UserNewPackSize,
       VCAI_Exception.UserUnit_Price as UserNewUnitCost, VCAI_Exception.UserStart_Date, VCAI_Exception.UserEnd_Date,
       VCAI_Exception.OrigUnit_price as LastUnitCost, 0 as LastUnitFreight, VCAI_Exception.OrigCase_Size as LastPackSize,
       VCAI_Exception.Item_Description, VCAI_Exception.Start_Date, VCAI_Exception.End_Date, VCAI_Exception.ExDescription,
       VCAI_Exception.MSRP   
from VendorCostHistoryExceptions VCAI_Exception
where VCAI_Exception.ExRuleID = @ExRuleID
      AND VCAI_Exception.SubTeam_no = @SubTeam_no
      AND VCAI_Exception.ExSeverity = isnull(@Severity, VCAI_Exception.ExSeverity)
      AND VCAI_Exception.Vendor_ID = @Vendor_ID
      AND VCAI_Exception.ExStatus = 0
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


/*@SubTeam_No int,
@ExRuleID int,
@Severity int,
@Vendor_ID int

exec getvcai_exceptions 4200, 2, 1, 5039*/
--exec getvcai_exceptions 4200, 2, 1, 0
--exec GetVCAI_Exceptions 4200, 2, 1, 5039


