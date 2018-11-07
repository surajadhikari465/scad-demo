SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertConversion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertConversion]
GO


CREATE PROCEDURE dbo.InsertConversion
@FromUnitID int,
@ToUnitID int,
@ConversionSymbol varchar(1),
@ConversionFactor decimal(9,4)
AS 

INSERT INTO ItemConversion (FromUnit_ID, ToUnit_ID, ConversionSymbol, ConversionFactor)
VALUES (@FromUnitID, @ToUnitID, @ConversionSymbol, @ConversionFactor)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


