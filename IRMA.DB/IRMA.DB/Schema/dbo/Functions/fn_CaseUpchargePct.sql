CREATE FUNCTION [dbo].[fn_CaseUpchargePct] 
	(@SubTeam_No int)
RETURNS decimal
AS

BEGIN
      DECLARE @CaseUpchargePct decimal(9,4)
 
--    SELECT @CaseUpchargePct = Distribution_Markup 
--    FROM ZoneSupply (NOLOCK) 
--    WHERE SubTeam_No = @SubTeam_No AND
--          FromZone_ID = (SELECT Zone_ID FROM Store (NOLOCK) WHERE Store_No = @TeamNo) AND
--          ToZone_ID = (SELECT Zone_ID FROM Store (NOLOCK) WHERE Store_No = @BusUnit)

	-- By business rules (per Lawrence Priest 1/24/08) the CaseUpchargePct (Distribution_Markup)
	-- will be one amount across the region, i.e. the same for all zones, 
	-- but it is unknown at this time what (or how many) records there will
	-- be in the ZoneSupply table. So taking the MAX() (or MIN() for that matter) of Distribution_Markup
	-- will still give the correct result per these business rules.
	--			Rick Kelleher

	SELECT @CaseUpchargePct=MAX(Distribution_Markup)
		FROM ZoneSupply (NOLOCK) 
		WHERE SubTeam_No = Isnull(@SubTeam_No,ZoneSupply.SubTeam_No) 

   RETURN @CaseUpchargePct
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CaseUpchargePct] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CaseUpchargePct] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CaseUpchargePct] TO [IRMAReportsRole]
    AS [dbo];

