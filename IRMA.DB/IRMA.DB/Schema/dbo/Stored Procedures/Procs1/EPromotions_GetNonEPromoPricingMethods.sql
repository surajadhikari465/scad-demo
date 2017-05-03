CREATE PROCEDURE dbo.EPromotions_GetNonEPromoPricingMethods
AS
BEGIN


-- this needs to be renamed and to use the new field instead of EnableOfferEditor.  

	SET NOCOUNT ON;

	-- return a list of PricingMethods that are excluded from the Epromotions screens.
	-- This is to be used on the Promotion Change Screen.
	SELECT [PricingMethod_ID], [PricingMethod_Name], [EnableOfferEditor]
	FROM [PricingMethod]
	WHERE ([EnableOfferEditor] = 0)
	ORDER BY PricingMethod_Name

END