CREATE PROCEDURE dbo.CheckForDuplicateInvLocation
@InvLocID int,
@StoreID int,
@SubTeamID int,
@LocName varchar(50)

AS 

SET NOCOUNT ON

IF @InvLocID > 0 
	--If a RecID was passed in, then we are looking for a match on a record where the RecID does not match.
	BEGIN
		SELECT Count(InvLoc_ID) AS Found 
		FROM InventoryLocation (nolock)
		WHERE Store_No = @StoreID and SubTeam_No = @SubTeamID and InvLoc_Name = @LocName and InvLoc_ID <> @InvLocID
	END
ELSE
	--If no RecID was passed in, then search for a match regardless of the RecID.
	BEGIN
		SELECT Count(InvLoc_ID) AS Found 
		FROM InventoryLocation (nolock)
		WHERE Store_No = @StoreID and InvLoc_Name = @LocName and SubTeam_No = @SubTeamID
	END 

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateInvLocation] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateInvLocation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateInvLocation] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateInvLocation] TO [IRMAReportsRole]
    AS [dbo];

