CREATE PROCEDURE dbo.Scale_GetExtraTextByItem 
	@Item_Key as INT,
	@StoreJurisdictionID as INT
AS

BEGIN

IF ISNULL(@StoreJurisdictionID, 0) = 0 

-- if no StoreJurisdictionID is specified, pull the default jurisdiction data
	SELECT		dbo.ItemScale.Scale_ExtraText_ID, dbo.Scale_ExtraText.ExtraText, dbo.Scale_ExtraText.Description,dbo.Scale_ExtraText.Scale_LabelType_ID
	FROM		dbo.ItemScale INNER JOIN
				dbo.Scale_ExtraText ON dbo.ItemScale.Scale_ExtraText_ID = dbo.Scale_ExtraText.Scale_ExtraText_ID
	WHERE
				dbo.ItemScale.Item_Key = @Item_Key

-- otherwise, pull data for the specified jurisdiction from the ItemScaleOverride table
ELSE
    SELECT		dbo.ItemScaleOverride.Scale_ExtraText_ID, dbo.Scale_ExtraText.ExtraText, dbo.Scale_ExtraText.Description,dbo.Scale_ExtraText.Scale_LabelType_ID
	FROM		dbo.ItemScaleOverride INNER JOIN
				dbo.Scale_ExtraText ON dbo.ItemScaleOverride.Scale_ExtraText_ID = dbo.Scale_ExtraText.Scale_ExtraText_ID
	WHERE
				dbo.ItemScaleOverride.Item_Key = @Item_Key and dbo.ItemScaleOverride.StoreJurisdictionID = @StoreJurisdictionID
   
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraTextByItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_GetExtraTextByItem] TO [IRMAClientRole]
    AS [dbo];

