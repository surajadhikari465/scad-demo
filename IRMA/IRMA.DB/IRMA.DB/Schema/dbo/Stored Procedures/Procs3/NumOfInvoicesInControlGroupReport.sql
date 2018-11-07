CREATE PROCEDURE [dbo].[NumOfInvoicesInControlGroupReport]
@ControlGroup_ID int,
@PoNumber_Id int,
@BeginDate DateTime,
@EndDate DateTime
AS
BEGIN
SET NOCOUNT ON
	    Select count(*) as Count from fn_3WayMatchDetails(@ControlGroup_ID,@PoNumber_Id,@BeginDate,@EndDate)
SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[NumOfInvoicesInControlGroupReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[NumOfInvoicesInControlGroupReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[NumOfInvoicesInControlGroupReport] TO [IRMAReportsRole]
    AS [dbo];

