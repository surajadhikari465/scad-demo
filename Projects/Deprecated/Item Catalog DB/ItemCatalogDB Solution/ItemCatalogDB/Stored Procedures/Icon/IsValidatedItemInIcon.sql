SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsValidatedItemInIcon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IsValidatedItemInIcon]
GO

CREATE PROCEDURE [dbo].[IsValidatedItemInIcon] 
	@ItemKey int,
	@IsValidatedInIcon bit OUTPUT
AS 
BEGIN

    SET @IsValidatedInIcon = 
		ISNULL((SELECT
			vsc.ScanCode
		FROM
			ValidatedScanCode vsc
		WHERE
			vsc.ScanCode = (SELECT ii.Identifier FROM ItemIdentifier ii WHERE ii.Item_Key = @ItemKey AND ii.Default_Identifier = 1)), 0)

	SELECT @IsValidatedInIcon

END
GO