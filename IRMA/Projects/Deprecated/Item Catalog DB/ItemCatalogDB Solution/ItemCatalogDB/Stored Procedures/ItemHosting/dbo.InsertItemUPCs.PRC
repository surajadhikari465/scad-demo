SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemUPCs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemUPCs]
GO


CREATE PROCEDURE dbo.InsertItemUPCs
@Item_Key int,
@UPC varchar(20)
AS 

INSERT INTO ItemUPC (Item_Key, UPC, UPC_Changed, UPC_Number)
VALUES (@Item_Key, @UPC, 1, CAST(@UPC AS decimal(18,0)))


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


