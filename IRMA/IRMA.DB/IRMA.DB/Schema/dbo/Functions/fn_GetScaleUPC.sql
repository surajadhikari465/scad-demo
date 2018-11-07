CREATE FUNCTION [dbo].[fn_GetScaleUPC]
    (@Item_Key             INT,
     @IsScaleZoneData      BIT)
RETURNS VARCHAR(13)
AS
BEGIN
	DECLARE @ScaleUPC VARCHAR(13)
	
	SELECT @ScaleUPC = SUBSTRING(II.Identifier, 2, 5)
    FROM   ItemIdentifier II (nolock)
    WHERE  II.Item_Key = @Item_Key 
     -- For POS Push, Adds are sent outside of this stored proc
	 -- For Scale Push, Adds should be sent here with the Zone price records
    AND ((@IsScaleZoneData = 0 AND II.Add_Identifier = 0) OR @IsScaleZoneData = 1)

    RETURN @ScaleUPC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetScaleUPC] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetScaleUPC] TO [IRMAClientRole]
    AS [dbo];

