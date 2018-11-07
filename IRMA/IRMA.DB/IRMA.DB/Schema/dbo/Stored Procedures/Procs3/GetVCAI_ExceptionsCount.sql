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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVCAI_ExceptionsCount] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVCAI_ExceptionsCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVCAI_ExceptionsCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVCAI_ExceptionsCount] TO [IRMAReportsRole]
    AS [dbo];

