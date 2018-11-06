SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTaxRateForStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetTaxRateForStore]
GO

CREATE PROCEDURE dbo.GetTaxRateForStore
	@Item_Key as int,
	@Store_No as int
AS 
BEGIN
    SET NOCOUNT ON
    
SELECT
	SUM(TaxRates.TaxRate) as TaxRate
FROM
	(
            --------------------------------------------
            --Select the stores's tax rates
            --------------------------------------------
			SELECT
				CASE WHEN (TOV.TaxFlagValue IS NOT NULL)
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
				Zone (NOLOCK)
				ON Zone.Zone_Id = Store.Zone_Id
            LEFT JOIN
                  TaxFlag TF (NOLOCK)
                  ON (TF.TaxClassID = Item.TaxClassID AND TF.TaxJurisdictionID = Store.TaxJurisdictionID)
            LEFT JOIN
                  TaxDefinition TD (NOLOCK)
                  ON (TD.TaxJurisdictionID = TF.TaxJurisdictionID AND TD.TaxFlagKey = TF.TaxFlagKey)
            LEFT JOIN
                  TaxOverride TOV (NOLOCK)
                  ON TOV.Item_Key = Item.Item_Key AND TOV.Store_No = Store.Store_No AND TOV.TaxFlagKey = TF.TaxFlagKey
			WHERE
				Item.Item_Key = @Item_Key
				AND Store.Store_No = @Store_No

      UNION

			-------------------------------------------------------------------------------
			-- Select any overrides that are not associated to current taxflags.
			-- ie. overrides that are not overriding existing tax flags.
			-------------------------------------------------------------------------------
			SELECT
				TD.TaxPercent
            FROM
                Store (NOLOCK)
            CROSS JOIN
                Item (NOLOCK)
            LEFT JOIN
				Zone (NOLOCK)
					ON Zone.Zone_Id = Store.Zone_Id
            LEFT JOIN
                Vendor (NOLOCK)
					ON Store.Store_No = Vendor.Store_No
					AND (Store.Mega_Store = 1 OR Store.WFM_Store = 1)
            RIGHT JOIN
                  TaxOverride TOV (NOLOCK)
					  ON (TOV.Item_Key = Item.Item_Key AND TOV.Store_No = Store.Store_No)
					  AND TOV.TaxFlagKey NOT IN
						(SELECT TaxFlagKey
						 FROM TaxDefinition (NOLOCK)
						 WHERE TaxJurisdictionID = Store.TaxJurisdictionID)
            LEFT JOIN
                  TaxDefinition TD (NOLOCK)
                  ON (TOV.TaxFlagKey = TD.TaxFlagKey AND
                        TD.TaxJurisdictionID = Store.TaxJurisdictionID)
          WHERE
				Item.Item_Key = @Item_Key
				AND Store.Store_No = @Store_No

	) AS TaxRates

    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


