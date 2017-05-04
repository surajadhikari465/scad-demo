SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetConversionInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetConversionInfo]
GO


CREATE PROCEDURE dbo.GetConversionInfo
@FromUnitID int,
@ToUnitID int
AS 

SELECT FromUnit_ID, ToUnit_ID, ConversionSymbol, ConversionFactor 
FROM ItemConversion 
WHERE FromUnit_ID = @FromUnitID AND ToUnit_ID = @ToUnitID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



