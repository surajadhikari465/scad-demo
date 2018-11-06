SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SecurityGetTitles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SecurityGetTitles]
GO

CREATE PROCEDURE dbo.SecurityGetTitles
AS
BEGIN
    SET NOCOUNT ON

	SELECT Title_ID, Title_Desc 
	FROM Title 
	ORDER BY Title_Desc
  
    SET NOCOUNT OFF
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
