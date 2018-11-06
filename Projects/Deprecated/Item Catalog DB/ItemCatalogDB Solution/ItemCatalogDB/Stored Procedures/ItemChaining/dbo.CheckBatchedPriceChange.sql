if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckBatchedPriceChange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckBatchedPriceChange]
GO

CREATE PROCEDURE [dbo].[CheckBatchedPriceChange] 
	@Items varchar(MAX),
	@Stores varchar(MAX)
AS

BEGIN
	DECLARE @SQL varchar(MAX)
  
	SELECT @SQL = ' select count(*) Found from PriceBatchDetail PBD'
	SELECT @SQL = @SQL + ' inner join PriceBatchHeader PBH'
	SELECT @SQL = @SQL + ' on PBD.PriceBatchHeaderId = PBH.PriceBatchHeaderId'
	SELECT @SQL = @SQL + ' where PBD.Store_No in ('+@Stores+')'
	SELECT @SQL = @SQL + ' and PBD.Item_Key in('+@Items+')'
	SELECT @SQL = @SQL + ' and PBD.Expired = 0'
	SELECT @SQL = @SQL + ' and PBH.PriceBatchStatusID	 < 6'

	EXECUTE(@SQL)
END 
go