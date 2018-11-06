SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUrgentVCAI_Exceptions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUrgentVCAI_Exceptions]
GO


CREATE PROCEDURE dbo.GetUrgentVCAI_Exceptions

AS
BEGIN
    SET NOCOUNT ON


    SELECT * FROM VendorCostHistoryExceptions where isnull(UserStart_Date, Start_Date) <= dateadd(day, 1, GetDate()) and ExStatus = 0

    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


--exec GetUrgentVCAI_Exceptions 4,1,null