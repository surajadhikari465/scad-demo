SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertSASIItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertSASIItem]
GO


CREATE PROCEDURE dbo.InsertSASIItem 
AS 

BEGIN

    DELETE FROM SASIItem WHERE Store_No IN (SELECT Store_No FROM Store WHERE SASI = 1)

    BULK INSERT SASIItem
    FROM 'E:\SASIItem.DAT'
    WITH ( FIELDTERMINATOR = '\t',
           ROWTERMINATOR = '\n' )

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

