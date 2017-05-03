 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOSSystemTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPOSSystemTypes]
GO

CREATE PROCEDURE dbo.GetPOSSystemTypes 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT POSSystemId, POSSystemType
    FROM POSSystemTypes (NOLOCK)     
    ORDER BY POSSystemType
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

