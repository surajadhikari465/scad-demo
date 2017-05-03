IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Conv_UpcToID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_Conv_UpcToID]
go
Create FUNCTION [dbo].[fn_Conv_UpcToID]
	(@upcno varchar(13))
	 RETURNS varchar(12)
AS
BEGIN  

RETURN cast(cast(substring(@upcno, 1, 12) as bigint) as varchar(12))

END
go
 