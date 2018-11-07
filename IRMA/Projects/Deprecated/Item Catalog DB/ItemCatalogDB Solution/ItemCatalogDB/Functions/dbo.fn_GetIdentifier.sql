IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_GetIdentifier') 
    DROP FUNCTION fn_GetIdentifier
GO

CREATE FUNCTION [dbo].[fn_GetIdentifier]
    (@Item_Key             INT,
     @IsScaleZoneData      BIT)
RETURNS VARCHAR(13)
AS
BEGIN
	DECLARE @Identifier VARCHAR(13)
	
	SELECT @Identifier = II.Identifier
	FROM   ItemIdentifier II (nolock)
    WHERE  II.Item_Key = @Item_Key 
     -- For POS Push, Adds are sent outside of this stored proc
	 -- For Scale Push, Adds should be sent here with the Zone price records
    AND ((@IsScaleZoneData = 0 AND II.Add_Identifier = 0) OR @IsScaleZoneData = 1)

    RETURN @Identifier
END


GO
