 if exists (select * from dbo.sysobjects where id = object_id(N'dbo.InsertCurrency') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.InsertCurrency
GO

CREATE PROCEDURE dbo.InsertCurrency 
	@CurrencyCode char(3),
	@CurrencyName varchar(255),
	@CurrencyID int OUTPUT
AS

BEGIN
	SET NOCOUNT ON

	DECLARE @error_no int
	SELECT @error_no = 0

	IF EXISTS (SELECT 1 
		FROM Currency 
		WHERE CurrencyCode = @CurrencyCode 
		AND IsDeleted = 0) 
		BEGIN
			RAISERROR ('Currency Code already exists.',0,0,0)
		END
	ELSE 
		BEGIN

		INSERT INTO Currency (CurrencyCode, CurrencyName)
		VALUES (@CurrencyCode, @CurrencyName) 

		SELECT @error_no = @@ERROR,
			@CurrencyID = SCOPE_IDENTITY()

		IF @error_no != 0
		BEGIN
			SET NOCOUNT OFF
			IF @@TRANCOUNT <> 0
				ROLLBACK TRAN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
			RAISERROR ('InsertCurrency failed with @@ERROR: %d', @Severity, 1, @error_no)
		END
	END
	
	SET NOCOUNT OFF
	
END
GO
