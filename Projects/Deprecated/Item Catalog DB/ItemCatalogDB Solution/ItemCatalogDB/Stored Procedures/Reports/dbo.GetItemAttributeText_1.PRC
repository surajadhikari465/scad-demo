SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetItemAttributeText_1]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetItemAttributeText_1]
GO

CREATE PROCEDURE [dbo].[GetItemAttributeText_1]
AS
BEGIN
    SET NOCOUNT ON
		SELECT
		DISTINCT CASE 
			WHEN Text_1 = '' THEN ''
			WHEN Text_1 IS NULL THEN ''
			ELSE Text_1
		END as OrderBy,		
		CASE 
			WHEN Text_1 = '' THEN 'Commodity Not Assigned'
			WHEN Text_1 IS NULL THEN 'Commodity Not Assigned'
			ELSE Text_1 
		END as ItemAttributeText1
	FROM 
		ItemAttribute
	ORDER BY
		OrderBy
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
	  