CREATE PROCEDURE [dbo].[ApUpClosedReport]
	@lReportIndex	int,
	@sBegin_Date	datetime,
	@sEnd_Date		datetime,
	@sStore_No		int,
	@optSelected	varchar(100)
AS 
-- ****************************************************************************************************************
-- Procedure: ApUpClosedReport
--    Author: Sekhara
--      Date: 2007/10/26
--
-- Description:
-- This procedure is called from a single RDL file and generates a report consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 12/26/2011	KM		3744	Added update history template; coding standards;
-- 09/12/2013   MZ		13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- ****************************************************************************************************************
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

 IF @lReportIndex = 0 
        EXEC GetAPUpAppDailySum @sBegin_Date, @sEnd_Date, @sStore_No
 ELSE IF @lReportIndex = 1
       EXEC GetAPUpExceptions @sBegin_Date, @sEnd_Date, @sStore_No
 ELSE IF @lReportIndex = 2
       EXEC GetAPUpNotApproved @sBegin_Date, @sEnd_Date, @sStore_No
 ELSE IF @lReportIndex = 3
       EXEC GetAPUpNoPSInfo @sBegin_Date, @sEnd_Date, @sStore_No
 ELSE IF @lReportIndex = 4
       EXEC GetAPUpNoInvoice @sBegin_Date, @sEnd_Date, @sStore_No
 ELSE IF @lReportIndex = 5
       EXEC GetAPUpUploaded @sBegin_Date, @sEnd_Date, @sStore_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ApUpClosedReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ApUpClosedReport] TO [IRMAReportsRole]
    AS [dbo];

