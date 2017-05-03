SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteOrderInvoice]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteOrderInvoice]
GO


CREATE PROCEDURE dbo.DeleteOrderInvoice 
@OrderHeader_ID int
AS

DELETE 
FROM OrderInvoice
WHERE OrderHeader_ID = @OrderHeader_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


