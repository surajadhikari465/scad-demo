SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SecurityGetApplications]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SecurityGetApplications]
GO

CREATE PROCEDURE dbo.SecurityGetApplications
	@AppStatus as int  
AS
BEGIN
    SET NOCOUNT ON

	IF @AppStatus = 2

		BEGIN
			SELECT ApplicationID, [Name] FROM Applications
		END

	ELSE

		BEGIN
			SELECT ApplicationID, [Name] FROM Applications
			WHERE [Enabled] = @AppStatus
		END
    
    SET NOCOUNT OFF
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

