SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ValidateWeek]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ValidateWeek]
GO


CREATE PROCEDURE dbo.ValidateWeek
@Year smallint,
@Month tinyint
AS

SELECT MIN(Date_Key) AS StartDate 
FROM Date (NOLOCK) 
WHERE ((Period-1)*4) + Week = @Month AND Year = @Year


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


