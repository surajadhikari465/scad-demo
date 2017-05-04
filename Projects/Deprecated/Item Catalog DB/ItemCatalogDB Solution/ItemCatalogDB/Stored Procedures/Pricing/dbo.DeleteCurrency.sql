 if exists (select * from dbo.sysobjects where id = object_id(N'dbo.DeleteCurrency') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.DeleteCurrency
GO

CREATE PROCEDURE dbo.DeleteCurrency 
	@CurrencyId		int
AS

BEGIN
	SET NOCOUNT ON

	DECLARE @error_no int
	SELECT @error_no = 0

	UPDATE Currency
	SET IsDeleted = 1
	WHERE CurrencyId = @CurrencyId

	SELECT @error_no = @@ERROR

	IF @error_no != 0
	BEGIN
		SET NOCOUNT OFF
		IF @@TRANCOUNT <> 0
			ROLLBACK TRAN
		DECLARE @Severity smallint
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
		RAISERROR ('DeleteCurrency failed with @@ERROR: %d', @Severity, 1, @error_no)
	END
	
	SET NOCOUNT OFF

END
GO
  