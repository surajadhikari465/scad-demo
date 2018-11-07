SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetItemIdentifiers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetItemIdentifiers]
GO


CREATE PROCEDURE dbo.GetItemIdentifiers
    @Item_Key int
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @ScaleIDCnt int

    SELECT @ScaleIDCnt = (SELECT COUNT(*) FROM ItemIdentifier WHERE Item_Key = @Item_Key AND (dbo.fn_IsScaleItem(Identifier) = 1))

    SELECT Identifier_ID, Identifier, Default_Identifier, Add_Identifier, Remove_Identifier, National_Identifier,
           @ScaleIDCnt As ScaleIdentifierCount, NumPluDigitsSentToScale, dbo.fn_IsScaleItem(Identifier) AS IsScaleIdentifier,
           ISNULL(Scale_Identifier,'0') AS Scale_Identifier, I.Subteam_No, 
		   CASE WHEN IdentifierType = 'S' THEN 'SKU'
				WHEN IdentifierType = 'B' THEN 'Barcode'
				WHEN IdentifierType = 'O' THEN 'Other'
				ELSE 'PLU' END AS IdentifierType
    FROM ItemIdentifier II
    INNER JOIN
		Item I
		ON I.Item_Key = II.Item_Key
    WHERE II.Item_Key = @Item_Key
    ORDER BY Identifier

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO