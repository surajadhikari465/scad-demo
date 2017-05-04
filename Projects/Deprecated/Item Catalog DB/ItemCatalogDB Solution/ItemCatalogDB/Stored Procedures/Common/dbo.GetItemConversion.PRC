SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemConversion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemConversion]
GO


CREATE PROCEDURE dbo.GetItemConversion 
@FromUnit_ID int, 
@ToUnit_ID int 
AS 

SELECT FromUnit_ID, ToUnit_ID,  ConversionSymbol, ConversionFactor 
FROM ItemConversion 
WHERE FromUnit_ID = @FromUnit_ID AND ToUnit_ID = @ToUnit_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


