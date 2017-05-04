
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_ValidateDeleteVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[EIM_ValidateDeleteVendor]
GO

CREATE PROCEDURE [dbo].[EIM_ValidateDeleteVendor]
    @Item_Key int,    
    @Vendor_ID int,
    @StoreListForItem varchar(2000), -- comma delimited list of store numbers
    @Delete_Vendor int 
   
	-- Validate if vendor can be deauthorized or deleted in EIM
	-- 01/21/2011 Vicky
	
	-- If delete_vendor = 1, validate vendor deletion for -ALL- stores. 
	-- Find all stores where vendor is primary. 	
	-- We don't need to check stores where vendor is secondary 
	-- since we can always delete secondary vendor.
	-- for all stores where vendor is primary:
	-- check how many secondary vendors exist
	-- No secondary vendors: warning that item will be deauthorized
	-- One secondary vendor: warning that vendors will be swapped
	-- More than one secondary vendor: error that multiple secondary vendors exist.
	
	-- If delete_vendor = 0, validate vendor deauthorization 
	-- for store list passed as a parameter
	-- For each store in the store list
	-- check if it's a primary vendor. If yes :
	-- check how many secondary vendors exist
	-- No secondary vendors: warning that item will be deauthorized
	-- One secondary vendor: warning that vendors will be swapped
	-- More than one secondary vendor: error that multiple secondary vendors exist.    
	
AS 
BEGIN
    SET NOCOUNT ON
	
	-- warning item will be de-authorized for the store
	DECLARE @WarningDeauthCount int 
	
	-- warning item will be swapped to a new primary vendor
	DECLARE @WarningSwapCount int
	
	-- error primary vendor cannot be deleted
	DECLARE @ErrorCount int
	
	DECLARE @CurrDate datetime
	DECLARE @PrimVend int
	DECLARE @Store_no int
	DECLARE @Count int
	DECLARE @tmp_PrimVend TABLE (PrimVend int)	
	DECLARE @tmp_ValidationResult TABLE (WarningDeauthCount int, WarningSwapCount int, ErrorCount int)
		
	SELECT @WarningDeauthCount = 0
	SELECT @WarningSwapCount = 0
	SELECT @ErrorCount = 0
	SELECT @Count = 0
	
	SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
	
	IF @Delete_Vendor = 0
	
	BEGIN

		DECLARE StoreNo_Cursor CURSOR FOR
		Select Key_Value from fn_Parse_List(@StoreListForItem,',')

		OPEN StoreNo_Cursor 
		FETCH NEXT FROM StoreNo_Cursor into @Store_No

		WHILE @@FETCH_STATUS = 0

		BEGIN

			DELETE from @tmp_PrimVend
			SELECT @Count = 0
	
			INSERT into @tmp_PrimVend
			SELECT count(*) 
			FROM StoreItemVendor (nolock)                  
			WHERE Item_key = @Item_key
			AND Vendor_ID = @Vendor_ID 
			AND store_no = @Store_No                     
			AND PrimaryVendor = 1
						
			SELECT @PrimVend = PrimVend from @tmp_PrimVend 
   
			IF @PrimVend = 1
			BEGIN
                                             
				SELECT @Count = count(*)
				FROM  StoreItemVendor SIV (nolock)
				WHERE  SIV.Store_No = @Store_No 
				AND SIV.Item_Key = @Item_Key 
				AND SIV.PrimaryVendor = 0
				AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
		
				IF @Count = 0
				BEGIN	
					-- No secondary vendor exists. 
					-- Warning item to be de-authorized for store.
					SELECT @WarningDeauthCount = @WarningDeauthCount + 1	
				END
		
				IF @Count = 1
				BEGIN
					-- One secondary vendor exists. 
					-- Warning item to be swapped to a new primary vendor.
					SELECT @WarningSwapCount = @WarningSwapCount + 1
				END
		
				IF @Count > 1
				BEGIN		
					-- More than one secondary vendor exists. 
					-- Error primary vendor can't be deleted.
					SELECT @ErrorCount = @ErrorCount + 1			
				END
    
			END 
   
		FETCH NEXT FROM StoreNo_Cursor into @Store_No
		END -- while @@fetch_status

		CLOSE StoreNo_Cursor
		DEALLOCATE StoreNo_Cursor
  
	END -- if @Delete_Vendor = 0
	
	IF @Delete_Vendor = 1 BEGIN
		
		DECLARE StoreNo_Cursor CURSOR FOR
		SELECT store_no 
		FROM StoreItemVendor (nolock)
		WHERE Item_Key = @Item_Key
		AND Vendor_ID = @Vendor_ID
		AND PrimaryVendor = 1

		OPEN StoreNo_Cursor 
		FETCH NEXT FROM StoreNo_Cursor into @Store_No

		WHILE @@FETCH_STATUS = 0

		BEGIN
			
				SELECT @Count = count(*)
				FROM  StoreItemVendor SIV (nolock)
				WHERE  SIV.Store_No = @Store_No 
				AND SIV.Item_Key = @Item_Key 
				AND SIV.PrimaryVendor = 0
				AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
		
				IF @Count = 0
				BEGIN	
					-- No secondary vendor exists. 
					-- Warning item to be de-authorized for store.
					SELECT @WarningDeauthCount = @WarningDeauthCount + 1	
				END
		
				IF @Count = 1
				BEGIN
					-- One secondary vendor exists. 
					-- Warning item to be swapped to a new primary vendor.
					SELECT @WarningSwapCount = @WarningSwapCount + 1
				END
		
				IF @Count > 1
				BEGIN		
					-- More than one secondary vendor exists. 
					-- Error primary vendor can't be deleted.
					SELECT @ErrorCount = @ErrorCount + 1			
				END
			
			FETCH NEXT FROM StoreNo_Cursor into @Store_No
		END -- while @@ fetch_status

		CLOSE StoreNo_Cursor
		DEALLOCATE StoreNo_Cursor
	
	
	END -- if @Delete_Vendor = 1
  
   
	INSERT into @tmp_ValidationResult (WarningDeauthCount, WarningSwapCount, ErrorCount)
	VALUES (@WarningDeauthCount, @WarningSwapCount, @ErrorCount) 
	
	SELECT  WarningDeauthCount, WarningSwapCount, ErrorCount from @tmp_ValidationResult
                    
    SET NOCOUNT OFF
END
GO


