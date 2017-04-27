SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_CaseUpchargePct]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_CaseUpchargePct]
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 