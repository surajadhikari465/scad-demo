IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_RemoveApp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_RemoveApp]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AppConfig_RemoveApp]

	@ApplicationID uniqueidentifier,
	@EnvironmentID uniqueidentifier,
	@User_ID int
	
AS

BEGIN

SET NOCOUNT ON

DECLARE @Error_No int, @Severity smallint
SELECT @Error_No = 0

BEGIN TRAN

	UPDATE AppConfigValue 
		SET 
			Deleted = 1,
			LastUpdate = GetDate(),
			LastUpdateUserID = @User_ID
	WHERE ApplicationID = @ApplicationID
	AND EnvironmentID = @EnvironmentID
	
	SELECT @Error_No = @@ERROR
	
	IF @Error_No = 0
		BEGIN
			UPDATE AppConfigApp 
				SET 
					Deleted = 1,
					LastUpdate = GetDate(),
					LastUpdateUserID = @User_ID
			WHERE ApplicationID = @ApplicationID
			AND EnvironmentID = @EnvironmentID
			
			SELECT @Error_No = @@ERROR
		END
		
	SET NOCOUNT OFF

	IF @Error_No = 0
		COMMIT TRAN
	ELSE
	BEGIN
		IF @@TRANCOUNT <> 0
			ROLLBACK TRAN
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
		RAISERROR ('Remove Application Configuration failed with @@ERROR: %d', @Severity, 1, @error_no)
	END
END

GO