IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_POSAudit_FindExceptions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_POSAudit_FindExceptions]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Replenishment_POSAudit_FindExceptions]
		@Store_No INT

AS

-- ****************************************************************************************************************
-- Procedure: Replenishment_POSAudit_FindExceptions()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Finds POS Audit exceptions?
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-06	KM		9251	Add update history template; Check ItemOverride for new 4.8 override values (Brand, Not_Available, LockAuth);
-- ****************************************************************************************************************

BEGIN
	
	-- Check the Store Jurisdiction Flag
	DECLARE @UseStoreJurisdictions int
	SELECT @UseStoreJurisdictions = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseStoreJurisdictions'
 
    DECLARE @SessionID INT
    SET @SessionID = (SELECT MAX(SessionID) + 1 FROM POSAuditException)
    SET @SessionID = ISNULL(@SessionID,1)

    -- First delete all previous audit exception records
    -- for the applicable store(s)
    DELETE FROM POSAuditException
    WHERE Store_No IN (SELECT DISTINCT
                            CASE
                                WHEN @Store_No = -1 THEN
                                    Store_No
                                ELSE
                                    @Store_No
                            END
                        FROM Store)

    -- Insert Price Exception records.
    -- Note: These exceptions were already detected and written to Temp_PriceAudit as part of the POSPull process.
    INSERT INTO POSAuditException
    (
		SessionID,
        Item_Key,
        Identifier,
        Store_No,
        POSAuditExceptionTypeID
	)
    SELECT DISTINCT 
        @SessionID,
        P.Item_Key,
        P.Identifier,
        P.Store_No,
        1
    FROM       
		POSItem           				(nolock) P
        INNER JOIN Temp_PriceAudit TPA	(nolock)	ON	P.Identifier		= TPA.Identifier 
													AND TPA.BusinessUnit_ID	= (SELECT BusinessUnit_ID FROM Store S WHERE S.Store_No = P.Store_No)
        WHERE P.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store)
        AND P.Item_Key <> -1


    -- Insert In-store special records.
    -- These are not exceptions; it is merely feedback as to the number
    -- of ISSs a store has.   
    INSERT INTO POSAuditException
    (
		SessionID,
        Item_Key,
        Identifier,
        Store_No,
        POSAuditExceptionTypeID
	)
    SELECT DISTINCT 
        @SessionID,
        P.Item_Key,
        P.Identifier,
        P.Store_No,
        2
    FROM       
		POSItem				(nolock) P
        INNER JOIN Price    (nolock) PR		ON	P.Item_Key			= PR.Item_Key 
											AND	P.Store_No			= PR.Store_No 
											AND PR.PriceChgTypeId	= (SELECT PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = 'ISS')
    WHERE 
		P.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store)

    -- Insert FoodStamp flag exceptions
    INSERT INTO POSAuditException
    (
		SessionID,
        Item_Key,
        Identifier,
        Store_No,
        POSAuditExceptionTypeID
	)
    SELECT DISTINCT 
        @SessionID,
        P.Item_Key,
        P.Identifier,
        P.Store_No,
        3
    FROM
		POSItem					P	(nolock)
        INNER JOIN	Item		I	(nolock)	ON	P.Item_Key     = I.Item_Key                                          
		INNER JOIN	Store		S	(nolock)	ON	P.Store_No	   = S.Store_No  	
		INNER JOIN	SubTeam		ST	(nolock)	ON	I.SubTeam_No   = ST.SubTeam_No
												AND ST.SubTeam_Name not like '%INGREDIENTS'
		LEFT JOIN	ItemOverride	(nolock)	ON	P.Item_Key     = ItemOverride.Item_Key  
												AND ItemOverride.StoreJurisdictionID = S.StoreJurisdictionID
												AND @UseStoreJurisdictions = 1
    WHERE 
		P.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store) AND
                (CASE
                    WHEN ItemOverride.Food_Stamps IS NOT NULL 
                    THEN ItemOverride.Food_Stamps
                    ELSE I.Food_Stamps
                 END <> P.Food_Stamps)

    -- Insert Tax flag exceptions
    INSERT INTO POSAuditException
    (
		SessionID,
        Item_Key,
        Identifier,
        Store_No,
        POSAuditExceptionTypeID
	)
    
	SELECT DISTINCT 
        @SessionID,
        P.Item_Key,
        P.Identifier,
        P.Store_No,
        4
    
	FROM       
		POSItem					P	(nolock)
        INNER JOIN Item         I	(nolock)	ON	P.Item_Key          = I.Item_Key
		INNER JOIN SubTeam		ST	(nolock)	ON	I.SubTeam_No		= ST.SubTeam_No
												AND ST.SubTeam_Name not like '%INGREDIENTS'
        INNER JOIN Store         S	(nolock)	ON	P.Store_No          = S.Store_No
        INNER JOIN TaxFlag     TF1 (nolock)		ON	I.TaxClassID		= TF1.TaxClassID        
												AND	S.TaxJurisdictionID = TF1.TaxJurisdictionID 
												AND	TF1.TaxFlagKey      = '1'
        INNER JOIN TaxFlag     TF2 (nolock)		ON	I.TaxClassID        = TF2.TaxClassID        
												AND	S.TaxJurisdictionID = TF2.TaxJurisdictionID 
												AND	TF2.TaxFlagKey      = '2'
        INNER JOIN TaxFlag     TF3 (nolock)		ON	I.TaxClassID        = TF3.TaxClassID        
												AND S.TaxJurisdictionID = TF3.TaxJurisdictionID 
												AND	TF3.TaxFlagKey      = '3'
        INNER JOIN TaxFlag     TF4 (nolock)		ON	I.TaxClassID        = TF4.TaxClassID        
												AND	S.TaxJurisdictionID = TF4.TaxJurisdictionID 
												AND	TF4.TaxFlagKey      = '4'
        LEFT  JOIN TaxOverride TO1 (nolock)		ON	P.Item_Key          = TO1.Item_Key          
												AND	P.Store_No          = TO1.Store_No          
												AND	TO1.TaxFlagKey      = '1'
        LEFT  JOIN TaxOverride TO2 (nolock)		ON	P.Item_Key          = TO2.Item_Key          
												AND	P.Store_No          = TO2.Store_No          
												AND	TO2.TaxFlagKey      = '2'
        LEFT  JOIN TaxOverride TO3 (nolock)		ON	P.Item_Key          = TO3.Item_Key          
												AND	P.Store_No          = TO3.Store_No          
												AND TO3.TaxFlagKey      = '3'
        LEFT  JOIN TaxOverride TO4 (nolock)		ON	P.Item_Key          = TO4.Item_Key          
												AND	P.Store_No          = TO4.Store_No          
												AND	TO4.TaxFlagKey      = '4'
    
	WHERE 
		((P.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store)) AND
                ((CASE
                    WHEN TO1.Item_Key IS NOT NULL THEN
                        TO1.TaxFlagValue
                    ELSE
                        TF1.TaxFlagValue
                  END <> P.Tax_Table_A) OR
                 (CASE
                    WHEN TO2.Item_Key IS NOT NULL THEN
                        TO2.TaxFlagValue
                    ELSE
                        TF2.TaxFlagValue
                  END <> P.Tax_Table_B) OR
                 (CASE
                    WHEN TO3.Item_Key IS NOT NULL THEN
                        TO3.TaxFlagValue
                    ELSE
                        TF3.TaxFlagValue
                  END <> P.Tax_Table_C) OR
                 (CASE
                    WHEN TO4.Item_Key IS NOT NULL THEN
                        TO4.TaxFlagValue
                    ELSE
                        TF4.TaxFlagValue
                  END <> P.Tax_Table_D)))
                

    -- Insert SubTeam (Department) exceptions
    INSERT INTO POSAuditException
    (
		SessionID,
        Item_Key,
        Identifier,
        Store_No,
        POSAuditExceptionTypeID
	)
    SELECT DISTINCT 
        @SessionID,
        P.Item_Key,
        P.Identifier,
        P.Store_No,
        5
    FROM       
		POSItem				P (nolock)
        INNER JOIN Item     I (nolock)	ON	P.Item_Key     = I.Item_Key 
										AND	P.SubTeam_No  <> I.SubTeam_No
    WHERE 
		P.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store)


    -- Insert SoldByWeight (Scale item) exceptions
    INSERT INTO POSAuditException
    (
		SessionID,
		Item_Key,
		Identifier,
		Store_No,
		POSAuditExceptionTypeID
	)
    SELECT DISTINCT 
        @SessionID,
        P.Item_Key,
        P.Identifier,
        P.Store_No,
        6
    FROM
		POSItem						P	(nolock)
        INNER JOIN Item				I	(nolock)	ON  P.Item_Key				= I.Item_Key 
		INNER JOIN SubTeam			ST	(nolock)	ON	I.SubTeam_No			= ST.SubTeam_No
													AND ST.SubTeam_Name not like '%INGREDIENTS'
        INNER JOIN ItemIdentifier	II	(nolock)	ON  P.Item_Key				= II.Item_Key 
													AND II.Default_Identifier	=  1
 		INNER JOIN Store			S	(nolock)	ON	P.Store_No				= S.Store_No  	
		LEFT  JOIN ItemOverride			(nolock)	ON	P.Item_Key				= ItemOverride.Item_Key  
													AND ItemOverride.StoreJurisdictionID = S.StoreJurisdictionID
													AND @UseStoreJurisdictions	= 1
        LEFT  JOIN ItemUnit			RU	(nolock)	ON	RU.Unit_ID				= I.Retail_Unit_ID
		LEFT  JOIN ItemUnit	RU_Override (nolock)	ON RU_Override.Unit_ID		= ItemOverride.Retail_Unit_ID
    WHERE 
		P.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store) AND
            (CASE 
                WHEN ISNULL(ISNULL(RU_Override.Weight_Unit, RU.Weight_Unit), 0) = 1 AND dbo.fn_IsScaleItem(II.Identifier) = 0 THEN 
                    1 
                ELSE 
                    0 
            END) <> P.SoldByWeight


    -- Insert "item in POS but not in IRMA" exceptions
    INSERT INTO POSAuditException
    (
		SessionID,
		Item_Key,
		Identifier,
		Store_No,
		POSAuditExceptionTypeID
	)
    SELECT DISTINCT 
        @SessionID,
        P.Item_Key,
        P.Identifier,
        P.Store_No,
        7
    FROM       
		POSItem   P (nolock)
    WHERE 
		P.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store) AND
             P.Item_Key = -1


    -- Insert "Deleted or Stop Sale items in POS" exceptions
    INSERT INTO POSAuditException
    (
		SessionID,
        Item_Key,
        Identifier,
        Store_No,
        POSAuditExceptionTypeID
	)
    SELECT DISTINCT 
        @SessionID,
        P.Item_Key,
        P.Identifier,
        P.Store_No,
        8
    FROM       
		POSItem				P	(nolock)
        INNER JOIN Item		I	(nolock)	ON	P.Item_Key		= I.Item_Key 
		INNER JOIN SubTeam	ST	(nolock)	ON	I.SubTeam_No	= ST.SubTeam_No
											AND ST.SubTeam_Name not like '%INGREDIENTS'
        INNER JOIN Price	PR	(nolock)	ON  P.Item_Key		= PR.Item_Key 
											AND P.Store_No		= PR.Store_No
    WHERE 
		((P.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store)) AND
              (I.Deleted_Item          = 1 OR
               PR.NotAuthorizedForSale = 1))


    -- Insert "Item authorized for the store in IRMA but not in POS" exceptions
    INSERT INTO POSAuditException
    (
		SessionID,
        Item_Key,
        Identifier,
        Store_No,
        POSAuditExceptionTypeID
	)
    SELECT DISTINCT 
        @SessionID,
        SI.Item_Key,
        II.Identifier,
        SI.Store_No,
        9
    FROM
	    StoreItem					SI	(nolock)
        INNER JOIN ItemIdentifier	II	(nolock)	ON	SI.Item_Key				= II.Item_Key 
													AND II.Default_Identifier	= 1
        LEFT JOIN  POSItem			P	(nolock)	ON	SI.Item_Key				= P.Item_Key 
													AND SI.Store_No				= P.Store_No
    WHERE 
		((SI.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store)) AND
               (SI.Authorized = 1)       AND
               (P.Item_Key IS NULL))

    -- Insert "Item not authorized for the store in IRMA but in POS" exceptions
    INSERT INTO POSAuditException
    (
		SessionID,
        Item_Key,
        Identifier,
        Store_No,
        POSAuditExceptionTypeID
	)
    SELECT DISTINCT 
        @SessionID,
        SI.Item_Key,
        P.Identifier,
        SI.Store_No,
        10
    FROM       
		POSItem					P	(nolock)
        INNER JOIN StoreItem	SI	(nolock)	ON	P.Item_Key    = SI.Item_Key 
												AND P.Store_No    = SI.Store_No 
												AND SI.Authorized = 0
    WHERE 
		SI.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store)


    --Select the exception records along with other data items 
    --that will be included in the exception file.
    SELECT DISTINCT 
        PA.Store_No                AS Store_No,
        CASE
            WHEN PA.POSAuditExceptionTypeID <> 9 THEN
                P.Identifier
            ELSE
                II.Identifier
        END                        AS Identifier,
        CASE
            WHEN PA.POSAuditExceptionTypeID <> 9 THEN
                P.POS_Description
            ELSE
                ISNULL(ItemOverride.Item_Description, I.Item_Description)
        END                        AS Item_Description,
        CASE
            WHEN S1.SubTeam_Name IS NOT NULL THEN
                S1.SubTeam_Name 
            WHEN S2.SubTeam_Name IS NOT NULL THEN
                S2.SubTeam_Name 
            ELSE
                ''
        END                        AS SubTeam_Name,
        CASE
            WHEN ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation) IS NOT NULL THEN
                    ISNULL(RU_Override.Unit_Abbreviation, RU.Unit_Abbreviation)
            ELSE
                ''
        END                        AS Unit_Abbreviation,
        PA.POSAuditExceptionTypeID AS ExceptionTypeID
    
	FROM       
		POSAuditException PA
        LEFT  JOIN Item               I (nolock)	ON  PA.Item_Key							= I.Item_Key
        LEFT  JOIN PosItem            P (nolock)	ON  PA.Item_Key							= P.Item_Key 
													AND PA.Store_No							= P.Store_No
        LEFT  JOIN SubTeam           S1 (nolock)	ON  P.SubTeam_No						= S1.SubTeam_No
        LEFT  JOIN SubTeam           S2 (nolock)	ON  I.SubTeam_No						= S2.SubTeam_No
        LEFT  JOIN ItemIdentifier    II (nolock)	ON  PA.Item_Key							= II.Item_Key 
													AND II.Default_Identifier				= 1
    	LEFT  JOIN Store			 S  (nolock)	ON  PA.Store_No							= S.Store_No  	
		LEFT  JOIN ItemOverride			(nolock)	ON  PA.Item_Key							= ItemOverride.Item_Key  
													AND  ItemOverride.StoreJurisdictionID	= S.StoreJurisdictionID
													AND  @UseStoreJurisdictions				= 1
        LEFT  JOIN ItemUnit			RU	(nolock)	ON   I.Package_Unit_ID					= RU.Unit_ID
		LEFT  JOIN ItemUnit RU_Override (nolock)	ON ItemOverride.Package_Unit_ID			= RU_Override.Unit_ID 
    
	WHERE 
		PA.Store_No IN (SELECT DISTINCT
                                CASE
                                    WHEN @Store_No = -1 THEN
                                        Store_No
                                    ELSE
                                        @Store_No
                                END
                            FROM Store)
        AND PA.SessionID = @SessionID                            
        ORDER BY Store_No, ExceptionTypeID, Identifier        

	-- Also select the resolution batch detail info
	SELECT
		II.Identifier,
		II.IdentifierType,
		ISNULL(ItemOverride.POS_Description, I.POS_Description),
		ISNULL(ItemOverride.Item_Description, I.Item_Description),
		ISNULL(ItemOverride.Package_Desc2, I.Package_Desc2) AS [Item Size],
		I.max_temperature AS [Tmp Range High],
		I.min_temperature AS [Tmp Range Low],
		I.high,
		I.Units_Per_Pallet,
		ISNULL(ItemOverride.Sign_Description, I.Sign_Description),
		ISNULL(ItemOverride.Package_Desc1, I.Package_Desc1) AS [Item Pack],
		I.Yield,
		I.Tie,
		CASE ISNULL(ItemOverride.Food_Stamps, I.Food_Stamps)
			WHEN 1 THEN 'TRUE'
			ELSE 'FALSE' END AS [Food Stamps],
		TC.TaxClassDesc AS [Tax Class],
		IB.Brand_Name,
		IC.Category_Name,
		NIF.NatFamilyName + ': ' + NICat.NatCatName + ': ' + NIClass.ClassName + ' - ' + Convert(char(4), NIClass.ClassID) AS [National Class],
		S.Store_Name,
		ST.SubTeam_Name,
		ISNULL(ItemUOM_Override.Unit_Name, ItemUOM.Unit_Name) AS [Item UOM], 
		ISNULL(RetailUOM_Override.Unit_Name, RetailUOM.Unit_Name) AS [Retail Units],
		ISNULL(DistributionUOM_Override.Unit_Name, DistributionUOM.Unit_Name) AS [Distribution Units], 
		ISNULL(VendorUOM_Override.Unit_Name, VendorUOM.Unit_Name) AS [Vendor Units], 
		V.Vendor_Key AS [PS Vendor],
		CASE P.NotAuthorizedForSale
			WHEN 1 THEN 'TRUE'
			ELSE 'FALSE' END AS [Stop Sale],
		CASE AE.POSAuditExceptionTypeID
			WHEN 1 THEN 'TRUE'					-- Price exception
			ELSE 'FALSE' END AS [Price Change],
		PCT.PriceChgTypeDesc AS [Price Type],
		P.Sale_End_Date AS [Promo Price End],
		P.Sale_Start_Date AS [Promo Price Start],
		P.POSPrice,
		P.POSSale_Price,
		P.Sale_Multiple,
		P.Multiple,
		P.MSRPPrice,
		P.MSRPMultiple,
		CASE P.SrCitizenDiscount
			WHEN 1 THEN 'FALSE'
			ELSE 'TRUE' END AS [Sr Discount],
		CASE SI.Authorized
			WHEN 1 THEN 'TRUE'
			ELSE 'FALSE' END AS Authorize,
		CASE ISNULL(ItemOverride.LockAuth, I.LockAuth)
			WHEN 1 THEN 'TRUE'
			ELSE 'FALSE' END AS [Auth Lock],
		CASE ISNULL(ItemOverride.Not_Available, I.Not_Available)
			WHEN 1 THEN 'TRUE'
			ELSE 'FALSE' END AS [Not Available]

	FROM
		POSAuditException AE
		INNER JOIN ItemIdentifier II				(nolock) ON AE.Item_Key = II.Item_Key
		LEFT OUTER JOIN Item I						(nolock) ON AE.Item_Key = I.Item_Key
		LEFT OUTER JOIN StoreItemVendor SIV			(nolock) ON AE.Item_Key = SIV.Item_Key AND AE.Store_No = SIV.Store_No AND SIV.PrimaryVendor = 1
		LEFT OUTER JOIN Vendor V					(nolock) ON SIV.Vendor_ID = V.Vendor_ID
		LEFT OUTER JOIN Price P						(nolock) ON AE.Item_Key = P.Item_Key AND AE.Store_No = P.Store_No
		LEFT OUTER JOIN StoreItem SI				(nolock) ON AE.Item_Key = SI.Item_Key AND AE.Store_No = SI.Store_No
		LEFT OUTER JOIN TaxClass TC					(nolock) ON I.TaxClassID = TC.TaxClassID
		LEFT OUTER JOIN ItemCategory IC				(nolock) ON I.Category_ID = IC.Category_ID
		LEFT OUTER JOIN NatItemClass NIClass		(nolock) ON I.ClassID = NIClass.ClassID
		LEFT OUTER JOIN NatItemCat NICat			(nolock) ON NICat.NatCatID = NIClass.NatCatID
		LEFT OUTER JOIN NatItemFamily NIF			(nolock) ON NIF.NatFamilyID = NICat.NatFamilyID
		LEFT OUTER JOIN Store S						(nolock) ON AE.Store_No = S.Store_No
		LEFT OUTER JOIN SubTeam ST					(nolock) ON I.SubTeam_No = ST.SubTeam_No
		LEFT OUTER JOIN ItemUnit ItemUOM			(nolock) ON I.Package_Unit_ID = ItemUOM.Unit_ID
		LEFT OUTER JOIN ItemUnit RetailUOM			(nolock) ON I.Retail_Unit_ID = RetailUOM.Unit_ID
		LEFT OUTER JOIN ItemUnit DistributionUOM	(nolock) ON I.Distribution_Unit_ID = DistributionUOM.Unit_ID
		LEFT OUTER JOIN ItemUnit VendorUOM			(nolock) ON I.Vendor_Unit_ID = VendorUOM.Unit_ID	
		LEFT OUTER JOIN PriceChgType PCT			(nolock) ON P.PriceChgTypeID = PCT.PriceChgTypeID
		LEFT       JOIN ItemOverride				(nolock) ON I.Item_Key = ItemOverride.Item_Key  
																AND  ItemOverride.StoreJurisdictionID = S.StoreJurisdictionID
																AND  @UseStoreJurisdictions = 1
		LEFT OUTER JOIN ItemBrand IB				(nolock) ON ISNULL(ItemOverride.Brand_ID, I.Brand_ID) = IB.Brand_ID
		LEFT	   JOIN ItemUnit ItemUOM_Override	(nolock) ON ItemOverride.Package_Unit_ID = ItemUOM_Override.Unit_ID 
		LEFT	   JOIN ItemUnit RetailUOM_Override	(nolock) ON ItemOverride.Retail_Unit_ID = RetailUOM_Override.Unit_ID
		LEFT       JOIN ItemUnit DistributionUOM_Override	(nolock) ON ItemOverride.Distribution_Unit_ID = DistributionUOM_Override.Unit_ID
		LEFT       JOIN ItemUnit VendorUOM_Override			(nolock) ON ItemOverride.Vendor_Unit_ID = VendorUOM_Override.Unit_ID	
	
	WHERE
		AE.SessionID = @SessionID
		AND
		AE.POSAuditExceptionTypeID <> 2		-- ISS type requires no changes
		AND
		AE.POSAuditExceptionTypeID <> 7		-- In POS, not in IRMA requires no changes
		AND
		II.Default_Identifier = 1
END
GO
SET QUOTED_IDENTIFIER ON
GO