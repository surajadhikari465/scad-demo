SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Benjamin Sims
-- Create date: 2014-09-29
-- Description:	Receives a Validated Icon Item and 
--				adds a Validated Scan Code if necessary
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID('IconItemAddValidatedScanCode') AND type in (N'P', N'PC'))
BEGIN
	EXEC ('CREATE PROCEDURE [dbo].[IconItemAddValidatedScanCode] as SELECT 1')
END
GO

ALTER PROCEDURE dbo.IconItemAddValidatedScanCode
	@ValidatedItemList dbo.IconUpdateItemType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @now datetime;
	SET @now = (SELECT GETDATE());

	-- =====================================================
	-- Add Brand and Validated Brand if they don't exist yet
	-- =====================================================
	BEGIN TRY
		INSERT INTO ValidatedScanCode
		SELECT
			sc.ScanCode as ScanCode,
			@now		as InsertDate
		FROM
			(SELECT vi.ScanCode
			FROM @ValidatedItemList vi
			EXCEPT
			SELECT vsc.ScanCode
			FROM ValidatedScanCode vsc) sc
	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemAddValidatedScanCode failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH
END
GO
