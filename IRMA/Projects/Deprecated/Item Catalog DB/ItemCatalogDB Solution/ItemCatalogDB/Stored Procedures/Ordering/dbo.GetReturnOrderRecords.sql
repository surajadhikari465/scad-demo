SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetReturnOrderRecords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetReturnOrderRecords]
GO


CREATE PROCEDURE dbo.GetReturnOrderRecords (
    @Instance   INT,
    @User_ID    INT)
AS
BEGIN
    SELECT * 
    FROM  ReturnOrder 
    WHERE Instance = @Instance AND 
          User_ID  = @User_ID 
    ORDER BY OrderItem_ID
END

GO
