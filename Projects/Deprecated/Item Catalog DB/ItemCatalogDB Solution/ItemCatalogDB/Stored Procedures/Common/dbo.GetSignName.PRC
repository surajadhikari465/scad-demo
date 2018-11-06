SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSignName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSignName]
GO


CREATE PROCEDURE dbo.GetSignName
@Sign_ID int
AS 

SELECT Sign_Desc
FROM Sign
WHERE Sign_ID = @Sign_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

