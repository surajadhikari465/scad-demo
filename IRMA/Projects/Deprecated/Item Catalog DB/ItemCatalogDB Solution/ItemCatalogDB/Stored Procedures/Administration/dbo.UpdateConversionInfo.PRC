SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateConversionInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateConversionInfo]
GO


CREATE PROCEDURE dbo.UpdateConversionInfo
@FromUnitID int,
@ToUnitID int,
@ConversionSymbol varchar(1),
@ConversionFactor decimal(9,4)
AS 

UPDATE ItemConversion 
SET ConversionSymbol = @ConversionSymbol,
    ConversionFactor = @ConversionFactor
WHERE FromUnit_ID = @FromUnitID AND ToUnit_ID = @ToUnitID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


