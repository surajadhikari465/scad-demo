SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAllControlGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
   drop procedure [dbo].[GetAllControlGroups]
GO

-- ================================================================================
-- Author      :	Sekhara
-- Create date :    12/27/2007
-- Description :	3WayMatchDetailSummaryReport
-- =================================================================================

GO 
CREATE PROCEDURE [dbo].[GetAllControlGroups]
AS
BEGIN
   	select distinct OrderInvoice_ControlGroup_ID from Orderinvoice_ControlGroup
    order by OrderInvoice_ControlGroup_ID asc
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




