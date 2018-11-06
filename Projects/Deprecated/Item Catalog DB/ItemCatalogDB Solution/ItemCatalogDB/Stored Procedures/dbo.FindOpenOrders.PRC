SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FindOpenOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[FindOpenOrders]
GO


CREATE PROCEDURE dbo.FindOpenOrders
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT MIN(OrderHeader_ID) AS FirstOrder 
    FROM OrderHeader (nolock)
    WHERE Sent = 1
        AND SentToFaxDate IS NULL
        AND SentToEmailDate IS NULL
        AND SentToElectronicDate IS NULL
        AND SentDate IS NULL
        AND CloseDate IS NULL
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


