
--=====================================================================
--*********      CRUD Procedures for AttributeIdentifier                               
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_GetAllAttributeIdentifiers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_GetAllAttributeIdentifiers]
GO

CREATE PROCEDURE dbo.ItemAttributes_Regen_GetAllAttributeIdentifiers
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the AttributeIdentifier table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_AttributeIdentifier_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Mar 01, 2007

	SELECT
		[AttributeIdentifier_ID],
		[Screen_Text],
		[field_type],
		[combo_box],
		[max_width],
		[default_value],
		[field_values]
	FROM AttributeIdentifier (NOLOCK)
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




--=====================================================================
--*********      dbo.ItemAttributes_Regen_GetAttributeIdentifierByPK                             
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_GetAttributeIdentifierByPK]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_GetAttributeIdentifierByPK]
GO

CREATE PROCEDURE dbo.ItemAttributes_Regen_GetAttributeIdentifierByPK
	@AttributeIdentifier_ID int
	
AS

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the AttributeIdentifier table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_AttributeIdentifier_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Mar 01, 2007

	SELECT
		[AttributeIdentifier_ID],
		[Screen_Text],
		[field_type],
		[combo_box],
		[max_width],
		[default_value],
		[field_values]
	FROM AttributeIdentifier (NOLOCK) 
	WHERE AttributeIdentifier_ID = @AttributeIdentifier_ID
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO







--=====================================================================
--*********      dbo.ItemAttributes_Regen_InsertAttributeIdentifier                         
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_InsertAttributeIdentifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_InsertAttributeIdentifier]
GO
CREATE PROCEDURE dbo.ItemAttributes_Regen_InsertAttributeIdentifier
		@Screen_Text varchar(50),
			@field_type varchar(50),
			@combo_box bit,
			@max_width int,
			@default_value varchar(50),
			@field_values varchar(8000)
		,
		@AttributeIdentifier_ID int OUTPUT
	
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the AttributeIdentifier table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_AttributeIdentifier_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Mar 01, 2007

	INSERT INTO AttributeIdentifier
	(
		[Screen_Text],
		[field_type],
		[combo_box],
		[max_width],
		[default_value],
		[field_values]
	)
	VALUES (
		@Screen_Text,
		@field_type,
		@combo_box,
		@max_width,
		@default_value,
		@field_values
	)
	
		SELECT @AttributeIdentifier_ID  = SCOPE_IDENTITY()
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--=====================================================================
--*********      dbo.ItemAttributes_Regen_UpdateAttributeIdentifier                    
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_UpdateAttributeIdentifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_UpdateAttributeIdentifier]
GO
CREATE PROCEDURE dbo.ItemAttributes_Regen_UpdateAttributeIdentifier
		@AttributeIdentifier_ID int,
		@Screen_Text varchar(50),
		@field_type varchar(50),
		@combo_box bit,
		@max_width int,
		@default_value varchar(50),
		@field_values varchar(8000)
		,
		@UpdateCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the AttributeIdentifier table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_AttributeIdentifier_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Mar 01, 2007

	UPDATE AttributeIdentifier
	SET
		[Screen_Text] = @Screen_Text,
		[field_type] = @field_type,
		[combo_box] = @combo_box,
		[max_width] = @max_width,
		[default_value] = @default_value,
		[field_values] = @field_values
	WHERE
		AttributeIdentifier_ID = @AttributeIdentifier_ID
		
	SELECT @UpdateCount = @@ROWCOUNT
	
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--=====================================================================
--*********      dbo.ItemAttributes_Regen_DeleteAttributeIdentifier                
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_Regen_DeleteAttributeIdentifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_Regen_DeleteAttributeIdentifier]
GO
CREATE PROCEDURE dbo.ItemAttributes_Regen_DeleteAttributeIdentifier
		@AttributeIdentifier_ID int
		,
		@DeleteCount int OUTPUT
AS 

	-- This stored procedure can be found with all the
	-- Generated CRUD procedures for the AttributeIdentifier table in the
	-- \ItemAttributes_Regen_CRUD_Procedures\ItemAttributes_Regen_AttributeIdentifier_CRUD_Procs.sql file in the ItemCatalogDB project.
	-- Created By:	James Winfield
	-- Created   :	Mar 01, 2007
	
	DELETE FROM AttributeIdentifier
	WHERE
		AttributeIdentifier_ID = @AttributeIdentifier_ID
	
			
	SELECT @DeleteCount = @@ROWCOUNT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO