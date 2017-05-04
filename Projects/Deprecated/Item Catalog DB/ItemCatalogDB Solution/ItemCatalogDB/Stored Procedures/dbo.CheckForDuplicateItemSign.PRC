SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateItemSign]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateItemSign]
GO


CREATE PROCEDURE dbo.CheckForDuplicateItemSign 
@Store_No int, 
@Item_Key int,
@Sign_ID int 
AS 

SELECT COUNT(*) AS ItemSignCount 

FROM ItemSign

WHERE Store_No = @Store_No AND 
      Item_Key = @Item_Key AND 
      Sign_ID  = @Sign_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


