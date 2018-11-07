CREATE PROCEDURE [dbo].[PSISubTeam]
AS
-- 20071121 DaveStacey -  Edited script, added error handling
	BEGIN TRY
		SELECT SubTeam_No, SubTeam_Name 
		FROM dbo.SubTeam (nolock)
		
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('PSISubTeam failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PSISubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PSISubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PSISubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PSISubTeam] TO [IRMAReportsRole]
    AS [dbo];

