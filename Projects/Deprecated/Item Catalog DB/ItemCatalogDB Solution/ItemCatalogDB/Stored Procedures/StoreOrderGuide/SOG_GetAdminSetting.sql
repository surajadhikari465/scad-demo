SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetAdminSetting')
	BEGIN
		DROP Procedure [dbo].SOG_GetAdminSetting
	END
GO

CREATE PROCEDURE [dbo].[SOG_GetAdminSetting]
	@AdminKey varchar(50)
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetAdminSetting()
--    Author: Billy Blackerby
--      Date: 6/29/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a specific Administrator Value
--
-- Modification History:
-- Date			Init	Comment
-- 06/29/2009	BS		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT 
		[AdminValue]
	FROM 
		[dbo].[CatalogAdmin]
	WHERE
		AdminKey = ISNULL(@AdminKey, AdminKey)

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 