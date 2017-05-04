CREATE PROCEDURE dbo.SLIM_CreateStoreSpecial
	@ItemKey as int,
	@Store_no as int,
	@Price as money, 
	@Multiple as int,
	@SalePrice as money, 
	@SaleMultiple as int,
	@POSPrice as money, 
	@POSSalePrice as money,
	@StartDate as smalldatetime,
	@EndDate as smalldatetime,
	@Status as int,
	@Identifier as varchar(13),
	@Item_Description as varchar(100),
	@SubTeam_Name as varchar(50),
	@RequestedBy as varchar(25),
	@RequestID as int OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	INSERT INTO SLIM_InStoreSpecials
	(
		Item_Key,
		Store_no,
		Price,
		Multiple,
		SalePrice,
		SaleMultiple,
		POSPrice,
		POSSalePrice,
		StartDate,
		EndDate,
		Status,
		Identifier,
		Item_Description,
		SubTeam_Name,
		RequestedBy
	) 
	VALUES
	(
		@ItemKey,
		@Store_no ,
		@Price, 
		@Multiple ,
		@SalePrice , 
		@SaleMultiple ,
		@POSPrice , 
		@POSSalePrice ,
		@StartDate ,
		@EndDate ,
		@Status ,
		@Identifier ,
		@Item_Description ,
		@SubTeam_Name ,
		@RequestedBy 
	)

	set @RequestID = scope_identity()

	COMMIT TRAN
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_CreateStoreSpecial] TO [IRMASLIMRole]
    AS [dbo];

