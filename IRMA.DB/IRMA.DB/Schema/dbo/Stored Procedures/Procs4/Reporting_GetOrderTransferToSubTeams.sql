-- =============================================
-- Author:		Dave Stacey
-- Create date: 01/15/2009
-- Description:	SP used for parameter list for reports
-- =============================================
CREATE PROCEDURE [dbo].[Reporting_GetOrderTransferToSubTeams]
AS
BEGIN

	Select ST.SubTeam_No, ST.SubTeam_Name 
	FROM dbo.Subteam ST
		JOIN dbo.OrderHeader AS OH ON OH.Transfer_To_SubTeam = ST.SubTeam_No
	GROUP BY ST.SubTeam_No, ST.SubTeam_Name  
	ORDER BY ST.SubTeam_No, ST.SubTeam_Name  

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetOrderTransferToSubTeams] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetOrderTransferToSubTeams] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetOrderTransferToSubTeams] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetOrderTransferToSubTeams] TO [IRMAReportsRole]
    AS [dbo];

