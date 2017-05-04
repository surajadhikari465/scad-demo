CREATE PROCEDURE dbo.ItemAttributes_GetItemAttributeByItemKey
	@Item_Key int
	
AS

	-- Created By:	James Winfield
	-- Created   :	Feb 23, 2007

	SELECT
		[ItemAttribute_ID],
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
	FROM ItemAttribute (NOLOCK) 
	WHERE Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemAttributes_GetItemAttributeByItemKey] TO [IRMAClientRole]
    AS [dbo];

