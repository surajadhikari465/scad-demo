CREATE PROCEDURE [dbo].[SubTeams_GetAllSubteams]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	 [SubTeam_No] AS SubTeamNo
	,[Team_No] AS TeamNo
	,[SubTeam_Name] AS SubTeamName
	,[SubTeam_Abbreviation] AS SubTeamAbbreviation
	,[Dept_No] AS DeptNo
	,[SubDept_No] AS SubDeptNo
	,[Buyer_User_ID] AS BuyerUserID
	,[Target_Margin] AS TargetMargin
	,[JDA]
	,[GLPurchaseAcct]
	,[GLDistributionAcct]
	,[GLTransferAcct]
	,[GLSalesAcct]
	,[Transfer_To_Markup] AS TransferToMarkup
	,[EXEWarehouseSent]
	,[ScaleDept]
	,[Retail]
	,[EXEDistributed]
	,[SubTeamType_ID] AS SubTeamType_ID 
	,[PurchaseThresholdCouponAvailable]
	,[GLSuppliesAcct]
	,[GLPackagingAcct]
	,[FixedSpoilage]
	,[InventoryCountByCase]
	,[Beverage]
	,[AlignedSubTeam]
	,[IsDisabled]
	,CASE SubTeamType_ID
		WHEN 1 THEN (RTRIM(SubTeam_Name) + ' (RET)')
		WHEN 2 THEN (RTRIM(SubTeam_Name) + ' (MFG)')
		WHEN 3 THEN (RTRIM(SubTeam_Name) + ' (RET/MFG)')
		WHEN 4 THEN (RTRIM(SubTeam_Name) + ' (EXP)')
		WHEN 5 THEN (RTRIM(SubTeam_Name) + ' (PKG)')
		WHEN 6 THEN (RTRIM(SubTeam_Name) + ' (SUP)')
		END AS SubTeamDescription
FROM dbo.SubTeam
ORDER BY SubTeam_Name
END
GO

GRANT EXECUTE ON OBJECT::[dbo].[SubTeams_GetAllSubteams] TO [IRMAAdminRole] AS [dbo];
GO

GRANT EXECUTE ON OBJECT::[dbo].[SubTeams_GetAllSubteams] TO [IRMASupportRole] AS [dbo];
GO

GRANT EXECUTE ON OBJECT::[dbo].[SubTeams_GetAllSubteams] TO [IRMAClientRole] AS [dbo];
GO