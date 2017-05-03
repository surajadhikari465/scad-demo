/****** Object:  StoredProcedure [dbo].[Reporting_GetOrderTransferToSubTeams]    Script Date: 01/15/2009 14:03:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reporting_GetOrderTransferToSubTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetOrderTransferToSubTeams]
GO
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