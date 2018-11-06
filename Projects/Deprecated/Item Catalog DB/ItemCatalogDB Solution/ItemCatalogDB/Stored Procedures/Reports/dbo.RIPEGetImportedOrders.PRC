
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RIPEGetImportedOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RIPEGetImportedOrders]
GO


CREATE PROCEDURE dbo.RIPEGetImportedOrders
    @ImportDate varchar(22) 
AS

SELECT Distinct IRS_OH.IRSOrderHeaderID
FROM Recipe..IRSOrderHistory IRS_OH
    INNER JOIN 
        Recipe..Orders1 RIPE_OH
        on RIPE_OH.Orders1ID = IRS_OH.RIPEOrders1ID
    INNER JOIN
        Recipe..Customer RIPE_Cust
        on RIPE_Cust.CustomerID = RIPE_OH.CustomerID
WHERE IRS_OH.ImportDateTime = @ImportDate

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


