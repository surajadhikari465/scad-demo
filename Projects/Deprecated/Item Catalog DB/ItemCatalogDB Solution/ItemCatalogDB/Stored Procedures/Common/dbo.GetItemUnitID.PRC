SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemUnitID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemUnitID]
GO


CREATE PROCEDURE dbo.GetItemUnitID
@Unit_Name varchar(25)
AS 

SELECT Unit_ID 
FROM ItemUnit
WHERE Unit_Name = @Unit_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

