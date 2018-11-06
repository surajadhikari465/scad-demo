CREATE PROCEDURE dbo.GetItemVideo
@ItemVideo_ID int
AS 

SELECT PageDescription, PageText
FROM ItemVideo
WHERE ItemVideo_ID = @ItemVideo_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemVideo] TO [IRMAReportsRole]
    AS [dbo];

