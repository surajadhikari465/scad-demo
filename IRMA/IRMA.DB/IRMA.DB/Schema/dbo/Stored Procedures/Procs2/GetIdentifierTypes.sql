CREATE PROCEDURE dbo.GetIdentifierTypes
AS 

BEGIN
    SET NOCOUNT ON
    
	SELECT 'B' As IdentifierType
	UNION
	SELECT 'O' As IdentifierType
	UNION
	SELECT 'P' As IdentifierType
	UNION
	SELECT 'S' As IdentifierType
	    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIdentifierTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIdentifierTypes] TO [IRMASLIMRole]
    AS [dbo];

