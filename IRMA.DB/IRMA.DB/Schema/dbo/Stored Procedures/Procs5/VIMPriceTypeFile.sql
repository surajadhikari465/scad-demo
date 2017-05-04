CREATE PROCEDURE dbo.VIMPriceTypeFile
AS 

BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	/* cleanup of dbo.VIMPriceTypeFileLoad  */
	if object_id('dbo.VIMPriceTypeFileLoad') is not null DROP TABLE dbo.VIMPriceTypeFileLoad
	
    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo.VIMPriceTypeFileLoad]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.VIMPriceTypeFileLoad

	CREATE TABLE dbo.VIMPriceTypeFileLoad (
		PRICETYPE varchar(20), REGION varchar(2), VT_DESCRIPTION varchar(20)
		) ON [PRIMARY]
	
	DECLARE itr CURSOR READ_ONLY
	FOR
		SELECT PriceChgTypeDesc FROM PriceChgType (nolock)
	
	DECLARE @PriceType varchar(20)
	
	DECLARE @RegionCode varchar(2)
	SET @RegionCode = (select top 1 runmode from conversion_runmode)--(SELECT primaryRegionCode FROM InstanceData)
	
	OPEN itr
	
	FETCH NEXT FROM itr INTO @PriceType
	WHILE (@@fetch_status <> -1)
	BEGIN  -- begin itr fetch_status <> -1
		IF (@@fetch_status <> -2)
		BEGIN  -- begin itr fetch_status <> -2
			INSERT INTO dbo.VIMPriceTypeFileLoad (PRICETYPE, REGION, VT_DESCRIPTION)
			VALUES (@PriceType, @RegionCOde, @PriceType)			
		END  -- end itr fetch_status <> -2
		FETCH NEXT FROM itr INTO @PriceType
	END  -- end itr fetch_status <> -1
	
	SELECT * FROM dbo.VIMPriceTypeFileLoad
	
	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPriceTypeFile] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPriceTypeFile] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPriceTypeFile] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMPriceTypeFile] TO [IRMAReportsRole]
    AS [dbo];

