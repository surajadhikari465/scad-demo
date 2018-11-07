CREATE FUNCTION dbo.fn_HasScaleIdentifier 
(
	@Item_Key int
)
RETURNS BIT
AS
BEGIN
    DECLARE @Result BIT
    
    IF EXISTS (SELECT * FROM ItemIdentifier (nolock) WHERE Item_Key = @Item_Key AND Scale_Identifier = 1)
	    SET @Result = 1
    ELSE
	    SET @Result = 0
	    
	RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasScaleIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasScaleIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_HasScaleIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];

