 set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_StoreSubTeamExists]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_StoreSubTeamExists]

GO

CREATE FUNCTION [dbo].[fn_StoreSubTeamExists]
(
    @StoreNo int,
    @SubTeamNo int
)
RETURNS BIT
AS
BEGIN
    DECLARE @Exists bit	

    IF EXISTS(SELECT Store_No FROM StoreSubTeam WHERE Store_No = @StoreNo AND SubTeam_No = @SubTeamNo)
		SELECT @Exists = 1
	ELSE
		SELECT @Exists = 0

    RETURN @Exists
END

GO

