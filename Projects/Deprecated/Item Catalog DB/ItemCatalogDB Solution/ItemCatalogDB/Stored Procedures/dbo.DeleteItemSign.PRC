SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteItemSign]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteItemSign]
GO


CREATE PROCEDURE dbo.DeleteItemSign
@Store_No int,
@ItemKey int, 
@Sign_ID int 
AS 

DELETE 
FROM ItemSign
WHERE Item_Key = @ItemKey AND Store_No = @Store_No AND Sign_ID = @Sign_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

