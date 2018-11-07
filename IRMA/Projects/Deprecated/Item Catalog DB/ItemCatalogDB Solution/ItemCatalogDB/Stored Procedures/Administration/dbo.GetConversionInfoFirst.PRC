SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetConversionInfoFirst]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetConversionInfoFirst]
GO


CREATE PROCEDURE dbo.GetConversionInfoFirst
AS 

SELECT FromUnit_ID, ToUnit_ID, ConversionSymbol, ConversionFactor 
FROM ItemConversion
WHERE ToUnit_ID = (SELECT MIN(ToUnit_ID) FROM ItemConversion) AND 
      FromUnit_ID = (SELECT MIN(FromUnit_ID) FROM ItemConversion IC WHERE ToUnit_ID = ItemConversion.ToUnit_ID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



