SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteOrigin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteOrigin]
GO


CREATE PROCEDURE dbo.DeleteOrigin
@Origin_ID int 
AS 

DELETE 
FROM ItemOrigin 
WHERE Origin_ID = @Origin_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


