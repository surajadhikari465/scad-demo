-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE dbo.[Replenishment_UKTLog_CreateItemFact]
	-- Add the parameters for the stored procedure here
	@DateKey smalldatetime, 
	@StoreNo int ,
	@RegisterNo int, 
	@TransactionNo int ,
	@OperatorNo int ,
	@SubTeamNo int ,
	@BarCode Varchar(50) ,
	@SalesQty int ,
	@Weight real,
	@RetailPrice money, 
	@SalesValue money 

AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO IRMA_TLog_ItemFact (Date_Key,  Store_No, Register_No, Transaction_No, Operator_No, SubTeam_No, BarCode, Sales_Qty ,Weight, Retail_Price, Sales_Value )
	VALUES (@DateKey, @StoreNo, @RegisterNo, @TransactionNo, @OperatorNo, @SubTeamNo, @BarCode, @SalesQty ,@Weight, @RetailPrice, @SalesValue )
END