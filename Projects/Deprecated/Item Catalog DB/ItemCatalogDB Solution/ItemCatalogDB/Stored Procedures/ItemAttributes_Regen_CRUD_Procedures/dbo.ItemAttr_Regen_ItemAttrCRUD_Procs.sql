
--=====================================================================
--*********      CRUD Procedures for ItemAttribute                               
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_GetAllItemAttributes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_GetAllItemAttributes]
GO

CREATE PROCEDURE dbo.ItemAttributes_Regen_GetAllItemAttributes
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the ItemAttribute table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_ItemAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
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
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




--=====================================================================
--*********      dbo.ItemAttributes_Regen_GetItemAttributeByPK                             
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_GetItemAttributeByPK]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_GetItemAttributeByPK]
GO

CREATE PROCEDURE dbo.ItemAttributes_Regen_GetItemAttributeByPK
	@ItemAttribute_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the ItemAttribute table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_ItemAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
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
	WHERE ItemAttribute_ID = @ItemAttribute_ID
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO










































--=====================================================================
--*********      dbo.ItemAttributes_Regen_InsertItemAttribute                         
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_InsertItemAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_InsertItemAttribute]
GO
Create PROCEDURE [dbo].[ItemAttributes_Regen_InsertItemAttribute]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--=====================================================================
--*********      dbo.ItemAttributes_Regen_UpdateItemAttribute                    
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_UpdateItemAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_UpdateItemAttribute]
GO
CREATE PROCEDURE [dbo].[ItemAttributes_Regen_UpdateItemAttribute]
		@ItemAttribute_ID int,
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
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the ItemAttribute table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_ItemAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Feb 23, 2007

	DECLARE @checkBox3Updated bit = 0 -- exluisve
	DECLARE @checkBox5Updated bit -- color added

	SELECT	@checkBox3Updated = CASE	WHEN [Check_Box_3] is not null and @Check_Box_3 is not null AND [Check_Box_3] <> @Check_Box_3 THEN 1
										WHEN  ([Check_Box_3] is null and @Check_Box_3 is not null)  OR ([Check_Box_3] is not null and @Check_Box_3 is null )THEN 1
										ELSE 0
								END,
			 @checkBox5Updated = CASE	WHEN [Check_Box_5] is not null and @Check_Box_5 is not null and [Check_Box_5] <> @Check_Box_5 THEN 1
										WHEN ([Check_Box_5] is null and @Check_Box_5 is not null ) OR ([Check_Box_5] is not null and @Check_Box_5 is null ) THEN 1
										ELSE 0
								END
	FROM ItemAttribute
	WHERE
		ItemAttribute_ID = @ItemAttribute_ID

	UPDATE ItemAttribute
	SET
		[Item_Key] = @Item_Key,
		[Check_Box_1] = @Check_Box_1,
		[Check_Box_2] = @Check_Box_2,
		[Check_Box_3] = @Check_Box_3,
		[Check_Box_4] = @Check_Box_4,
		[Check_Box_5] = @Check_Box_5,
		[Check_Box_6] = @Check_Box_6,
		[Check_Box_7] = @Check_Box_7,
		[Check_Box_8] = @Check_Box_8,
		[Check_Box_9] = @Check_Box_9,
		[Check_Box_10] = @Check_Box_10,
		[Check_Box_11] = @Check_Box_11,
		[Check_Box_12] = @Check_Box_12,
		[Check_Box_13] = @Check_Box_13,
		[Check_Box_14] = @Check_Box_14,
		[Check_Box_15] = @Check_Box_15,
		[Check_Box_16] = @Check_Box_16,
		[Check_Box_17] = @Check_Box_17,
		[Check_Box_18] = @Check_Box_18,
		[Check_Box_19] = @Check_Box_19,
		[Check_Box_20] = @Check_Box_20,
		[Text_1] = @Text_1,
		[Text_2] = @Text_2,
		[Text_3] = @Text_3,
		[Text_4] = @Text_4,
		[Text_5] = @Text_5,
		[Text_6] = @Text_6,
		[Text_7] = @Text_7,
		[Text_8] = @Text_8,
		[Text_9] = @Text_9,
		[Text_10] = @Text_10,
		[Date_Time_1] = @Date_Time_1,
		[Date_Time_2] = @Date_Time_2,
		[Date_Time_3] = @Date_Time_3,
		[Date_Time_4] = @Date_Time_4,
		[Date_Time_5] = @Date_Time_5,
		[Date_Time_6] = @Date_Time_6,
		[Date_Time_7] = @Date_Time_7,
		[Date_Time_8] = @Date_Time_8,
		[Date_Time_9] = @Date_Time_9,
		[Date_Time_10] = @Date_Time_10
	WHERE
		ItemAttribute_ID = @ItemAttribute_ID
		
	SELECT @UpdateCount = @@ROWCOUNT

	IF @checkBox3Updated  = 1 or @checkBox5Updated = 1
	BEGIN
		EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemLocaleAddOrUpdate', NULL, NULL
	END
	
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--=====================================================================
--*********      dbo.ItemAttributes_Regen_DeleteItemAttribute                
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_DeleteItemAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_DeleteItemAttribute]
GO
CREATE PROCEDURE dbo.ItemAttributes_Regen_DeleteItemAttribute
		@ItemAttribute_ID int
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the ItemAttribute table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_ItemAttribute_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Feb 23, 2007
	
	DELETE FROM ItemAttribute
	WHERE
		ItemAttribute_ID = @ItemAttribute_ID
	
			
	SELECT @DeleteCount = @@ROWCOUNT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO