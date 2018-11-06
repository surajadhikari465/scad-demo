/****** Object:  StoredProcedure [dbo].[Reporting_SpecialsByDateRange]    Script Date: 11/08/2006 09:37:00 ******/
CREATE PROCEDURE dbo.[Reporting_SpecialsByDateRange]
	@StoreNo int,
	@TeamNo int,
	@SubTeamNo int,
	@StartDate smalldatetime,
	@EndDate smalldatetime,
	@UseEndDate smallint,		-- sale end (or start) date falls within the date range
	@IncludeOngoing smallint	-- include all specials that occur during the date range
AS

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	--------------------------------
	-- return the results
	--------------------------------
	SELECT II.Identifier, 
		I.Item_Description, 
		P.Multiple, 
		P.POSPrice AS [Price], 
		P.Sale_Multiple, 
		P.POSSale_Price AS [Sale_Price], 
		P.Sale_Start_Date, 
		P.Sale_End_Date, 
		T.Team_Name,
		ST.SubTeam_Name
	FROM Item I (NOLOCK) 
		INNER JOIN ItemIdentifier II (NOLOCK) ON (II.Item_Key = I.Item_Key AND II.Default_Identifier = 1)
		INNER JOIN Price P (NOLOCK) ON (I.Item_Key = P.Item_Key) 
		INNER JOIN SubTeam ST (NOLOCK) ON (I.SubTeam_No = ST.SubTeam_No)
		INNER JOIN Team T (NOLOCK) ON (T.Team_No = ST.Team_No)
	WHERE P.Store_No = ISNULL(@StoreNo, P.Store_No) 
		AND I.SubTeam_No = ISNULL(@SubTeamNo, I.SubTeam_No) 
		AND ST.Team_No = ISNULL(@TeamNo, ST.Team_No) 
		AND ((CASE WHEN ISNULL(@UseEndDate, 1) = 1 THEN P.Sale_End_Date ELSE P.Sale_Start_Date END >= @StartDate 
				AND CASE WHEN ISNULL(@UseEndDate, 1) = 1 THEN P.Sale_End_Date ELSE P.Sale_Start_Date END <= @EndDate)
			OR (@IncludeOngoing = 1 AND P.Sale_Start_Date <= @EndDate AND P.Sale_End_Date >= @StartDate))
	ORDER BY T.Team_Name, ST.SubTeam_Name, CAST(II.Identifier AS bigint)

	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_SpecialsByDateRange] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_SpecialsByDateRange] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_SpecialsByDateRange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_SpecialsByDateRange] TO [IRMAReportsRole]
    AS [dbo];

