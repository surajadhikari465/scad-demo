CREATE PROCEDURE [dbo].[SetItemDefaultValues]
	(
		@Item_Key int
		,
		-- if 'EIM' then all defaultable attribute values are set
		@InsertApplication varchar(30)
	)

AS

	DECLARE @AttributeTable VARCHAR(50),
		@AttributeField VARCHAR(50),
		@Type TINYINT,
		@Value VARCHAR(50),
		@AttributeAssignement VARCHAR(500),
		@ItemSetClause VARCHAR(500),
		@PriceSetClause VARCHAR(500),
		@ItemWhereClause VARCHAR(8000),
		@PriceWhereClause VARCHAR(8000),
		@ItemSQL VARCHAR(8000),
		@PriceSQL VARCHAR(8000)
		
		SELECT @ItemSetClause = ''
		SELECT @PriceSetClause = ''
		SELECT @ItemSQL = ''
		SELECT @PriceSQL = ''


	-- declare cursor for getting and looping through the default values for the item
	DECLARE itemDefaultValuesCursor CURSOR FOR
		SELECT ida.AttributeTable
			,ida.AttributeField
			,ida.Type
			,idv.Value
		FROM ItemDefaultValue (NOLOCK) idv
			JOIN ItemDefaultAttribute (NOLOCK) ida
				ON ida.ItemDefaultAttribute_ID = idv.ItemDefaultAttribute_ID
			JOIN Item (NOLOCK) itm ON itm.Item_Key = @Item_Key
				AND (((idv.ProdHierarchyLevel4_ID is not null) 
					AND itm.ProdHierarchyLevel4_ID = idv.ProdHierarchyLevel4_ID)
				OR ((idv.ProdHierarchyLevel4_ID is null)
					AND itm.Category_ID = idv.Category_ID))
					
	OPEN itemDefaultValuesCursor
	FETCH itemDefaultValuesCursor INTO @AttributeTable, @AttributeField, @Type, @Value

	-- loop through the default values
	WHILE @@Fetch_Status = 0
	BEGIN
	
		-- begin constructing the set column value phrase
		SELECT @AttributeAssignement = @AttributeField + ' = ' ;
		
		-- format the value according to the column's data type
		IF @Type = 1 -- numeric
			SELECT @AttributeAssignement = @AttributeAssignement + @Value ;
		ELSE IF @Type = 2 -- bit
			BEGIN
				-- need to add a handler for bit types
				SET @Value = REPLACE(@Value, 'True', 1)
				SET @Value = REPLACE(@Value, 'False', 0)
				SELECT @AttributeAssignement = @AttributeAssignement + @Value ;
			END
		ELSE -- text & date
		BEGIN
			-- escape any single quotes
			SET @Value = REPLACE(@Value, '''', '''''')

			SELECT @AttributeAssignement = @AttributeAssignement + '''' + @Value + '''';
		END
		
		-- add to correct set clause variable by table name
		IF LOWER(@AttributeTable) = 'item'
		BEGIN
			
				-- if the executing tool is not EIM
				-- then skip over the fields that are on the
				-- AddItem form which have their default values preset
			IF @InsertApplication = 'EIM' OR
				(LOWER(@AttributeField) <> 'taxclassid'
				AND LOWER(@AttributeField) <> 'labeltype_id'
				AND LOWER(@AttributeField) <> 'retailsale'
				AND LOWER(@AttributeField) <> 'costedbyweight')
			BEGIN
				SELECT @ItemSetClause = @AttributeAssignement ;
				
				-- some default values are set in the database when the new item is inserted to we're removing the NULL criteria here
				--SELECT @ItemWhereClause = ' AND ' + @AttributeField + ' IS NULL ';
				--SELECT @ItemSQL = 'UPDATE Item SET ' + @ItemSetClause + ' WHERE ITEM_KEY = ' + STR(@Item_Key) + @ItemWhereClause;
				SELECT @ItemSQL = 'UPDATE Item SET ' + @ItemSetClause + ' WHERE ITEM_KEY = ' + STR(@Item_Key);
				--SELECT @ItemSQL as ItemSql
				EXECUTE(@ItemSQL)
			END
		END
		
		IF LOWER(@AttributeTable) = 'price'
		BEGIN
			SELECT @PriceSetClause = @AttributeAssignement ;
			
			-- some default values are set in the database when the new item is inserted to we're removing the NULL criteria here
			--SELECT @PriceWhereClause = ' AND ' + @AttributeField + ' IS NULL ';
			--SELECT @PriceSQL = 'UPDATE Price SET ' + @PriceSetClause + ' WHERE ITEM_KEY = ' + STR(@Item_Key) + @PriceWhereClause;
			SELECT @PriceSQL = 'UPDATE Price SET ' + @PriceSetClause + ' WHERE ITEM_KEY = ' + STR(@Item_Key);
			--SELECT @PriceSQL as PriceSql
			EXECUTE(@PriceSQL)
		END
				
		FETCH itemDefaultValuesCursor INTO @AttributeTable, @AttributeField, @Type, @Value             

	END
	
	CLOSE itemDefaultValuesCursor
	DEALLOCATE itemDefaultValuesCursor
	
	Return @@ERROR