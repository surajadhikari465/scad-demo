CREATE PROCEDURE dbo.[Replenishment_TLog_UK_CreateVoidRecord] 

	@TimeKey smalldatetime,
	@Store_No int,
	@Register_No int, 
	@Transaction_No int,
	@Operator_No int, 
	@Sales_Value money
AS
BEGIN
	SET NOCOUNT ON;


		IF EXISTS 
		(
			SELECT TimeKey FROM TLOG_UK_Voids
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @Transaction_No AND 
			Store_No = @Store_No AND 
			Register_No = @Register_No
		)
		BEGIN
			DELETE FROM TLOG_UK_Voids
			WHERE TimeKey = @TimeKey AND 
			Transaction_No = @Transaction_No AND 
			Store_No = @Store_No AND 
			Register_No = @Register_No 
			
		END

		INSERT INTO TLOG_UK_Voids
		(
			TimeKey,
			Store_No,
			Register_No,
			Operator_No,
			Transaction_No,
			Sales_Value
		) 
		VALUES
		(
			@TimeKey, 
			@Store_No,
			@Register_No,
			@Operator_No,
			@Transaction_No,
			@Sales_Value
		)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateVoidRecord] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_TLog_UK_CreateVoidRecord] TO [IRMAClientRole]
    AS [dbo];

