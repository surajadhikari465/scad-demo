if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteItemChain]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteItemChain]
GO

CREATE PROCEDURE [dbo].[DeleteItemChain] 
@Chain_ID int

AS

BEGIN

delete from ItemChainItem where ItemChainID=@Chain_ID

delete from ItemChain where ItemChainID=@Chain_ID

END 
go