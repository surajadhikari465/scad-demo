SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemSign]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemSign]
GO


CREATE PROCEDURE dbo.InsertItemSign
@Item_Key int,
@Store_No int,
@Sign_ID int
AS 

INSERT INTO ItemSign (Item_Key, Store_No, Sign_ID)
VALUES (@Item_Key, @Store_No, @Sign_ID)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


