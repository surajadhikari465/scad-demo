CREATE PROCEDURE dbo.CheckForDuplicateItemSign 
@Store_No int, 
@Item_Key int,
@Sign_ID int 
AS 

SELECT COUNT(*) AS ItemSignCount 

FROM ItemSign

WHERE Store_No = @Store_No AND 
      Item_Key = @Item_Key AND 
      Sign_ID  = @Sign_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemSign] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemSign] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemSign] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateItemSign] TO [IRMAReportsRole]
    AS [dbo];

