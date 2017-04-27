IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[IRMA_Main_GetMenuAccess]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[IRMA_Main_GetMenuAccess]
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.IRMA_Main_GetMenuAccess
AS
BEGIN
SELECT [MenuAccessID]
      ,[MenuName]
      ,[Visible]
  FROM [MenuAccess] WITH (NOLOCK)
  ORDER BY [MenuName]
  END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


