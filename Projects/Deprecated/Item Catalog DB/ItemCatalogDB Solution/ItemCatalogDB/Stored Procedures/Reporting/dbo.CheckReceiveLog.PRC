SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckReceiveLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckReceiveLog]
GO


CREATE PROCEDURE dbo.CheckReceiveLog 
@Store_No int
AS 

SELECT LastRecvLogDate
FROM Store
WHERE Store_No = @Store_No


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

