/****** Object:  StoredProcedure [dbo].[TaxHosting_DeleteTaxFlag]    Script Date: 05/19/2006 16:33:25 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[TaxHosting_DeleteTaxClass]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[TaxHosting_DeleteTaxClass]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.TaxHosting_DeleteTaxClass
	@TaxClassID int
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

	-- delete associated Item data
	UPDATE 
		Item
	SET 
		TaxClassID = NULL
	WHERE
		TaxClassID = @TaxClassID
		
	SELECT @error_no = @@ERROR
	
	-- delete associated ItemChangeHistory data
	UPDATE 
		ItemChangeHistory
	SET 
		TaxClassID = NULL
	WHERE
		TaxClassID = @TaxClassID
		
	SELECT @error_no = @@ERROR
	
	-- delete Tax Class
	DELETE FROM
		TaxClass
	WHERE
		TaxClassID = @TaxClassID		
	
	SELECT @error_no = @@ERROR	

    SET NOCOUNT OFF

    IF @error_no = 0
	COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('DeleteTaxClass failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO

