 if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdateCurrency') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdateCurrency
GO

CREATE PROCEDURE dbo.UpdateCurrency 
	@CurrencyId		int,
	@CurrencyCode	char(3),
	@CurrencyName	varchar(255)
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
		
		UPDATE Currency
		SET CurrencyCode = ISNULL(@CurrencyCode, CurrencyCode),
			CurrencyName = ISNULL(@CurrencyName, CurrencyName)
		WHERE CurrencyId = @CurrencyId

		SELECT @error_no = @@ERROR

		IF @error_no != 0
		BEGIN
			SET NOCOUNT OFF
			IF @@TRANCOUNT <> 0
				ROLLBACK TRAN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
			RAISERROR ('UpdateCurrency failed with @@ERROR: %d', @Severity, 1, @error_no)
		END
	END
	
	SET NOCOUNT OFF
	
END
GO
 