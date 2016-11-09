CREATE PROCEDURE dbo.GetItemVideoList
@Item_Key int
AS 

SELECT ItemVideo_ID, Sequence_ID, PageDescription
FROM ItemVideo
WHERE Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideoList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideoList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideoList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideoList] TO [IRMAReportsRole]
    AS [dbo];

