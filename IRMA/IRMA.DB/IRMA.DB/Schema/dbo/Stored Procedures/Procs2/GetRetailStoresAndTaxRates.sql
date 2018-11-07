CREATE PROCEDURE dbo.GetRetailStoresAndTaxRates
	@ItemKey as int
AS 
BEGIN
    SET NOCOUNT ON
    
SELECT
   Zone_id
      ,Zone_Name
      ,TaxRates.Store_No
      ,Store_Name
      ,Mega_Store
      ,WFM_Store
      ,State
      ,CustomerType
      ,SUM(TaxRates.TaxRate) as TaxRate
      ,isnull(siv.PrimaryVendor,0) as HasVendor
FROM (
            --------------------------------------------
            --Select all stores and their tax rates
            --------------------------------------------
            SELECT
                  Zone.Zone_id
                  ,Item.Item_Key  -- Added this RS
                  ,Zone_Name
                  ,Store.Store_No
                  ,Store_Name
                  ,Mega_Store
                  ,WFM_Store
                  ,ISNULL(Vendor.State, '') As State
                  ,dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType -- 3 = Regional
                  ,CASE WHEN (TOV.TaxFlagValue IS NOT NULL)
                        THEN CASE WHEN (TOV.TaxFlagValue = 1)
                              THEN TD.TaxPercent
                              ELSE 0
                              END
                        ELSE CASE WHEN (TF.TaxFlagValue = 1)
                              THEN TD.TaxPercent
                              ELSE 0
                              END
                        END as TaxRate
            FROM
                  Store (NOLOCK)
            CROSS JOIN
                  Item (NOLOCK)
            LEFT JOIN
                        Zone (nolock)
                        ON Zone.Zone_Id = Store.Zone_Id
            LEFT JOIN
                        Vendor (nolock)
                        ON Store.Store_No = Vendor.Store_No
                  AND (Store.Mega_Store = 1 OR Store.WFM_Store = 1)
            LEFT JOIN
                  TaxFlag TF
                  ON (TF.TaxClassID = Item.TaxClassID AND TF.TaxJurisdictionID = Store.TaxJurisdictionID)
            LEFT JOIN
                  TaxDefinition TD
                  ON (TD.TaxJurisdictionID = TF.TaxJurisdictionID AND TD.TaxFlagKey = TF.TaxFlagKey)
            LEFT JOIN
                  TaxOverride TOV
                  ON TOV.Item_Key = Item.Item_Key AND TOV.Store_No = Store.Store_No AND TOV.TaxFlagKey = TF.TaxFlagKey
            WHERE
                  (Mega_Store = 1 OR WFM_Store = 1) AND Item.Item_Key = @ItemKey

      UNION

            -------------------------------------------------------------------------------
            -- Select any overrides that are not associated to current taxflags.
            -- ie. overrides that are not overriding existing tax flags.
            -------------------------------------------------------------------------------
            SELECT
                  Zone.Zone_id
                  ,Item.Item_Key
                  ,Zone_Name
                  ,Store.Store_No
                  ,Store_Name
                  ,Mega_Store
                  ,WFM_Store
                  ,ISNULL(Vendor.State, '') As State
                  ,dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) as CustomerType -- 3 = Regional
                  ,TD.TaxPercent
            FROM
                  Store
            CROSS JOIN
                  Item
            LEFT JOIN
                        Zone (nolock)
                        ON Zone.Zone_Id = Store.Zone_Id
            LEFT JOIN
                        Vendor (nolock)
                        ON Store.Store_No = Vendor.Store_No
                  AND (Store.Mega_Store = 1 OR Store.WFM_Store = 1)
            RIGHT JOIN
                  TaxOverride TOV
                  ON (TOV.Item_Key = Item.Item_Key AND TOV.Store_No = Store.Store_No)
                  AND TOV.TaxFlagKey NOT IN
                        (SELECT TaxFlagKey
                         FROM TaxDefinition
                         WHERE TaxJurisdictionID = Store.TaxJurisdictionID)
            LEFT JOIN
                  TaxDefinition TD
                  ON (TOV.TaxFlagKey = TD.TaxFlagKey AND
                        TD.TaxJurisdictionID = Store.TaxJurisdictionID)
          WHERE
                 Item.Item_Key = @ItemKey) AS TaxRates

			LEFT OUTER JOIN StoreItemVendor SIV
			   ON ( SIV.Item_Key = TaxRates.Item_Key
					and  SIV.Store_No = TaxRates.Store_No
					and SIV.PrimaryVendor = 1 )

GROUP BY   Store_Name
		  ,Taxrates.Store_No
		  ,Zone_ID
		  ,Zone_Name
		  ,Mega_Store
		  ,WFM_Store
		  ,State
		  ,CustomerType
		  ,SIV.PrimaryVendor

ORDER BY Store_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoresAndTaxRates] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoresAndTaxRates] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoresAndTaxRates] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailStoresAndTaxRates] TO [IRMAReportsRole]
    AS [dbo];

