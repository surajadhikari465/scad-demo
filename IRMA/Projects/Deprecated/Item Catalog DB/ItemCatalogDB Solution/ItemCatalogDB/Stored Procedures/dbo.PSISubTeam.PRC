SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PSISubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PSISubTeam]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


