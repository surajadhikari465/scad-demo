SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemHosting_UpdateStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	exec('create procedure [dbo].[ItemHosting_UpdateStore] as begin set nocount on; end')
GO

ALTER PROCEDURE dbo.ItemHosting_UpdateStore
	@Store_No int,
	@Store_Name varchar(50),
	@StoreAbbr varchar(5),
	@Phone_Number varchar(20),
	@Zone_ID int,
	@BusinessUnit_ID int,
	@UNFI_Store varchar(12),
	@EXEWarehouse smallint,
	@Internal bit,
	@Regional bit,
	@Mega_Store bit,
	@WFM_Store bit,
	@Distribution_Center bit,
	@Manufacturer bit,
	@TaxJurisdictionID int,
	@PSI_Store_No int,
	@StoreJurisdictionID int,
	@GeoCode varchar(15),
	@PLUMStoreNo int
AS

BEGIN
    SET NOCOUNT ON

	DECLARE @oldMegaStore bit = (SELECT Mega_Store from Store s where s.Store_No = @Store_No);

    UPDATE Store 
		SET Store_Name = @Store_Name,
			StoreAbbr = @StoreAbbr,
			Phone_Number = @Phone_Number,
			Zone_ID = @Zone_ID,
			BusinessUnit_ID = @BusinessUnit_ID,
			UNFI_Store = @UNFI_Store,
			EXEWarehouse = @EXEWarehouse,
			Internal = @Internal,
			Regional = @Regional,
			Mega_Store = @Mega_Store,
			WFM_Store = @WFM_Store,
			Distribution_Center = @Distribution_Center,
			Manufacturer = @Manufacturer,
			TaxJurisdictionID = @TaxJurisdictionID,
			PSI_Store_No=@PSI_Store_No,
			StoreJurisdictionID = @StoreJurisdictionID,
			GeoCode = @GeoCode,
			PLUMStoreNo = @PLUMStoreNo
    WHERE Store_No = @Store_No

	--If Mega_Store is changing then update StoreRegionMapping table so that 
	--scheduled jobs and new store creation function properly
	IF @oldMegaStore <> @Mega_Store
	BEGIN
		IF @Mega_Store = 1
		BEGIN
			UPDATE StoreRegionMapping
				SET Region_Code = 'TS'
			WHERE Store_No = @Store_No
		END
		ELSE IF @Mega_Store = 0
		BEGIN
			UPDATE StoreRegionMapping
				SET Region_Code = (SELECT RegionCode FROM Region)
			WHERE Store_No = @Store_No
		END
	END

    SET NOCOUNT OFF

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
