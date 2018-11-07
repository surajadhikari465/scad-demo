create function [dbo].[fn_IsEqual] (
    @StringValue1 varchar(4000),
    @StringValue2 varchar(4000))
returns bit
as
begin

    declare @IsEqual bit
    
    SET @IsEqual = 1

    If @StringValue1 <> @StringValue2
		OR (@StringValue1 IS NULL AND @StringValue2 IS NOT NULL)
		OR (@StringValue1 IS NOT NULL AND @StringValue2 IS NULL)
	BEGIN
		SET @IsEqual = 0
	END
    
    return @IsEqual
end