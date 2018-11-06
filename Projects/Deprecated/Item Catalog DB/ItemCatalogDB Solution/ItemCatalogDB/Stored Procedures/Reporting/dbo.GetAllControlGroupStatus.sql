SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAllControlGroupStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
   drop procedure [dbo].[GetAllControlGroupStatus]
GO

CREATE PROCEDURE [dbo].[GetAllControlGroupStatus]
AS
BEGIN
Select OrderInvoice_ControlGroupStatus_ID,
    OrderInvoice_ControlGroupStatus_Desc 
    from Orderinvoice_ControlGroupStatus
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO