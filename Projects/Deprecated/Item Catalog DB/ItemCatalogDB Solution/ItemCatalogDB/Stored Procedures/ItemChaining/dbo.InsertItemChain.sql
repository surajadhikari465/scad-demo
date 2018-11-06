if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemChain]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemChain]
GO

CREATE PROCEDURE [dbo].[InsertItemChain] 
@Chain_Name varchar(100),
@Chain_ID int OUTPUT

AS

BEGIN

Insert into ItemChain (ItemChainDesc) values (@Chain_Name)

SET @Chain_ID = SCOPE_IDENTITY()

END 

go