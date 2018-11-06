SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Benjamin Sims
-- Create date: 2014-09-29
-- Description:	Receives a Validated Icon Item and updates Irma
--				with new tax class if necessary
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID('IconItemAddTax') AND type in (N'P', N'PC'))
BEGIN
	EXEC ('CREATE PROCEDURE [dbo].[IconItemAddTax] as SELECT 1')
END
GO

ALTER PROCEDURE dbo.IconItemAddTax
	-- Add the parameters for the stored procedure here
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
	-- Add Tax Class if it does not exist
	-- =====================================================
	BEGIN TRY
		INSERT INTO TaxClass
		SELECT
			vi.TaxClassName						as TaxClassDesc,
			SUBSTRING(vi.TaxClassName, 1, 7)	as ExternalTaxGroupCode
		FROM
			@ValidatedItemList vi
		WHERE 
			NOT EXISTS (SELECT * 
						FROM TaxClass tc 
						WHERE 
							tc.TaxClassDesc = vi.TaxClassName
							OR SUBSTRING(tc.TaxClassDesc, 1, 7) = SUBSTRING(vi.TaxClassName, 1, 7))

	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemAddTax failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH

END
GO
