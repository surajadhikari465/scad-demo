﻿CREATE   PROCEDURE dbo.Administration_POSPush_PopulatePIRUSConfigData(
	@writerID int, 
	@changeTypeID int,
	@Deletes bit) AS	
BEGIN
	SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,1,1,'P',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,2,1,'PIRUS_HeaderAction',NULL,2,0,NULL,0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,3,1,'06',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,4,1,'PIRUS_StartDate',NULL,6,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,5,1,'SubTeam_No',NULL,4,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,6,1,'Category_ID',NULL,4,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,7,1,'Item_Key',NULL,12,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,8,1,'    ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,9,1,'IdentifierWithCheckDigit',NULL,13,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,10,1,'00000000000000',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,11,1,'Item_Description',NULL,45,0,NULL,0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,12,1,'PackSize',NULL,15,0,NULL,0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,13,1,'PIRUS Active Flag',NULL,4,0,NULL,1,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,14,1,'POSCurrPrice',NULL,4,0,NULL,0,0,1,2,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,15,1,'POSCurrPrice',NULL,4,0,NULL,0,0,1,2,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,16,1,'Barcode_Type',NULL,1,0,'@',0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,17,1,'N',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,18,1,'00',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,19,1,'Compulsory_Price_Input',NULL,1,0,'N',0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,20,1,' ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,21,1,' ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,22,1,'Target_Margin',NULL,2,0,NULL,0,0,1,2,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,23,1,'Availability_Flag',NULL,1,0,NULL,0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,24,1,'0',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,25,1,'PIRUS_InsertDate',NULL,6,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,26,1,'PIRUS_CurrentDate',NULL,6,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,27,1,'PIRUS_DeleteDate',NULL,6,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,28,1,'N',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,29,1,'             ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,30,1,'LabelTypeDesc',NULL,4,0,'NONE',0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,31,1,'PIRUS_Sold_By_Weight',NULL,1,0,'N',0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,32,1,'Y',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,33,1,'    ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,34,1,'PIRUS_ItemTypeID',NULL,1,0,'N',0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,35,1,'PIRUS_OnSale',NULL,1,0,'N',0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,36,1,'PIRUS_SaleEndDate',NULL,6,0,'000000',0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,37,1,'NotAuthorizedForSale',NULL,1,0,'N',0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,38,1,'N',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,39,1,' ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,40,1,'S',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,41,1,'    ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,42,1,'00',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,43,1,'000000',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,44,1,'POS_Description',NULL,25,0,NULL,0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,45,1,'Y',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,46,1,'   ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,47,1,'N',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,48,1,'000',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,49,1,'000000',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,50,1,'  ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,51,1,'Item_Key',NULL,7,1,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,52,1,'0',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,53,1,'    ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,54,1,'00',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,55,1,'Calculated_Cost_Item',NULL,1,0,'N',0,0,0,NULL,0,0,' ',NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,56,1,'        ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,57,1,'000',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	
	-- R RECORD TYPE NOT NEEDED FOR DELETE CHANGE TYPES
	IF @Deletes  = 0
	BEGIN
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,1,2,'R',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,2,2,'PIRUS_HeaderAction',NULL,2,0,NULL,0,0,0,NULL,0,0,' ',NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,3,2,'03',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,4,2,'PIRUS_StartDate',NULL,6,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,5,2,'IdentifierWithCheckDigit',NULL,13,0,NULL,0,0,0,NULL,0,1,'0',NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,6,2,'01',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,7,2,'01',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,8,2,'Vendor_Key',NULL,8,0,NULL,0,0,0,NULL,0,0,' ',NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,9,2,'Vendor_Item_ID',NULL,13,0,NULL,0,0,0,NULL,0,0,' ',NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,10,2,'     ',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,11,2,'Package_Desc1',NULL,6,0,NULL,0,0,1,3,0,1,'0',NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,12,2,'UnitCost',NULL,4,0,NULL,0,0,1,3,0,1,'0',NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,13,2,'0000',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,14,2,'UnitCost',NULL,4,0,NULL,0,0,1,3,0,1,'0',NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,15,2,'UnitCost',NULL,4,0,NULL,0,0,1,2,0,1,'0',NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,16,2,'Y',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
		INSERT INTO POSWriterFileConfig(POSFileWriterKey, POSChangeTypeKey, ColumnOrder, RowOrder, DataElement, FieldID, MaxFieldWidth, TruncLeft, DefaultValue, IsTaxFlag, IsLiteral, IsDecimalValue, DecimalPrecision, IncludeDecimal, PadLeft, FillChar, LeadingChars, TrailingChars) VALUES (@writerID, @changeTypeID,17,2,'N',NULL,NULL,0,NULL,0,1,0,NULL,0,0,NULL,NULL,NULL);
	END	
	
	SET NOCOUNT OFF

    IF @error_no = 0
		COMMIT TRAN		
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('Administration_POSPush_PopulatePIRUSConfigData failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_PopulatePIRUSConfigData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_PopulatePIRUSConfigData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_PopulatePIRUSConfigData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_PopulatePIRUSConfigData] TO [IRMAReportsRole]
    AS [dbo];

