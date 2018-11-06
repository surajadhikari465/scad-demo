/****** Object:  StoredProcedure [dbo].[GetTaxClasses]    Script Date: 07/05/2006 09:51:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTaxClasses]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GetTaxClasses]
GO

/****** Object:  StoredProcedure [dbo].[GetTaxClasses]    Script Date: 07/04/2006 12:57:38 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.[GetTaxClasses] AS

BEGIN
    SET NOCOUNT ON

	SELECT [TaxClassID],
		[TaxClassDesc]
	FROM [TaxClass]
	ORDER BY [TaxClassDesc]
    
    SET NOCOUNT OFF
END

GO
