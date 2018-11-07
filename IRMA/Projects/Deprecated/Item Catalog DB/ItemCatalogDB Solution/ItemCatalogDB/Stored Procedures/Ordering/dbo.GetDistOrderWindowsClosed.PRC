SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetDistOrderWindowsClosed') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetDistOrderWindowsClosed
GO


CREATE PROCEDURE dbo.GetDistOrderWindowsClosed
    @Store_No int,
    @SubTeam_No int,
    @NonRetail bit
AS

BEGIN
    SET NOCOUNT ON

    SELECT COUNT(*)
    FROM   ZoneSubTeam
    WHERE  Supplier_Store_No = @Store_No    AND 
           SubTeam_No        = @SubTeam_No  AND 
           ((DATEDIFF(minute, 
                      CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()),                         108),
                      CONVERT(varchar(255), ISNULL(CASE WHEN @NonRetail = 0 THEN OrderEnd ELSE OrderEndTransfers END, CONVERT(smalldatetime, GETDATE())), 108)) > 0) AND
            (DATEDIFF(minute, 
                      CONVERT(varchar(255), ISNULL(OrderStart,     CONVERT(smalldatetime, GETDATE())), 108), 
                      CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()),                         108)) >= 0))

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


