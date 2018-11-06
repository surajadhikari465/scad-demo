IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID(N'dbo.VIM365StoreFile'))
	EXEC('CREATE PROCEDURE [dbo].[VIM365StoreFile] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[VIM365StoreFile]
AS
BEGIN
   -- **************************************************************************
   -- Procedure: VIMStoreFile
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 11/06/2009  BBB	update existing SP to specifically declare table source 
   --					for BusinessUnit_ID column to prevent ambiguity between
   --					Store and Vendor table
   -- **************************************************************************

   SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @365RegionCode varchar(2) = 'TS';

SELECT Store.BusinessUnit_ID AS PS_BU,
       SUBSTRING ('00000', 1, 5 - LEN(Store.Store_No)) + CONVERT(varchar(5),Store.Store_No) AS REG_STORE_NUM,
       @365RegionCode	AS REGION, 
       Store_Name		AS STORE_NAME,
       'NULL'			AS STORE_ABBR,
       'IBM'			AS POS_TYPE,
       Address_Line_1	AS ADDR1,
       Address_Line_2	AS ADDR2,
       City				AS CITY,
       State			AS STATE,
       Zip_Code			AS ZIP,
       Vendor.Phone		AS PHONE,
       Vendor.Fax		AS FAX,
       'NULL'			AS LAST_USER
FROM Store (NOLOCK)
	INNER JOIN Vendor (NOLOCK) ON Store.Store_No = Vendor.Store_No
WHERE (MEGA_STORE = 1 AND WFM_STORE = 0)
		and Store.Store_No in (select store_no from storeregionmapping where region_code = @365RegionCode)

 END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

