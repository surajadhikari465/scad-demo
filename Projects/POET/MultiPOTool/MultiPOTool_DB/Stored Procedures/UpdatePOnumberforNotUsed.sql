
IF exists (SELECT * FROM dbo.sysobjects where id = object_id(N'[dbo].[UpdatePOnumberforNotUsed]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdatePOnumberforNotUsed]
GO

CREATE PROCEDURE [dbo].[UpdatePOnumberforNotUsed]
	@Ponumber as int
AS
BEGIN
	UPDATE ponumber set used = 1 
	where ponumber = @Ponumber
END

GO


