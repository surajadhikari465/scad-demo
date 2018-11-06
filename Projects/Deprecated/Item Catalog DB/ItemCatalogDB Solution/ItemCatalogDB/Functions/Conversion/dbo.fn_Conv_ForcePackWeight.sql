IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Conv_ForcePackWeight]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_Conv_ForcePackWeight]
go

Create FUNCTION [dbo].[fn_Conv_ForcePackWeight]
	(@upcno varchar(13), @scale char(1), @itemuom char(2), @scale_uom char(2))
	 RETURNS bit
AS
BEGIN  
	DECLARE @return bit

	select @return = (
		case 
			when dbo.fn_IsScaleIdentifier(dbo.fn_Conv_UpcToID(@upcno)) = 1 
				and @scale_uom = 'RW' then 1
			when @scale  = 'Y' 
				and @itemuom in ('RW','LB','OZ') then 1
			when @itemuom = 'RW' then 1
			else 0 
		end)

	RETURN @return
END
go
 