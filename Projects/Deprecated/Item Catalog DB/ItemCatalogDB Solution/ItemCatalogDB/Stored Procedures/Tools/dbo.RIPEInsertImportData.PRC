
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RIPEInsertImportData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RIPEInsertImportData]
GO


CREATE PROCEDURE dbo.RIPEInsertImportData
    @IRSOrderHeaderID int,
    @RIPEOrders1ID int,
    @DistDate varchar(10),
    @ImportDateTime varchar(22)
AS

INSERT INTO Recipe..IRSOrderHistory values(@IRSOrderHeaderID, @RIPEOrders1ID, @DistDate, @ImportDateTime)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


