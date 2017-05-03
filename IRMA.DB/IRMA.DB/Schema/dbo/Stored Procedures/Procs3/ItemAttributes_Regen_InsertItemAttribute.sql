﻿Create PROCEDURE [dbo].[ItemAttributes_Regen_InsertItemAttribute]
		@Item_Key int,
			@Check_Box_1 bit,
			@Check_Box_2 bit,
			@Check_Box_3 bit,
			@Check_Box_4 bit,
			@Check_Box_5 bit,
			@Check_Box_6 bit,
			@Check_Box_7 bit,
			@Check_Box_8 bit,
			@Check_Box_9 bit,
			@Check_Box_10 bit,
			@Check_Box_11 bit,
			@Check_Box_12 bit,
			@Check_Box_13 bit,
			@Check_Box_14 bit,
			@Check_Box_15 bit,
			@Check_Box_16 bit,
			@Check_Box_17 bit,
			@Check_Box_18 bit,
			@Check_Box_19 bit,
			@Check_Box_20 bit,
			@Text_1 varchar(50),
			@Text_2 varchar(50),
			@Text_3 varchar(50),
			@Text_4 varchar(50),
			@Text_5 varchar(50),
			@Text_6 varchar(50),
			@Text_7 varchar(50),
			@Text_8 varchar(50),
			@Text_9 varchar(50),
			@Text_10 varchar(50),
			@Date_Time_1 datetime,
			@Date_Time_2 datetime,
			@Date_Time_3 datetime,
			@Date_Time_4 datetime,
			@Date_Time_5 datetime,
			@Date_Time_6 datetime,
			@Date_Time_7 datetime,
			@Date_Time_8 datetime,
			@Date_Time_9 datetime,
			@Date_Time_10 datetime
		,
		@ItemAttribute_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the ItemAttribute table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_ItemAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Feb 23, 2007

	INSERT INTO ItemAttribute
	(
		[Item_Key],
		[Check_Box_1],
		[Check_Box_2],
		[Check_Box_3],
		[Check_Box_4],
		[Check_Box_5],
		[Check_Box_6],
		[Check_Box_7],
		[Check_Box_8],
		[Check_Box_9],
		[Check_Box_10],
		[Check_Box_11],
		[Check_Box_12],
		[Check_Box_13],
		[Check_Box_14],
		[Check_Box_15],
		[Check_Box_16],
		[Check_Box_17],
		[Check_Box_18],
		[Check_Box_19],
		[Check_Box_20],
		[Text_1],
		[Text_2],
		[Text_3],
		[Text_4],
		[Text_5],
		[Text_6],
		[Text_7],
		[Text_8],
		[Text_9],
		[Text_10],
		[Date_Time_1],
		[Date_Time_2],
		[Date_Time_3],
		[Date_Time_4],
		[Date_Time_5],
		[Date_Time_6],
		[Date_Time_7],
		[Date_Time_8],
		[Date_Time_9],
		[Date_Time_10]
	)
	VALUES (
		@Item_Key,
		@Check_Box_1,
		@Check_Box_2,
		@Check_Box_3,
		@Check_Box_4,
		@Check_Box_5,
		@Check_Box_6,
		@Check_Box_7,
		@Check_Box_8,
		@Check_Box_9,
		@Check_Box_10,
		@Check_Box_11,
		@Check_Box_12,
		@Check_Box_13,
		@Check_Box_14,
		@Check_Box_15,
		@Check_Box_16,
		@Check_Box_17,
		@Check_Box_18,
		@Check_Box_19,
		@Check_Box_20,
		@Text_1,
		@Text_2,
		@Text_3,
		@Text_4,
		@Text_5,
		@Text_6,
		@Text_7,
		@Text_8,
		@Text_9,
		@Text_10,
		@Date_Time_1,
		@Date_Time_2,
		@Date_Time_3,
		@Date_Time_4,
		@Date_Time_5,
		@Date_Time_6,
		@Date_Time_7,
		@Date_Time_8,
		@Date_Time_9,
		@Date_Time_10
	)
	
		SELECT @ItemAttribute_ID  = SCOPE_IDENTITY()

		EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemLocaleAddOrUpdate', NULL, NULL
	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_Regen_InsertItemAttribute] TO [IRMAClientRole]
    AS [dbo];

