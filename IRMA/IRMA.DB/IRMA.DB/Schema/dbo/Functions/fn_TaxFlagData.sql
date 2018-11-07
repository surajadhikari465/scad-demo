/****** Object:  UserDefinedFunction [dbo].[fn_TaxFlagData]    Script Date: 05/19/2006 16:35:12 ******/
CREATE  FUNCTION [dbo].[fn_TaxFlagData] 
	(@Item_Key int, @Store_No int)
RETURNS @TaxFlagValues TABLE (Store_No int, Item_Key int, TaxFlagKey char(1), TaxFlagValue bit, TaxPercent decimal(9,4), POSID int)
AS
BEGIN
	-- Read the tax information for the current item/store combination, making sure not to include any
	-- items with a TaxOverride value set for the store..
	-- Populate the @TaxFlagValues table with the results.
	INSERT INTO @TaxFlagValues (Store_No, Item_Key, TaxFlagKey, TaxFlagValue, TaxPercent, POSID)
		SELECT @Store_No, @Item_Key, TF.TaxFlagKey, TF.TaxFlagValue, TD.TaxPercent, TD.POSID   
		FROM TaxFlag TF, TaxDefinition TD 
		WHERE 
		TF.TaxFlagKey=TD.TaxFlagKey 
		AND
		TF.TaxJurisdictionID =  
			(SELECT TJ.TaxJurisdictionID 
			 FROM TaxJurisdiction TJ 
			 INNER JOIN Store S
			 ON TJ.TaxJurisdictionID=S.TaxJurisdictionID and S.Store_No=@Store_No)
		AND
		TD.TaxJurisdictionID =  
			(SELECT TJ.TaxJurisdictionID 
			 FROM TaxJurisdiction TJ 
			 INNER JOIN Store S
			 ON TJ.TaxJurisdictionID=S.TaxJurisdictionID and S.Store_No=@Store_No)
		AND
		TF.TaxClassID =
			(SELECT TC.TaxClassID 
			 FROM TaxClass TC 
			 INNER JOIN Item I
			 ON TC.TaxClassID=I.TaxClassID and I.Item_Key=@Item_Key)
		AND 
		TF.TaxFlagKey NOT IN 
			(SELECT TOV.TaxFlagKey 
			 FROM TaxOverride TOV 
			 WHERE TOV.Store_No=@Store_No and TOV.Item_Key=@Item_Key)

	-- Read any tax override information for the current item/store combination.
	-- Populate the @TaxFlagValues table with the results.
	INSERT INTO @TaxFlagValues (Store_No, Item_Key, TaxFlagKey, TaxFlagValue)
		SELECT @Store_No, @Item_Key, TOV.TaxFlagKey, TOV.TaxFlagValue 
		FROM TaxOverride TOV 
		WHERE TOV.Store_No=@Store_No and TOV.Item_Key=@Item_Key

	RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_TaxFlagData] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_TaxFlagData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_TaxFlagData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_TaxFlagData] TO [IRMAReportsRole]
    AS [dbo];

