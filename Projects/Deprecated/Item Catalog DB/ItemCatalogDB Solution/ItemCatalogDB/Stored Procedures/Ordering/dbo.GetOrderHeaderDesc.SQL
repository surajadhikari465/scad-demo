SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderHeaderDesc]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderHeaderDesc]
GO


CREATE PROCEDURE dbo.GetOrderHeaderDesc
@OrderHeader_ID int
AS 
select top 1 * from 
(
SELECT OrderHeaderDesc ,1 Deleted
FROM OrderHeader 
WHERE OrderHeader_ID = @OrderHeader_ID
UNION
SELECT OrderHeaderDesc ,0 Deleted 
FROM DeletedOrder 
WHERE OrderHeader_ID = @OrderHeader_ID
) a
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

