CREATE PROCEDURE dbo.GetCurrentOnHand 
    @Item_Key int,
    @Store_No int,
    @SubTeam_No int
AS 
SELECT Quantity,
       Weight
FROM   OnHand (nolock)
WHERE  Item_Key = @Item_Key AND 
       Store_No = @Store_No AND
       SubTeam_No = @SubTeam_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentOnHand] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentOnHand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentOnHand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrentOnHand] TO [IRMAReportsRole]
    AS [dbo];

