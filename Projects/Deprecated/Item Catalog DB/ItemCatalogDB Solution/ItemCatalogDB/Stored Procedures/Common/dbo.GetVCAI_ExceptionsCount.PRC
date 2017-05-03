SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVCAI_ExceptionsCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVCAI_ExceptionsCount]
GO


CREATE PROCEDURE dbo.GetVCAI_ExceptionsCount
@TeamList varchar(8000)

AS
BEGIN
    SET NOCOUNT ON
    select VCAI_EX.Vendor_ID, Vendor.CompanyName, Count(VCAI_EX.Vendor_ID) ExceptionCount
    from VendorCostHistoryExceptions VCAI_EX
        inner join
            SubTeam 
            on SubTeam.SubTeam_No = VCAI_EX.Subteam_No
        inner join
            dbo.fn_Parse_List(@TeamList, '|') TeamList
            ON SubTeam.team_no = TeamList.Key_Value
        inner join 
            vendor
            on vcai_ex.Vendor_ID = Vendor.vendor_ID
    where VCAI_EX.ExStatus = 0
    group by VCAI_EX.Vendor_ID, vendor.CompanyName

    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


