 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountUnrankedSustainabilityRequiredOrderItems]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CountUnrankedSustainabilityRequiredOrderItems]
GO

/****** Object:  StoredProcedure [dbo].[CountUnrankedSustainabilityRequiredOrderItems]    Script Date: 07/02/2010 14:21:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[CountUnrankedSustainabilityRequiredOrderItems]
    @OrderHeader_ID int
AS
SELECT COUNT(*) AS OrderItemCount 
FROM OrderItem OI (nolock) 
INNER JOIN Item (nolock) ON Item.Item_Key = OI.Item_Key
WHERE OrderHeader_ID = @OrderHeader_ID
    AND Item.SustainabilityRankingRequired = 1 
    AND OI.SustainabilityRankingID IS NULL
GO