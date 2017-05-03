/****** Object:  StoredProcedure [dbo].[GetCompetitivePriceChgTypeStatus]    Script Date: 08/18/2010 09:20:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCompetitivePriceChgTypeStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCompetitivePriceChgTypeStatus]
GO

/****** Object:  StoredProcedure [dbo].[GetCompetitivePriceChgTypeStatus]    Script Date: 08/18/2010 09:20:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--USE [ItemCatalog_Test]
--GO
--/****** Object:  StoredProcedure [dbo].[GetCompetitiveBatchItemDateChecked]    Script Date: 08/17/2010 15:43:08 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER OFF
--GO
CREATE PROCEDURE [dbo].[GetCompetitivePriceChgTypeStatus] 
	@PriceChgTypeID INT, 
	@CompStatus bit OUTPUT
AS
BEGIN

	
SELECT 
	@CompStatus = Competitive
	FROM
	PriceChgType (NOLOCK)
	WHERE PriceChgTypeID = @PriceChgTypeID
	
END

GO

