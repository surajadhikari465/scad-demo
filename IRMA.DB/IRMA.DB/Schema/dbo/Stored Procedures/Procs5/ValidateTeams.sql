CREATE PROCEDURE dbo.ValidateTeams
@Store_No as int,
@Zone_ID as tinyint
AS

SELECT Distinct Store.Store_No, Team_No, WFM_Store, Zone_Id 
FROM Store (NOLOCK) INNER JOIN StoreSubteam (NOLOCK) ON Store.Store_no = StoreSubteam.Store_No 
WHERE ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND Zone_Id = @Zone_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateTeams] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateTeams] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateTeams] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateTeams] TO [IRMAReportsRole]
    AS [dbo];

