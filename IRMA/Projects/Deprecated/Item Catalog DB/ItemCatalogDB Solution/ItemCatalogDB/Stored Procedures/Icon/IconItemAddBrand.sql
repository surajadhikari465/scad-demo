SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Benjamin Sims
-- Create date: 2014-09-29
-- Description:	Receives a Validated Icon Item and updates Irma 
--				with new Brand and ValidatedBrand if necessary
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID('IconItemAddBrand') AND type in (N'P', N'PC'))
BEGIN
	EXEC ('CREATE PROCEDURE [dbo].[IconItemAddBrand] as SELECT 1')
END
GO

ALTER PROCEDURE [dbo].[IconItemAddBrand]
	-- Add the parameters for the stored procedure here
	@ValidatedItemList dbo.IconUpdateItemType READONLY,
	@UserName varchar(25)
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Declare Variables
	-- =====================================================
	DECLARE @UserId int;
	DECLARE @now datetime;

	SET @UserId = (SELECT u.User_ID FROM Users u WHERE u.UserName = @UserName);
	SET @now = (SELECT GETDATE());

	-- =====================================================
	-- Add Brand and Validated Brand if they don't exist yet
	-- =====================================================
	BEGIN TRY
		-- Check if it exists in ItemBrand.  If it does not, then add it
		SELECT 
			vi.BrandId as IconBrandId,
			vi.BrandName as BrandName
		INTO #brandsNotInIrma
		FROM @ValidatedItemList	vi
		WHERE NOT EXISTS (SELECT * FROM ItemBrand ib WHERE ib.Brand_Name = vi.BrandName)
		  AND NOT EXISTS (SELECT * FROM ValidatedBrand vb WHERE vb.IconBrandId = vi.BrandId)

		-- Check if brand name has been changed in Icon. If so, brand name needs to be updated in IRMA
		SELECT 
			vi.BrandId as IconBrandId,
			vi.BrandName as BrandName,
			vb.IrmaBrandId as IrmaBrandId
		INTO #brandNamesChangedWithNoDup
		FROM @ValidatedItemList	vi
		JOIN ValidatedBrand vb ON vb.IconBrandId = vi.BrandId
		WHERE NOT EXISTS (SELECT * FROM ItemBrand ib WHERE ib.Brand_Name = vi.BrandName)

	    -- Check if brand name has been changed in Icon. If so, brand name needs to be updated in IRMA
		SELECT 
		    ib.Brand_ID as IrmaBrandIdNew,
			vb.IrmaBrandId as IrmaBramdIdOld,
			vi.BrandId as IconBrandId,
			vi.BrandName as BrandName
		INTO #brandNamesChangedWithDup
		FROM @ValidatedItemList	vi
		JOIN ItemBrand ib ON ib.Brand_Name = vi.BrandName
		JOIN ValidatedBrand vb ON vb.IconBrandId = vi.BrandId
	   WHERE ib.Brand_ID <> vb.IrmaBrandId

		-- Keep track of Brands not in IRMA to use later when inserting into ValidatedBrand table
		SELECT
			vi.BrandId		as IconBrandId,
			vi.BrandName	as BrandName
		INTO #brandsNotValidated
		FROM @ValidatedItemList vi
		WHERE NOT EXISTS (SELECT * FROM ValidatedBrand vb WHERE vb.IconBrandId = vi.BrandId)

		-- Insert into ItemBrand if it doesn't exist yet.
		INSERT INTO [dbo].[ItemBrand]
		SELECT
			vi.BrandName	as Brand_Name,
			@UserId			as User_ID,
			@now			as LastUpdateTimestamp
		FROM
			@ValidatedItemList			vi
			JOIN #brandsNotInIrma	nvb on vi.BrandId = nvb.IconBrandId

		-- Update Brand name if the brand name simply got updated in Icon, and the new brand name is not in IRMA
		UPDATE ib
		   SET ib.Brand_Name = bn.BrandName
		  FROM [dbo].[ItemBrand] ib
	INNER JOIN #brandNamesChangedWithNoDup bn ON bn.IrmaBrandId = ib.Brand_Id
		
		-- If the brand name to be updated to is already in IRMA with the brand to be updated from also in IRMA as a validated brand,
		-- need to update Item/Brand association to reassign items to the updated-to-brand, and mark the updated-to brand to be the 
		-- validated brand. The updated-from brand will be removed from the validatedbrand table.
		UPDATE i
		   SET i.Brand_ID = bn.IrmaBrandIdNew
		  FROM Item i
	INNER JOIN #brandNamesChangedWithDup bn ON bn.IrmaBramdIdOld = i.Brand_Id

		UPDATE vb
		   SET vb.IrmaBrandId = bn.IrmaBrandIdNew
		  FROM ValidatedBrand vb
	INNER JOIN #brandNamesChangedWithDup bn ON bn.IconBrandId = vb.IconBrandId
	
		-- Insert into ValidatedBrand if it doesn't exist yet.
		INSERT INTO [dbo].[ValidatedBrand]
		SELECT
			ib.Brand_ID		as IrmaBrandId,
			nvb.IconBrandId	as IconBrandId
		FROM
			#brandsNotValidated nvb
			JOIN ItemBrand		ib	on nvb.BrandName = ib.Brand_Name
	END TRY
	BEGIN CATCH
		DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
		SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
		RAISERROR ('IconItemAddBrand failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
	END CATCH
END
GO