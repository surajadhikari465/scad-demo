SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.SysObjects WHERE Id = OBJECT_Id(N'[dbo].[GetInventoryServiceExport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetInventoryServiceExport]
GO

CREATE  PROCEDURE [dbo].[GetInventoryServiceExport]
	@ICVID int, 
	@BusinessUnit_ID int
AS 
BEGIN
    SET NOCOUNT ON
    
    DECLARE @Period int, @Year int

    SELECT 
		@Period = Period, @Year = [Year]
    FROM 
		[Date] D (nolock)
    WHERE 
		Date_Key = CONVERT(smalldatetime, CONVERT(varchar(255), GETDATE(), 101))

    SELECT 
          Region,
            LEFT(STORE_NAME, 20) AS 'STORE_NAME',
            BUSINESSUNIT_ID AS 'PS_BU',
            SUBSTRING ('0000', 1, 4 - LEN(SubTeam_No)) + CAST(SubTeam_No AS VARCHAR(4)) AS 'PS_PROD_SUBTEAM',
            RTRIM(LEFT(SUBTEAM_NAME, 30)) AS 'PS_PROD_DESCRIPTION',
            SUBSTRING ('0000000000000', 1, 13 - LEN(Identifier)) + Identifier AS UPC,
            REPLACE(RTRIM(LEFT(ITEM_DESCRIPTION, 30)), CHAR(9), ' ') AS 'DESCRIPTION',
            CONVERT(varchar(9),Price) AS 'EFF_PRICE', 
			CONVERT(varchar(9),AvgCost) AS 'AVG_COST',
            ITEM_KEY AS 'SKU',
            Vendor_ID AS 'REG_VEND_NUM_CZ',
            REPLACE(RTRIM(LEFT(Item_Description, 50)), CHAR(9), ' ') AS 'LONG_DESCRIPTION',
            Package_Desc1 AS CASE_SIZE,
            RetailUnit AS CASE_UOM
    FROM	
		InventoryServiceExportLoad (nolock)
    WHERE 
		Year = @Year AND 
		Period = @Period AND 
		ICVID = @ICVID AND
		BusinessUnit_ID = ISNULL(@BusinessUnit_ID, BusinessUnit_ID)
    ORDER BY 
		BusinessUnit_ID, SubTeam_No, Identifier
      		
    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO