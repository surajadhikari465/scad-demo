if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckREGPriceDifference]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckREGPriceDifference]
GO

CREATE PROCEDURE [dbo].[CheckREGPriceDifference] 
	@Items varchar(MAX),
	@Stores varchar(MAX)
AS

BEGIN
	DECLARE @SQL varchar(MAX)

	SELECT @SQL = ' WITH MyTable (price,c) AS'
	SELECT @SQL = @SQL + ' ('
	SELECT @SQL = @SQL + ' select price,count(*) c from price where '
	SELECT @SQL = @SQL + ' item_key in('+@Items+') and store_no in ('+@Stores+') group by price)'
	SELECT @SQL = @SQL + ' SELECT count(*) [Found]'
	SELECT @SQL = @SQL + ' FROM MyTable'

EXECUTE(@SQL)

END 

go