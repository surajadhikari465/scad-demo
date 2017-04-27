SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetDaysInFiscalMonth]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetDaysInFiscalMonth]
GO


CREATE PROCEDURE dbo.GetDaysInFiscalMonth
@Month int,
@Year int
AS

SELECT Date.Day_Of_Month, Date.Date_Key 
FROM Date (NOLOCK) 
WHERE Period = @Month AND Year = @Year

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



