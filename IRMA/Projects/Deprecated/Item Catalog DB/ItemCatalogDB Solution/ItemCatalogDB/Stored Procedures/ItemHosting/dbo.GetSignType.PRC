SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSignType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSignType]
GO


CREATE PROCEDURE dbo.GetSignType
@Sign_ID int
AS 

SELECT * 
FROM Sign
WHERE Sign_ID = @Sign_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



