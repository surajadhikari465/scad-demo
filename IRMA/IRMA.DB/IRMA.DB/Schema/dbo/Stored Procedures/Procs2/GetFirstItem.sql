CREATE PROCEDURE dbo.GetFirstItem 
AS 

SELECT MIN(Item_Key) AS FirstItem 

FROM Item
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFirstItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFirstItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFirstItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFirstItem] TO [IRMAReportsRole]
    AS [dbo];

