
CREATE PROCEDURE [dbo].[VIM365PriceTypeFile]
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	if object_id('dbo.VIM365PriceTypeFileLoad') is not null DROP TABLE dbo.VIM365PriceTypeFileLoad
    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo.VIM365PriceTypeFileLoad]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.VIM365PriceTypeFileLoad

	CREATE TABLE dbo.VIM365PriceTypeFileLoad (
		PRICETYPE varchar(20), REGION varchar(2), VT_DESCRIPTION varchar(20)
		) ON [PRIMARY]
	
	DECLARE itr CURSOR READ_ONLY
	FOR
		SELECT PriceChgTypeDesc FROM PriceChgType (nolock)
	
	DECLARE @PriceType varchar(20)
	
	DECLARE @RegionCode varchar(2)
	SET @RegionCode = 'TS';
	
	OPEN itr
	
	FETCH NEXT FROM itr INTO @PriceType
	WHILE (@@fetch_status <> -1)
	BEGIN
		IF (@@fetch_status <> -2)
		BEGIN
			INSERT INTO dbo.VIM365PriceTypeFileLoad (PRICETYPE, REGION, VT_DESCRIPTION)
			VALUES (@PriceType, @RegionCOde, @PriceType)			
		END
		FETCH NEXT FROM itr INTO @PriceType
	END
	
	SELECT * FROM dbo.VIM365PriceTypeFileLoad
	
	SET NOCOUNT OFF

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIM365PriceTypeFile] TO [IRMASchedJobsRole]
    AS [dbo];

