SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ValidateMonth]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ValidateMonth]
GO


CREATE PROCEDURE dbo.ValidateMonth
@sYear smallint,
@sMonth tinyint
AS

SELECT MIN(Date_Key) AS StartDate, MAX(Week) as Weeks
                         FROM Date (NOLOCK)
                         WHERE Period = @sMonth AND Year = @sYear

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


