CREATE PROCEDURE dbo.GetHandPrinterLabels
@Item_Key int,
@Store_No int
AS

BEGIN
    SET NOCOUNT ON
        
    SELECT 
	SQ.Item_Key, 
	SQ.Store_No, 
	Sign_Description, 
	Ingredients, 
	Identifier, 
	Multiple, 
	Price, 
	MSRPMultiple, 
	MSRPPrice, 
	ROUND(
		dbo.fn_Price(SQ.PriceChgTypeId, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price) * CasePriceDiscount * Package_Desc1, 2
	) As Case_Price, 
	Sale_Multiple, 
	Sale_Price, 
	Sale_Start_Date, 
	Sale_End_Date, 
	Sale_Earned_Disc1, 
	Sale_Earned_Disc2, 
	Sale_Earned_Disc3, 
	PricingMethod_ID, 
	SQ.SubTeam_No, 
	Origin_Name, 
	Brand_Name, 
	Retail_Unit_Abbr As Unit_Abbreviation, 
	Retail_Unit_Full As Unit_Name, 
	Package_Unit AS Unit, 
	Package_Desc1, 
	Package_Desc2, 
	Organic, 
	GETDATE() AS Insert_Date, 
	isnull(PCT.MSRP_Required,0) as Sale_EDLP,
	Vendor_ID, 
	SubTeam_Name,
	TagTypeID                                          
    FROM SignQueue SQ (nolock)
    INNER JOIN
        StoreSubTeam SST (nolock)
        ON SST.Store_No = SQ.Store_No AND SST.SubTeam_No = SQ.SubTeam_No
    INNER JOIN
        SubTeam (nolock)
        ON SubTeam.SubTeam_No = SQ.SubTeam_No
    LEFT JOIN
		PriceChgType PCT
		ON PCT.PriceChgTypeID = SQ.PriceChgTypeID
    WHERE SQ.Item_Key = @Item_Key
        AND SQ.Store_No = @Store_No
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetHandPrinterLabels] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetHandPrinterLabels] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetHandPrinterLabels] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetHandPrinterLabels] TO [IRMAReportsRole]
    AS [dbo];

