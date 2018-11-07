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
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_StoreSubTeamExists] TO [IRMAClientRole]
    AS [dbo];

