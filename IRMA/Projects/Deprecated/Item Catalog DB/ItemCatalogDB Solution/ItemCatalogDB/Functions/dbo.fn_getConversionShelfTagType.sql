IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_getConversionShelfTagType]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_getConversionShelfTagType]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_getConversionShelfTagType]
(@Item_Key INT, @LabelType_ID INT, @PriceChgTypeID INT, @SubTeam_No INT, @TaxJurisdictionID INT, @Store_No INT)
RETURNS @TblList TABLE
(Item_Key int, LabelType_ID INT, PriceChgTypeID INT, SubTeam_No INT, TaxJurisdictionID INT, Store_No INT, ShelfTag_Type varchar(5), ShelfTag_Ext varchar(10), ShelfTagAttrDesc varchar(50), Exempt_ShelfTag_Type varchar(5), ShelfTag_Ext2 varchar(10), ShelfTagAttrDesc2 varchar(50))
AS
BEGIN
--DECLARE @Item_Key INT, @LabelType_ID INT, @PriceChgTypeID INT, @SubTeam_No INT, @TaxJurisdictionID INT, @Store_No INT
--DECLARE @TblList TABLE
--(Item_Key int, ShelfTag_Type INT, ShelfTag_Ext VarChar(10), ShelfTagAttrDesc varchar(50), Exempt_ShelfTag_Type INT, ShelfTag_Ext2 varchar(10), ShelfTagAttrDesc2 varchar(50))
--SET NOCOUNT ON
--DECLARE @Item_Key INT, @LabelType_ID INT, @PriceChgTypeID INT, @SubTeam_No INT, @TaxJurisdictionID INT, @Store_No INT
--DECLARE @TblList TABLE
--(Item_Key int, ShelfTag_Type INT, ShelfTag_Ext VarChar(10), ShelfTagAttrDesc varchar(50), Exempt_ShelfTag_Type INT, ShelfTag_Ext2 varchar(10), ShelfTagAttrDesc2 varchar(50))

DECLARE @Exempt INT, @ShelfTag varchar(10), @Zone_ID INT
SET @Exempt = 0
SELECT @Zone_ID = Zone_ID FROM store
WHERE store_no = @Store_No

--SET @Item_Key = 36655  -- exists in store item exempt table
--SET @LabelType_ID = 2 --
--SET @PriceChgTypeID = 8
--SET @SubTeam_No = 4230
--SET @TaxJurisdictionID = 2
--SET @Store_No = 925
-- For MA items, we need to check for exemptions on the Item attribute as well as the item store attribute level
--SELECT @SpecDiet = Case LEN(RTRIM(LTRIM(Text1))) > 0 then 1 else 0 end
--FROM dbo.itemattribute
--WHERE item_key = @item_key

DECLARE @RecCount INT, @Check_Box_A_1 INT, @Check_Box_A_2 INT, @Check_Box_A_3 INT, @Check_Box_A_4 INT, @Check_Box_A_5 INT, @Check_Box_A_6 INT,
@Check_Box_A_7 INT, @Check_Box_A_8 INT, @Check_Box_A_9 INT, @Check_Box_A_10 INT,
@Text_A_1 INT, @Text_A_2 INT, @Text_A_3 INT, @Text_A_4 INT, @Text_A_5 INT, @Text_A_6 INT, @Text_A_7 INT, @Text_A_8 INT, @Text_A_9 INT, @Text_A_10 INT, @Text_B_1 INT
-- Check for ItemAttribute record
SELECT @RecCount = count(item_key) 
FROM dbo.itemattribute WHERE item_key = @item_key 
-- If there's a record, set all variables at once.

IF @RecCount > 0 
	Select @Check_Box_A_1 = Check_Box_1, @Check_Box_A_2 = Check_Box_2, @Exempt = check_box_3, @Check_Box_A_4 = Check_Box_4, @Check_Box_A_5 = Check_Box_5, 
	@Check_Box_A_6 = Check_Box_6, @Check_Box_A_7 = Check_Box_7, @Check_Box_A_8 = Check_Box_8, @Check_Box_A_9 = Check_Box_9, @Check_Box_A_10 = Check_Box_10, 
	@Text_A_1 = CASE WHEN Text_1 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_1))) END, 
	@Text_A_2 = CASE WHEN Text_2 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_2))) END, 
	@Text_A_3 = CASE WHEN Text_3 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_3))) END, 
	@Text_A_4 = CASE WHEN Text_4 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_4))) END,
	@Text_A_5 = CASE WHEN Text_5 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_5))) END, 
	@Text_A_6 = CASE WHEN Text_6 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_6))) END, 
	@Text_A_7 = CASE WHEN Text_7 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_7))) END, 
	@Text_A_8 = CASE WHEN Text_8 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_8))) END,
	@Text_A_9 = CASE WHEN Text_9 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_9))) END, 
	@Text_A_10 = CASE WHEN Text_10 is null then 0 ELSE LEN(RTRIM(LTRIM(Text_10))) END
	from dbo.itemattribute
	where item_key = @item_key
-- Otherwise, set all attribute and exempt to 0
ELSE 
Select  @Check_Box_A_1 = 0, @Check_Box_A_2 = 0, @Exempt = 0, @Check_Box_A_4 = 0, @Check_Box_A_5 = 0, 
	@Check_Box_A_6 = 0, @Check_Box_A_7 = 0, @Check_Box_A_8 = 0, @Check_Box_A_9 = 0, @Check_Box_A_10 = 0, 
	@Text_A_1 = 0, @Text_A_2 = 0, @Text_A_3 = 0, @Text_A_4 = 0,
	@Text_A_5 = 0, @Text_A_6 = 0, @Text_A_7 = 0, @Text_A_8 = 0,
	@Text_A_9 = 0, @Text_A_10 = 0


--PRINT ' value ' + Cast(@Check_Box_A_1 as varchar(5))

IF @Exempt <> 1
BEGIN 
	SELECT @RecCount = count(item_key) 
	FROM dbo.StoreItemAttribute
	WHERE Item_Key = @Item_Key and Store_No = @Store_No
IF @RecCount > 0
	SELECT @Exempt = Exempt
	FROM dbo.StoreItemAttribute
	WHERE Item_Key = @Item_Key and Store_No = @Store_No
END

IF @TaxJurisdictionID <> 2
	SET @Exempt = NULL
DECLARE @Check_Box_1 INT, @Check_Box_2 INT, @Check_Box_3 INT, @Check_Box_4 INT, @Check_Box_5 INT, @Check_Box_6 INT,
@Check_Box_7 INT, @Check_Box_8 INT, @Check_Box_9 INT, @Check_Box_10 INT, @Text_1 INT, @Text_2 INT, @Text_3 INT, 
@Text_4 INT, @Text_5 INT, @Text_6 INT, @Text_7 INT, @Text_8 INT, @Text_9 INT, @Text_10 INT, 
@AttributeIdentifier_ID INT, @field_type varchar(10), @RegType INT

DECLARE attr_cursor CURSOR FOR 
SELECT distinct ai.AttributeIdentifier_ID, ai.field_type
 FROM shelftagrule st
	JOIN attributeidentifier ai ON ai.AttributeIdentifier_ID = st.AttributeIdentifier_ID
	WHERE ai.Screen_Text <> 'Exempt'
OPEN attr_cursor

FETCH NEXT FROM attr_cursor 
INTO @AttributeIdentifier_ID, @field_type

WHILE @@FETCH_STATUS = 0
BEGIN
If @field_type = 'CheckBox1' 
	SET @Check_Box_1 = @AttributeIdentifier_ID
ELSE If @field_type = 'CheckBox2' 
	SET @Check_Box_2 = @AttributeIdentifier_ID
ELSE If @field_type = 'CheckBox3' 
	SET @Check_Box_3 = @AttributeIdentifier_ID
ELSE If @field_type = 'CheckBox4' 
	SET @Check_Box_4 = @AttributeIdentifier_ID
ELSE If @field_type = 'CheckBox5' 
	SET @Check_Box_5 = @AttributeIdentifier_ID
ELSE If @field_type = 'CheckBox6' 
	SET @Check_Box_6 = @AttributeIdentifier_ID
ELSE If @field_type = 'CheckBox7' 
	SET @Check_Box_7 = @AttributeIdentifier_ID
ELSE If @field_type = 'CheckBox8' 
	SET @Check_Box_8 = @AttributeIdentifier_ID
ELSE If @field_type = 'CheckBox9' 
	SET @Check_Box_9 = @AttributeIdentifier_ID
ELSE If @field_type = 'CheckBox10' 
	SET @Check_Box_10 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text1' 
	SET @Text_1 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text2' 
	SET @Text_2 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text3' 
	SET @Text_3 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text4' 
	SET @Text_4 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text5' 
	SET @Text_5 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text6' 
	SET @Text_6 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text7' 
	SET @Text_7 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text8' 
	SET @Text_8 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text9' 
	SET @Text_9 = @AttributeIdentifier_ID
ELSE If @field_type = 'Text10' 
	SET @Text_10 = @AttributeIdentifier_ID
--PRINT 'AttributeID = ' + ' ' + Cast(@AttributeIdentifier_ID as varchar(5)) + ' Field Type =  ' + @field_type + ' value ' + Cast(@Check_Box_A_1 as varchar(5))

FETCH NEXT FROM attr_cursor 
INTO @AttributeIdentifier_ID, @field_type
END 
CLOSE attr_cursor
DEALLOCATE attr_cursor

If @Check_Box_A_1 = 0
	SET @Check_Box_1 = NULL
If @Check_Box_A_2 = 0 
	SET @Check_Box_2 = NULL
-- NA using this for exempt
--ELSE If @Check_Box_A_3 = 0
--	SET @Check_Box_3 = NULL
If @Check_Box_A_4 = 0
	SET @Check_Box_4 = NULL
If @Check_Box_A_5 = 0
	SET @Check_Box_5 = NULL
If @Check_Box_A_6 = 0
	SET @Check_Box_6 = NULL
If @Check_Box_A_7 = 0
	SET @Check_Box_7 = NULL
If @Check_Box_A_8 = 0 
	SET @Check_Box_8 = NULL
If @Check_Box_A_9 = 0
	SET @Check_Box_9 = NULL
If @Check_Box_A_10 = 0 
	SET @Check_Box_10 = NULL
If @Text_A_1 = 0 
	SET @Text_1 = NULL
If @Text_A_2 = 0
	SET @Text_2 = NULL
If @Text_A_3 = 0
	SET @Text_3 = NULL
If @Text_A_4 = 0
	SET @Text_4 = NULL
If @Text_A_5 = 0 
	SET @Text_5 = NULL
If @Text_A_6 = 0 
	SET @Text_6 = NULL
If @Text_A_7 = 0
	SET @Text_7 = NULL
If @Text_A_8 = 0 
	SET @Text_8 = NULL
If @Text_A_9 = 0
	SET @Text_9 = NULL
If @Text_A_10 = 0 
	SET @Text_10 = NULL
--PRINT @Exempt
-- Primary Rule
INSERT INTO @TblList (Item_Key, ShelfTag_Type, Exempt_ShelfTag_Type)
SELECT TOP 1 @Item_Key, ShelfTag_Type, Exempt_ShelfTag_Type--, RulePriority
FROM dbo.shelftagrule
WHERE
(
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID = @Zone_ID AND Store_No = @Store_No)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID is Null AND Store_No = @Store_No)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID is Null and Zone_ID is Null AND Store_No = @Store_No)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID is Null and Zone_ID = @Zone_ID AND Store_No = @Store_No)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID = @Zone_ID AND Store_No = @Store_No)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID is Null and Zone_ID is Null AND Store_No = @Store_No)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID is Null AND Store_No = @Store_No)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID is Null and Zone_ID = @Zone_ID AND Store_No = @Store_No)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID = @Zone_ID AND Store_No is null)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID is Null AND Store_No is null)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID is Null and Zone_ID is Null AND Store_No is null)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID is Null and Zone_ID = @Zone_ID AND Store_No is null)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID = @Zone_ID AND Store_No is null)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID is Null and Zone_ID is Null AND Store_No is null)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID is Null AND Store_No is null)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID is Null and Zone_ID = @Zone_ID AND Store_No is null)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID = @Zone_ID AND Store_No = @Store_No)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID is Null AND Store_No = @Store_No)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID is Null and Zone_ID is Null AND Store_No = @Store_No)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID is Null and Zone_ID = @Zone_ID AND Store_No = @Store_No)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID = @Zone_ID AND Store_No = @Store_No)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID is Null and Zone_ID is Null AND Store_No = @Store_No)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID is Null AND Store_No = @Store_No)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID is Null and Zone_ID = @Zone_ID AND Store_No = @Store_No)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID = @Zone_ID AND Store_No is null)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID is Null AND Store_No is null)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID is Null and Zone_ID is Null AND Store_No is null)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No = @SubTeam_No and TaxJurisdictionID is Null and Zone_ID = @Zone_ID AND Store_No is null)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID = @Zone_ID AND Store_No is null)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID is Null and Zone_ID is Null AND Store_No is null)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID = @TaxJurisdictionID and Zone_ID is Null AND Store_No is null)
OR
(LabelType_ID is null and PriceChgTypeID = @PriceChgTypeID and SubTeam_No is Null and TaxJurisdictionID is Null and Zone_ID = @Zone_ID AND Store_No is null)
OR
(LabelType_ID = @LabelType_ID and PriceChgTypeID  is Null and SubTeam_No is Null and TaxJurisdictionID is Null and Zone_ID = @Zone_ID AND Store_No is null)
)
AND
(ShelfTagRuleTypeID = 1 OR 
AttributeIdentifier_ID IN (@Check_Box_1, @Check_Box_2, @Check_Box_3, @Check_Box_4, @Check_Box_5, @Check_Box_6,
@Check_Box_7, @Check_Box_8, @Check_Box_9, @Check_Box_10,
@Text_1, @Text_2, @Text_3, @Text_4, @Text_5, @Text_6, @Text_7, @Text_8, @Text_9, @Text_10))
ORDER BY RulePriority
UPDATE @TblList SET LabelType_ID = @LabelType_ID, PriceChgTypeID = @PriceChgTypeID, 
SubTeam_No = @SubTeam_No, TaxJurisdictionID = @TaxJurisdictionID, Store_No = @Store_No
-- Default the Shelf Tag Extension in all cases
UPDATE @TblList SET ShelfTag_Ext = a.ShelfTag_Ext, ShelfTagAttrDesc = a.ShelfTagAttrDesc
FROM shelftagattribute a
JOIN @TblList b ON a.ShelfTag_Type = b.ShelfTag_Type
If @Exempt <> 0
-- EXEMPT
BEGIN
Select @RegType = ShelfTag_Type from @TblList
IF @RegType = 11
-- Overwrite default shelf tag in this one case to be the exemption shelf tag instead,
--   also set the exemption shelf tag to NULL
UPDATE @TblList SET ShelfTag_Type = b.Exempt_ShelfTag_Type, ShelfTag_Ext = a.ShelfTag_Ext,
ShelfTagAttrDesc = a.ShelfTagAttrDesc, Exempt_ShelfTag_Type = NULL
FROM shelftagattribute a
JOIN @TblList b ON a.ShelfTag_Type = b.Exempt_ShelfTag_Type
ELSE
-- set the exempt shelf tag extension
UPDATE @TblList SET ShelfTag_Ext2 = a.ShelfTag_Ext, ShelfTagAttrDesc2 = a.ShelfTagAttrDesc
FROM shelftagattribute a
JOIN @TblList b ON a.ShelfTag_Type = b.Exempt_ShelfTag_Type
END
ELSE
-- NOT EXEMPT
-- Set the Exempt Shelf Tag Extension to NULL
UPDATE @TblList SET Exempt_ShelfTag_Type = NULL
FROM shelftagattribute a
JOIN @TblList b ON a.ShelfTag_Type = b.Exempt_ShelfTag_Type
RETURN
END
GO


