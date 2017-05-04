SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetVersion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetVersion]
GO

CREATE PROCEDURE dbo.GetVersion
	@ApplicationName varchar(50)
AS 

	SELECT * FROM Version WHERE ApplicationName = @ApplicationName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO