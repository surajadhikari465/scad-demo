SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvgCostAdjustmentReasons]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAvgCostAdjustmentReasons]
GO


CREATE PROCEDURE dbo.GetAvgCostAdjustmentReasons

	@Filter bit,
	@Active bit

AS 

DECLARE @SQL varchar(MAX)

SELECT @SQL = 'SELECT ID, Description, Active FROM	AvgCostAdjReason '

IF @Filter = 1
	BEGIN
		SELECT @SQL = @SQL + 'WHERE Active = ' + CAST(@Active As varchar)
	END

SELECT @SQL = @SQL + + ' ORDER BY Description '
	
EXEC (@SQL)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

