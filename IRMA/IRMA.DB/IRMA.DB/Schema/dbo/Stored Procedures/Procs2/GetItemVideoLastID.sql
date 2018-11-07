CREATE PROCEDURE dbo.GetItemVideoLastID
@Item_Key int
AS 

SELECT MAX(ItemVideo_ID) AS ItemVideo_ID
FROM ItemVideo
WHERE Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideoLastID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideoLastID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideoLastID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideoLastID] TO [IRMAReportsRole]
    AS [dbo];

