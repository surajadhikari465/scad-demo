SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateUnits]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateUnits]
GO


CREATE PROCEDURE dbo.CheckForDuplicateUnits 
@Unit_ID int, 
@Unit_Name varchar(25) 
AS 

SELECT COUNT(*) AS UnitCount 
FROM ItemUnit 
WHERE Unit_Name = @Unit_Name AND Unit_ID <> @Unit_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

