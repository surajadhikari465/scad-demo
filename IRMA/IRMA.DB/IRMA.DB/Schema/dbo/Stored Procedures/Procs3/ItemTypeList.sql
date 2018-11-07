CREATE PROCEDURE dbo.ItemTypeList
AS 
SELECT ItemType_ID, ItemType_Name
FROM ItemType
ORDER By ItemType_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemTypeList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemTypeList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemTypeList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemTypeList] TO [IRMAReportsRole]
    AS [dbo];

