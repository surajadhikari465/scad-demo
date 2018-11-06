SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemConversionAll]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemConversionAll]
GO


CREATE PROCEDURE dbo.GetItemConversionAll 

AS 

SELECT FromUnit_ID, ToUnit_ID,  ConversionSymbol, ConversionFactor 
FROM ItemConversion WITH(nolock)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

