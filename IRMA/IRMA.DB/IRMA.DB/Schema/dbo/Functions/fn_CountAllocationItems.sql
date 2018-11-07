CREATE FUNCTION [dbo].[fn_CountAllocationItems]
()
RETURNS INT
AS
BEGIN
    DECLARE @ItemCount int

    SELECT @ItemCount = COUNT(*) FROM tmpOrdersAllocateItems

    RETURN @ItemCount
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CountAllocationItems] TO [IRMAClientRole]
    AS [dbo];

