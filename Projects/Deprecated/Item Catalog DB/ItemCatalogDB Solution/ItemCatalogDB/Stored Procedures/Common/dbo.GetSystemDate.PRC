SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetSystemDate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetSystemDate]
GO

CREATE PROCEDURE dbo.GetSystemDate 
AS 
	DECLARE @CentralTimeZoneOffset int
	
	SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region

	SELECT DATEADD(HOUR, @CentralTimeZoneOffset, GETDATE()) As SystemDate

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



