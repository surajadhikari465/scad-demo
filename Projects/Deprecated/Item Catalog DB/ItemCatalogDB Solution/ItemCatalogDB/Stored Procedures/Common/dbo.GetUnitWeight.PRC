SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUnitWeight]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUnitWeight]
GO


CREATE PROCEDURE dbo.GetUnitWeight 
@Unit_ID int 
AS 

SELECT Weight_Unit
FROM ItemUnit
WHERE Unit_ID = @Unit_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



