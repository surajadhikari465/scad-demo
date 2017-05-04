-- =============================================
-- Author:		Faisal Ahmed
-- Create date: 03/22/2013
-- Description:	This procedure return the fiscal year, period, and week for a given day
-- =============================================
CREATE PROCEDURE [dbo].[GetFiscalCalendarInfo] 
	-- Add the parameters for the stored procedure here
	@myDate as smalldatetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		Period as FiscalPeriod,
		Week as FiscalWeek,
		Year as FiscalYear,
		Quarter as FiscalQuarter,
		Day_Name,
		Day_Of_Week,
		Day_Of_Month,
		Day_Of_Year
	FROM
		Date
	WHERE
		Date_Key = @myDate

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFiscalCalendarInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFiscalCalendarInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFiscalCalendarInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFiscalCalendarInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFiscalCalendarInfo] TO [IRMAReportsRole]
    AS [dbo];

