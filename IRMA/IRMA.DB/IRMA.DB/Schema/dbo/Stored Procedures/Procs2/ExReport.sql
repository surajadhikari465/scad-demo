CREATE PROCEDURE dbo.ExReport
@ExRuleID INT, 
@SubTeam_No INT, 
@Team_No INT, 
@Severity INT,
@VendorID INT,
@Status INT,
@BeginDate varchar(20),
@EndDate varchar(20)

AS
--EXEC ExReport null, null, null, null, 5832, null, null, null

DECLARE @BeginDateDt SMALLDATETIME, @EndDateDt SMALLDATETIME
SELECT @BeginDateDt = CONVERT(SMALLDATETIME, isnull(@BeginDate, '1900-01-01')), @EndDateDt = CONVERT(SMALLDATETIME, isnull(@EndDate,'2079-06-06'))

--SELECT @BeginDateDt, @EndDateDt


select VCHE.ExStatus, VCHE.ExSeverity, VCHE.ExRuleID, SubTeam.SubTeam_Name, Team.Team_Name,
       VCHE.UPC as ItemIdentifier, VCHE.Item_Description, 
       isnull(VCHE.UserCase_Size, VCHE.Case_Size) as NewPackSize,
       VCHE.OrigCase_Size as OrigPackSize,
       isnull(VCHE.UserUnit_Price, VCHE.Case_Price / Case_Size) as NewUnitCost,
       VCHE.OrigUnit_price as OrigUnitCost,
       isnull(VCHE.UserStart_Date,VCHE.Start_Date) as Start_Date,
       isnull(VCHE.UserEnd_Date,VCHE.End_Date) as End_Date,
       VCHE.InsertDate,
       VCHE.LastModified,
       Users.UserName,
       VCHE.ExDescription
              
from VendorCostHistoryExceptions VCHE
    inner join
        SubTeam
        on SubTeam.SubTeam_no = VCHE.SubTeam_No
    inner join    
        Team
        on SubTeam.Team_no = Team.Team_No
    left join 
        users
        on VCHE.User_ID = Users.User_ID
where VCHE.ExRuleID = isnull(@ExRuleID, VCHE.ExRuleID)
      and VCHE.SubTeam_no = case when @SubTeam_No = -9999 then VCHE.SubTeam_No else @SubTeam_No end
      and Team.Team_No = case when @Team_No = -9999 then Team.Team_no else @Team_No end
      and VCHE.ExSeverity = isnull(@Severity, VCHE.ExSeverity)
      and VCHE.Vendor_ID = @VendorID
      and VCHE.ExStatus = isnull(@status,VCHE.ExStatus)
      and VCHE.InsertDate >= @BeginDateDt AND VCHE.InsertDate <= @EndDateDt
order by VCHE.subteam_no
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ExReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ExReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ExReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ExReport] TO [IRMAReportsRole]
    AS [dbo];

