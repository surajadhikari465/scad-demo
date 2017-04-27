SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAllClosedControlGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
   drop procedure [dbo].[GetAllClosedControlGroups]
GO

CREATE PROCEDURE [dbo].[GetAllClosedControlGroups]
AS
BEGIN
	select distinct OrderInvoice_ControlGroup_ID from Orderinvoice_ControlGroup
    where OrderInVoice_ControlGroupStatus_ID=2 -- All closed controlGroups
    order by OrderInvoice_ControlGroup_ID asc
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





