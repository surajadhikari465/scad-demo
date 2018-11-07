CREATE PROCEDURE dbo.[Replenishment_TLog_UK_CreateItemRecord] 
	@TimeKey smalldatetime,
	@TransactionNo int,
	@StoreNo int, 
	@RegisterNo int, 
	@SalesQty int, 
	@Weight float,
	@SalesAmt money,
	@Identifier varchar(50),
	@Dept_No int,
	@VatCode int,
	@Turnover_Dept varchar(10),
	@Row_No int,
	@Retail_Price money,
	@Trans_Type varchar(10)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Item_Key int	
	DECLARE @Food_Stamp int
	DECLARE @SubTeam_No int
	DECLARE @IsScaleItem bit
	DECLARE @Price money
	
	set @IsScaleItem = 0
	
	
	
	-- trim leading 0's from identifier.
	set @Identifier = (select convert(varchar, convert(decimal(18,0), @Identifier)))
	print 'ID: ' + @Identifier
	if len(@Identifier) > 10
	BEGIN
		if ((len(@Identifier) = 12 and substring(@Identifier,1,1)='2')
		OR (len(@Identifier) = 13 and substring(@Identifier,1,2)='20')
		OR (len(@Identifier) = 13 and substring(@Identifier,1,2)='21')
		OR (len(@Identifier) = 13 and substring(@Identifier,1,2)='22'))
		BEGIN
			set @IsScaleItem = 1
		END
	END
	
	
	
	

	set @Item_Key = (Select Item_Key from ItemIdentifier where Deleted_Identifier <> 1 and Identifier + isnull(CheckDigit,'')  = @Identifier)
	print 'Key: ' + cast(@Item_Key as varchar)
	set @Food_Stamp = (Select Food_Stamps from Item where Item_Key = @Item_Key)
	set @SubTeam_No = (Select SubTeam_No from Item where Item_Key = @Item_Key)
	if @IsScaleItem = 1
	BEGIN
		set @Price = (select PosPrice from Price where Item_Key= @Item_Key and store_no = @StoreNo)
		print cast(@Price as varchar)
		print cast(@SalesAmt as varchar)
		print cast(@Weight as varchar)
			set @Weight = @SalesAmt / @Price
			
				
			print cast((cast(@SalesAmt as float) / cast(@Price as float)) as varchar)
	END
	
	
	if LTRIM(RTRIM(@Trans_Type)) = 'VOID' 
	begin
		set @Weight = ABS(@Weight) * -1
	end
	
	IF EXISTS 
		(
			SELECT TimeKey FROM TLOG_UK_Item
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo AND
			Item_Key = @Item_Key AND
			Row_No = @Row_No			
		)
		BEGIN
			DELETE FROM TLOG_UK_Item
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @TransactionNo AND 
			Store_No = @StoreNo AND 
			Register_No = @RegisterNo AND
			Item_Key = @Item_Key AND
			Row_No = @Row_No
		END 


	Insert Into TLOG_UK_Item
	(
		TimeKey,
		Transaction_No, 
		Store_No, 
		Register_No, 
		Item_Key,
		SubTeam_No,
		Food_Stamp, 
		Sales_Quantity, 
		Weight, 
		Sales_Amount,
		Identifier,
		Dept_No,
		Vat_Code,
		Turnover_Dept,
		Row_No,
		Retail_Price,
		Trans_Type
		
	)
	Values
	(
		@TimeKey, 
		@TransactionNo,
		@StoreNo,
		@RegisterNo,
		@Item_Key,
		@SubTeam_No,
		@Food_Stamp, 
		@SalesQty, 
		@Weight, 
		@SalesAmt,
		@Identifier,
		@Dept_No,
		@VatCode,
		@Turnover_Dept,
		@Row_No,
		@Retail_Price,
		@Trans_Type
	)
	SET NOCOUNT OFF;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateItemRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateItemRecord] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateItemRecord] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateItemRecord] TO [IRMAReportsRole]
    AS [dbo];

