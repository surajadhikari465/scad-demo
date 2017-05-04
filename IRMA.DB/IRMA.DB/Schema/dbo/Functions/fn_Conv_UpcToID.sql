Create FUNCTION [dbo].[fn_Conv_UpcToID]
	(@upcno varchar(13))
	 RETURNS varchar(12)
AS
BEGIN  

RETURN cast(cast(substring(@upcno, 1, 12) as bigint) as varchar(12))

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_Conv_UpcToID] TO [DataMigration]
    AS [dbo];

