IF EXISTS ( SELECT  *
            FROM    sysobjects
            WHERE   name = 'SLIM_VendorDealView'
                    AND xtype = 'v' ) 
    DROP VIEW [dbo].[SLIM_VendorDealView]
GO


--------------------------------------------------
-- The Fuax VendorDeal Table
--------------------------------------------------
CREATE VIEW [dbo].[SLIM_VendorDealView]
AS
    SELECT  ItemRequest.ItemRequest_ID AS VendorDeal_ID ,
            CAST(CASE WHEN allowances = 0 THEN NULL
                      ELSE allowances
                 END AS DECIMAL) AS allowance ,
            CAST(CASE WHEN discounts = 0 THEN NULL
                      ELSE discounts
                 END AS DECIMAL) AS discount ,
            CAST(CASE WHEN allowances IS NULL
                           OR allowances = 0 THEN NULL
                      ELSE allowancestartdate
                 END AS DATETIME) AS allowancestartdate ,
            CAST(CASE WHEN allowances IS NULL
                           OR allowances = 0 THEN NULL
                      ELSE allowanceenddate
                 END AS DATETIME) AS allowanceenddate ,
            CAST(CASE WHEN discounts IS NULL
                           OR discounts = 0 THEN NULL
                      ELSE discountstartdate
                 END AS DATETIME) AS discountstartdate ,
            CAST(CASE WHEN discounts IS NULL
                           OR discounts = 0 THEN NULL
                      ELSE discountenddate
                 END AS DATETIME) AS discountenddate
    FROM    ItemRequest (NOLOCK)
    WHERE   ItemStatus_ID = 2

GO


