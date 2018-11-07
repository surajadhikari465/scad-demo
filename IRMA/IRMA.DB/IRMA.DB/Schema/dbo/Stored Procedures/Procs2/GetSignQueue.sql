CREATE PROCEDURE [dbo].[GetSignQueue]
    @ItemList varchar(max),
    @ItemListSeparator char(1),
    @Store_No int,
    @MarkPrinted bit, 
	@StartLabelPosition AS INT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    IF @MarkPrinted = 1
    BEGIN
        EXEC UpdateSignQueuePrinted 
            @ItemList, 
            @ItemListSeparator, 
            @Store_No, 1, 
            NULL, 
            NULL

        SELECT @error_no = @@ERROR

        IF @error_no <> 0
        BEGIN
            SET NOCOUNT OFF
            DECLARE @Severity smallint
            SELECT 
                @Severity = ISNULL(
                    (SELECT severity 
                        FROM master.dbo.sysmessages 
                        WHERE error = @error_no
                    ), 16)
            RAISERROR (
                'GetSignQueue failed with @@ERROR: %d', 
                @Severity, 
                1, 
                @error_no)
            RETURN
        END
    END

	SELECT TOP (@StartLabelPosition - 1)
			NULL AS Origin_Name, 
			NULL AS Brand_Name, 
			'' AS Identifier, 
			'' AS CheckDigit,
			'' AS Sign_Description, 
			'' AS Ingredients,
			0 AS Multiple, 
			0 AS Price, 
			0 AS MSRPPrice, 
			0 AS Sale_Multiple, 
			0 AS Sale_Price, 
			NULL AS Sale_Start_Date, 
			NULL AS Sale_End_Date, 
			'' AS Retail_Unit_Abbr, 
			'' AS Retail_Unit_Full, 
			0 AS Sold_By_weight, 
			0 AS Organic, 
			0 AS Package_Desc2, 
			'' AS Package_Unit, 
			'' AS SubTeam_Name, 
			0 AS On_Sale,
			NULL AS PricingMethod_ID, 
			0 AS Package_Desc1, 
			0 AS Case_Price, 
			0 AS Vendor_ID,
			'' AS Vendor_Key,
			'' AS VendorItemID,
			0 AS Item_Key,
			NULL AS PLU,
			0 AS LabelType_ID,
			0 AS CurrentPrice, 
			0 AS POSPrice,
			0 AS POSSale_Price,
			0 as category_ID,
			0 as TagTypeID
	FROM ITEM

	UNION ALL 

    SELECT 
        Origin_Name, 
        Brand_Name, 
        SQ.Identifier, 
        II.CheckDigit,
        SQ.Sign_Description, 
        CASE 
            WHEN SQ.Ingredients <> '0' 
                THEN SQ.Ingredients 
                ELSE '' 
            END As Ingredients,
        Multiple, 
        Price, 
        MSRPPrice, 
        Sale_Multiple, 
        Sale_Price, 
        Sale_Start_Date, 
        Sale_End_Date, 
        Retail_Unit_Abbr, 
        Retail_Unit_Full, 
        Sold_By_weight, 
        SQ.Organic, 
        SQ.Package_Desc2, 
        Package_Unit, 
        SubTeam.SubTeam_Name, 
        dbo.fn_OnSale(SQ.PriceChgTypeID) as On_Sale,
        SQ.PricingMethod_ID, 
		dbo.fn_GetCurrentVendorPackage_Desc1(SQ.Item_Key, @Store_No) AS Package_Desc1,
        Case_Price, 
        SQ.Vendor_ID,
        V.Vendor_Key,
		IV.Item_ID AS VendorItemID,
        SQ.Item_Key,
        CASE 
            WHEN (LEFT(SQ.Identifier, 1) = '2') AND (LEN(SQ.Identifier) = 11) 
                THEN SUBSTRING(SQ.Identifier, 2, 5) 
                ELSE NULL 
            END AS PLU,
        I.LabelType_ID,
        dbo.fn_PricingMethodMoney(SQ.PriceChgTypeId, SQ.PricingMethod_ID, SQ.POSPrice, SQ.POSSale_Price) as CurrentPrice, 
        SQ.POSPrice,
        SQ.POSSale_Price,
		I.Category_ID,
		SQ.TagTypeID
    FROM SignQueue SQ (nolock)
        INNER JOIN SubTeam (nolock)
            ON (SQ.SubTeam_No = SubTeam.SubTeam_No)
        INNER JOIN fn_Parse_List(@ItemList, @ItemListSeparator) IL
            ON IL.Key_Value = SQ.Item_Key 
        INNER JOIN Item I (nolock)
            ON I.Item_Key = IL.Key_Value
        LEFT JOIN Vendor V (nolock)
            ON SQ.Vendor_ID = V.Vendor_ID
        LEFT JOIN ItemVendor IV (nolock) 
			ON IV.Item_Key = I.Item_Key AND IV.Vendor_ID = SQ.Vendor_ID
        INNER JOIN ItemIdentifier II (nolock)
            ON SQ.Item_Key = II.Item_Key
    WHERE SQ.Store_No = @Store_No
        AND II.Default_Identifier = 1

    Order by subteam_name, category_ID, Sign_Description

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignQueue] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignQueue] TO [IRMAReportsRole]
    AS [dbo];

