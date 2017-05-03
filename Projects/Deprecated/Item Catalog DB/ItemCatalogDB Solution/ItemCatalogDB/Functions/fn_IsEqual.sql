 /****** Object:  UserDefinedFunction [dbo].[fn_IsEqual]    Script Date: 10/19/2006 13:10:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsEqual]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_IsEqual]

/****** Object:  UserDefinedFunction [dbo].[fn_IsEqual]    Script Date: 10/19/2006 13:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO