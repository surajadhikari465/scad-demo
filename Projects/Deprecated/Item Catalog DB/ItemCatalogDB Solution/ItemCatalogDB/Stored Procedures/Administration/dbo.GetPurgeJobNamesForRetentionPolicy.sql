SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetPurgeJobNamesForRetentionPolicy]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].GetPurgeJobNamesForRetentionPolicy
GO

CREATE PROCEDURE dbo.GetPurgeJobNamesForRetentionPolicy
AS
BEGIN

	SELECT DISTINCT PurgeJobName FROM [RetentionPolicy]
	ORDER BY PurgeJobName DESC

END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 