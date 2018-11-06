if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemChainItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemChainItem]
GO

CREATE PROCEDURE [dbo].[InsertItemChainItem] 
@Chain_ID int,
@Item_Key int

AS

BEGIN

Insert into ItemChainItem (ItemChainId,Item_Key) values (@Chain_ID,@Item_Key)

END 

go