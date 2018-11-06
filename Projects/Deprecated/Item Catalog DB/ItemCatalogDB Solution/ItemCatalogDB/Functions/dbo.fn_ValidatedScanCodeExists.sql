IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[fn_ValidatedScanCodeExists]') and xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[fn_ValidatedScanCodeExists]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE FUNCTION dbo.fn_ValidatedScanCodeExists
	(
	@Item_Key INT
	)
RETURNS BIT
AS
	BEGIN
		DECLARE @ReceiveUpdateFromIcon INT
		DECLARE @ValidatedScanCodeExists SMALLINT = 0

		SET @ReceiveUpdateFromIcon = (SELECT dbo.fn_ReceiveUPCPLUUpdateFromIcon())
		IF @ReceiveUpdateFromIcon > 0 
			BEGIN
				IF EXISTS (SELECT Item_Key FROM ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK) ON II.Identifier = VSC.Scancode WHERE II.Item_Key = @Item_Key)
					SET @ValidatedScanCodeExists = 1
				ELSE
					SET @ValidatedScanCodeExists = 0
			END
		ELSE
			SET @ValidatedScanCodeExists = 0

		RETURN @ValidatedScanCodeExists

	END
GO