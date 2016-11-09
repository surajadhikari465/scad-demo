CREATE PROCEDURE [dbo].[NumOfInvoicesMatchedInControlGroupReport]
@ControlGroup_ID int,
@PoNumber_Id int,
@BeginDate DateTime,
@EndDate DateTime
AS
BEGIN
   SET NOCOUNT ON
	    Select count(*) as 'MatchedCount'
        from fn_3WayMatchDetails(@ControlGroup_ID,@PoNumber_Id,@BeginDate,@EndDate) as MD
        where MD.MatchingValidationCode <> 500
        SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[NumOfInvoicesMatchedInControlGroupReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[NumOfInvoicesMatchedInControlGroupReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[NumOfInvoicesMatchedInControlGroupReport] TO [IRMAReportsRole]
    AS [dbo];

