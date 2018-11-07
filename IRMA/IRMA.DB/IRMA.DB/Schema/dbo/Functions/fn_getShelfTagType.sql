/****** Object:  UserDefinedFunction [dbo].[fn_getShelfTagType]    Script Date: 06/22/2007 11:03:31 ******/
CREATE FUNCTION [dbo].[fn_getShelfTagType] 
 (@Item_Key INT, @LabelType_ID INT, @PriceChgTypeID INT, @ItemChgTypeID INT, @SubTeam_No INT, @TaxJurisdictionID INT, @Store_No INT, @Zone_ID INT, @UseExempt BIT, @UseEST BIT )

RETURNS @TblList TABLE
(Item_Key int PRIMARY KEY CLUSTERED, ShelfTag_Type INT, ShelfTag_Ext varchar(10), ShelfTagAttrDesc varchar(50), Exempt_ShelfTag_Type INT, 
 ShelfTag_Ext2 varchar(10), ShelfTagAttrDesc2 varchar(50))
AS
BEGIN
-- Updated 20071130 - Dave Stacey - for Electronic Shelftag Implementation
DECLARE @Exempt BIT, @ShelfTagPrimaryRuleTypeId INT

SELECT @ShelfTagPrimaryRuleTypeId = ShelfTagRuleTypeID 
FROM ShelfTagRuleType 
WHERE ShelfTagRuleDesc = 'Primary Rule'

-- default item's exempt status to false
SET @Exempt = 0
-- Set ItemAttribute record values for item - set variables to the corresponding desc field in the attribute table
-- This function sets up use of the first 10 checkboxes and the first 10 Text Item Attributes as potential rule triggers
-- If the attribute value is:  Checked or has a Text string in it, it is considered to be 
-- TRUE for the item and thus the query looks for these attributes in the shelftag rule table.
DECLARE @Check_Box_1 VARCHAR(10), @Check_Box_2 VARCHAR(10), @Check_Box_3 VARCHAR(10), @Check_Box_4 VARCHAR(10), @Check_Box_5 VARCHAR(10), @Check_Box_6 VARCHAR(10), @Check_Box_7 VARCHAR(10), 
@Check_Box_8 VARCHAR(10), @Check_Box_9 VARCHAR(9), @Check_Box_10 VARCHAR(10), @Check_Box_11 VARCHAR(10), @Check_Box_12 VARCHAR(10), @Check_Box_13 VARCHAR(10), @Check_Box_14 VARCHAR(10), 
@Check_Box_15 VARCHAR(10), @Check_Box_16 VARCHAR(10), @Check_Box_17 VARCHAR(10), @Check_Box_18 VARCHAR(10), @Check_Box_19 VARCHAR(10), @Check_Box_20 VARCHAR(10), 
@Text_1 VARCHAR(10), @Text_2 VARCHAR(10), @Text_3 VARCHAR(10), @Text_4 VARCHAR(10), @Text_5 VARCHAR(10), @Text_6 VARCHAR(10), @Text_7 VARCHAR(10), @Text_8 VARCHAR(10), @Text_9 VARCHAR(10), 
@Text_10 VARCHAR(10), @Text_11 VARCHAR(10), @Text_12 VARCHAR(10)

SELECT @Check_Box_1 = CASE WHEN Check_Box_1 > 0 THEN 'CheckBox1' END, 	@Check_Box_2 = CASE WHEN Check_Box_2 > 0 THEN 'CheckBox2' END, 
	@Check_Box_3 = CASE WHEN Check_Box_3 > 0 THEN 'CheckBox3' END, 	@Check_Box_4 = CASE WHEN Check_Box_4 > 0 THEN 'CheckBox4' END, 
	@Check_Box_5 = CASE WHEN Check_Box_5 > 0 THEN 'CheckBox5' END, 	@Check_Box_6 = CASE WHEN Check_Box_6 > 0 THEN 'CheckBox6' END, 
	@Check_Box_7 = CASE WHEN Check_Box_7 > 0 THEN 'CheckBox7' END, 	@Check_Box_8 = CASE WHEN Check_Box_8 > 0 THEN 'CheckBox8' END, 
	@Check_Box_9 = CASE WHEN Check_Box_9 > 0 THEN 'CheckBox9' END, 	@Check_Box_10 = CASE WHEN Check_Box_10 > 0 THEN 'CheckBox10' END,
	@Check_Box_11 = CASE WHEN Check_Box_11 > 0 THEN 'CheckBox11' END, 	@Check_Box_12 = CASE WHEN Check_Box_12 > 0 THEN 'CheckBox12' END, 
	@Check_Box_13 = CASE WHEN Check_Box_13 > 0 THEN 'CheckBox13' END, 	@Check_Box_14 = CASE WHEN Check_Box_14 > 0 THEN 'CheckBox14' END, 
	@Check_Box_15 = CASE WHEN Check_Box_15 > 0 THEN 'CheckBox15' END, 	@Check_Box_16 = CASE WHEN Check_Box_16 > 0 THEN 'CheckBox16' END, 
	@Check_Box_17 = CASE WHEN Check_Box_17 > 0 THEN 'CheckBox17' END, 	@Check_Box_18 = CASE WHEN Check_Box_18 > 0 THEN 'CheckBox18' END, 
	@Check_Box_19 = CASE WHEN Check_Box_19 > 0 THEN 'CheckBox19' END, 	@Check_Box_20 = CASE WHEN Check_Box_20 > 0 THEN 'CheckBox20' END,
	@Text_1 = CASE WHEN LEN(RTRIM(LTRIM(Text_1))) > 0 THEN 'Text1' END, @Text_2 = CASE WHEN LEN(RTRIM(LTRIM(Text_2))) > 0 THEN 'Text2' END, 
	@Text_3 = CASE WHEN LEN(RTRIM(LTRIM(Text_3))) > 0 THEN 'Text3' END, @Text_4 = CASE WHEN LEN(RTRIM(LTRIM(Text_4))) > 0 THEN 'Text4' END, 
	@Text_5 = CASE WHEN LEN(RTRIM(LTRIM(Text_5))) > 0 THEN 'Text5' END, @Text_6 = CASE WHEN LEN(RTRIM(LTRIM(Text_6))) > 0 THEN 'Text6' END, 
	@Text_7 = CASE WHEN LEN(RTRIM(LTRIM(Text_7))) > 0 THEN 'Text7' END, @Text_8 = CASE WHEN LEN(RTRIM(LTRIM(Text_8))) > 0 THEN 'Text8' END, 
	@Text_9 = CASE WHEN LEN(RTRIM(LTRIM(Text_9))) > 0 THEN 'Text9' END, @Text_10 = CASE WHEN LEN(RTRIM(LTRIM(Text_10))) > 0 THEN 'Text10' END
FROM dbo.itemattribute (NOLOCK)
WHERE item_key = @item_key

-- Hard-Coded for NA's special use of Exempt 
-- If region uses exempt (NA ONLY) and Tax Jurisdiction is Massachusetts (only state where exempt is used)
IF @UseExempt = 1 AND @TaxJurisdictionID = 2
BEGIN
-- See if Item Attribute is Exempt (NA uses Check Box 3 for this)
SET @Exempt = CASE WHEN LEN(@Check_Box_3) > 0 THEN 1 ELSE 0 END 
-- If not, check if it has a store-level exemption (more Mass. blue laws)
IF @Exempt <> 1
	SELECT @Exempt = 1 
	WHERE EXISTS (SELECT item_key
	FROM dbo.StoreItemAttribute (NOLOCK)
	WHERE Item_Key = @Item_Key and Store_No = @Store_No AND Exempt = 1)
END

-- This query inserts the highest-priority rule and primary tag info that applies to this item into a temp table
INSERT INTO @TblList (Item_Key, ShelfTag_Type, Exempt_ShelfTag_Type, ShelfTag_Ext, ShelfTagAttrDesc)
SELECT TOP 1 @Item_Key, st.ShelfTag_Type, CASE WHEN (@Exempt > 0 OR @UseEST = 1) THEN st.Exempt_ShelfTag_Type END, 
	sa.ShelfTag_Ext, sa.ShelfTagAttrDesc 
FROM dbo.shelftagrule st (NOLOCK)
	JOIN dbo.shelftagattribute sa (NOLOCK) ON sa.ShelfTag_Type = st.ShelfTag_Type
WHERE (ShelfTagRuleTypeID = @ShelfTagPrimaryRuleTypeId OR 
	st.AttributeIdentifier_ID IN (
		SELECT AI.AttributeIdentifier_ID
		FROM dbo.attributeidentifier AI (NOLOCK)
		WHERE AI.Field_Type IN (@Check_Box_1, @Check_Box_2, @Check_Box_3, @Check_Box_4, @Check_Box_5, @Check_Box_6,
			@Check_Box_7, @Check_Box_8, @Check_Box_9, @Check_Box_10,
			@Check_Box_11, @Check_Box_12, @Check_Box_13, @Check_Box_14, @Check_Box_15, @Check_Box_16,
			@Check_Box_17, @Check_Box_18, @Check_Box_19, @Check_Box_20,
			@Text_1, @Text_2, @Text_3, @Text_4, @Text_5, @Text_6, @Text_7, @Text_8, @Text_9, @Text_10)))
	AND ((st.LabelType_ID IS NULL OR st.LabelType_ID = @LabelType_ID) 
	AND (st.PriceChgTypeID IS NULL OR st.PriceChgTypeID = @PriceChgTypeID)
	AND (st.TaxJurisdictionID IS NULL OR st.TaxJurisdictionID = @TaxJurisdictionID)
	AND (st.Zone_ID IS NULL OR Zone_ID = @Zone_ID)
	AND (st.SubTeam_No IS NULL OR st.SubTeam_No = @SubTeam_No)
	AND (st.ItemChgTypeID IS NULL OR st.ItemChgTypeID = @ItemChgTypeID))
ORDER BY st.RulePriority

-- Here set up the second tag output type or Electronic ShelfTag 
IF @UseEST <> 0 
	-- set the EST shelf tag extension
	UPDATE @TblList SET ShelfTag_Ext2 = a.ShelfTag_Ext, ShelfTagAttrDesc2 = a.ShelfTagAttrDesc
	FROM dbo.shelftagattribute a
	JOIN @TblList b ON a.ShelfTag_Type = b.Exempt_ShelfTag_Type
ELSE If @Exempt = 1 --AND @UseExempt = 1 AND @TaxJurisdictionID = 2
	-- EXEMPT
	BEGIN 
		-- Mass. Only Large Reg Tags only output one - the exempt so both reg and exempt tags are set to be identical
		-- added SM_Reg logic for bug 8604 - Brian Robichaud
		DECLARE @LG_Reg INT, @RegType INT, @SM_Reg INT
		SELECT @RegType = ShelfTag_Type FROM @TblList
		SELECT @LG_Reg = ShelfTag_Type FROM dbo.ShelfTagAttribute WHERE ShelfTag_Ext = 'LG_REG'
		SELECT @SM_Reg = ShelfTag_Type FROM dbo.ShelfTagAttribute WHERE ShelfTag_Ext = 'SM_REG'
	
		IF @RegType = @LG_Reg OR @RegType = @SM_Reg
			-- Lined up both tagtypes to be exempt
			UPDATE @TblList SET ShelfTag_Type = b.Exempt_ShelfTag_Type, ShelfTag_Ext = a.ShelfTag_Ext, 
								ShelfTagAttrDesc = a.ShelfTagAttrDesc,  ShelfTag_Ext2 = a.ShelfTag_Ext, ShelfTagAttrDesc2 = a.ShelfTagAttrDesc
			FROM shelftagattribute a
			JOIN @TblList b ON a.ShelfTag_Type = b.Exempt_ShelfTag_Type
		ELSE
			-- set the exempt shelf tag extension for all other Mass. exempt types
			UPDATE @TblList SET ShelfTag_Ext2 = a.ShelfTag_Ext, ShelfTagAttrDesc2 = a.ShelfTagAttrDesc
			FROM dbo.shelftagattribute a
			JOIN @TblList b ON a.ShelfTag_Type = b.Exempt_ShelfTag_Type
	END

RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_getShelfTagType] TO [IRMAClientRole]
    AS [dbo];

