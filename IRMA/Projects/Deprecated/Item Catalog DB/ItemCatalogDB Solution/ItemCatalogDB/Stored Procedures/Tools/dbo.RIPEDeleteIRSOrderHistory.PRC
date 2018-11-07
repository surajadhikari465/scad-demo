
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RIPEDeleteIRSOrderHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RIPEDeleteIRSOrderHistory]
GO


CREATE PROCEDURE dbo.RIPEDeleteIRSOrderHistory
    @IRSOrderHeaderID int
AS

DELETE FROM Recipe..IRSOrderHistory 
WHERE IRSOrderHeaderID = @IRSOrderHeaderID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

