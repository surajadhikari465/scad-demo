USE [ItemCatalog]
SET NOCOUNT ON;
BEGIN
	DECLARE @TeamNo INT = (
	  SELECT Team_No
	  FROM Team
	  WHERE Team_Abbreviation = 'Specialty'); --Team_No varies based on region but abbreviation is constant

	DECLARE @SubTeamNo INT = (
	  SELECT SubTeam_No
	  FROM SubTeam 
	  WHERE SubDept_No = 2400)--Coffee Subteam PS

	BEGIN TRY
		BEGIN TRANSACTION;
		-- Update statement here to modify Team_No for Coffee Subteam
		UPDATE Subteam
		SET Team_No = @TeamNo
		FROM SubTeam
		WHERE SubDept_No = 2400 --Coffee Subteam PS

		-- Update statement here to modify Team_No for Coffee Subteam in each store
		UPDATE StoreSubTeam
		SET 
			Team_No = @TeamNo,
			PS_Team_No = 140 --Specialty PS_Team_No
		FROM StoreSubTeam
		WHERE SubTeam_No = @SubTeamNo AND PS_Team_No = 100 --This condition grabs the Coffee subteams for stores only

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		PRINT 'Error:';
    SELECT ERROR_NUMBER() AS ErrorNumber
	  ,ERROR_SEVERITY() AS ErrorSeverity
	  ,ERROR_STATE() AS ErrorState
	  ,ERROR_PROCEDURE() AS ErrorProcedure
	  ,ERROR_LINE() AS ErrorLine
	  ,ERROR_MESSAGE() AS ErrorMessage;
    ROLLBACK TRANSACTION;
  END CATCH
END