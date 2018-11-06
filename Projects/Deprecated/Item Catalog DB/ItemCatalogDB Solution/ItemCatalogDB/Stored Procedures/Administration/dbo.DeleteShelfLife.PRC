SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteShelfLife]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteShelfLife]
GO


CREATE PROCEDURE dbo.DeleteShelfLife 
@ShelfLife_ID int 
AS 

DELETE 
FROM ItemShelfLife 
WHERE ShelfLife_ID = @ShelfLife_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

