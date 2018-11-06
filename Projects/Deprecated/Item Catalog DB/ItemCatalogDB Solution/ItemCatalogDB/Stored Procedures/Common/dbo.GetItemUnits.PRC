SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemUnits]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemUnits]
GO


CREATE PROCEDURE dbo.GetItemUnits
AS 

SELECT
    Unit_ID ,
    Unit_Name ,
    Weight_Unit ,
    User_id, 
    Unit_Abbreviation ,
    ISNULL ( LOWER ( UnitSysCode ) , '' ) AS UnitSysCode ,
    IsPackageUnit,
    PlumUnitAbbr,
    EDISysCode
FROM
    ItemUnit ( nolock )
ORDER BY
    Unit_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

